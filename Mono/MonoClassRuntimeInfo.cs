using System;

namespace Mono {
    class MonoClassRuntimeInfo {
        UInt16 max_domain; // 0x00
        // Padding
        UInt32[] /* MonoVTable* */ domain_vtables; // 0x08
    }
}
