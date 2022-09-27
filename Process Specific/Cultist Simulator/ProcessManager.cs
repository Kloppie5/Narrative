
using System;
using System.Collections.Generic;

using Narrative;

namespace CultistSimulator {
  public class ProcessManager : Narrative.ProcessManager64 {

    public Dictionary<String, Int32> dict = new Dictionary<String, Int32>();

    public ProcessManager ( ) : base("Cultist Simulator") {
      MonoInjector32 injector = new MonoInjector32(this);
      Int32 rootDomain = injector.GetRootDomain();
      Dictionary<String, Int32> assemblies = injector.DomainGetAssembliesByName(rootDomain, new List<String>() {
        "Assembly-CSharp",
        "SecretHistories.Main",
      });

      Int32 image = injector.AssemblyGetImage(assemblies["SecretHistories.Main"]);
      Int32 watchman = injector.ImageGetClassByName(image, "SecretHistories.UI", "Watchman");
      Int32 vtable = injector.ClassGetVTable(watchman);

      Int32 staticFieldData = injector.VTableGetStaticFieldData(vtable);

      Int32 registered = MemoryHelper.ReadAbsolute<Int32>(this, staticFieldData);
      Int32 entries = MemoryHelper.ReadAbsolute<Int32>(this, registered + 0xC);
      Int32 count = MemoryHelper.ReadAbsolute<Int32>(this, entries + 0xC);

      for ( Int64 i = 0; i < count; ++i ) {
        Int32 hashcode = MemoryHelper.ReadAbsolute<Int32>(this, entries + 0x10 + i * 0x10);
        Int32 next = MemoryHelper.ReadAbsolute<Int32>(this, entries + 0x14 + i * 0x10);
        Int32 key = MemoryHelper.ReadAbsolute<Int32>(this, entries + 0x18 + i * 0x10);
        Int32 value = MemoryHelper.ReadAbsolute<Int32>(this, entries + 0x1C + i * 0x10);
        if ( value == 0 )
          continue;
        Int32 valueVTable = MemoryHelper.ReadAbsolute<Int32>(this, value);
        Int32 valueClass = MemoryHelper.ReadAbsolute<Int32>(this, valueVTable);
        String valueClassName = MemoryHelper.ReadAbsoluteUTF8String(this, MemoryHelper.ReadAbsolute<Int32>(this, valueClass + 0x2C));
        dict.Add(valueClassName, value);
      }
    }

    public void Dump ( ) {
      foreach ( KeyValuePair<String, Int32> pair in dict ) {
        Console.WriteLine($"  {pair.Key}: {pair.Value:X}");
      }
    }
  }
}
