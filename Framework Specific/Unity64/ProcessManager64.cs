using System;
using System.Collections.Generic;

using UInt8 = System.Byte;

namespace Unity64 {
    public class ProcessManager64 : Mono64.ProcessManager64 {

        public ProcessManager64 ( String processName ) : base( processName ) {
            // TODO
        }

        public UInt64 ExtractRootDomain ( UInt64 mono_get_root_domain ) {
            // mono-2.0-bdwgc.mono_get_root_domain
            // mono.mono_get_root_domain
            UInt8 MonoGetRootDomainOpcode = ReadAbsolute<UInt8>(mono_get_root_domain);
            if ( MonoGetRootDomainOpcode == 0xA1 ) {
                // A1 ???????? - mov eax, {RootDomain}
                // TODO
                Console.WriteLine("TODO");
                return 0;
            }
            if ( MonoGetRootDomainOpcode == 0x48 ) {
                // 48 8B 05 ???????? - mov rax, QWORD PTR [rip+{offset}]
                UInt32 offset = ReadAbsolute<UInt32>(mono_get_root_domain + 0x3);
                UInt64 RootDomainAddress = mono_get_root_domain + 0x7 + offset;
                return ReadAbsolute<UInt64>(RootDomainAddress);
            }
            Console.WriteLine("TODO");
            return 0;
        }
    }
}
