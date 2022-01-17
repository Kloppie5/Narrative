﻿using System;
using System.IO;

namespace Narrative {
    class Program {
        static void Main ( String[] args ) {
            // MonsterHunterWorldMemoryManager mm = new MonsterHunterWorldMemoryManager();
            // mm.debug_dump();
            // TODO: separate the user id
            // MonsterHunterWorldSaveFileEditor mhwsfe = new MonsterHunterWorldSaveFileEditor();
            // @"C:\Program Files (x86)\Steam\userdata\49682378\582010\remote\SAVEDATA1000"
            // mhwsfe.DecryptGlobal();
            // mhwsfe.DecryptCharacters();
            // mhwsfe.dump_character(2);

            String path = Directory.GetCurrentDirectory();

            //String[] files = Directory.GetFiles(Path.Combine(path, "saves"), "*.sav");
            //for ( int i = 0 ; i < files.Length - 1 ; ++i ) {
            //    MonsterHunterWorldSaveFileEditor.smart_diff_characters(files[i], files[i + 1], 16);
            //    Console.ReadLine();
            //}

            // mhwsfe.load_character(2, path);
            // mhwsfe.save_character(0, Path.Combine(path, "saves", "character_0.backup"));

            MonsterHunterWorld.Character c = new MonsterHunterWorld.Character(Path.Combine(path, "saves", "character_0.backup"));
            c.Dump();

            /*
            MonsterHunterWorldSaveFileEditor.smart_diff_characters(
                Path.Combine(path, "saves", "character_0.backup"),
                Path.Combine(path, "saves", "character_1.backup"),
                16
            );
            */

        }
    }
}
