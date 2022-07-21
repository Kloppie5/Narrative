using System;
using System.Collections.Generic;
using System.Text;

namespace Narrative {
  public static class MonoHelper32 {
    public static Boolean debug = false;
    public static UInt64 FindMonoModule ( ProcessManager64 manager ) {
      Dictionary<String, UInt64> modules = PEHelper.GetModules64(manager);
      foreach (var (moduleName, moduleBase) in modules) {
        if (moduleName.Contains("mono-2.0-bdwgc.dll")) {
          if ( debug )
            Console.WriteLine($"Found mono-2.0-bdwgc.dll: {moduleBase:X}");
          return moduleBase;
        }
      }
      throw new Exception("Could not find mono-2.0-bdwgc.dll");
    }
    public static MonoDomain32 GetRootDomain ( ProcessManager64 manager ) {
      UInt64 monoModule = FindMonoModule(manager);
      UInt64 mono_get_root_domain = PEHelper.FindExportedFunction(manager, monoModule, "mono_get_root_domain");
      if ( debug )
        Console.WriteLine($"mono_get_root_domain: {mono_get_root_domain:X}");
      MemoryHelper.debug = true;
      if ( MemoryHelper.Match(manager, mono_get_root_domain, "A1 ???????? C3") ) {
        // mov eax, [...]; ret
        UInt32 rootDomainAddress = MemoryHelper.ReadAbsolute<UInt32>(manager, mono_get_root_domain + 1);
        UInt32 rootDomain = MemoryHelper.ReadAbsolute<UInt32>(manager, rootDomainAddress);
        if ( debug )
          Console.WriteLine($"rootDomain: {rootDomain:X}");
        for ( UInt32 offset = 0; offset < 0x60 ; offset += 4 ) {
          Console.WriteLine($"{offset:X}: {MemoryHelper.ReadAbsolute<UInt32>(manager, rootDomain + offset):X}");
        }
        return MemoryHelper.ReadAbsolute<MonoDomain32>(manager, rootDomain);
      }
      throw new Exception("Could not match mono_get_root_domain");
    }
    /*
    public static UInt64 GetAssemblyInDomain ( ProcessManager64 manager, UInt64 domain, String name ) {
      MonoAssembly32 Read
      String friendly_name = ReadAbsoluteUTF8String(ReadAbsolute<UInt64>(domain + 0xD8));
      UInt64 domain_assemblies_GSList_pointer = ReadAbsolute<UInt64>(domain + 0xC8);
      for ( UInt64 CURR = domain_assemblies_GSList_pointer ; CURR != 0 ; CURR = ReadAbsolute<UInt64>(CURR+0x8) ) {
        UInt64 assembly = ReadAbsolute<UInt64>(CURR);
        UInt64 assembly_name_pointer = ReadAbsolute<UInt64>(assembly + 0x10);
        String assembly_name = ReadAbsoluteUTF8String(assembly_name_pointer);
        if ( assembly_name.Equals(name) )
          return assembly;
      }
      return 0;
    }
    */
  }
}
