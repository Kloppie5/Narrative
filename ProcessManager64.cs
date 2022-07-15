using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using UInt8 = System.Byte;

namespace Narrative {
    public class ProcessManager64 {
        public Process process;

        public String ProcessName;
        public UInt64 BaseAddress => (UInt64) process.MainModule.BaseAddress;

        public ProcessManager64 ( String processName ) {
            ProcessName = processName;
            CheckConnected();
        }
        public Boolean CheckConnected ( ) {
            if ( process == null )
                return TryConnect();

            process.Refresh();
            if ( process.HasExited ) {
                process = null;
                return TryConnect();
            }
            return true;
        }
        public Boolean TryConnect ( ) {
            foreach ( var process in Process.GetProcesses() ) {
                if ( process.MainWindowTitle == ProcessName ) {
                    this.process = process;
                    return true;
                }
            }
            return false;
        }

        #region Structs
        public struct MEMORY_BASIC_INFORMATION64 {
            public UInt64 BaseAddress;
            public UInt64 AllocationBase;
            public UInt32 AllocationProtect;
            public UInt32 __alignment1;
            public UInt64 RegionSize;
            public UInt32 State;
            public UInt32 Protect;
            public UInt32 Type;
            public UInt32 __alignment2;
        }
        public struct Rect {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }

        #region PE Structs
        public struct IMAGE_DOS_HEADER {        // DOS .EXE header
            public UInt16 e_magic;              // Magic number
            public UInt16 e_cblp;               // Bytes on last page of file
            public UInt16 e_cp;                 // Pages in file
            public UInt16 e_crlc;               // Relocations
            public UInt16 e_cparhdr;            // Size of header in paragraphs
            public UInt16 e_minalloc;           // Minimum extra paragraphs needed
            public UInt16 e_maxalloc;           // Maximum extra paragraphs needed
            public UInt16 e_ss;                 // Initial (relative) SS value
            public UInt16 e_sp;                 // Initial SP value
            public UInt16 e_csum;               // Checksum
            public UInt16 e_ip;                 // Initial IP value
            public UInt16 e_cs;                 // Initial (relative) CS value
            public UInt16 e_lfarlc;             // File address of relocation table
            public UInt16 e_ovno;               // Overlay number
            public UInt16 e_res_0;              // Reserved words
            public UInt16 e_res_1;              // Reserved words
            public UInt16 e_res_2;              // Reserved words
            public UInt16 e_res_3;              // Reserved words
            public UInt16 e_oemid;              // OEM identifier (for e_oeminfo)
            public UInt16 e_oeminfo;            // OEM information; e_oemid specific
            public UInt16 e_res2_0;             // Reserved words
            public UInt16 e_res2_1;             // Reserved words
            public UInt16 e_res2_2;             // Reserved words
            public UInt16 e_res2_3;             // Reserved words
            public UInt16 e_res2_4;             // Reserved words
            public UInt16 e_res2_5;             // Reserved words
            public UInt16 e_res2_6;             // Reserved words
            public UInt16 e_res2_7;             // Reserved words
            public UInt16 e_res2_8;             // Reserved words
            public UInt16 e_res2_9;             // Reserved words
            public UInt32 e_lfanew;             // File address of new exe header

