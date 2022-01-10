using System;
using MemoryManager;

namespace Narrative {
    class Program {
        static void Main ( String[] args ) {
            MonsterHunterWorldMemoryManager mm = new MonsterHunterWorldMemoryManager();
            mm.dump_const_table();
            Console.ReadLine();
        }
    }
}
