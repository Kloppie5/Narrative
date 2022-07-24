
using System;

using Narrative;

namespace CultistSimulator {
    public class ProcessManager : Narrative.ProcessManager64 {

        public ProcessManager ( ) : base("Cultist Simulator") { }

        public void Dump ( ) {
          MonoHelper32.debug = true;
          UInt64 monomodule = MonoHelper32.FindMonoModule(this);
          Console.WriteLine($"mono-2.0-bdwgc.dll: {monomodule:X}");
          MonoDomain32 rootDomain = MonoHelper32.GetRootDomain(this);
          rootDomain.DumpToConsole(this);
        }
    }
}
