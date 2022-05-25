using System;

namespace Mono64 {

    class MonoImage64 {
        UInt64 /* int */ ref_count; // 0x000

        UInt32 /* char* */ name_1; // 0x014
        UInt32 /* char* */ name_2; // 0x018
        UInt32 /* char* */ name_3; // 0x01C
        UInt32 /* char* */ version; // 0x020

    }
}
