
using System;
using System.Collections.Generic;

using Narrative;

namespace CultistSimulator {
  public class ProcessManager : Narrative.ProcessManager64 {

    public Dictionary<String, Int32> dict = new Dictionary<String, Int32>();
    public Dictionary<String, Int32> assemblies = new Dictionary<String, Int32>();
    public Dictionary<String, Dictionary<String, Int32>> edict = new Dictionary<String, Dictionary<String, Int32>>();

    MonoInjector32 injector;

    public ProcessManager ( ) : base("Cultist Simulator") {
      injector = new MonoInjector32(this);
      Int32 rootDomain = injector.GetRootDomain();
      assemblies = injector.DomainGetAssembliesByName(rootDomain, new List<String>() {
        "Assembly-CSharp",
        "SecretHistories.Main",
      });

      ExtractWatchman();
      ExtractCompendium();
    }

    public void ExtractWatchman ( ) {
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
    public void ExtractCompendium ( ) {
      Int32 compendium = dict["Compendium"];
      Console.WriteLine($"Compendium: {compendium:X}");

      Int32 entityStores = MemoryHelper.ReadAbsolute<Int32>(this, compendium + 0x8);
      Int32 entityStoresEntries = MemoryHelper.ReadAbsolute<Int32>(this, entityStores + 0xC);
      Int32 entityStoresCount = MemoryHelper.ReadAbsolute<Int32>(this, entityStoresEntries + 0xC);
      for ( Int32 i = 0; i < entityStoresCount; ++i ) {
        Int32 ihashcode = MemoryHelper.ReadAbsolute<Int32>(this, entityStoresEntries + 0x10 + i * 0x10);
        Int32 inext = MemoryHelper.ReadAbsolute<Int32>(this, entityStoresEntries + 0x14 + i * 0x10);
        Int32 ikey = MemoryHelper.ReadAbsolute<Int32>(this, entityStoresEntries + 0x18 + i * 0x10);
        Int32 ivalue = MemoryHelper.ReadAbsolute<Int32>(this, entityStoresEntries + 0x1C + i * 0x10);
        if ( ivalue == 0 )
          continue;
        Int32 keyType = MemoryHelper.ReadAbsolute<Int32>(this, ikey + 0x8);
        Int32 keyClass = MemoryHelper.ReadAbsolute<Int32>(this, keyType);
        String keyClassName = MemoryHelper.ReadAbsoluteUTF8String(this, MemoryHelper.ReadAbsolute<Int32>(this, keyClass + 0x2C));

        Dictionary<String, Int32> entityStore = new Dictionary<String, Int32>();

        Int32 entities = MemoryHelper.ReadAbsolute<Int32>(this, ivalue + 0x8);
        Int32 entries = MemoryHelper.ReadAbsolute<Int32>(this, entities + 0xC);
        Int32 count = MemoryHelper.ReadAbsolute<Int32>(this, entries + 0xC);
        for ( Int64 j = 0; j < count; ++j ) {
          Int32 jhashcode = MemoryHelper.ReadAbsolute<Int32>(this, entries + 0x10 + j * 0x10);
          Int32 jnext = MemoryHelper.ReadAbsolute<Int32>(this, entries + 0x14 + j * 0x10);
          Int32 jkey = MemoryHelper.ReadAbsolute<Int32>(this, entries + 0x18 + j * 0x10);
          Int32 jvalue = MemoryHelper.ReadAbsolute<Int32>(this, entries + 0x1C + j * 0x10);
          if ( jvalue == 0 )
            continue;
          String name = MemoryHelper.ReadAbsoluteMonoWideString(this, jkey);
          Console.WriteLine($"{keyClassName}: {name} {jvalue:X}");
          entityStore.Add(name, jvalue);
        }

        edict.Add(keyClassName, entityStore);
      }
    }

    public void Dump ( ) {
      foreach ( KeyValuePair<String, Int32> pair in dict ) {
        Console.WriteLine($"  {pair.Key}: {pair.Value:X}");
      }
    }
  }
}
