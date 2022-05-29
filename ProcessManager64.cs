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
            try {
                process = Process.GetProcessesByName(ProcessName).First();
                return true;
            } catch ( Exception ) {
                return false;
            }
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

        public UInt64 GetModule ( params String[] moduleNames ) {
            Dictionary<String, UInt64> modules = GetModules64();
            foreach ( KeyValuePair<String, UInt64> module in modules )
                foreach ( String moduleName in moduleNames )
                    if ( module.Key.Contains(moduleName) )
                        return module.Value;
            return 0;
        }

        public Dictionary<String, UInt64> GetExportedFunctions(UInt64 ImageBase) {
            bool PE32 = false;
            bool PE32plus = false;
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
                return null;
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

            Dictionary<String, UInt64> exportedFunctions = new Dictionary<String, UInt64>();
            for ( UInt32 i = 0; i < ExportTableAddressTableEntries; ++i ) {
                UInt32 FunctionNameOffset = ReadAbsolute<UInt32>(ImageBase + ExportTableNamePointerRva + i * 4);
                String FunctionName = ReadAbsoluteUTF8String(ImageBase + FunctionNameOffset);
                UInt32 FunctionOffset = ReadAbsolute<UInt32>(ImageBase + ExportTableExportAddressTableRva + i * 4);
                exportedFunctions.Add(FunctionName, ImageBase + FunctionOffset);
            }

            return exportedFunctions;
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
    }
}
