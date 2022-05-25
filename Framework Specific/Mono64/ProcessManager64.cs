using System;
using System.Collections.Generic;

namespace Mono64 {
    public class ProcessManager64 : Narrative.ProcessManager64 {

        public ProcessManager64 ( String processName ) : base( processName ) { }

        public UInt64 GetAssemblyInDomain ( UInt64 domain, String name ) {
            String friendly_name = ReadAbsoluteUTF8String(ReadAbsolute<UInt64>(domain + 0xD8));
            UInt64 domain_assemblies_GSList_pointer = ReadAbsolute<UInt64>(domain + 0xC8);
            for ( UInt64 CURR = domain_assemblies_GSList_pointer ; CURR != 0 ; CURR = ReadAbsolute<UInt64>(CURR+0x8) ) {
                UInt64 assembly = ReadAbsolute<UInt64>(CURR);
                UInt64 assembly_name_pointer = ReadAbsolute<UInt64>(assembly + 0x10);
                String assembly_name = ReadAbsoluteUTF8String(assembly_name_pointer);
                if ( assembly_name.Equals(name) )
                    return assembly;
            }
            return 0;
        }

        public List<UInt64> EnumImageClassCache ( UInt64 image ) {
            List<UInt64> entries = new List<UInt64>();
            Console.WriteLine($"Enumerating class cache for image {image:X}");
            /*
            UInt32 class_cache_size = ReadAbsolute<UInt32>(image + 0x360);
            UInt32 class_cache_table = ReadAbsolute<UInt32>(image + 0x368);
            Console.WriteLine($"Class Cache Size: {class_cache_size}");
            Console.WriteLine($"Class Cache Table: {class_cache_table:X}");
            for ( UInt32 i = 0 ; i < class_cache_size ; ++i ) {
                UInt32 pointer = ReadAbsolute<UInt32>(class_cache_table + i * 4);
                if ( pointer != 0 ) {
                    UInt32 klass = ReadAbsolute<UInt32>(pointer);
                    UInt32 nameAddress = ReadAbsolute<UInt32>(klass + 0x2C);
                    String name = ReadAbsoluteUTF8String(nameAddress);
                    UInt32 name_spaceAddress = ReadAbsolute<UInt32>(klass + 0x30);
                    String name_space = ReadAbsoluteUTF8String(name_spaceAddress);
                    UInt32 type_token = ReadAbsolute<UInt32>(klass + 0x34);
                    // Print all fields
                    UInt32 fields = ReadAbsolute<UInt32>(klass + 0x60);
                    UInt32 num_fields = ReadAbsolute<UInt32>(klass + 0xA4);
                    for ( UInt32 j = 0 ; j < num_fields ; ++j ) {
                        UInt32 field_type = ReadAbsolute<UInt32>(fields + j * 16);
                        UInt32 field_nameAddress = ReadAbsolute<UInt32>(fields + j * 16 + 0x4);
                        String field_name = ReadAbsoluteUTF8String(field_nameAddress);
                        UInt32 field_parent = ReadAbsolute<UInt32>(fields + j * 16 + 0x8);
                        UInt32 field_offset = ReadAbsolute<UInt32>(fields + j * 16 + 0xC);
                        Console.WriteLine($"  {field_name} {field_offset:X}");
                    }

                    UInt32 MonoMethodArray = ReadAbsolute<UInt32>(klass + 0x64);
                    UInt32 MonoClassRuntimeInfo = ReadAbsolute<UInt32>(klass + 0x84);
                    UInt32 MonoVTable = ReadAbsolute<UInt32>(MonoClassRuntimeInfo + 0x04);
                    Console.WriteLine($"Found class {type_token:X8}:\"{name_space}.{name}\" ({klass:X8}) with Vtable at {MonoVTable:X8}");
                    entries.Add(klass);
                }
            }
            */
            return entries;
        }
    }
}
