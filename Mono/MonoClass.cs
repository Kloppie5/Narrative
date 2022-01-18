using System;

namespace Mono {

    class MonoClass {
        UInt32 /* MonoClass* */ element_class; // 0x00
        UInt32 /* MonoClass* */ cast_class; // 0x04

        UInt32 /* const char* */ name; // 0x2C
        UInt32 /* const char* */ name_space; // 0x30
        UInt32 type_token; // 0x34
        Int32 vtable_size; // 0x38

        UInt32 fields_first;

        UInt32 /* MonoClassField*  */ fields; // 0x60
        UInt32 /* MonoMethod**  */ methods; // 0x64

        UInt32 /* MonoClassRuntimeInfo* */ runtime_info; // 0x84;

        UInt32 native_size; // 0x9C
        UInt32 min_align; // 0xA0
        UInt32 num_fields; // 0xA4

    }

    class MonoClassField {
        UInt32 /* MonoType* */ type; // 0x00
        UInt32 /* const char* */ name; // 0x04
        UInt32 /* MonoClass* */ parent; // 0x08
        Int32 offset; // 0x0C
    }
}
