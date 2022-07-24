// using System;
// using System.Collections.Generic;

// namespace Mono32 {

//     public class ProcessManager32 : Narrative.ProcessManager64 {

//         public ProcessManager32 ( String processName ) : base( processName ) {
//             // TODO
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
