using System;
using System.Runtime.InteropServices;

namespace Narrative {
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
}
