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

    public void DumpToConsole ( ProcessManager64 manager ) {
      Console.WriteLine($"type: {type:X}");
      Console.WriteLine($"name: {MemoryHelper.ReadAbsoluteUTF8String(manager, name)}");
      Console.WriteLine($"parent: {parent:X}");
      Console.WriteLine($"offset: {offset}");
    }
  }
}
