using System;
using System.Runtime.InteropServices;

namespace Narrative {
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
}
