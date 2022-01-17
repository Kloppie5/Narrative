using System;
using System.Diagnostics;
using System.IO;

namespace Narrative {
    class Program {
        static void Main ( String[] args ) {
            String path = Directory.GetCurrentDirectory();
            // (new MonsterHunterWorld.Character(Path.Combine(path, "saves", "character_0.backup"))).Dump();

            ProcessManager pm = new ProcessManager("cultistsimulator");

            Console.WriteLine($"Found Unity Root Domain at {pm.GetUnityRootDomain():X8}");

        }
    }
}
