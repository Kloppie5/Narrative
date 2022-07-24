
using System;
using System.Collections.Generic;

using Narrative;

namespace CultistSimulator {
    public class ProcessManager : Narrative.ProcessManager64 {

        public ProcessManager ( ) : base("Cultist Simulator") { }

        public void Dump ( ) {
          UInt64 monomodule = MonoHelper32.FindMonoModule(this);
          Console.WriteLine($"mono-2.0-bdwgc.dll: {monomodule:X}");
          MonoDomain32 rootDomain = MonoHelper32.GetRootDomain(this);
          List<String> targets = new List<String> {
            "SecretHistories.Enums",
            "SecretHistories.Main",
            "OrbCreations",
            "SecretHistories.Interfaces",
            "SecretHistories.Constants"
          };
          rootDomain
           .GetAssemblies(this)
           .FindAll(assembly => targets.Contains(assembly.GetAssemblyName(this)))
           .ForEach(assembly => {
            assembly.DumpToConsole(this);
          });
        }
    }
}
