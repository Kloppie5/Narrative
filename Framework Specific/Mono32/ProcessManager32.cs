// using System;
// using System.Collections.Generic;

// namespace Mono32 {

//     public class ProcessManager32 : Narrative.ProcessManager64 {

//         public ProcessManager32 ( String processName ) : base( processName ) {
//             // TODO
//         }

//         public UInt32 GetAssemblyInDomain ( UInt32 domain, String name ) {
//             UInt32 it = ReadAbsolute<UInt32>(domain + 0x6C);
//             while ( it != 0 ) {
//                 UInt32 assembly = ReadAbsolute<UInt32>(it);
//                 UInt32 assemblyNameAddress = ReadAbsolute<UInt32>(assembly + 0x08);
//                 String assemblyName = ReadAbsoluteUTF8String(assemblyNameAddress);
//                 if ( assemblyName.Equals(name) )
//                     return assembly;

//                 it = ReadAbsolute<UInt32>(it + 0x4);
//             }
//             return 0;
//         }

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

//         public UInt32 GetVTableOfClassInClassCache ( UInt32 image, String name ) {
//             UInt32 class_cache_size = ReadAbsolute<UInt32>(image + 0x360);
//             UInt32 class_cache_table = ReadAbsolute<UInt32>(image + 0x368);
//             for ( UInt32 i = 0 ; i < class_cache_size ; ++i ) {
//                 UInt32 pointer = ReadAbsolute<UInt32>(class_cache_table + i * 4);
//                 if ( pointer == 0 )
//                     continue;

//                 UInt32 klass = ReadAbsolute<UInt32>(pointer);
//                 UInt32 classnameAddress = ReadAbsolute<UInt32>(klass + 0x2C);
//                 String classname = ReadAbsoluteUTF8String(classnameAddress);

//                 if ( !classname.Equals(name) )
//                     continue;

//                 UInt32 MonoClassRuntimeInfo = ReadAbsolute<UInt32>(klass + 0x84);
//                 UInt32 MonoVTable = ReadAbsolute<UInt32>(MonoClassRuntimeInfo + 0x04);
//                 return MonoVTable;
//                 // pointer = Read<Int32>(pointer + 0xA8);
//             }
//             return 0;
//         }
//     }
// }
