using System;
using System.IO;

using MonsterHunterWorld;

namespace Narrative {
    class Program {
        static void Main ( String[] args ) {
            // MonsterHunterWorldMemoryManager mm = new MonsterHunterWorldMemoryManager();
            // mm.debug_dump();
            // TODO: separate the user id
            MonsterHunterWorldSaveFileEditor mhwsfe = new MonsterHunterWorldSaveFileEditor(@"C:\Program Files (x86)\Steam\userdata\49682378\582010\remote\SAVEDATA1000");
            mhwsfe.DecryptGlobal();
            mhwsfe.DecryptCharacters();
            // mhwsfe.dump_character(2);
            String path = Directory.GetCurrentDirectory();
            String path1 = Path.Combine(path, "saves", $"character_p00_empty.sav");
            String path2 = Path.Combine(path, "saves", $"character_p01_default.sav");
            mhwsfe.diff_characters(path1, path2);
            // mhwsfe.load_character(2, path);
            // mhwsfe.save_character(2, path);
        }
    }
}
