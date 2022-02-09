using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

using CryptOfTheNecroDancer;
using CultistSimulator;
using GTFO;
using MonsterHunterWorld;

namespace Narrative {
    class Program {
        static void Main ( String[] args ) {
            if ( args.Length == 0 ) {
                Console.WriteLine ( "Usage: exe <flag>" );
                return;
            }
            switch ( args[0] ) {
                case "--overlay":
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                    Overlay overlay = new Overlay();
                    Orchestrator orchestrator = new Orchestrator(overlay);
                    Application.Run(overlay);
                    Console.ReadLine();
                    break;
                case "--mhw-dumpsaves":
                    String path = Directory.GetCurrentDirectory();
                    (new MonsterHunterWorld.Character(Path.Combine(path, "Monster Hunter World", "saves", "character_0.backup"))).Dump();
                    (new MonsterHunterWorld.Character(Path.Combine(path, "Monster Hunter World", "saves", "character_1.backup"))).Dump();
                    (new MonsterHunterWorld.Character(Path.Combine(path, "Monster Hunter World", "saves", "character_p25_mjurder.sav"))).Dump();
                    break;
                case "--mhw-live":
                    SaveFile mhwsf = new SaveFile(@"C:\Program Files (x86)\Steam\userdata\49682378\582010\remote\SAVEDATA1000");
                    mhwsf.character1.Dump();
                    break;
                default:
                    Console.WriteLine ( "Unknown flag" );
                    break;
            }
        }
    }
}
