// using System;

// namespace Mono64 {

//     class MonoImage64 {
//         UInt64 /* int */ ref_count; // 0x000

//         UInt64 /* char* */ dll_path; // 0x020
//         UInt64 /* char* */ name; // 0x028
//         UInt64 /* char* */ dll; // 0x030
//         UInt64 /* char* */ version; // 0x038

//         UInt64 /* char* */ build; // 0x048

//         UInt64 /* MonoAssembly* */ assembly; // 0x4B0
//         UInt64 /* GHashTable* */ method_cache; // 0x4B8
//         /* GHashFunc */ // 0x4C0
//         /* MonoInternalHashKeyExtractFunc */ // 0x4C8
//         /* MonoInternalHashNextValueFunc */ // 0x4D0
//         UInt32 class_cache_size ; // 0x4D8
//         UInt32 class_cache_num_entries; // 0x4DC
//         UInt64 /* gpointer* */ class_cache_table; // 0x4E0
//     }
// }
