using System;
using System.Runtime.InteropServices;

using UInt8 = System.Byte;

namespace Narrative {
  [StructLayout(LayoutKind.Explicit)]
  public struct MonoClass32 {
    [FieldOffset(0x00)]
    public UInt32 /* MonoClass* */ element_class;
    [FieldOffset(0x04)]
    public UInt32 /* MonoClass* */ cast_class;
    [FieldOffset(0x08)]
    public UInt32 /* MonoClass** */ supertypes;
    [FieldOffset(0x0C)]
    public UInt16 idepth;
    [FieldOffset(0x0E)]
    public UInt8 rank;
    [FieldOffset(0x0F)]
    public UInt8 child_class;
    [FieldOffset(0x10)]
    public Int32 instance_size;

    [FieldOffset(0x20)]
    public UInt32 /* MonoClass32* */ parent;
    [FieldOffset(0x24)]
    public UInt32 /* MonoClass32* */ nested_in;
    [FieldOffset(0x28)]
    public UInt32 /* MonoImage32* */ image;
    [FieldOffset(0x2C)]
    public UInt32 /* char* */ name;
    [FieldOffset(0x30)]
    public UInt32 /* char* */ name_space;
    [FieldOffset(0x34)]
    public UInt32 type_token;
    [FieldOffset(0x38)]
    public Int32 vtable_size;
    
    [FieldOffset(0x60)]
    public UInt32 /* MonoClassField32*  */ fields;
    [FieldOffset(0x64)]
    public UInt32 /* MonoMethod32**  */ methods;
    
    [FieldOffset(0x84)]
    public UInt32 /* MonoClassRuntimeInfo32* */ runtime_info;

    // UInt32 native_size; // 0x9C
    // UInt32 min_align; // 0xA0
    // UInt32 num_fields; // 0xA4

    public void DumpToConsole ( ProcessManager64 manager ) {
      Console.WriteLine($"element_class: {element_class:X}");
      Console.WriteLine($"cast_class: {cast_class:X}");
      Console.WriteLine($"supertypes: {supertypes:X}");
      Console.WriteLine($"idepth: {idepth}");
      Console.WriteLine($"rank: {rank}");
      Console.WriteLine($"child_class: {child_class}");
      Console.WriteLine($"instance_size: {instance_size}");
      Console.WriteLine($"parent: {parent:X}");
      Console.WriteLine($"nested_in: {nested_in:X}");
      Console.WriteLine($"image: {image:X}");
      Console.WriteLine($"name: {MemoryHelper.ReadAbsoluteUTF8String(manager, name)}");
      Console.WriteLine($"name_space: {MemoryHelper.ReadAbsoluteUTF8String(manager, name_space)}");
      Console.WriteLine($"type_token: {type_token:X}");
      Console.WriteLine($"vtable_size: {vtable_size}");
      Console.WriteLine($"fields: {fields:X}");
      Console.WriteLine($"methods: {methods:X}");
      Console.WriteLine($"runtime_info: {runtime_info:X}");
    }
  }
}
