using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Int8 = System.SByte;
using UInt8 = System.Byte;

namespace Narrative {

    public class MonoInjector32 {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern Int64 OpenProcess ( UInt32 dwDesiredAccess, bool bInheritHandle, Int32 dwProcessId );
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern bool VirtualFreeEx ( Int64 hProcess, Int64 lpAddress, Int64 dwSize, UInt32 dwFreeType );
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern Int64 VirtualAllocEx ( Int64 hProcess, Int64 lpAddress, Int64 dwSize, UInt32 flAllocationType, UInt32 flProtect );
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory ( Int64 hProcess, Int64 lpBaseAddress, byte[] lpBuffer, Int64 nSize, out Int64 lpNumberOfBytesWritten );
        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern Int64 GetProcAddress ( Int64 hModule, string procName );
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern Int64 GetModuleHandle ( string lpModuleName );
        [DllImport("kernel32.dll")]
        static extern Int64 CreateRemoteThread ( Int64 hProcess, Int64 lpThreadAttributes, UInt32 dwStackSize, Int64 lpStartAddress, Int64 lpParameter, UInt32 dwCreationFlags, Int64 lpThreadId );

        public enum WaitResult : UInt32 {
            WAIT_ABANDONED = 0x00000080,
            WAIT_OBJECT_0 = 0x00000000,
            WAIT_TIMEOUT = 0x00000102,
            WAIT_FAILED = 0xFFFFFFFF
        }
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern WaitResult WaitForSingleObject ( Int64 hHandle, UInt32 dwMilliseconds );


        ProcessManager64 _manager;
        Int64 monoModule;
        Int64 hProcess = 0;

        Int32 dataSpace = 0;
        Int64 codeSpace = 0;

        Int32 _rootDomain = 0;

        Dictionary<String, Int64> exportedFunctions = new Dictionary<String, Int64>();

        public MonoInjector32 ( ProcessManager64 manager ) {
            _manager = manager;
            monoModule = MonoHelper32.FindMonoModule(manager);
            hProcess = OpenProcess(0x1F0FFF, false, manager.process.Id);
            dataSpace = (Int32)VirtualAllocEx(hProcess, 0, 0x1000, 0x3000, 0x40);
            codeSpace = (Int64)VirtualAllocEx(hProcess, 0, 0x1000, 0x3000, 0x40);
            exportedFunctions = PEHelper.GetExportedFunctions(manager, monoModule);
            _rootDomain = CallFunction<Int32>("mono_get_root_domain");
        }
        public void Execute ( byte[] code ) {
            Int64 bytesWritten;
            WriteProcessMemory(hProcess, codeSpace, code, code.Length, out bytesWritten);
            Int64 thread = CreateRemoteThread(hProcess, 0, 0, codeSpace, 0, 0, 0);
            WaitForSingleObject(thread, 0xFFFFFFFF);
        }
        public T CallFunction<T> ( String functionName, params object[] args ) where T : struct {
            // TODO: some complicated design to deal with different argument handling
            if ( !exportedFunctions.ContainsKey(functionName) )
                throw new Exception($"Could not find exported function {functionName}");
            Int32 functionAddress = (Int32)exportedFunctions[functionName];
            Int32 mono_thread_attach = (Int32)exportedFunctions["mono_thread_attach"];

            Assembler assembler = new Assembler();

            if ( functionName != "mono_get_root_domain" ) {
                assembler.PUSHi32(_rootDomain);
                assembler.MOVi32r(0, mono_thread_attach);
                assembler.CALLr(0);
                assembler.ADDi8r(4, 4);
            }


            assembler.MOVi32r(0, functionAddress);
            for ( int i = args.Length - 1; i >= 0; --i ) {
                if ( args[i] is Int32 ) {
                    assembler.PUSHi32((Int32)args[i]);
                } else if ( args[i] is String ) {
                    Int32 stringAddress = (Int32)VirtualAllocEx(hProcess, 0, 0x1000, 0x3000, 0x40);
                    Int64 bytesWritten;
                    WriteProcessMemory(hProcess, stringAddress, Encoding.UTF8.GetBytes((String)args[i]), ((String)args[i]).Length, out bytesWritten);
                    assembler.PUSHi32(stringAddress);
                } else {
                    throw new Exception($"Unsupported argument type {args[i].GetType()}");
                }
            }
            assembler.CALLr(0);
            assembler.MOVr0m32(dataSpace);
            assembler.ADDi8r(4, (Int8) (4 * args.Length));


            assembler.RET();
            byte[] code = assembler.finalize();
            Execute(code);
            return MemoryHelper.ReadAbsolute<T>(_manager, dataSpace);
        }

