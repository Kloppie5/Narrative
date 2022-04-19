using System;

namespace Mono {

    class MonoImage {
        UInt32 /* int */ ref_count; // 0x000

        UInt32 /* char* */ name_1; // 0x014
        UInt32 /* char* */ name_2; // 0x018
        UInt32 /* char* */ name_3; // 0x01C
        UInt32 /* char* */ version; // 0x020

    }
}
