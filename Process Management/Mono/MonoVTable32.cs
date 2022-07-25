using System;
using System.Runtime.InteropServices;

namespace Narrative {
    [StructLayout(LayoutKind.Explicit)]
    public struct MonoVTable32 {
        [FieldOffset(0x00)]
        public UInt32 /* MonoClass* */ klass;
        [FieldOffset(0x04)]
        public UInt32 /* void* */ gc_descr;
        [FieldOffset(0x08)]
        public UInt32 /* MonoDomain* */ domain;

        public void DumpToConsole ( ProcessManager64 manager, String prefix = "" ) {
            Console.WriteLine($"{prefix}MonoVTable32;");
            Console.WriteLine($"{prefix}  klass: {klass:X}");
            Console.WriteLine($"{prefix}  gc_descr: {gc_descr:X}");
            Console.WriteLine($"{prefix}  domain: {domain:X}");
        }
    }
}
