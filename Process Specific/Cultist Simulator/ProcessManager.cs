
using System;
using System.Collections.Generic;

using Narrative;

namespace CultistSimulator {
  public class ProcessManager : Narrative.ProcessManager64 {

    public Injector injector;

    public ProcessManager ( ) : base("Cultist Simulator") {
      injector = new Injector(this);

      Int32 Explore = injector.situationsDict["Explore"];
      Console.WriteLine($"Explore: {Explore:X}");
      List<Int32> Morlands = injector.elementStacksDict["Morland's Shop"];
      Console.WriteLine($"Morlands: {Morlands[0]:X}");
    }

    public void Dump ( ) {

    }
  }
}
