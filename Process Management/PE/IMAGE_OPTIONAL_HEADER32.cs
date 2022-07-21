using System;
using System.Runtime.InteropServices;

namespace Narrative {
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
}
