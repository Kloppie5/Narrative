// using System;

// namespace CryptOfTheNecroDancer {

//     public class RenderableObject {

//         public UInt32 address;
//         ProcessManager manager;

//         // vtable // 0x00
//         // gc_L // 0x04
//         // gc_R // 0x08
//         // gc_C // 0x0C
//         public Int32 x; // 0x14
//         public Int32 y; // 0x18

//         public RenderableObject ( UInt32 address, ProcessManager manager ) {
//             this.address = address;
//             this.manager = manager;
//             Read();
//         }

//         public void Read() {
//             Console.WriteLine($"Reading RenderableObject at {address:X}");
//             x = manager.ReadAbsolute<Int32>(address + 0x14);
//             y = manager.ReadAbsolute<Int32>(address + 0x18);
//         }
//     }
// }
