using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

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
