using System;

namespace Mono {
    class MonoVTable {
        UInt32 /* MonoClass* */ klass; // 0x00
        UInt32 /* void* */ gc_descr; // 0x04
        UInt32 /* MonoDomain* */ domain; // 0x08

    }
}
