using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Narrative {
  public static class MemoryHelper {
    public static Boolean debug = false;

    [DllImport("kernel32.dll")]
    public static extern Boolean ReadProcessMemory( IntPtr hProcess, IntPtr lpBaseAddress, Byte[] lpBuffer, Int32 dwSize, ref Int32 lpNumberOfBytesRead );
    [DllImport("kernel32.dll")]
    public static extern Boolean WriteProcessMemory( IntPtr hProcess, IntPtr lpBaseAddress, Byte[] lpBuffer, Int32 dwSize, ref Int32 lpNumberOfBytesWritten );
    [DllImport("kernel32.dll")]
    public static extern Int64 VirtualQueryEx( IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION64 lpBuffer, Int32 dwLength );

    public static Boolean Match( ProcessManager64 manager, Int64 address, String pattern ) {
      BytePattern bp = new BytePattern(pattern);
      Byte[] buffer = new Byte[bp.Length];
      Int32 bytesRead = 0;
      ReadProcessMemory( manager.process.Handle, (IntPtr) address, buffer, buffer.Length, ref bytesRead );
      if ( debug )
        Console.WriteLine($"Match @ {address:X}: {BitConverter.ToString(buffer).Replace("-", " ")} ?= {pattern}");
      return bp.Match(buffer);
    }

    /*
    public List<Int64> FindPatternAddresses ( Int64 start, Int64 end, BytePattern pattern ) {
      Console.WriteLine("Scanning memory for pattern: " + pattern.PatternString());
      List<Int64> matchAddresses = new List<Int64>();

      Int64 currentAddress = start;

      List<Byte[]> byteArrays = new List<Byte[]>();

      while ( currentAddress < end ) {
        Int64 bytesRead = VirtualQueryEx(process.Handle, (IntPtr) currentAddress, out MEMORY_BASIC_INFORMATION64 memoryRegion, (Int32) Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION64)));

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

                Int64 regionBytesToRead = regionEndAddress - regionStartAddress;
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
                    matchAddresses.Add(regionStartAddress + (Int64) matchIndex);
                }

                currentAddress = memoryRegion.BaseAddress + memoryRegion.RegionSize;
            }

            return matchAddresses;
        }
        */
    public static T ReadRelative<T> ( ProcessManager64 manager, Int64 offset, params Int64[] offsets ) where T : struct {
      return ReadAbsolute<T> ( manager, manager.BaseAddress + offset, offsets );
    }
    public static T ReadAbsolute<T> ( ProcessManager64 manager, Int64 address, params Int64[] offsets ) where T : struct {
      Byte[] Bytes = new Byte[Marshal.SizeOf(typeof(T))];

      // Console.WriteLine($"Reading {typeof(T).Name} at {address:X}, {string.Join(", ", offsets)}");
      foreach ( Int64 offset in offsets )
        address = ReadAbsolute<Int64>(manager, address) + (Int64) offset;

      Int32 lpNumberOfBytesRead = 0;
      ReadProcessMemory(manager.process.Handle, (IntPtr) address, Bytes, Bytes.Length, ref lpNumberOfBytesRead);

      T result;
      GCHandle handle = GCHandle.Alloc(Bytes, GCHandleType.Pinned);

      try {
        result = (T) Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
      } finally {
        handle.Free();
      }

      return result;
    }

    public static String ReadRelativeUTF8String ( ProcessManager64 manager, Int64 address, params Int64[] offsets ) {
      return ReadAbsoluteUTF8String ( manager, manager.BaseAddress + address, offsets );
    }
    public static String ReadAbsoluteUTF8String ( ProcessManager64 manager, Int64 address, params Int64[] offsets ) {
      List<Byte> bytes = new List<Byte>();

      foreach ( Int64 offset in offsets )
        address = ReadAbsolute<Int64>(manager, address) + (Int64) offset;

      Byte b;
      while ( (b = ReadAbsolute<Byte>(manager, address++)) != 0 )
        bytes.Add(b);

      return Encoding.UTF8.GetString(bytes.ToArray());
    }
    public static String ReadAbsoluteMonoWideString ( ProcessManager64 manager, Int64 address, params Int64[] offsets ) {
      List<Byte> bytes = new List<Byte>();

      foreach ( Int64 offset in offsets )
        address = ReadAbsolute<Int64>(manager, address) + (Int64) offset;

      Int32 length = ReadAbsolute<Int32>(manager, address + 0x8);
      for ( Int32 i = 0; i < length; i++ )
        bytes.Add(ReadAbsolute<Byte>(manager, address + 0xC + i*2));

      return Encoding.UTF8.GetString(bytes.ToArray());
    }
    public static String ReadString ( ProcessManager64 manager, Int64 address, Int32 length ) {
      Byte[] Bytes = new Byte[length];

      Int32 lpNumberOfBytesRead = 0;
      ReadProcessMemory(manager.process.Handle, (IntPtr) address, Bytes, Bytes.Length, ref lpNumberOfBytesRead);

      Int32 nullTerminatorIndex = Array.FindIndex(Bytes, ( Byte b ) => b == 0);
      if ( nullTerminatorIndex >= 0 ) {
        Array.Resize(ref Bytes, nullTerminatorIndex);
        return Encoding.UTF8.GetString(Bytes);
      }

      return null;
    }
    public static void WriteRelative<T> ( ProcessManager64 manager, Int64 offset, T value, params Int64[] offsets ) where T : struct {
      WriteAbsolute<T> ( manager, manager.BaseAddress + offset, value, offsets );
    }
    public static void WriteAbsolute<T> ( ProcessManager64 manager, Int64 address, T value, params Int64[] offsets ) where T : struct {
      Int32 size = Marshal.SizeOf(value);
      Byte[] Bytes = new Byte[size];

      // Console.WriteLine($"Writing ({typeof(T).Name}) {value} to {address:X}, {string.Join(", ", offsets)}");
      foreach ( Int64 offset in offsets )
        address = ReadAbsolute<Int64>(manager, address) + (Int64) offset;

      IntPtr ptr = Marshal.AllocHGlobal(size);
      Marshal.StructureToPtr(value, ptr, true);
      Marshal.Copy(ptr, Bytes, 0, size);
      Marshal.FreeHGlobal(ptr);

      Int32 lpNumberOfBytesWritten = 0;
      WriteProcessMemory(manager.process.Handle, (IntPtr) address, Bytes, Bytes.Length, ref lpNumberOfBytesWritten);
    }
  }
}
