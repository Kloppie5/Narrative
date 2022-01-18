using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Narrative {
    public class BytePattern {
        public String String { get; private set; }
        public Byte?[] Bytes { get; private set; }

        public BytePattern ( String ByteString ) {
            String = ByteString;

            List<Byte?> ByteList = new List<Byte?>();
            var singleByteStrings = ByteString.Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            foreach ( var singleByteString in singleByteStrings )
                ByteList.Add(
                    Byte.TryParse(singleByteString, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out Byte parsedByte)
                    ? (Byte?) parsedByte
                    : null
                );

            Bytes = ByteList.ToArray();
        }
    }

    public class ProcessManager {
        Process process;
        public UInt64 BaseAddress => (UInt64) process.MainModule.BaseAddress;

        public ProcessManager ( String processName ) {
            process = Process.GetProcessesByName(processName).First();
            Console.WriteLine($"Initialized MemoryReader for process {process.Id} ({processName})");
        }

        public struct MEMORY_BASIC_INFORMATION32 {
            public UInt32 BaseAddress;
            public UInt32 AllocationBase;
            public UInt32 AllocationProtect;
            public UInt32 RegionSize;
            public UInt32 State;
            public UInt32 Protect;
            public UInt32 Type;
        }
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

        [DllImport("user32.dll")]
        public static extern Boolean SetForegroundWindow( IntPtr hWnd );
        [DllImport("user32.dll")]
        public static extern Int32 SetWindowLong( IntPtr hWnd, Int32 nIndex, Int32 dwNewLong );
        [DllImport("user32.dll", SetLastError = true)]
        public static extern Int32 GetWindowLong( IntPtr hWnd, Int32 nIndex );

        [DllImport("kernel32.dll")]
        public static extern Boolean ReadProcessMemory( IntPtr hProcess, IntPtr lpBaseAddress, Byte[] lpBuffer, Int32 dwSize, ref Int32 lpNumberOfBytesRead );
        [DllImport("kernel32.dll")]
        public static extern Boolean WriteProcessMemory( IntPtr hProcess, IntPtr lpBaseAddress, Byte[] lpBuffer, Int32 dwSize, ref Int32 lpNumberOfBytesWritten );
        [DllImport("kernel32.dll")]
        public static extern Int32 VirtualQueryEx32( IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION32 lpBuffer, UInt32 dwLength );
        [DllImport("kernel32.dll")]
        public static extern Int32 VirtualQueryEx64( IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION64 lpBuffer, UInt32 dwLength );
        [DllImport("psapi.dll")]
        public static extern Boolean EnumProcessModulesEx( IntPtr hProcess, [Out] IntPtr[] lphModule, Int32 cb, ref Int32 lpcbNeeded, UInt32 dwFilterFlag );
        [DllImport("psapi.dll")]
        public static extern Int32 GetModuleFileNameEx( IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName, Int32 nSize );

        public UInt32 GetModuleBaseAddress ( String name ) {
            IntPtr[] modules = new IntPtr[1024];
            Int32 needed = 0;
            EnumProcessModulesEx(process.Handle, modules, 1024, ref needed, 0x3);
            for ( Int32 i = 0; i < needed / IntPtr.Size; i++ ) {
                StringBuilder sb = new StringBuilder(1024);
                GetModuleFileNameEx(process.Handle, modules[i], sb, 1024);
                if ( sb.ToString().Contains(name) )
                    return (UInt32) modules[i];
            }

            return 0;
        }

        public UInt32 GetVTableOfClassInClassCache ( UInt32 image, String name ) {
            UInt32 class_cache_size = ReadAbsolute<UInt32>(image + 0x360);
            UInt32 class_cache_table = ReadAbsolute<UInt32>(image + 0x368);
            for ( UInt32 i = 0 ; i < class_cache_size ; ++i ) {
                UInt32 pointer = ReadAbsolute<UInt32>(class_cache_table + i * 4);
                if ( pointer == 0 )
                    continue;

                UInt32 klass = ReadAbsolute<UInt32>(pointer);
                UInt32 classnameAddress = ReadAbsolute<UInt32>(klass + 0x2C);
                String classname = ReadAbsoluteUTF8String(classnameAddress);

                if ( !classname.Equals(name) )
                    continue;

                UInt32 MonoClassRuntimeInfo = ReadAbsolute<UInt32>(klass + 0x84);
                UInt32 MonoVTable = ReadAbsolute<UInt32>(MonoClassRuntimeInfo + 0x04);
                return MonoVTable;
                // pointer = Read<Int32>(pointer + 0xA8);
            }
            return 0;
        }

        public List<UInt32> EnumImageClassCache ( UInt32 image ) {
            List<UInt32> entries = new List<UInt32>();

            UInt32 class_cache_size = ReadAbsolute<UInt32>(image + 0x360);
            UInt32 class_cache_table = ReadAbsolute<UInt32>(image + 0x368);
            for ( UInt32 i = 0 ; i < class_cache_size ; ++i ) {
                UInt32 pointer = ReadAbsolute<UInt32>(class_cache_table + i * 4);
                if ( pointer != 0 ) {
                    UInt32 klass = ReadAbsolute<UInt32>(pointer);
                    UInt32 nameAddress = ReadAbsolute<UInt32>(klass + 0x2C);
                    String name = ReadAbsoluteUTF8String(nameAddress);
                    UInt32 name_spaceAddress = ReadAbsolute<UInt32>(klass + 0x30);
                    String name_space = ReadAbsoluteUTF8String(name_spaceAddress);
                    UInt32 type_token = ReadAbsolute<UInt32>(klass + 0x34);
                    // Print all fields
                    UInt32 fields = ReadAbsolute<UInt32>(klass + 0x60);
                    UInt32 num_fields = ReadAbsolute<UInt32>(klass + 0xA4);
                    for ( UInt32 j = 0 ; j < num_fields ; ++j ) {
                        UInt32 field_type = ReadAbsolute<UInt32>(fields + j * 16);
                        UInt32 field_nameAddress = ReadAbsolute<UInt32>(fields + j * 16 + 0x4);
                        String field_name = ReadAbsoluteUTF8String(field_nameAddress);
                        UInt32 field_parent = ReadAbsolute<UInt32>(fields + j * 16 + 0x8);
                        UInt32 field_offset = ReadAbsolute<UInt32>(fields + j * 16 + 0xC);
                        Console.WriteLine($"  {field_name} {field_type} {field_parent} {field_offset}");
                    }

                    UInt32 MonoMethodArray = ReadAbsolute<UInt32>(klass + 0x64);
                    UInt32 MonoClassRuntimeInfo = ReadAbsolute<UInt32>(klass + 0x84);
                    UInt32 MonoVTable = ReadAbsolute<UInt32>(MonoClassRuntimeInfo + 0x04);
                    Console.WriteLine($"Found class {type_token:X8}:\"{name_space}.{name}\" ({klass:X8}) with Vtable at {MonoVTable:X8}");
                    entries.Add(klass);
                }
            }
            return entries;
        }

        public UInt32 GetUnityRootDomain() {
            UInt32 MonoBaseAddress = GetModuleBaseAddress("mono-2.0-bdwgc.dll");
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
            }

            // TODO: split into two functions

            UInt32 GetRootDomainOffset = exportedFunctions["mono_get_root_domain"];
            // mono-2.0-bdwgc.mono_get_root_domain - A1 AC41067A - mov eax, {RootDomain}
            UInt32 RootDomainPointer = ReadAbsolute<UInt32>(MonoBaseAddress + GetRootDomainOffset + 0x1);
            return ReadAbsolute<UInt32>(RootDomainPointer);
        }

        public UInt32 GetAssemblyInDomain ( UInt32 domain, String name ) {
            UInt32 it = ReadAbsolute<UInt32>(domain + 0x6C);
            while ( it != 0 ) {
                UInt32 assembly = ReadAbsolute<UInt32>(it);
                UInt32 assemblyNameAddress = ReadAbsolute<UInt32>(assembly + 0x08);
                String assemblyName = ReadAbsoluteUTF8String(assemblyNameAddress);
                if ( assemblyName.Equals(name) )
                    return assembly;

                it = ReadAbsolute<UInt32>(it + 0x4);
            }
            return 0;
        }

        public List<UInt64> FindPatternAddresses64( UInt64 start, UInt64 end, BytePattern pattern ) {
            List<UInt64> matchAddresses = new List<UInt64>();

            UInt64 currentAddress = start;

            List<Byte[]> byteArrays = new List<Byte[]>();

            while ( currentAddress < end ) {
                if ( VirtualQueryEx64(process.Handle, (IntPtr) currentAddress, out MEMORY_BASIC_INFORMATION64 memoryRegion, (UInt32) Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION64))) > 0
                    && memoryRegion.RegionSize > 0
                    && memoryRegion.State == 0x1000 // MEM_COMMIT
                    && (memoryRegion.Protect & 0x20) > 0 ) { // PAGE_EXECUTE_READ

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
                }
                currentAddress = memoryRegion.BaseAddress + memoryRegion.RegionSize;
            }

            return matchAddresses;
        }
        public List<UInt32> FindPatternAddresses32( UInt32 start, UInt32 end, BytePattern pattern ) {
            List<UInt32> matchAddresses = new List<UInt32>();

            UInt32 currentAddress = start;

            List<Byte[]> byteArrays = new List<Byte[]>();

            while ( currentAddress < end ) {
                if ( VirtualQueryEx32(process.Handle, (IntPtr) currentAddress, out MEMORY_BASIC_INFORMATION32 memoryRegion, (UInt32) Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION32))) > 0
                    && memoryRegion.RegionSize > 0
                    && memoryRegion.State == 0x1000 // MEM_COMMIT
                    && (memoryRegion.Protect & 0x20) > 0 ) { // PAGE_EXECUTE_READ

                    var regionStartAddress = memoryRegion.BaseAddress;
                    if ( start > regionStartAddress )
                        regionStartAddress = start;

                    var regionEndAddress = memoryRegion.BaseAddress + memoryRegion.RegionSize;
                    if ( end < regionEndAddress )
                        regionEndAddress = end;

                    UInt32 regionBytesToRead = regionEndAddress - regionStartAddress;
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
                        matchAddresses.Add(regionStartAddress + (UInt32) matchIndex);
                    }
                }
                currentAddress = memoryRegion.BaseAddress + memoryRegion.RegionSize;
            }

            return matchAddresses;
        }
        public T ReadRelative<T> ( UInt64 offset, params Int64[] offsets ) where T : struct {
            return ReadAbsolute<T> ( BaseAddress + offset, offsets );
        }
        public T ReadAbsolute<T> ( UInt64 address, params Int64[] offsets ) where T : struct {
            Byte[] Bytes = new Byte[Marshal.SizeOf(typeof(T))];

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
        public String ReadAbsoluteUTF8String ( UInt64 address ) {
            List<Byte> bytes = new List<Byte>();

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
        public void Write<T> ( UInt64 address, T t ) where T : struct {
            Int32 size = Marshal.SizeOf(t);
            Byte[] Bytes = new Byte[size];

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(t, ptr, true);
            Marshal.Copy(ptr, Bytes, 0, size);
            Marshal.FreeHGlobal(ptr);

            Int32 lpNumberOfBytesWritten = 0;
            WriteProcessMemory(process.Handle, (IntPtr) address, Bytes, Bytes.Length, ref lpNumberOfBytesWritten);

        }
    }
}