        #region Domain
        public Int32 GetRootDomain ( ) { // -> MonoDomain*
            return _rootDomain;
        }
        public List<Int32> DomainGetAssemblies ( Int32 domain ) { // MonoDomain* -> List<MonoAssembly*>
            MonoDomain32 domainStruct = MemoryHelper.ReadAbsolute<MonoDomain32>(_manager, domain);
            List<Int32> assemblies = new List<Int32>();
            Int32 it = domainStruct.domain_assemblies;
            while ( it != 0 ) {
                GSList32 list = MemoryHelper.ReadAbsolute<GSList32>(_manager, it);
                assemblies.Add(list.data);
                it = list.next;
            }
            return assemblies;
        }
        public Dictionary<String, Int32> DomainGetAssembliesByName ( Int32 domain, List<String> assemblyNames = null ) { // MonoDomain*, List<String> -> Dictionary<String, MonoAssembly*>
            Dictionary<String, Int32> assemblies = new Dictionary<String, Int32>();
            foreach ( Int32 assembly in DomainGetAssemblies(domain) ) {
                String assemblyName = AssemblyGetName(assembly);
                if ( assemblyNames == null || assemblyNames.Contains(assemblyName) )
                    assemblies[assemblyName] = assembly;
            }
            return assemblies;
        }
        #endregion
        #region Assembly
        public String AssemblyGetName ( Int32 assembly ) { // MonoAssembly* -> String
            MonoAssembly32 assemblyStruct = MemoryHelper.ReadAbsolute<MonoAssembly32>(_manager, assembly);
            return MemoryHelper.ReadAbsoluteUTF8String(_manager, assemblyStruct.monoAssemblyNameDOTname);
        }
        public Int32 AssemblyGetImage ( Int32 assembly ) { // MonoAssembly* -> MonoImage*
            MonoAssembly32 assemblyStruct = MemoryHelper.ReadAbsolute<MonoAssembly32>(_manager, assembly);
            return assemblyStruct.image;
        }
        #endregion
        #region Image
        public Int32 ImageGetClassByName ( Int32 image, String @namespace, String className ) { // MonoImage*, String, String -> MonoClass*
            return CallFunction<Int32>("mono_class_from_name", image, @namespace, className);
        }
        #endregion
        #region Class
        public String ClassGetName ( Int32 @class ) { // MonoClass* -> String
            MonoClass32 classStruct = MemoryHelper.ReadAbsolute<MonoClass32>(_manager, @class);
            return MemoryHelper.ReadAbsoluteUTF8String(_manager, classStruct.name);
        }
        public List<Int32> ClassGetMethods ( Int32 @class ) { // MonoClass* -> List<MonoMethod*>
            MonoClass32 classStruct = MemoryHelper.ReadAbsolute<MonoClass32>(_manager, @class);
            List<Int32> methods = new List<Int32>();
            Int32 it = classStruct.methods;
            while ( true ) {
                Int32 method = MemoryHelper.ReadAbsolute<Int32>(_manager, it);
                if ( method == 0 )
                    break;
                methods.Add(method);
                it += 4;
            }
            return methods;
        }
        public Int32 ClassGetMethodByName ( Int32 @class, String methodName ) { // MonoClass*, String -> MonoMethod*
            foreach ( Int32 method in ClassGetMethods(@class) ) {
                if ( MethodGetName(method) == methodName )
                    return method;
            }
            return 0;
        }
        public Int32 ClassGetVTable ( Int32 @class ) { // MonoClass* -> MonoVTable*
            return CallFunction<Int32>("mono_class_vtable", _rootDomain, @class);
        }
        #endregion
        #region VTable
        public Int32 VTableGetStaticFieldData ( Int32 vtable ) { // MonoVTable* -> MonoObject*
            Int32 staticFieldData = CallFunction<Int32>("mono_vtable_get_static_field_data", vtable);
            return staticFieldData;
        }
        #endregion
        #region Method
        public String MethodGetName ( Int32 method ) { // MonoMethod* -> String
            Int32 name = CallFunction<Int32>("mono_method_get_name", method);
            return MemoryHelper.ReadAbsoluteUTF8String(_manager, name);
        }
        public T InvokeMethod<T> ( Int32 method, Int32 instance, params object[] args ) where T : struct { // MonoMethod*, MonoObject*, ... -> void
            Console.WriteLine($"Invoking {MethodGetName(method)}");
            Assembler assembler = new Assembler();
            assembler.debug = true;

            Int32 mono_thread_attach = (Int32)exportedFunctions["mono_thread_attach"];
            assembler.MOVi32r(0, mono_thread_attach);
            assembler.PUSHi32(_rootDomain);
            assembler.CALLr(0);
            assembler.ADDi8r(4, 4);

            Int32 mono_runtime_invoke = (Int32)exportedFunctions["mono_runtime_invoke"];
            assembler.MOVi32r(0, mono_runtime_invoke);
            assembler.PUSHi32(0); // exception handler
            Int32 argspace = (Int32)VirtualAllocEx(hProcess, 0, 0x1000, 0x3000, 0x40);
            for ( int i = 0; i < args.Length; i++ ) {
                if ( args[i] is Int32 )
                    MemoryHelper.WriteAbsolute(_manager, argspace + 4 * i, (Int32)args[i]);
                else
                    throw new Exception($"Unsupported argument type {args[i].GetType()}");
            }
            assembler.PUSHi32(argspace);
            assembler.PUSHi32(instance);
            assembler.PUSHi32(method);
            assembler.CALLr(0);
            assembler.MOVr0m32(dataSpace);
            assembler.ADDi8r(4, 16);

            assembler.RET();
            byte[] code = assembler.finalize();
            Execute(code);
            VirtualFreeEx(hProcess, argspace, 0, 0x8000);
            return MemoryHelper.ReadAbsolute<T>(_manager, dataSpace);
        }
        #endregion
    }
}
