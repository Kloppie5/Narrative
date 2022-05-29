using System;

namespace Mono64 {

    class MonoClassRuntimeInfo64 {
        UInt64 /* int */ max_domain; // 0x000
        UInt64 /* MonoVTable** */ vtables; // 0x008
    }
}
