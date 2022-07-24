using System;
using System.Runtime.InteropServices;


namespace Narrative {
  [StructLayout(LayoutKind.Explicit)]
  public struct MonoClassField32 {
    [FieldOffset(0x00)]
    public UInt32 /* MonoType* */ type;
    [FieldOffset(0x04)]
    public UInt32 /* const char* */ name;
    [FieldOffset(0x08)]
    public UInt32 /* MonoClass* */ parent;
    [FieldOffset(0x0C)]
    public Int32 offset;

    public void DumpToConsole ( ProcessManager64 manager, String prefix = "" ) {
      Console.WriteLine($"{prefix}MonoClassField32;");
      Console.WriteLine($"{prefix}  type: {type:X}");
      Console.WriteLine($"{prefix}  name: {MemoryHelper.ReadAbsoluteUTF8String(manager, name)}");
      Console.WriteLine($"{prefix}  parent: {parent:X}");
      Console.WriteLine($"{prefix}  offset: {offset}");
    }
  }
}
