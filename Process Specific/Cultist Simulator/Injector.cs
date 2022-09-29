using System;
using System.Collections.Generic;

using Narrative;

namespace CultistSimulator {
  public class Injector : MonoInjector32 {

    Int32 mainAssembly = -1;
    Int32 mainImage = -1;

    public Dictionary<String, Int32> watchmanDict = new Dictionary<String, Int32>();
    public Dictionary<String, Int32> compendiumDict = new Dictionary<String, Int32>();
    public HashSet<Int32> registeredSpheresSet = new HashSet<Int32>();

    public List<Int32> tokens = new List<Int32>();
    public Dictionary<String, Int32> situationsDict = new Dictionary<String, Int32>();
    public Dictionary<String, List<Int32>> elementStacksDict = new Dictionary<String, List<Int32>>();

    public Injector ( ProcessManager64 manager ) : base ( manager ) {
      mainAssembly = DomainGetAssemblyByName(_rootDomain, "SecretHistories.Main");
      mainImage = AssemblyGetImage(mainAssembly);

      ExtractWatchman();
      ExtractCompendium();
      ExtractHornedAxe();
      ExtractTabletopSphere();
    }
    public void ExtractWatchman ( ) {
      Int32 watchman = ImageGetClassByName(mainImage, "SecretHistories.UI", "Watchman");
      Int32 vtable = ClassGetVTable(watchman);
      Int32 staticFieldData = VTableGetStaticFieldData(vtable);
      Int32 registered = MemoryHelper.ReadAbsolute<Int32>(_manager, staticFieldData);
      watchmanDict = ReadDictionaryTypeObject(registered);
    }
    public void ExtractCompendium ( ) {
      Int32 compendium = watchmanDict["Compendium"];
      Int32 entityStores = MemoryHelper.ReadAbsolute<Int32>(_manager, compendium + 0x8);
      compendiumDict = ReadDictionaryTypeObject(entityStores);
    }
    public void ExtractHornedAxe ( ) {
      Int32 hornedaxe = watchmanDict["HornedAxe"];
      Int32 registeredSpheres = MemoryHelper.ReadAbsolute<Int32>(_manager, hornedaxe + 0x1C);
      registeredSpheresSet = ReadHashSetObject(registeredSpheres);
    }
    public void ExtractTabletopSphere ( ) {
      foreach ( var sphere in registeredSpheresSet ) {
        Int32 valueVTable = MemoryHelper.ReadAbsolute<Int32>(_manager, sphere);
        Int32 valueClass = MemoryHelper.ReadAbsolute<Int32>(_manager, valueVTable);
        String valueClassName = MemoryHelper.ReadAbsoluteUTF8String(_manager, MemoryHelper.ReadAbsolute<Int32>(_manager, valueClass + 0x2C));

        if ( valueClassName == "TabletopSphere" ) {
          Int32 tokenList = MemoryHelper.ReadAbsolute<Int32>(_manager, sphere + 0x28);
          tokens = ReadListObject(tokenList);
          break;
        }
      }

      situationsDict.Clear();
      elementStacksDict.Clear();

      foreach ( var token in tokens ) {
        Console.WriteLine(TokenToPrettyString(token));
        Int32 payload = MemoryHelper.ReadAbsolute<Int32>(_manager, token + 0x28);
        Int32 payloadVTable = MemoryHelper.ReadAbsolute<Int32>(_manager, payload);
        Int32 payloadClass = MemoryHelper.ReadAbsolute<Int32>(_manager, payloadVTable);
        String payloadClassName = MemoryHelper.ReadAbsoluteUTF8String(_manager, MemoryHelper.ReadAbsolute<Int32>(_manager, payloadClass + 0x2C));

        if ( payloadClassName == "Situation" ) {
          Int32 verb = MemoryHelper.ReadAbsolute<Int32>(_manager, payload + 0x1C);
          Int32 label = MemoryHelper.ReadAbsolute<Int32>(_manager, verb + 0x1C);
          String labelString = MemoryHelper.ReadAbsoluteMonoWideString(_manager, label);
          situationsDict[labelString] = token;
        } else if ( payloadClassName == "ElementStack" ) {
          Int32 element = MemoryHelper.ReadAbsolute<Int32>(_manager, payload + 0x14);
          Int32 label = MemoryHelper.ReadAbsolute<Int32>(_manager, element + 0x1C);
          String labelString = MemoryHelper.ReadAbsoluteMonoWideString(_manager, label);
          if ( !elementStacksDict.ContainsKey(labelString) )
            elementStacksDict[labelString] = new List<Int32>();
          elementStacksDict[labelString].Add(token);
        }
      }
    }
    public String TokenToPrettyString ( Int32 token ) {
      Int32 state = MemoryHelper.ReadAbsolute<Int32>(_manager, token + 0x20);
      String stateString = MemoryHelper.ReadAbsoluteMonoWideString(_manager, state);
      Int32 fullPath = MemoryHelper.ReadAbsolute<Int32>(_manager, token + 0x24);
      String fullPathString = MemoryHelper.ReadAbsoluteMonoWideString(_manager, fullPath);
      Int32 payload = MemoryHelper.ReadAbsolute<Int32>(_manager, token + 0x28);
      Int32 payloadVTable = MemoryHelper.ReadAbsolute<Int32>(_manager, payload);
      Int32 payloadClass = MemoryHelper.ReadAbsolute<Int32>(_manager, payloadVTable);
      String payloadClassName = MemoryHelper.ReadAbsoluteUTF8String(_manager, MemoryHelper.ReadAbsolute<Int32>(_manager, payloadClass + 0x2C));

      if ( payloadClassName == "ElementStack" ) {
        Int32 id = MemoryHelper.ReadAbsolute<Int32>(_manager, payload + 0x10);
        String idString = MemoryHelper.ReadAbsoluteMonoWideString(_manager, id);
        Int32 element = MemoryHelper.ReadAbsolute<Int32>(_manager, payload + 0x14);
        Int32 label = MemoryHelper.ReadAbsolute<Int32>(_manager, element + 0x1C);
        String labelString = MemoryHelper.ReadAbsoluteMonoWideString(_manager, label);
        Int32 description = MemoryHelper.ReadAbsolute<Int32>(_manager, element + 0x20);
        String descriptionString = MemoryHelper.ReadAbsoluteMonoWideString(_manager, description);
        Int32 quantity = MemoryHelper.ReadAbsolute<Int32>(_manager, payload + 0x40);
        return $"Stack({idString}): {quantity}x {labelString}";
      }
      if ( payloadClassName == "Situation" ) {
        Int32 recipe = MemoryHelper.ReadAbsolute<Int32>(_manager, payload + 0x14);
        Int32 verb = MemoryHelper.ReadAbsolute<Int32>(_manager, payload + 0x1C);
        Int32 label = MemoryHelper.ReadAbsolute<Int32>(_manager, verb + 0x1C);
        String labelString = MemoryHelper.ReadAbsoluteMonoWideString(_manager, label);
        Int32 id = MemoryHelper.ReadAbsolute<Int32>(_manager, payload + 0x20);
        String idString = MemoryHelper.ReadAbsoluteMonoWideString(_manager, id);
        return $"Situation({idString}): {labelString}";
      }
      return $"{payloadClassName}({fullPathString})";
    }

    public void Something ( Int32 dictionary ) {
        /*
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
        */
    }

    #region Situation
    public void Open ( Int32 situation ) {
      Int32 situationVTable = MemoryHelper.ReadAbsolute<Int32>(_manager, situation);
      Int32 situationClass = MemoryHelper.ReadAbsolute<Int32>(_manager, situationVTable);

      Int32 Open = ClassGetMethodByName(situationClass, "Open");

      Assembler assembler = new Assembler();
      assembler.debug = true;

      AssembleMonoThreadAttach(assembler);

      AssembleMonoRuntimeInvoke(assembler, Open, situation);

      assembler.RET();

      Execute(assembler.finalize());
    }
    #endregion
    #region Token
    // ExecuteHeartbeat(float seconds, float metaseconds)
    // this._manifestation.Highlight(HighlightType.Hover (2), this._payload);
    // InteractWithIncomingToken(Token incomingToken, PointerEventData eventData)
    #endregion
  }
}
