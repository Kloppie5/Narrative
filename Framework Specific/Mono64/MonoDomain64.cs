using System;

using UInt8 = System.Byte;

namespace Mono64 {

    class MonoDomain64 {
        UInt8[] /* pthread_mutex_t */ _lock; // 0x000
        UInt64 /* * */ pointer_28_1; // 0x028
        UInt64 /* * */ pointer_28_2; // 0x030
        UInt64 /* * */ pointer_28_3; // 0x038
        UInt64 /* * */ pointer_28_4; // 0x040
        UInt64 /* * */ pointer_28_5; // 0x048
        UInt64 /* * */ pointer_28_6; // 0x050
        UInt64 /* * */ pointer_28_7; // 0x058
        UInt64 /* * */ pointer_28_8; // 0x060
        UInt64 /* * */ pointer_28_9; // 0x068
        UInt64 /* * */ pointer_28_10; // 0x070
        UInt64 /* * */ pointer_28_11; // 0x078
        UInt64 /* * */ pointer_28_12; // 0x080
        UInt64 /* * */ pointer_28_13; // 0x088
        UInt64 /* * */ pointer_28_14; // 0x090
        UInt64 /* * */ pointer_28_15; // 0x098
        UInt64 /* * */ pointer_28_16; // 0x0A0
        UInt64 /* nullptr */ pointer_A8_1; // 0x0A8
        UInt64 /* nullptr */ pointer_A8_2; // 0x0B0
        UInt64 /* nullptr */ pointer_A8_3; // 0x0B8
        UInt64 /* nullptr */ pointer_A8_4; // 0x0C0
        UInt64 /* GSList<MonoAssembly64*>* */ domain_assemblies; // 0x0C8
        UInt64 /* nullptr */ pointer_D0; // 0x0D0
        UInt64 /* char* */ friendly_name; // 0x0D8
        UInt64 /* * */ pointer_E0; // 0x0E0
        UInt64 /* * */ pointer_E8; // 0x0E8
        UInt64 /* * */ pointer_F0; // 0x0F0
        UInt64 /* * */ pointer_F8; // 0x0F8
        UInt64 /* * */ pointer_100; // 0x100
        UInt64 /* * */ pointer_108; // 0x108
        UInt64 /* * */ pointer_110; // 0x110
    }
}
