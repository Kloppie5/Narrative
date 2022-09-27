
using System;
using System.Collections.Generic;

using Narrative;

namespace CultistSimulator {
  public class ProcessManager : Narrative.ProcessManager64 {

    public ProcessManager ( ) : base("Cultist Simulator") { }

    public void Dump ( ) {
      Int64 monomodule = MonoHelper32.FindMonoModule(this);
      Console.WriteLine($"mono-2.0-bdwgc.dll: {monomodule:X}");
      MonoDomain32 rootDomain = MonoHelper32.GetRootDomain(this);
      /*
      rootDomain
       .GetAssemblies(this)
       .Find(assembly => "SecretHistories.Main" == assembly.GetAssemblyName(this))
       .GetImage(this)
       .GetClasses(this)
       .ForEach(klass => {
        Console.WriteLine($"  {klass.GetNamespace(this)}.{klass.GetName(this)}");
       });
       */

      MonoInjector32 injector = new MonoInjector32(this);
      Int32 injRootDomain = injector.GetRootDomain();
      Dictionary<String, Int32> assemblies = injector.DomainGetAssembliesByName(injRootDomain, new List<String>() {
        "Assembly-CSharp",
        "SecretHistories.Main",
      });
      Console.WriteLine($"assemblies: {assemblies.Count}");

      Int32 image = injector.AssemblyGetImage(assemblies["SecretHistories.Main"]);

      Int32 watchman = injector.ImageGetClassByName(image, "SecretHistories.UI", "Watchman");
      Console.WriteLine($"watchman: {watchman:X}");

      // Watchman.Get<StageHand>
    }
  }
}
