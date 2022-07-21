using System;
using System.Runtime.InteropServices;

namespace Narrative {
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
}
