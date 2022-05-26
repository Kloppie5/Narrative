using System;

namespace Mono64 {

    class MonoClass64 {

        UInt64 /* MonoClass* */ element_class; // 0x00
        UInt64 /* MonoClass* */ cast_class; // 0x08
        UInt64 /* MonoClass** */ supertypes; // 0x10

        UInt64 /* char* */ name; // 0x48;
        UInt64 /* char* */ name_space; // 0x50;
        UInt32 /* gint32 */ token_type; // 0x58;
        UInt32 /* gint32 */ vtable_size; // 0x5C;

        UInt64 /* MonoClassField* */ fields; // 0x98
        UInt64 /* MonoMethod** */ methods;

        UInt64 /* MonoClassRuntimeInfo* */ runtime_info;
        UInt64 /* MonoMethod** */ vtable;
    }
}
