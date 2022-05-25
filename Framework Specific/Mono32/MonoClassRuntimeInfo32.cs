using System;

namespace Mono32 {
    class MonoClassRuntimeInfo32 {
        UInt16 max_domain; // 0x00
        // Padding
        UInt32[] /* MonoVTable* */ domain_vtables; // 0x08
    }
}
