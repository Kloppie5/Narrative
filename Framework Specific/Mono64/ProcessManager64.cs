using System;
using System.Collections.Generic;

using UInt8 = System.Byte;

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

        public UInt64 GetClassInImage ( UInt64 image, String name ) {
            UInt32 class_cache_size = ReadAbsolute<UInt32>(image + 0x4D8);
            UInt32 class_cache_num_entries = ReadAbsolute<UInt32>(image + 0x4DC);
            UInt64 class_cache_table = ReadAbsolute<UInt64>(image + 0x4E0);

            for ( UInt32 i = 0 ; i < class_cache_size ; ++i ) {
                UInt64 pointer = ReadAbsolute<UInt64>(class_cache_table + i * 8);
                if ( pointer == 0 )
                    continue;
                UInt64 klass = ReadAbsolute<UInt64>(pointer);
                if ( klass == 0 )
                    continue;

                UInt64 klassNameAddress = ReadAbsolute<UInt64>(klass + 0x48);
                String klassName = ReadAbsoluteUTF8String(klassNameAddress);

                if ( klassName.Equals(name) )
                    return klass;
            }
            return 0;
        }

        public List<UInt64> EnumImageClassCache ( UInt64 image ) {
            List<UInt64> entries = new List<UInt64>();
            Console.WriteLine($"Enumerating class cache for image {image:X}");
            UInt32 class_cache_size = ReadAbsolute<UInt32>(image + 0x4D8);
            UInt32 class_cache_num_entries = ReadAbsolute<UInt32>(image + 0x4DC);
            UInt64 class_cache_table = ReadAbsolute<UInt64>(image + 0x4E0);
            Console.WriteLine($"Class Cache Size: {class_cache_size}");
            Console.WriteLine($"Class Cache Table: {class_cache_table:X}");

            for ( UInt32 i = 0 ; i < class_cache_size ; ++i ) {
                UInt64 pointer = ReadAbsolute<UInt64>(class_cache_table + i * 8);
                if ( pointer == 0 )
                    continue;
                UInt64 klass = ReadAbsolute<UInt64>(pointer);
                if ( klass == 0 )
                    continue;
                UInt64 nameAddress = ReadAbsolute<UInt64>(klass + 0x48);
                String name = ReadAbsoluteUTF8String(nameAddress);
                UInt64 name_spaceAddress = ReadAbsolute<UInt64>(klass + 0x50);
                String name_space = ReadAbsoluteUTF8String(name_spaceAddress);
                UInt64 type_token = ReadAbsolute<UInt32>(klass + 0x58);
                Console.WriteLine($"Class {klass:X} Name: {name}{name_space}");

                // Print all fields
                UInt64 fields = ReadAbsolute<UInt64>(klass + 0x98);
                for ( UInt64 offset = 0 ; ReadAbsolute<UInt64>(fields + offset + 0x10) == klass ; offset += 0x20 ) {
                    UInt64 field_type = ReadAbsolute<UInt64>(fields + offset);
                    UInt64 field_nameAddress = ReadAbsolute<UInt64>(fields + offset + 0x8);
                    String field_name = ReadAbsoluteUTF8String(field_nameAddress);
                    UInt64 field_parent = ReadAbsolute<UInt64>(fields + offset + 0x10);
                    UInt64 field_offset = ReadAbsolute<UInt64>(fields + offset + 0x18);
                    Console.WriteLine($" {field_offset:X03}: {field_name}");
                }
                /*
                UInt32 MonoMethodArray = ReadAbsolute<UInt32>(klass + 0x64);
                UInt32 MonoClassRuntimeInfo = ReadAbsolute<UInt32>(klass + 0x84);
                UInt32 MonoVTable = ReadAbsolute<UInt32>(MonoClassRuntimeInfo + 0x04);
                Console.WriteLine($"Found class {type_token:X8}:\"{name_space}.{name}\" ({klass:X8}) with Vtable at {MonoVTable:X8}");
                entries.Add(klass);
                /**/
            }
            return entries;
        }

        public List<UInt64> FindInstancesOfClass ( UInt64 monoVTable ) {
            String patternString =
                $"{(UInt8) monoVTable:X02} {(UInt8) (monoVTable >> 8):X02} {(UInt8) (monoVTable >> 16):X02} {(UInt8) (monoVTable >> 24):X02} {(UInt8) (monoVTable >> 32):X02} {(UInt8) (monoVTable >> 40):X02} {(UInt8) (monoVTable >> 48):X02} {(UInt8) (monoVTable >> 56):X02} " +
                $"00 00 00 00 00 00 00 00";
            Console.WriteLine($"Searching for pattern {patternString}");
            return FindPatternAddresses64 ( 0x00000000000, 0x30000000000, new Narrative.BytePattern(patternString) );
        }
    }
}
