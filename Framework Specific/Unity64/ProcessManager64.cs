using System;
using System.Collections.Generic;

using UInt8 = System.Byte;

namespace Unity64 {
    public class ProcessManager64 : Mono64.ProcessManager64 {

        public ProcessManager64 ( String processName ) : base( processName ) {
            // TODO
        }

        public UInt64 GetUnityRootDomain() {
            Dictionary<String, UInt64> modules = GetModules64();
            UInt64 ImageBase = 0;
            bool PE32 = false;
            bool PE32plus = false;
            foreach ( KeyValuePair<String, UInt64> module in modules ) {
                if ( module.Key.Contains("mono-2.0-bdwgc.dll") ) {
                    // Console.WriteLine($"Found mono-2.0-bdwgc.dll at {module.Value:X}");
                    ImageBase = module.Value;
                    break;
                }
                if ( module.Key.Contains("mono.dll") ) {
                    // Console.WriteLine($"Found mono.dll at {module.Value:X}");
                    ImageBase = module.Value;
                    break;
                }
            }
            Console.WriteLine($"ImageBase: {ImageBase:X}");
            UInt32 PESignatureOffset = ReadAbsolute<UInt32>(ImageBase + 0x3C);
            UInt64 COFFFileHeaderAddress = ImageBase + PESignatureOffset + 0x4;
            // COFF File Header
            UInt16 Machine = ReadAbsolute<UInt16>(COFFFileHeaderAddress + 0x0);
            UInt16 NumberOfSections = ReadAbsolute<UInt16>(COFFFileHeaderAddress + 0x2);
            UInt32 TimeDateStamp = ReadAbsolute<UInt32>(COFFFileHeaderAddress + 0x4);
            UInt32 PointerToSymbolTable = ReadAbsolute<UInt32>(COFFFileHeaderAddress + 0x8);
            UInt32 NumberOfSymbols = ReadAbsolute<UInt32>(COFFFileHeaderAddress + 0xC);
            UInt16 SizeOfOptionalHeader = ReadAbsolute<UInt16>(COFFFileHeaderAddress + 0x10);
            UInt16 Characteristics = ReadAbsolute<UInt16>(COFFFileHeaderAddress + 0x12);
            // Optional Header
            UInt64 OptionalHeaderAddress = COFFFileHeaderAddress + 0x14;
            UInt16 Magic = ReadAbsolute<UInt16>(OptionalHeaderAddress + 0x0);
            if ( Magic == 0x10B ) {
                PE32 = true;
            } else if ( Magic == 0x20B ) {
                PE32plus = true;
            } else {
                Console.WriteLine($"Unknown Magic: {Magic:X}");
                return UInt64.MaxValue;
            }
            UInt8 MajorLinkerVersion = ReadAbsolute<UInt8>(OptionalHeaderAddress + 0x2);
            UInt8 MinorLinkerVersion = ReadAbsolute<UInt8>(OptionalHeaderAddress + 0x3);
            UInt32 SizeOfCode = ReadAbsolute<UInt32>(OptionalHeaderAddress + 0x4);
            UInt32 SizeOfInitializedData = ReadAbsolute<UInt32>(OptionalHeaderAddress + 0x8);
            UInt32 SizeOfUninitializedData = ReadAbsolute<UInt32>(OptionalHeaderAddress + 0xC);
            UInt32 AddressOfEntryPoint = ReadAbsolute<UInt32>(OptionalHeaderAddress + 0x10);
            UInt32 BaseOfCode = ReadAbsolute<UInt32>(OptionalHeaderAddress + 0x14);
            // Extended Optional Header
            UInt32 BaseOfData = 0;
            UInt64 ImageBaseAddress = 0;
            if ( PE32 ) {
                BaseOfData = ReadAbsolute<UInt32>(OptionalHeaderAddress + 0x18);
                ImageBaseAddress = ReadAbsolute<UInt32>(OptionalHeaderAddress + 0x1C);
            }
            if ( PE32plus ) {
                ImageBaseAddress = ReadAbsolute<UInt64>(OptionalHeaderAddress + 0x18);
            }
            UInt32 SectionAlignment = ReadAbsolute<UInt32>(OptionalHeaderAddress + 0x20);
            UInt32 FileAlignment = ReadAbsolute<UInt32>(OptionalHeaderAddress + 0x24);
            UInt16 MajorOperatingSystemVersion = ReadAbsolute<UInt16>(OptionalHeaderAddress + 0x28);
            UInt16 MinorOperatingSystemVersion = ReadAbsolute<UInt16>(OptionalHeaderAddress + 0x2A);
            UInt16 MajorImageVersion = ReadAbsolute<UInt16>(OptionalHeaderAddress + 0x2C);
            UInt16 MinorImageVersion = ReadAbsolute<UInt16>(OptionalHeaderAddress + 0x2E);
            UInt16 MajorSubsystemVersion = ReadAbsolute<UInt16>(OptionalHeaderAddress + 0x30);
            UInt16 MinorSubsystemVersion = ReadAbsolute<UInt16>(OptionalHeaderAddress + 0x32);
            UInt32 Win32VersionValue = ReadAbsolute<UInt32>(OptionalHeaderAddress + 0x34);
            UInt32 SizeOfImage = ReadAbsolute<UInt32>(OptionalHeaderAddress + 0x38);
            UInt32 SizeOfHeaders = ReadAbsolute<UInt32>(OptionalHeaderAddress + 0x3C);
            UInt32 CheckSum = ReadAbsolute<UInt32>(OptionalHeaderAddress + 0x40);
            UInt16 Subsystem = ReadAbsolute<UInt16>(OptionalHeaderAddress + 0x44);
            UInt16 DllCharacteristics = ReadAbsolute<UInt16>(OptionalHeaderAddress + 0x46);
            UInt64 SizeOfStackReserve = 0;
            UInt64 SizeOfStackCommit = 0;
            UInt64 SizeOfHeapReserve = 0;
            UInt64 SizeOfHeapCommit = 0;
            UInt32 LoaderFlags = 0;
            UInt32 NumberOfRvaAndSizes = 0;
            if ( PE32 ) {
                SizeOfStackReserve = ReadAbsolute<UInt32>(OptionalHeaderAddress + 0x48);
                SizeOfStackCommit = ReadAbsolute<UInt32>(OptionalHeaderAddress + 0x4C);
                SizeOfHeapReserve = ReadAbsolute<UInt32>(OptionalHeaderAddress + 0x50);
                SizeOfHeapCommit = ReadAbsolute<UInt32>(OptionalHeaderAddress + 0x54);
                LoaderFlags = ReadAbsolute<UInt32>(OptionalHeaderAddress + 0x58);
                NumberOfRvaAndSizes = ReadAbsolute<UInt32>(OptionalHeaderAddress + 0x5C);
            }
            if ( PE32plus ) {
                SizeOfStackReserve = ReadAbsolute<UInt64>(OptionalHeaderAddress + 0x48);
                SizeOfStackCommit = ReadAbsolute<UInt64>(OptionalHeaderAddress + 0x50);
                SizeOfHeapReserve = ReadAbsolute<UInt64>(OptionalHeaderAddress + 0x58);
                SizeOfHeapCommit = ReadAbsolute<UInt64>(OptionalHeaderAddress + 0x60);
                LoaderFlags = ReadAbsolute<UInt32>(OptionalHeaderAddress + 0x68);
                NumberOfRvaAndSizes = ReadAbsolute<UInt32>(OptionalHeaderAddress + 0x6C);
            }
            // Data Directories
            UInt64 DataDirectoryAddress = 0;
            if ( PE32 ) {
                DataDirectoryAddress = OptionalHeaderAddress + 0x60;
            }
            if ( PE32plus ) {
                DataDirectoryAddress = OptionalHeaderAddress + 0x70;
            }
            UInt32 ExportTableOffset = ReadAbsolute<UInt32>(DataDirectoryAddress + 0x0);
            UInt32 ExportTableSize = ReadAbsolute<UInt32>(DataDirectoryAddress + 0x4);
            UInt32 ImportTableOffset = ReadAbsolute<UInt32>(DataDirectoryAddress + 0x8);
            UInt32 ImportTableSize = ReadAbsolute<UInt32>(DataDirectoryAddress + 0xC);
            UInt32 ResourceTableOffset = ReadAbsolute<UInt32>(DataDirectoryAddress + 0x10);
            UInt32 ResourceTableSize = ReadAbsolute<UInt32>(DataDirectoryAddress + 0x14);
            UInt32 ExceptionTableOffset = ReadAbsolute<UInt32>(DataDirectoryAddress + 0x18);
            UInt32 ExceptionTableSize = ReadAbsolute<UInt32>(DataDirectoryAddress + 0x1C);
            UInt32 CertificateTableOffset = ReadAbsolute<UInt32>(DataDirectoryAddress + 0x20);
            UInt32 CertificateTableSize = ReadAbsolute<UInt32>(DataDirectoryAddress + 0x24);
            UInt32 BaseRelocationTableOffset = ReadAbsolute<UInt32>(DataDirectoryAddress + 0x28);
            UInt32 BaseRelocationTableSize = ReadAbsolute<UInt32>(DataDirectoryAddress + 0x2C);
            UInt32 DebugOffset = ReadAbsolute<UInt32>(DataDirectoryAddress + 0x30);
            UInt32 DebugSize = ReadAbsolute<UInt32>(DataDirectoryAddress + 0x34);
            UInt32 ArchitectureOffset = ReadAbsolute<UInt32>(DataDirectoryAddress + 0x38);
            UInt32 ArchitectureSize = ReadAbsolute<UInt32>(DataDirectoryAddress + 0x3C);
            UInt32 GlobalPointerTableOffset = ReadAbsolute<UInt32>(DataDirectoryAddress + 0x40);
            UInt32 GlobalPointerTableSize = ReadAbsolute<UInt32>(DataDirectoryAddress + 0x44);
            UInt32 ThreadLocalStorageTableOffset = ReadAbsolute<UInt32>(DataDirectoryAddress + 0x48);
            UInt32 ThreadLocalStorageTableSize = ReadAbsolute<UInt32>(DataDirectoryAddress + 0x4C);
            UInt32 LoadConfigTableOffset = ReadAbsolute<UInt32>(DataDirectoryAddress + 0x50);
            UInt32 LoadConfigTableSize = ReadAbsolute<UInt32>(DataDirectoryAddress + 0x54);
            UInt32 BoundImportOffset = ReadAbsolute<UInt32>(DataDirectoryAddress + 0x58);
            UInt32 BoundImportSize = ReadAbsolute<UInt32>(DataDirectoryAddress + 0x5C);
            UInt32 ImportAddressTableOffset = ReadAbsolute<UInt32>(DataDirectoryAddress + 0x60);
            UInt32 ImportAddressTableSize = ReadAbsolute<UInt32>(DataDirectoryAddress + 0x64);
            UInt32 DelayImportDescriptorOffset = ReadAbsolute<UInt32>(DataDirectoryAddress + 0x68);
            UInt32 DelayImportDescriptorSize = ReadAbsolute<UInt32>(DataDirectoryAddress + 0x6C);
            UInt32 ClrRuntimeHeaderOffset = ReadAbsolute<UInt32>(DataDirectoryAddress + 0x70);
            UInt32 ClrRuntimeHeaderSize = ReadAbsolute<UInt32>(DataDirectoryAddress + 0x74);

            // Export Table Header
            UInt64 ExportTableAddress = ImageBase + ExportTableOffset;
            UInt32 ExportTableExportFlags = ReadAbsolute<UInt32>(ExportTableAddress + 0x0);
            UInt32 ExportTableTimeDateStamp = ReadAbsolute<UInt32>(ExportTableAddress + 0x4);
            UInt16 ExportTableMajorVersion = ReadAbsolute<UInt16>(ExportTableAddress + 0x8);
            UInt16 ExportTableMinorVersion = ReadAbsolute<UInt16>(ExportTableAddress + 0xA);
            UInt32 ExportTableNameRva = ReadAbsolute<UInt32>(ExportTableAddress + 0xC);
            UInt32 ExportTableOrdinalBase = ReadAbsolute<UInt32>(ExportTableAddress + 0x10);
            UInt32 ExportTableAddressTableEntries = ReadAbsolute<UInt32>(ExportTableAddress + 0x14);
            UInt32 ExportTableNumberOfNamePointers = ReadAbsolute<UInt32>(ExportTableAddress + 0x18);
            UInt32 ExportTableExportAddressTableRva = ReadAbsolute<UInt32>(ExportTableAddress + 0x1C);
            UInt32 ExportTableNamePointerRva = ReadAbsolute<UInt32>(ExportTableAddress + 0x20);
            UInt32 ExportTableOrdinalTableRva = ReadAbsolute<UInt32>(ExportTableAddress + 0x24);

            Dictionary<String, UInt32> exportedFunctions = new Dictionary<String, UInt32>();
            for ( UInt32 i = 0; i < ExportTableAddressTableEntries; ++i ) {
                UInt32 FunctionNameOffset = ReadAbsolute<UInt32>(ImageBase + ExportTableNamePointerRva + i * 4);
                String FunctionName = ReadAbsoluteUTF8String(ImageBase + FunctionNameOffset);
                UInt32 FunctionOffset = ReadAbsolute<UInt32>(ImageBase + ExportTableExportAddressTableRva + i * 4);
                exportedFunctions.Add(FunctionName, FunctionOffset);
            }

            UInt32 MonoGetRootDomainOffset = exportedFunctions["mono_get_root_domain"];
            // mono-2.0-bdwgc.mono_get_root_domain
            // mono.mono_get_root_domain
            UInt8 MonoGetRootDomainOpcode = ReadAbsolute<UInt8>(ImageBase + MonoGetRootDomainOffset);
            if ( MonoGetRootDomainOpcode == 0xA1 ) {
                // A1 ???????? - mov eax, {RootDomain}
                // TODO
                Console.WriteLine("TODO");
                return 0;
            }
            if ( MonoGetRootDomainOpcode == 0x48 ) {
                // 48 8B 05 ???????? - mov rax, QWORD PTR [rip+{offset}]
                UInt32 offset = ReadAbsolute<UInt32>(ImageBase + MonoGetRootDomainOffset + 0x3);
                UInt64 RootDomainAddress = ImageBase + MonoGetRootDomainOffset + 0x7 + offset;
                return ReadAbsolute<UInt64>(RootDomainAddress);
            }
            Console.WriteLine("TODO");
            return 0;
        }
    }
}
