using System;
using System.Collections.Generic;
using System.Text;

namespace Narrative {
  public static class MonoHelper32 {
    public static UInt64 FindMonoModule ( ProcessManager64 manager ) {
      Dictionary<String, UInt64> modules = PEHelper.GetModules64(manager);
      foreach (var (moduleName, moduleBase) in modules) {
        if (moduleName.Contains("mono-2.0-bdwgc.dll"))
          return moduleBase;
      }
      throw new Exception("Could not find mono-2.0-bdwgc.dll");
    }
    public static MonoDomain32 GetRootDomain ( ProcessManager64 manager ) {
      UInt64 monoModule = FindMonoModule(manager);
      UInt64 mono_get_root_domain = PEHelper.FindExportedFunction(manager, monoModule, "mono_get_root_domain");
      MemoryHelper.debug = true;
      if ( MemoryHelper.Match(manager, mono_get_root_domain, "A1 ???????? C3") ) {
        // mov eax, [...]; ret
        UInt32 rootDomainAddress = MemoryHelper.ReadAbsolute<UInt32>(manager, mono_get_root_domain + 1);
        UInt32 rootDomain = MemoryHelper.ReadAbsolute<UInt32>(manager, rootDomainAddress);
        Console.WriteLine($"rootDomain: {rootDomain:X}");
        return MemoryHelper.ReadAbsolute<MonoDomain32>(manager, rootDomain);
      }
      throw new Exception("Could not match mono_get_root_domain");
    }
  }
}