            public void DumpToConsole ( ) {
                Console.WriteLine("---- IMAGE_DOS_HEADER ----");
                Console.WriteLine($"e_magic: {e_magic:X4}");
                Console.WriteLine($"e_cblp: {e_cblp}");
                Console.WriteLine($"e_cp: {e_cp}");
                Console.WriteLine($"e_crlc: {e_crlc}");
                Console.WriteLine($"e_cparhdr: {e_cparhdr}");
                Console.WriteLine($"e_minalloc: {e_minalloc:X}");
                Console.WriteLine($"e_maxalloc: {e_maxalloc:X}");
                Console.WriteLine($"e_sp: {e_sp:X}");
                Console.WriteLine($"e_lfarlc: {e_lfarlc:X}");
                Console.WriteLine($"e_lfanew: {e_lfanew:X}");
                Console.WriteLine("--------------------------");
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMAGE_FILE_HEADER {
            public UInt32 Signature;
            public UInt16 Machine;
            public UInt16 NumberOfSections;
            public UInt32 TimeDateStamp;
            public UInt32 PointerToSymbolTable;
            public UInt32 NumberOfSymbols;
            public UInt16 SizeOfOptionalHeader;
            public UInt16 Characteristics;

            public void DumpToConsole ( ) {
                Console.WriteLine("---- IMAGE_FILE_HEADER ----");
                Console.WriteLine($"Signature: {Signature:X8}");
                Console.WriteLine($"Machine: {Machine:X4}");
                Console.WriteLine($"NumberOfSections: {NumberOfSections}");
                Console.WriteLine($"TimeDateStamp: {TimeDateStamp}");
                Console.WriteLine($"PointerToSymbolTable: {PointerToSymbolTable:X8}");
                Console.WriteLine($"NumberOfSymbols: {NumberOfSymbols}");
                Console.WriteLine($"SizeOfOptionalHeader: {SizeOfOptionalHeader:X4}");
                Console.WriteLine($"Characteristics: {Characteristics:X4}");
                Console.WriteLine("--------------------------");
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMAGE_OPTIONAL_HEADER32 {
            public UInt16 Magic;
            public Byte MajorLinkerVersion;
            public Byte MinorLinkerVersion;
            public UInt32 SizeOfCode;
            public UInt32 SizeOfInitializedData;
            public UInt32 SizeOfUninitializedData;
            public UInt32 AddressOfEntryPoint;
            public UInt32 BaseOfCode;
            public UInt32 BaseOfData;
            public UInt32 ImageBase;
            public UInt32 SectionAlignment;
            public UInt32 FileAlignment;
            public UInt16 MajorOperatingSystemVersion;
            public UInt16 MinorOperatingSystemVersion;
            public UInt16 MajorImageVersion;
            public UInt16 MinorImageVersion;
            public UInt16 MajorSubsystemVersion;
            public UInt16 MinorSubsystemVersion;
            public UInt32 Win32VersionValue;
            public UInt32 SizeOfImage;
            public UInt32 SizeOfHeaders;
            public UInt32 CheckSum;
            public UInt16 Subsystem;
            public UInt16 DllCharacteristics;
            public UInt32 SizeOfStackReserve;
            public UInt32 SizeOfStackCommit;
            public UInt32 SizeOfHeapReserve;
            public UInt32 SizeOfHeapCommit;
            public UInt32 LoaderFlags;
            public UInt32 NumberOfRvaAndSizes;

            public IMAGE_DATA_DIRECTORY ExportTable;
            public IMAGE_DATA_DIRECTORY ImportTable;
            public IMAGE_DATA_DIRECTORY ResourceTable;
            public IMAGE_DATA_DIRECTORY ExceptionTable;
            public IMAGE_DATA_DIRECTORY CertificateTable;
            public IMAGE_DATA_DIRECTORY BaseRelocationTable;
            public IMAGE_DATA_DIRECTORY Debug;
            public IMAGE_DATA_DIRECTORY Architecture;
            public IMAGE_DATA_DIRECTORY GlobalPtr;
            public IMAGE_DATA_DIRECTORY TLSTable;
            public IMAGE_DATA_DIRECTORY LoadConfigTable;
            public IMAGE_DATA_DIRECTORY BoundImport;
            public IMAGE_DATA_DIRECTORY IAT;
            public IMAGE_DATA_DIRECTORY DelayImportDescriptor;
            public IMAGE_DATA_DIRECTORY CLRRuntimeHeader;
            public IMAGE_DATA_DIRECTORY Reserved;

            public void DumpToConsole ( ) {
                Console.WriteLine("---- IMAGE_OPTIONAL_HEADER32 ----");
                Console.WriteLine($"Magic: {Magic:X4}");
                Console.WriteLine($"MajorLinkerVersion: {MajorLinkerVersion}");
                Console.WriteLine($"MinorLinkerVersion: {MinorLinkerVersion}");
                Console.WriteLine($"SizeOfCode: {SizeOfCode:X}");
                Console.WriteLine($"SizeOfInitializedData: {SizeOfInitializedData:X}");
                Console.WriteLine($"SizeOfUninitializedData: {SizeOfUninitializedData:X}");
                Console.WriteLine($"AddressOfEntryPoint: {AddressOfEntryPoint:X}");
                Console.WriteLine($"BaseOfCode: {BaseOfCode:X}");
                Console.WriteLine($"BaseOfData: {BaseOfData:X}");
                Console.WriteLine($"ImageBase: {ImageBase:X}");
                Console.WriteLine($"SectionAlignment: {SectionAlignment:X}");
                Console.WriteLine($"FileAlignment: {FileAlignment:X}");
                Console.WriteLine($"MajorOperatingSystemVersion: {MajorOperatingSystemVersion}");
                Console.WriteLine($"MinorOperatingSystemVersion: {MinorOperatingSystemVersion}");
                Console.WriteLine($"MajorImageVersion: {MajorImageVersion}");
                Console.WriteLine($"MinorImageVersion: {MinorImageVersion}");
                Console.WriteLine($"MajorSubsystemVersion: {MajorSubsystemVersion}");
                Console.WriteLine($"MinorSubsystemVersion: {MinorSubsystemVersion}");
                Console.WriteLine($"Win32VersionValue: {Win32VersionValue:X}");
                Console.WriteLine($"SizeOfImage: {SizeOfImage:X}");
                Console.WriteLine($"SizeOfHeaders: {SizeOfHeaders:X}");
                Console.WriteLine($"CheckSum: {CheckSum:X}");
                Console.WriteLine($"Subsystem: {Subsystem:X}");
                Console.WriteLine($"DllCharacteristics: {DllCharacteristics:X}");
                Console.WriteLine($"SizeOfStackReserve: {SizeOfStackReserve:X}");
                Console.WriteLine($"SizeOfStackCommit: {SizeOfStackCommit:X}");
                Console.WriteLine($"SizeOfHeapReserve: {SizeOfHeapReserve:X}");
                Console.WriteLine($"SizeOfHeapCommit: {SizeOfHeapCommit:X}");
                Console.WriteLine($"LoaderFlags: {LoaderFlags:X}");
                Console.WriteLine($"NumberOfRvaAndSizes: {NumberOfRvaAndSizes:X}");
                Console.WriteLine("--------------------------");
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMAGE_OPTIONAL_HEADER64 {
            public UInt16 Magic;
            public Byte MajorLinkerVersion;
            public Byte MinorLinkerVersion;
            public UInt32 SizeOfCode;
            public UInt32 SizeOfInitializedData;
            public UInt32 SizeOfUninitializedData;
            public UInt32 AddressOfEntryPoint;
            public UInt32 BaseOfCode;
            public UInt64 ImageBase;
            public UInt32 SectionAlignment;
            public UInt32 FileAlignment;
            public UInt16 MajorOperatingSystemVersion;
            public UInt16 MinorOperatingSystemVersion;
            public UInt16 MajorImageVersion;
            public UInt16 MinorImageVersion;
            public UInt16 MajorSubsystemVersion;
            public UInt16 MinorSubsystemVersion;
            public UInt32 Win32VersionValue;
            public UInt32 SizeOfImage;
            public UInt32 SizeOfHeaders;
            public UInt32 CheckSum;
            public UInt16 Subsystem;
            public UInt16 DllCharacteristics;
            public UInt64 SizeOfStackReserve;
            public UInt64 SizeOfStackCommit;
            public UInt64 SizeOfHeapReserve;
            public UInt64 SizeOfHeapCommit;
            public UInt32 LoaderFlags;
            public UInt32 NumberOfRvaAndSizes;

            public IMAGE_DATA_DIRECTORY ExportTable;
            public IMAGE_DATA_DIRECTORY ImportTable;
            public IMAGE_DATA_DIRECTORY ResourceTable;
            public IMAGE_DATA_DIRECTORY ExceptionTable;
            public IMAGE_DATA_DIRECTORY CertificateTable;
            public IMAGE_DATA_DIRECTORY BaseRelocationTable;
            public IMAGE_DATA_DIRECTORY Debug;
            public IMAGE_DATA_DIRECTORY Architecture;
            public IMAGE_DATA_DIRECTORY GlobalPtr;
            public IMAGE_DATA_DIRECTORY TLSTable;
            public IMAGE_DATA_DIRECTORY LoadConfigTable;
            public IMAGE_DATA_DIRECTORY BoundImport;
            public IMAGE_DATA_DIRECTORY IAT;
            public IMAGE_DATA_DIRECTORY DelayImportDescriptor;
            public IMAGE_DATA_DIRECTORY CLRRuntimeHeader;
            public IMAGE_DATA_DIRECTORY Reserved;

            public void DumpToConsole ( ) {
                Console.WriteLine("---- IMAGE_OPTIONAL_HEADER64 ----");
                Console.WriteLine($"Magic: {Magic:X4}");
                Console.WriteLine($"MajorLinkerVersion: {MajorLinkerVersion}");
                Console.WriteLine($"MinorLinkerVersion: {MinorLinkerVersion}");
                Console.WriteLine($"SizeOfCode: {SizeOfCode:X}");
                Console.WriteLine($"SizeOfInitializedData: {SizeOfInitializedData:X}");
                Console.WriteLine($"SizeOfUninitializedData: {SizeOfUninitializedData:X}");
                Console.WriteLine($"AddressOfEntryPoint: {AddressOfEntryPoint:X}");
                Console.WriteLine($"BaseOfCode: {BaseOfCode:X}");
                Console.WriteLine($"ImageBase: {ImageBase:X}");
                Console.WriteLine($"SectionAlignment: {SectionAlignment:X}");
                Console.WriteLine($"FileAlignment: {FileAlignment:X}");
                Console.WriteLine($"MajorOperatingSystemVersion: {MajorOperatingSystemVersion}");
                Console.WriteLine($"MinorOperatingSystemVersion: {MinorOperatingSystemVersion}");
                Console.WriteLine($"MajorImageVersion: {MajorImageVersion}");
                Console.WriteLine($"MinorImageVersion: {MinorImageVersion}");
                Console.WriteLine($"MajorSubsystemVersion: {MajorSubsystemVersion}");
                Console.WriteLine($"MinorSubsystemVersion: {MinorSubsystemVersion}");
                Console.WriteLine($"SizeOfImage: {SizeOfImage:X}");
                Console.WriteLine($"SizeOfHeaders: {SizeOfHeaders:X}");
                Console.WriteLine($"CheckSum: {CheckSum:X}");
                Console.WriteLine($"Subsystem: {Subsystem:X}");
                Console.WriteLine($"DllCharacteristics: {DllCharacteristics:X}");
                Console.WriteLine($"SizeOfStackReserve: {SizeOfStackReserve:X}");
                Console.WriteLine($"SizeOfStackCommit: {SizeOfStackCommit:X}");
                Console.WriteLine($"SizeOfHeapReserve: {SizeOfHeapReserve:X}");
                Console.WriteLine($"SizeOfHeapCommit: {SizeOfHeapCommit:X}");
                Console.WriteLine($"NumberOfRvaAndSizes: {NumberOfRvaAndSizes:X}");
                Console.WriteLine("--------------------------");
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct IMAGE_SECTION_HEADER {
            [FieldOffset(0)]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public char[] Name;
            [FieldOffset(8)]
            public UInt32 VirtualSize;
            [FieldOffset(12)]
            public UInt32 VirtualAddress;
            [FieldOffset(16)]
            public UInt32 SizeOfRawData;
            [FieldOffset(20)]
            public UInt32 PointerToRawData;
            [FieldOffset(24)]
            public UInt32 PointerToRelocations;
            [FieldOffset(28)]
            public UInt32 PointerToLinenumbers;
            [FieldOffset(32)]
            public UInt16 NumberOfRelocations;
            [FieldOffset(34)]
            public UInt16 NumberOfLinenumbers;
            [FieldOffset(36)]
            public DataSectionFlags Characteristics;

            public String Section {
                get { return new String(Name); }
            }

            public void DumpToConsole ( ) {
                Console.WriteLine("---- IMAGE_SECTION_HEADER ----");
                Console.WriteLine($"Name: {Section}");
                Console.WriteLine($"VirtualSize: {VirtualSize:X}");
                Console.WriteLine($"VirtualAddress: {VirtualAddress:X}");
                Console.WriteLine($"SizeOfRawData: {SizeOfRawData:X}");
                Console.WriteLine($"PointerToRawData: {PointerToRawData:X}");
                Console.WriteLine($"PointerToRelocations: {PointerToRelocations:X}");
                Console.WriteLine($"PointerToLinenumbers: {PointerToLinenumbers:X}");
                Console.WriteLine($"NumberOfRelocations: {NumberOfRelocations}");
                Console.WriteLine($"NumberOfLinenumbers: {NumberOfLinenumbers}");
                Console.WriteLine($"Characteristics: {Characteristics:X}");
                Console.WriteLine("--------------------------");
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct IMAGE_DATA_DIRECTORY {
            public UInt32 VirtualAddress;
            public UInt32 Size;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct IMAGE_EXPORT_DIRECTORY_TABLE {
            public UInt32 ExportFlags;
            public UInt32 TimeDateStamp;
            public UInt16 MajorVersion;
            public UInt16 MinorVersion;
            public UInt32 NameRva;
            public UInt32 OrdinalBase;
            public UInt32 AddressTableEntries;
            public UInt32 NumberOfNamePointers;
            public UInt32 ExportAddressTableRva;
            public UInt32 NamePointerRva;
            public UInt32 OrdinalTableRva;
        }

        [Flags]
        public enum DataSectionFlags : uint {
            TypeReg = 0x00000000,
            TypeDsect = 0x00000001,
            TypeNoLoad = 0x00000002,
            TypeGroup = 0x00000004,
            TypeNoPadded = 0x00000008,
            TypeCopy = 0x00000010,
            ContentCode = 0x00000020,
            ContentInitializedData = 0x00000040,
            ContentUninitializedData = 0x00000080,
            LinkOther = 0x00000100,
            LinkInfo = 0x00000200,
            TypeOver = 0x00000400,
            LinkRemove = 0x00000800,
            LinkComDat = 0x00001000,
            NoDeferSpecExceptions = 0x00004000,
            RelativeGP = 0x00008000,
            MemPurgeable = 0x00020000,
            Memory16Bit = 0x00020000,
            MemoryLocked = 0x00040000,
            MemoryPreload = 0x00080000,
            Align1Bytes = 0x00100000,
            Align2Bytes = 0x00200000,
            Align4Bytes = 0x00300000,
            Align8Bytes = 0x00400000,
            Align16Bytes = 0x00500000,
            Align32Bytes = 0x00600000,
            Align64Bytes = 0x00700000,
            Align128Bytes = 0x00800000,
            Align256Bytes = 0x00900000,
            Align512Bytes = 0x00A00000,
            Align1024Bytes = 0x00B00000,
            Align2048Bytes = 0x00C00000,
            Align4096Bytes = 0x00D00000,
            Align8192Bytes = 0x00E00000,
            LinkExtendedRelocationOverflow = 0x01000000,
            MemoryDiscardable = 0x02000000,
            MemoryNotCached = 0x04000000,
            MemoryNotPaged = 0x08000000,
            MemoryShared = 0x10000000,
            MemoryExecute = 0x20000000,
            MemoryRead = 0x40000000,
            MemoryWrite = 0x80000000
        }
        #endregion

        #endregion
        #region Imports
        [DllImport("user32.dll")]
        public static extern Boolean SetForegroundWindow( IntPtr hWnd );
        [DllImport("user32.dll")]
        public static extern Int32 SetWindowLong( IntPtr hWnd, Int32 nIndex, Int32 dwNewLong );
        [DllImport("user32.dll", SetLastError = true)]
        public static extern Int32 GetWindowLong( IntPtr hWnd, Int32 nIndex );
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);


        [DllImport("kernel32.dll")]
        public static extern Boolean ReadProcessMemory( IntPtr hProcess, IntPtr lpBaseAddress, Byte[] lpBuffer, Int32 dwSize, ref Int32 lpNumberOfBytesRead );
        [DllImport("kernel32.dll")]
        public static extern Boolean WriteProcessMemory( IntPtr hProcess, IntPtr lpBaseAddress, Byte[] lpBuffer, Int32 dwSize, ref Int32 lpNumberOfBytesWritten );
        [DllImport("kernel32.dll")]
        public static extern Int64 VirtualQueryEx( IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION64 lpBuffer, UInt32 dwLength );
        [DllImport("psapi.dll")]
        public static extern Boolean EnumProcessModulesEx( IntPtr hProcess, [Out] IntPtr[] lphModule, Int32 cb, ref Int32 lpcbNeeded, UInt32 dwFilterFlag );
        [DllImport("psapi.dll")]
        public static extern Int32 GetModuleFileNameEx( IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName, Int32 nSize );
        #endregion
        #region DLLs

        public void ReadPEHeaders (
            UInt64 imageBase,
            out IMAGE_DOS_HEADER dosHeader,
            out IMAGE_FILE_HEADER fileHeader,
            out IMAGE_OPTIONAL_HEADER32 optionalHeader32,
            out IMAGE_OPTIONAL_HEADER64 optionalHeader64,
            out IMAGE_SECTION_HEADER[] imageSectionHeaders
        ) {
            dosHeader = ReadAbsolute<IMAGE_DOS_HEADER>( imageBase );
            fileHeader = ReadAbsolute<IMAGE_FILE_HEADER>( imageBase + dosHeader.e_lfanew );
            
            optionalHeader32 = ReadAbsolute<IMAGE_OPTIONAL_HEADER32>( imageBase + dosHeader.e_lfanew + 0x18 );
            optionalHeader64 = ReadAbsolute<IMAGE_OPTIONAL_HEADER64>( imageBase + dosHeader.e_lfanew + 0x18 );
            
            UInt16 optionalHeaderMagic = ReadAbsolute<UInt16>( imageBase + dosHeader.e_lfanew + 0x18 );
            UInt64 imageSectionHeadersAddress = 0;
            if ( optionalHeaderMagic == 0x10B ) {
                imageSectionHeadersAddress = imageBase + dosHeader.e_lfanew + 0xF8;
            }
            if ( optionalHeaderMagic == 0x20B ) {
                imageSectionHeadersAddress = imageBase + dosHeader.e_lfanew + 0x108;
            }
            
            imageSectionHeaders = new IMAGE_SECTION_HEADER[fileHeader.NumberOfSections];
            for ( UInt32 i = 0; i < fileHeader.NumberOfSections; i++ ) {
                imageSectionHeaders[i] = ReadAbsolute<IMAGE_SECTION_HEADER>( imageSectionHeadersAddress + ( 0x28 * i ) );
            }
        }

        public Dictionary<String, UInt64> GetModules64 ( ) {
            Dictionary<String, UInt64> modules = new Dictionary<String, UInt64>();
            IntPtr[] hModules = new IntPtr[1024];
            Int32 needed = 0;
            EnumProcessModulesEx(process.Handle, hModules, 1024, ref needed, 0x3);
            for ( Int32 i = 0; i < needed / IntPtr.Size; i++ ) {
                StringBuilder sb = new StringBuilder(1024);
                GetModuleFileNameEx(process.Handle, hModules[i], sb, 1024);
                if ( modules.ContainsKey(sb.ToString()) ) {
                    continue;
                }
                modules.Add(sb.ToString(), (UInt64) hModules[i].ToInt64());
            }

            return modules;
        }
        public void DumpModules64 ( ) {
            Dictionary<String, UInt64> modules = GetModules64();
            foreach ( var (moduleName, moduleBase) in modules ) {
                Console.WriteLine($"{moduleName} @ {moduleBase:X}");
            }
        }
        public UInt64 GetModule ( params String[] moduleNames ) {
            Dictionary<String, UInt64> modules = GetModules64();
            foreach ( KeyValuePair<String, UInt64> module in modules )
                foreach ( String moduleName in moduleNames )
                    if ( module.Key.Contains(moduleName) )
                        return module.Value;
            return 0;
        }
        public void DumpModule ( UInt64 moduleBase ) {
            ReadPEHeaders(moduleBase, out IMAGE_DOS_HEADER dosHeader, out IMAGE_FILE_HEADER fileHeader, out IMAGE_OPTIONAL_HEADER32 optionalHeader32, out IMAGE_OPTIONAL_HEADER64 optionalHeader64, out IMAGE_SECTION_HEADER[] sectionHeaders);
            dosHeader.DumpToConsole();
            fileHeader.DumpToConsole();
            optionalHeader32.DumpToConsole();
            optionalHeader64.DumpToConsole();
            foreach ( IMAGE_SECTION_HEADER sectionHeader in sectionHeaders ) {
                sectionHeader.DumpToConsole();
            }
        }

        public Dictionary<String, UInt64> GetExportedFunctions ( UInt64 ImageBase ) {
            ReadPEHeaders(ImageBase, out IMAGE_DOS_HEADER dosHeader, out IMAGE_FILE_HEADER fileHeader, out IMAGE_OPTIONAL_HEADER32 optionalHeader32, out IMAGE_OPTIONAL_HEADER64 optionalHeader64, out IMAGE_SECTION_HEADER[] sectionHeaders);

            IMAGE_EXPORT_DIRECTORY_TABLE ExportTable = ReadAbsolute<IMAGE_EXPORT_DIRECTORY_TABLE>(ImageBase + optionalHeader64.ExportTable.VirtualAddress);

            Dictionary<String, UInt64> exportedFunctions = new Dictionary<String, UInt64>();
            for ( UInt32 i = 0; i < ExportTable.AddressTableEntries; ++i ) {
                UInt32 FunctionNameOffset = ReadAbsolute<UInt32>(ImageBase + ExportTable.NamePointerRva + i * 4);
                String FunctionName = ReadAbsoluteUTF8String(ImageBase + FunctionNameOffset);
                UInt32 FunctionOffset = ReadAbsolute<UInt32>(ImageBase + ExportTable.ExportAddressTableRva + i * 4);
                exportedFunctions.Add(FunctionName, ImageBase + FunctionOffset);
            }

            return exportedFunctions;
        }
        public void DumpExportedFunctions ( UInt64 imageBase ) {
            Dictionary<String, UInt64> exportedFunctions = GetExportedFunctions(imageBase);
            foreach ( var (exportedFunctionName, exportedFunctionAddress) in exportedFunctions ) {
                Console.WriteLine($"{exportedFunctionName} = {exportedFunctionAddress:X}");
            }
        }
        #endregion
        #region Memory Scanning
        public List<UInt64> FindPatternAddresses64( UInt64 start, UInt64 end, BytePattern pattern ) {
            Console.WriteLine("Scanning memory for pattern: " + pattern.PatternString());
            List<UInt64> matchAddresses = new List<UInt64>();

            UInt64 currentAddress = start;

            List<Byte[]> byteArrays = new List<Byte[]>();

            while ( currentAddress < end ) {
                Int64 bytesRead = VirtualQueryEx(process.Handle, (IntPtr) currentAddress, out MEMORY_BASIC_INFORMATION64 memoryRegion, (UInt32) Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION64)));

                if ( (memoryRegion.Protect & 0x001) > 0 // PAGE_NOACCESS
                  || (memoryRegion.Protect & 0x100) > 0 // PAGE_GUARD
                  || memoryRegion.State != 0x1000 ) { // MEM_COMMIT
                    currentAddress = memoryRegion.BaseAddress + memoryRegion.RegionSize;
                    continue;
                }

                var regionStartAddress = memoryRegion.BaseAddress;
                if ( start > regionStartAddress )
                    regionStartAddress = start;

                var regionEndAddress = memoryRegion.BaseAddress + memoryRegion.RegionSize;
                if ( end < regionEndAddress )
                    regionEndAddress = end;

                UInt64 regionBytesToRead = regionEndAddress - regionStartAddress;
                Byte[] regionBytes = new Byte[regionBytesToRead];

                Int32 lpNumberOfBytesRead = 0;
                ReadProcessMemory(process.Handle, (IntPtr) regionStartAddress, regionBytes, regionBytes.Length, ref lpNumberOfBytesRead);

                byteArrays.Add(regionBytes);

                if ( regionBytes.Length == 0 || pattern.Bytes.Length == 0 || regionBytes.Length < pattern.Bytes.Length )
                    continue;

                List<Int32> matchedIndices = new List<Int32>();
                Int32[] longestPrefixSuffices = new Int32[pattern.Bytes.Length];


                Int32 length = 0;
                Int32 patternIndex = 1;
                longestPrefixSuffices[0] = 0;
                while ( patternIndex < pattern.Bytes.Length ) {
                    if ( pattern.Bytes[patternIndex] == pattern.Bytes[length] ) {
                        length++;
                        longestPrefixSuffices[patternIndex] = length;
                        patternIndex++;
                    } else {
                        if ( length == 0 ) {
                            longestPrefixSuffices[patternIndex] = 0;
                            patternIndex++;
                        } else
                            length = longestPrefixSuffices[length - 1];
                    }
                }

                Int32 textIndex = 0;
                patternIndex = 0;

                while ( textIndex < regionBytes.Length ) {
                    if ( !pattern.Bytes[patternIndex].HasValue
                        || regionBytes[textIndex] == pattern.Bytes[patternIndex] ) {
                        textIndex++;
                        patternIndex++;
                    }

                    if ( patternIndex == pattern.Bytes.Length ) {
                        matchedIndices.Add(textIndex - patternIndex);
                        patternIndex = longestPrefixSuffices[patternIndex - 1];
                    } else if ( textIndex < regionBytes.Length
                            && (pattern.Bytes[patternIndex].HasValue
                            && regionBytes[textIndex] != pattern.Bytes[patternIndex]) ) {
                        if ( patternIndex != 0 )
                            patternIndex = longestPrefixSuffices[patternIndex - 1];
                        else
                            textIndex++;
                    }
                }

                foreach ( var matchIndex in matchedIndices ) {
                    matchAddresses.Add(regionStartAddress + (UInt64) matchIndex);
                }

                currentAddress = memoryRegion.BaseAddress + memoryRegion.RegionSize;
            }

            return matchAddresses;
        }
        #endregion
        #region Memory Manipulation
        public T ReadRelative<T> ( UInt64 offset, params Int64[] offsets ) where T : struct {
            return ReadAbsolute<T> ( BaseAddress + offset, offsets );
        }
        public T ReadAbsolute<T> ( UInt64 address, params Int64[] offsets ) where T : struct {
            Byte[] Bytes = new Byte[Marshal.SizeOf(typeof(T))];

            // Console.WriteLine($"Reading {typeof(T).Name} at {address:X}, {string.Join(", ", offsets)}");
            foreach ( Int64 offset in offsets )
                address = ReadAbsolute<UInt64>(address) + (UInt64) offset;

            Int32 lpNumberOfBytesRead = 0;
            ReadProcessMemory(process.Handle, (IntPtr) address, Bytes, Bytes.Length, ref lpNumberOfBytesRead);

            T result;
            GCHandle handle = GCHandle.Alloc(Bytes, GCHandleType.Pinned);

            try {
                result = (T) Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            } finally {
                handle.Free();
            }

            return result;
        }

        public String ReadRelativeUTF8String ( UInt64 address, params Int64[] offsets ) {
            return ReadAbsoluteUTF8String ( (UInt64) BaseAddress + address, offsets );
        }
        public String ReadAbsoluteUTF8String ( UInt64 address, params Int64[] offsets ) {
            List<Byte> bytes = new List<Byte>();

            foreach ( Int64 offset in offsets )
                address = ReadAbsolute<UInt64>(address) + (UInt64) offset;

            Byte b;
            while ( (b = ReadAbsolute<Byte>(address++)) != 0 )
                bytes.Add(b);

            return Encoding.UTF8.GetString(bytes.ToArray());
        }
        public String ReadString ( UInt64 address, UInt32 length ) {
            Byte[] Bytes = new Byte[length];

            Int32 lpNumberOfBytesRead = 0;
            ReadProcessMemory(process.Handle, (IntPtr) address, Bytes, Bytes.Length, ref lpNumberOfBytesRead);

            Int32 nullTerminatorIndex = Array.FindIndex(Bytes, ( Byte b ) => b == 0);
            if ( nullTerminatorIndex >= 0 ) {
                Array.Resize(ref Bytes, nullTerminatorIndex);
                return Encoding.UTF8.GetString(Bytes);
            }

            return null;
        }
        public void WriteRelative<T> ( UInt64 offset, T value, params Int64[] offsets ) where T : struct {
            WriteAbsolute<T> ( BaseAddress + offset, value, offsets );
        }
        public void WriteAbsolute<T> ( UInt64 address, T value, params Int64[] offsets ) where T : struct {
            Int32 size = Marshal.SizeOf(value);
            Byte[] Bytes = new Byte[size];

            // Console.WriteLine($"Writing ({typeof(T).Name}) {value} to {address:X}, {string.Join(", ", offsets)}");
            foreach ( Int64 offset in offsets )
                address = ReadAbsolute<UInt64>(address) + (UInt64) offset;

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(value, ptr, true);
            Marshal.Copy(ptr, Bytes, 0, size);
            Marshal.FreeHGlobal(ptr);

            Int32 lpNumberOfBytesWritten = 0;
            WriteProcessMemory(process.Handle, (IntPtr) address, Bytes, Bytes.Length, ref lpNumberOfBytesWritten);
        }
        #endregion
        #region Disassembly
        
        #endregion
    }
}
