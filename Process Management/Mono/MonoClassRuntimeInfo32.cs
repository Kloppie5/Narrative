using System;
using System.Runtime.InteropServices;

namespace Narrative {
  [StructLayout(LayoutKind.Explicit)]
  public struct MonoClassRuntimeInfo32 {
    [FieldOffset(0x00)]
    public UInt16 max_domain;
    [FieldOffset(0x04)]
    public UInt32 /* MonoVTable32*[] */ domain_vtables;
    public MonoVTable32 GetMonoVTable ( ProcessManager64 manager ) {
      return MemoryHelper.ReadAbsolute<MonoVTable32>(manager, domain_vtables);
    }

    public void DumpToConsole ( ProcessManager64 manager, String prefix = "" ) {
      Console.WriteLine($"{prefix}MonoClassRuntimeInfo32;");
      Console.WriteLine($"{prefix}  max_domain: {max_domain}");
      GetMonoVTable(manager).DumpToConsole(manager, prefix+"  ");
    }
  }
}
