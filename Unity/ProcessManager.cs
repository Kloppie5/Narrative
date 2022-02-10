using System;
using System.Collections.Generic;

namespace Unity {
    class ProcessManager : Mono.ProcessManager {

        public ProcessManager ( String processName ) : base( processName ) {
            // TODO
        }

        public UInt32 GetUnityRootDomain() {
            Dictionary<String, UInt64> modules = GetModules64();
            UInt64 MonoBaseAddress = 0;
            foreach ( KeyValuePair<String, UInt64> module in modules ) {
                if ( module.Key.Contains("mono-2.0-bdwgc.dll") ) {
                    Console.WriteLine($"Found mono-2.0-bdwgc.dll at {module.Value:X}");
                    MonoBaseAddress = module.Value;
                    break;
                }
                if ( module.Key.Contains("mono.dll") ) {
                    Console.WriteLine($"Found mono.dll at {module.Value:X}");
                    MonoBaseAddress = module.Value;
                    break;
                }
            }
            Console.WriteLine($"MonoBaseAddress: {MonoBaseAddress:X}");
            UInt32 PESignatureOffset = ReadAbsolute<UInt32>(MonoBaseAddress + 0x3C);
            UInt32 ExportedFunctionsOffset = ReadAbsolute<UInt32>(MonoBaseAddress + PESignatureOffset + 0x78);
            UInt32 IMAGE_EXPORT_DIRECTORY_NumberOfFunctions = ReadAbsolute<UInt32>(MonoBaseAddress + ExportedFunctionsOffset + 0x14);
            UInt32 IMAGE_EXPORT_DIRECTORY_NumberOfNames = ReadAbsolute<UInt32>(MonoBaseAddress + ExportedFunctionsOffset + 0x18);
            UInt32 IMAGE_EXPORT_DIRECTORY_AddressOfFunctions = ReadAbsolute<UInt32>(MonoBaseAddress + ExportedFunctionsOffset + 0x1C);
            UInt32 IMAGE_EXPORT_DIRECTORY_AddressOfNames = ReadAbsolute<UInt32>(MonoBaseAddress + ExportedFunctionsOffset + 0x20);

            Dictionary<String, UInt32> exportedFunctions = new Dictionary<String, UInt32>();
            for ( UInt32 i = 0; i < IMAGE_EXPORT_DIRECTORY_NumberOfFunctions; ++i ) {
                UInt32 FunctionNameOffset = ReadAbsolute<UInt32>(MonoBaseAddress + IMAGE_EXPORT_DIRECTORY_AddressOfNames + i * 4);
                String FunctionName = ReadAbsoluteUTF8String(MonoBaseAddress + FunctionNameOffset);
                UInt32 FunctionOffset = ReadAbsolute<UInt32>(MonoBaseAddress + IMAGE_EXPORT_DIRECTORY_AddressOfFunctions + i * 4);
                exportedFunctions.Add(FunctionName, FunctionOffset);
                Console.WriteLine($"{FunctionName} {FunctionOffset:X}");
            }

            // TODO: split into two functions

            UInt32 GetRootDomainOffset = exportedFunctions["mono_get_root_domain"];
            // mono-2.0-bdwgc.mono_get_root_domain
            // mono.mono_get_root_domain
            // A1 ???????? - mov eax, {RootDomain}
            UInt32 RootDomainPointer = ReadAbsolute<UInt32>(MonoBaseAddress + GetRootDomainOffset + 0x1);
            return ReadAbsolute<UInt32>(RootDomainPointer);
        }
    }
}
