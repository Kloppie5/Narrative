using System;
using System.Diagnostics;
using System.IO;

using CultistSimulator;

namespace Narrative {
    class Program {
        static void Main ( String[] args ) {
            String path = Directory.GetCurrentDirectory();
            // (new MonsterHunterWorld.Character(Path.Combine(path, "saves", "character_0.backup"))).Dump();

            CultistSimulatorProcessManager cspm = new CultistSimulatorProcessManager();

            UInt32 unityRootDomain = cspm.GetUnityRootDomain();
            Console.WriteLine($"Unity Root Domain: {unityRootDomain:X}");
            UInt32 assembly = cspm.GetAssemblyInDomain(unityRootDomain, "Assembly-CSharp");
            Console.WriteLine($"Assembly: {assembly:X}");
			UInt32 image = cspm.ReadAbsolute<UInt32>(assembly + 0x44);
			Console.WriteLine($"Image: {image:X8}");
            cspm.EnumImageClassCache(image);
        }
    }
}
