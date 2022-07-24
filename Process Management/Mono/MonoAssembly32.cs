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
    public UInt32 /* char* */ monoAssemblyNameDOTname;
    public String GetAssemblyName ( ProcessManager64 manager ) {
      return MemoryHelper.ReadAbsoluteUTF8String(manager, monoAssemblyNameDOTname);
    }
    [FieldOffset(0x48)]
    public UInt32 /* MonoImage* */ image;
    public MonoImage32 GetImage ( ProcessManager64 manager ) {
      return MemoryHelper.ReadAbsolute<MonoImage32>(manager, image);
    }

    public void DumpToConsole ( ProcessManager64 manager, String prefix = "" ) {
      Console.WriteLine($"{prefix}MonoAssembly32;");
      Console.WriteLine($"{prefix}  name: {GetAssemblyName(manager)}");
      GetImage(manager).DumpToConsole(manager, prefix+"  ");
    }
  }
}
