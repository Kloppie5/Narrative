using System;
using MemoryManager;

namespace Narrative {
    class Program {
        static void Main ( String[] args ) {
            MonsterHunterWorldMemoryManager mm = new MonsterHunterWorldMemoryManager();
            mm.debug_dump();
        }
    }
}
