using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

using CryptOfTheNecroDancer;
using CultistSimulator;
using MonsterHunterWorld;

namespace Narrative {
    class Program {
        static void Main ( String[] args ) {
            if ( args.Length == 0 ) {
                Console.WriteLine ( "Usage: exe <flag>" );
                return;
            }
            switch ( args[0] ) {
                case "--cotn":
                    CryptOfTheNecroDancerProcessManager cotndpm = new CryptOfTheNecroDancerProcessManager();
                    cotndpm.Dump();
                    break;
                case "--cultist":
                    CultistSimulatorProcessManager cspm = new CultistSimulatorProcessManager();
                    UInt32 unityRootDomain = cspm.GetUnityRootDomain();
                    Console.WriteLine($"Unity Root Domain: {unityRootDomain:X}");
                    UInt32 assembly = cspm.GetAssemblyInDomain(unityRootDomain, "Assembly-CSharp");
                    Console.WriteLine($"Assembly: {assembly:X}");
                    UInt32 image = cspm.ReadAbsolute<UInt32>(assembly + 0x44);
                    Console.WriteLine($"Image: {image:X8}");
                    cspm.EnumImageClassCache(image);
                    break;
                case "--mhw-memory-manager":
                    MonsterHunterWorldMemoryManager mhw = new MonsterHunterWorldMemoryManager ();
                    break;
                case "--mhw-overlay":
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                    Overlay overlay = new Overlay();
                    Application.Run(overlay);
                    break;
                case "--mhw-dumpsaves":
                    String path = Directory.GetCurrentDirectory();
                    (new MonsterHunterWorld.Character(Path.Combine(path, "saves", "character_0.backup"))).Dump();
                    (new MonsterHunterWorld.Character(Path.Combine(path, "saves", "character_1.backup"))).Dump();
                    (new MonsterHunterWorld.Character(Path.Combine(path, "saves", "character_p25_mjurder.sav"))).Dump();
                    break;
                default:
                    Console.WriteLine ( "Unknown flag" );
                    break;
            }
        }
    }
}
