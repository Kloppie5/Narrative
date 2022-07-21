using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

using Narrative;

using UInt8 = System.Byte;

namespace ThePerfectTowerII {

    public class ProvidenceWidget : Narrative.Widget {

        ProcessManager manager;
        public ProvidenceWidget ( ) {
            manager = new ProcessManager();

            UInt64 gameAssembly = PEHelper.GetModule(manager, "GameAssembly.dll");

            // manager.DumpExportedFunctions(gameAssembly);
            /*
            UInt64 domain_assemblies_pointer = manager.ReadAbsolute<UInt64>(gameAssembly + 0x1C22E68);
            Console.WriteLine($"domain_assemblies_pointer: {domain_assemblies_pointer:X}");
            for ( UInt64 i = 0 ; manager.ReadAbsolute<UInt64>(domain_assemblies_pointer + 0x8 * i) != 0 ; ++i ) {
                UInt64 assembly = manager.ReadAbsolute<UInt64>(domain_assemblies_pointer + 0x8 * i);
                String assembly_name = manager.ReadAbsoluteUTF8String(assembly + 0x18, 0);
                Console.WriteLine($"assembly {i}: {assembly_name} @ {assembly:X}");
            }
            */
        }
    }
}
