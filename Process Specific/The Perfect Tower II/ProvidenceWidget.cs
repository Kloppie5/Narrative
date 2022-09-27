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

            Int64 gameAssembly = PEHelper.GetModule(manager, "GameAssembly.dll");

            // manager.DumpExportedFunctions(gameAssembly);
            /*
            Int64 domain_assemblies_pointer = manager.ReadAbsolute<Int64>(gameAssembly + 0x1C22E68);
            Console.WriteLine($"domain_assemblies_pointer: {domain_assemblies_pointer:X}");
            for ( Int64 i = 0 ; manager.ReadAbsolute<Int64>(domain_assemblies_pointer + 0x8 * i) != 0 ; ++i ) {
                Int64 assembly = manager.ReadAbsolute<Int64>(domain_assemblies_pointer + 0x8 * i);
                String assembly_name = manager.ReadAbsoluteUTF8String(assembly + 0x18, 0);
                Console.WriteLine($"assembly {i}: {assembly_name} @ {assembly:X}");
            }
            */
        }
    }
}
