using System;

namespace MemoryManager {
    public class MonsterHunterWorldMemoryManager : MemoryManager64 {
        public MonsterHunterWorldMemoryManager ( ) : base("MonsterHunterWorld") {
            Console.WriteLine($"Initialized MonsterHunterWorldMemoryManager");
        }
    }
}
