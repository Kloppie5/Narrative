using System;
using System.Runtime.InteropServices;

namespace Narrative {

  [StructLayout(LayoutKind.Explicit)]
  public struct MonoImage32 {
    [FieldOffset(0x00)]
    public UInt32 /* int */ ref_count;
    [FieldOffset(0x14)]
    public UInt32 /* char* */ name;
    [FieldOffset(0x18)]
    public UInt32 /* char* */ filename;
    [FieldOffset(0x1C)]
    public UInt32 /* char* */ assembly_name;
    [FieldOffset(0x20)]
    public UInt32 /* char* */ module_name;
    [FieldOffset(0x24)]
    public UInt32 time_date_stamp;
    [FieldOffset(0x28)]
    public UInt32 /* char* */ version;
    [FieldOffset(0x2C)]
    public Int16 md_version_major;
    [FieldOffset(0x2E)]
    public Int16 md_version_minor;
    [FieldOffset(0x30)]
    public UInt32 /* char* */ guid;

    public void DumpToConsole ( ProcessManager64 manager ) {
      Console.WriteLine($"assembly_name: {MemoryHelper.ReadAbsoluteUTF8String(manager, assembly_name)}");
    }
  }
}
