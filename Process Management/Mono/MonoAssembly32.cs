using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Narrative {
  [StructLayout(LayoutKind.Explicit)]
  public unsafe struct MonoAssembly32 {
    [FieldOffset(0x00)]
    public UInt32 /* gint32 */ ref_count;
    [FieldOffset(0x04)]
    public UInt32 /* char* */ basedir;
    [FieldOffset(0x08)]
    public UInt32 /* MonoAssemblyName */ aname;
    [FieldOffset(0x0C)]
    public UInt32 /* MonoImage* */ image; // 0x0C

    public String GetAssemblyName ( ProcessManager64 manager ) {
      return MemoryHelper.ReadAbsoluteUTF8String(manager, aname);
    }

    public void DumpToConsole ( ProcessManager64 manager ) {
      Console.WriteLine($"assembly: {GetAssemblyName(manager)}");
    }
  }
}
