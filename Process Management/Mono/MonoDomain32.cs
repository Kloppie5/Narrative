using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Narrative {
  [StructLayout(LayoutKind.Explicit)]
  public unsafe struct MonoDomain32 {
    [FieldOffset(0x00)]
    public UInt32 /* ? */ unknown_0; // -1
    [FieldOffset(0x04)]
    public UInt32 /* ? */ unknown_4; // -1
    [FieldOffset(0x08)]
    public UInt32 /* ? */ unknown_8; // 0
    [FieldOffset(0x0C)]
    public UInt32 /* ? */ unknown_C; // 0
    [FieldOffset(0x10)]
    public UInt32 /* ? */ unknown_10; // -1

    [FieldOffset(0x1C)]
    public UInt32 /* ?* */ unknown_1C;
    [FieldOffset(0x20)]
    public UInt32 /* ?* */ unknown_20;
    [FieldOffset(0x24)]
    public UInt32 /* ?* */ unknown_24;
    [FieldOffset(0x28)]
    public UInt32 /* ?* */ unknown_28;
    [FieldOffset(0x2C)]
    public UInt32 /* ?* */ unknown_2C;
    [FieldOffset(0x30)]
    public UInt32 /* ?* */ unknown_30;

    [FieldOffset(0x58)]
    public UInt32 /* GSList32* */ domain_assemblies;
    public List<MonoAssembly32> GetAssemblies ( ProcessManager64 manager ) {
      List<MonoAssembly32> assemblies = new List<MonoAssembly32>();
      UInt32 it = domain_assemblies;
      while ( it != 0 ) {
        GSList32 list = MemoryHelper.ReadAbsolute<GSList32>(manager, it);
        MonoAssembly32 assembly = MemoryHelper.ReadAbsolute<MonoAssembly32>(manager, list.data);
        assemblies.Add(assembly);
        it = list.next;
      }
      return assemblies;
    }
    [FieldOffset(0x60)]
    public UInt32 /* char* */ friendly_name;
    public String GetFriendlyName ( ProcessManager64 manager ) {
      return MemoryHelper.ReadAbsoluteUTF8String(manager, friendly_name);
    }

    public void DumpToConsole ( ProcessManager64 manager ) {
      Console.WriteLine($"friendly_name: {GetFriendlyName(manager)}");
      foreach ( var assembly in GetAssemblies(manager) ) {
        assembly.DumpToConsole(manager);
      }
    }
  }
}
