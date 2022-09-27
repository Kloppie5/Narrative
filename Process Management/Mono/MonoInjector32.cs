using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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
        UInt64 monoModule;
        Int64 hProcess = 0;

        Int32 dataSpace = 0;
        Int64 codeSpace = 0;

        Dictionary<String, UInt64> exportedFunctions = new Dictionary<String, UInt64>();

        public MonoInjector32 ( ProcessManager64 manager ) {
            _manager = manager;
            monoModule = MonoHelper32.FindMonoModule(manager);
            hProcess = OpenProcess(0x1F0FFF, false, manager.process.Id);
            dataSpace = (Int32)VirtualAllocEx(hProcess, 0, 0x1000, 0x3000, 0x40);
            codeSpace = (Int64)VirtualAllocEx(hProcess, 0, 0x1000, 0x3000, 0x40);
            Console.WriteLine($"Allocated codeSpace: {codeSpace:X}");
            exportedFunctions = PEHelper.GetExportedFunctions(manager, monoModule);
            Console.WriteLine($"Found {exportedFunctions.Count} exported functions");
        }
        public void Execute ( byte[] code ) {
            Int64 bytesWritten;
            WriteProcessMemory(hProcess, codeSpace, code, code.Length, out bytesWritten);
            Int64 thread = CreateRemoteThread(hProcess, 0, 0, codeSpace, 0, 0, 0);
            WaitForSingleObject(thread, 0xFFFFFFFF);
        }
        public T CallFunction<T> ( String functionName, params object[] args ) where T : struct {
            if ( !exportedFunctions.ContainsKey(functionName) )
                throw new Exception($"Could not find exported function {functionName}");
            Int32 functionAddress = (Int32)exportedFunctions[functionName];

            Assembler assembler = new Assembler();
            assembler.MOVi32r(0, functionAddress);
            assembler.CALLr(0);
            assembler.MOVr0m32(dataSpace);
            assembler.RET();

            assembler.PrettyPrint();
            byte[] code = assembler.finalize();

            Execute(code);

            return MemoryHelper.ReadAbsolute<T>(_manager, (UInt64) dataSpace);
        }

        public Int32 GetRootDomain ( ) {
            return CallFunction<Int32>("mono_get_root_domain");
        }
    }
}