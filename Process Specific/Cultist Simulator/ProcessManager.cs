
using System;
using System.Collections.Generic;

using Narrative;

namespace CultistSimulator {
  public class ProcessManager : Narrative.ProcessManager64 {

    public Dictionary<String, Int32> dict = new Dictionary<String, Int32>();
    public Dictionary<String, Int32> assemblies = new Dictionary<String, Int32>();
    public Dictionary<String, Dictionary<String, Int32>> edict = new Dictionary<String, Dictionary<String, Int32>>();
    public Int32 tabletopSphere = -1;

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
      ExtractHornedAxe();
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
          entityStore.Add(name, jvalue);
        }

        Console.WriteLine($"{keyClassName}: {entityStore.Count}");

        edict.Add(keyClassName, entityStore);
      }
    }
    public void ExtractHornedAxe ( ) {
      {
        Int32 hornedaxe = dict["HornedAxe"];
        Int32 registeredSpheres = MemoryHelper.ReadAbsolute<Int32>(this, hornedaxe + 0x1C);
        Int32 slotArray = MemoryHelper.ReadAbsolute<Int32>(this, registeredSpheres + 0xC);
        Int32 entries = MemoryHelper.ReadAbsolute<Int32>(this, slotArray + 0xC);
        for ( Int32 i = 0 ; i < entries ; ++i ) {
          Int32 hashcode = MemoryHelper.ReadAbsolute<Int32>(this, slotArray + 0x10 + i * 0xC);
          Int32 next = MemoryHelper.ReadAbsolute<Int32>(this, slotArray + 0x14 + i * 0xC);
          Int32 value = MemoryHelper.ReadAbsolute<Int32>(this, slotArray + 0x18 + i * 0xC);
          if ( value == 0 )
            continue;
          Int32 valueVTable = MemoryHelper.ReadAbsolute<Int32>(this, value);
          Int32 valueClass = MemoryHelper.ReadAbsolute<Int32>(this, valueVTable);
          String valueClassName = MemoryHelper.ReadAbsoluteUTF8String(this, MemoryHelper.ReadAbsolute<Int32>(this, valueClass + 0x2C));

          if ( valueClassName == "TabletopSphere" )
            tabletopSphere = value;
        }
      }
      {
        Console.WriteLine($"TabletopSphere: {tabletopSphere:X}");
        Int32 tokens = MemoryHelper.ReadAbsolute<Int32>(this, tabletopSphere + 0x28);
        Int32 tokenArray = MemoryHelper.ReadAbsolute<Int32>(this, tokens + 0x8);
        Int32 entries = MemoryHelper.ReadAbsolute<Int32>(this, tokenArray + 0xC);
        for ( Int32 i = 0 ; i < entries ; ++i ) {
          Int32 token = MemoryHelper.ReadAbsolute<Int32>(this, tokenArray + 0x10 + i * 0x4);
          if ( token == 0 )
            continue;

          Int32 state = MemoryHelper.ReadAbsolute<Int32>(this, token + 0x20);
          String stateString = MemoryHelper.ReadAbsoluteMonoWideString(this, state);
          Int32 fullPath = MemoryHelper.ReadAbsolute<Int32>(this, token + 0x24);
          String fullPathString = MemoryHelper.ReadAbsoluteMonoWideString(this, fullPath);
          Int32 payload = MemoryHelper.ReadAbsolute<Int32>(this, token + 0x28);
          Int32 payloadVTable = MemoryHelper.ReadAbsolute<Int32>(this, payload);
          Int32 payloadClass = MemoryHelper.ReadAbsolute<Int32>(this, payloadVTable);
          String payloadClassName = MemoryHelper.ReadAbsoluteUTF8String(this, MemoryHelper.ReadAbsolute<Int32>(this, payloadClass + 0x2C));

          if ( payloadClassName == "ElementStack" ) {
            Int32 id = MemoryHelper.ReadAbsolute<Int32>(this, payload + 0x10);
            String idString = MemoryHelper.ReadAbsoluteMonoWideString(this, id);
            Int32 element = MemoryHelper.ReadAbsolute<Int32>(this, payload + 0x14);
            Int32 label = MemoryHelper.ReadAbsolute<Int32>(this, element + 0x1C);
            String labelString = MemoryHelper.ReadAbsoluteMonoWideString(this, label);
            Int32 description = MemoryHelper.ReadAbsolute<Int32>(this, element + 0x20);
            String descriptionString = MemoryHelper.ReadAbsoluteMonoWideString(this, description);
            Int32 quantity = MemoryHelper.ReadAbsolute<Int32>(this, payload + 0x40);

            Console.WriteLine($"{idString} | {quantity}x | {labelString}");
          }
          if ( payloadClassName == "Situation" ) {
            Int32 recipe = MemoryHelper.ReadAbsolute<Int32>(this, payload + 0x14);
            Int32 verb = MemoryHelper.ReadAbsolute<Int32>(this, payload + 0x1C);
            Int32 id = MemoryHelper.ReadAbsolute<Int32>(this, payload + 0x20);
            String idString = MemoryHelper.ReadAbsoluteMonoWideString(this, id);

            Console.WriteLine($"{fullPathString}: {idString}");
          }
        }
      }
    }

    public void Dump ( ) {
      foreach ( KeyValuePair<String, Int32> pair in dict ) {
        Console.WriteLine($"  {pair.Key}: {pair.Value:X}");
      }
    }
  }
}
