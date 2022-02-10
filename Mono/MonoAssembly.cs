using System;

namespace Mono {

    class MonoAssembly {

        UInt32 /* gint32 */ ref_count; // 0x00
        UInt32 /* char* */ basedir; // 0x04
        UInt32 /* MonoAssemblyName */ aname; // 0x08
        UInt32 /* MonoImage* */ image; // 0x0C
    }
}
