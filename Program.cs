using System;
using System.Diagnostics;
using System.IO;

namespace Narrative {
    class Program {
        static void Main ( String[] args ) {
            String path = Directory.GetCurrentDirectory();
            // (new MonsterHunterWorld.Character(Path.Combine(path, "saves", "character_0.backup"))).Dump();

            ProcessManager pm = new ProcessManager("cultistsimulator");

            UInt32 unityRootDomain = pm.GetUnityRootDomain();
            Console.WriteLine($"Unity Root Domain: {unityRootDomain:X}");
            UInt32 assembly = pm.GetAssemblyInDomain(unityRootDomain, "Assembly-CSharp");
            Console.WriteLine($"Assembly: {assembly:X}");

        }
    }
}
