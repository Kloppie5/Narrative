using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Narrative {

  [StructLayout(LayoutKind.Explicit)]
  public struct MonoImage32 {
    [FieldOffset(0x00)]
    public Int32 /* int */ ref_count;
    [FieldOffset(0x14)]
    public Int32 /* char* */ name;
    [FieldOffset(0x18)]
    public Int32 /* char* */ filename;
    [FieldOffset(0x1C)]
    public Int32 /* char* */ assembly_name;
    [FieldOffset(0x20)]
    public Int32 /* char* */ module_name;
    [FieldOffset(0x24)]
    public Int32 time_date_stamp;
    [FieldOffset(0x28)]
    public Int32 /* char* */ version;
    [FieldOffset(0x2C)]
    public Int16 md_version_major;
    [FieldOffset(0x2E)]
    public Int16 md_version_minor;
    [FieldOffset(0x30)]
    public Int32 /* char* */ guid;

    [FieldOffset(0x35C)]
    public MonoInternalHashTable class_cache;
    public List<MonoClass32> GetClasses ( ProcessManager64 manager ) {
      List<MonoClass32> classes = new List<MonoClass32>();
      for ( Int32 i = 0 ; i < class_cache.size ; ++i ) {
        Int32 pointer = MemoryHelper.ReadAbsolute<Int32>(manager, class_cache.table + i * 4);
        if ( pointer == 0 )
          continue;
        while ( pointer == MemoryHelper.ReadAbsolute<Int32>(manager, pointer) ) {
          MonoClass32 klass = MemoryHelper.ReadAbsolute<MonoClass32>(manager, pointer);
          classes.Add(klass);
          pointer += 0xA0;
        }
      }
      return classes;
    }

//         public List<UInt32> EnumImageClassCache ( UInt32 image ) {
//             List<UInt32> entries = new List<UInt32>();

//             UInt32 class_cache_size = ReadAbsolute<UInt32>(image + 0x360);
//             UInt32 class_cache_table = ReadAbsolute<UInt32>(image + 0x368);
//             Console.WriteLine($"Class Cache Size: {class_cache_size}");
//             Console.WriteLine($"Class Cache Table: {class_cache_table:X}");
//             for ( UInt32 i = 0 ; i < class_cache_size ; ++i ) {
//                 UInt32 pointer = ReadAbsolute<UInt32>(class_cache_table + i * 4);
//                 if ( pointer != 0 ) {
//                     UInt32 klass = ReadAbsolute<UInt32>(pointer);
//                     UInt32 nameAddress = ReadAbsolute<UInt32>(klass + 0x2C);
//                     String name = ReadAbsoluteUTF8String(nameAddress);
//                     UInt32 name_spaceAddress = ReadAbsolute<UInt32>(klass + 0x30);
//                     String name_space = ReadAbsoluteUTF8String(name_spaceAddress);
//                     UInt32 type_token = ReadAbsolute<UInt32>(klass + 0x34);
//                     // Print all fields
//                     UInt32 fields = ReadAbsolute<UInt32>(klass + 0x60);
//                     UInt32 num_fields = ReadAbsolute<UInt32>(klass + 0xA4);
//                     for ( UInt32 j = 0 ; j < num_fields ; ++j ) {
//                         UInt32 field_type = ReadAbsolute<UInt32>(fields + j * 16);
//                         UInt32 field_nameAddress = ReadAbsolute<UInt32>(fields + j * 16 + 0x4);
//                         String field_name = ReadAbsoluteUTF8String(field_nameAddress);
//                         UInt32 field_parent = ReadAbsolute<UInt32>(fields + j * 16 + 0x8);
//                         UInt32 field_offset = ReadAbsolute<UInt32>(fields + j * 16 + 0xC);
//                         Console.WriteLine($"  {field_name} {field_offset:X}");
//                     }

//                     UInt32 MonoMethodArray = ReadAbsolute<UInt32>(klass + 0x64);
//                     UInt32 MonoClassRuntimeInfo = ReadAbsolute<UInt32>(klass + 0x84);
//                     UInt32 MonoVTable = ReadAbsolute<UInt32>(MonoClassRuntimeInfo + 0x04);
//                     Console.WriteLine($"Found class {type_token:X8}:\"{name_space}.{name}\" ({klass:X8}) with Vtable at {MonoVTable:X8}");
//                     entries.Add(klass);
//                 }
//             }
//             return entries;
//         }

    public void DumpToConsole ( ProcessManager64 manager, String prefix = "" ) {
      Console.WriteLine($"{prefix}MonoImage32;");
      Console.WriteLine($"{prefix}  assembly_name: {MemoryHelper.ReadAbsoluteUTF8String(manager, assembly_name)}");
      class_cache.DumpToConsole(manager, prefix + "  ");
      List<MonoClass32> classes = GetClasses(manager);
      foreach ( MonoClass32 klass in classes ) {
        Console.WriteLine($"{prefix}  {MemoryHelper.ReadAbsoluteUTF8String(manager, klass.name_space)} | {MemoryHelper.ReadAbsoluteUTF8String(manager, klass.name)}");
      }
    }
  }
}
