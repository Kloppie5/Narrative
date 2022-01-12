using System;

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
            mhwsfe.dump_character(0);
        }
    }
}
