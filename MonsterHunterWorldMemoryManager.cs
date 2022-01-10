using System;

namespace MemoryManager {
    public class MonsterHunterWorldMemoryManager : MemoryManager64 {
        /**
        public static UInt64 QuestPointer = 0x506F240;
		public static UInt64 HunterBasePointer = 0x5073E80;
		public static UInt64 LobbyPointer = 0x5224BF8;
		public static UInt64 HunterSize = 0x27E9F0;



        // UInt32 Playtime = memoryManager.Read<UInt32>(address + 0x17FDE8);   // 17FDE8 Playtime
			// UInt32 Quests = memoryManager.Read<UInt32>(address +	0x17FDEC);     // 17FDEC Quests
			// UInt32 Tracks = memoryManager.Read<UInt32>(address + 0x17FDF0);     // 17FDF0 Tracks

			UInt32 AncientForest  = memoryManager.Read<UInt32>(address + 0x27B928); // 0x27B928 Forest
			UInt32 WildspireWaste = memoryManager.Read<UInt32>(address + 0x27B92C); // 0x27B92C Wildspire
			UInt32 CoralHighlands = memoryManager.Read<UInt32>(address + 0x27B930); // 0x27B930 Coral
			UInt32 RottenVale     = memoryManager.Read<UInt32>(address + 0x27B934); // 0x27B934 Rotten
			UInt32 Volcanic       = memoryManager.Read<UInt32>(address + 0x27B938); // 0x27B938 Volcano
			UInt32 Tundra         = memoryManager.Read<UInt32>(address + 0x27B93C); // 0x27B93C Tundra
			UInt32 Max            = memoryManager.Read<UInt32>(address + 0x27B948); // 0x27B948 Max
			UInt32 Sum = AncientForest + WildspireWaste + CoralHighlands + RottenVale + Volcanic + Tundra;

			Console.WriteLine(
				$"Guiding Lands {Sum / 10000.0f + 6:00.0000}/{Max / 10000.0f + 12:00.0000}\n" +
				$"  Ancient Forest:  {AncientForest / 10000.0f + 1:0.0000}\n" +
				$"  Wildspire Waste: {WildspireWaste / 10000.0f + 1:0.0000}\n" +
				$"  Coral Highlands: {CoralHighlands / 10000.0f + 1:0.0000}\n" +
				$"  Rotten Vale:     {RottenVale / 10000.0f + 1:0.0000}\n" +
				$"  Volcanic:        {Volcanic / 10000.0f + 1:0.0000}\n" +
				$"  Tundra:          {Tundra / 10000.0f + 1:0.0000}\n"
			);
		}

		public override String ToString ( ) {
			String HunterName     = memoryManager.ReadString(address + 0x50, 16); // 50-90 Hunter Name
			UInt32 HR             = memoryManager.Read<UInt32>(address + 0x90);   // 90-94 HR
			UInt32 Zenny          = memoryManager.Read<UInt32>(address + 0x94);   // 94-98 Zenny
			UInt32 ResearchPoints = memoryManager.Read<UInt32>(address + 0x98);   // 98-9C Research Points
			UInt32 HRExperience   = memoryManager.Read<UInt32>(address + 0x9C);   // 9C-A0 HR Experience
			UInt32 PlayTime       = memoryManager.Read<UInt32>(address + 0xA0);   // A0-A4 Playtime (seconds)
																			   // A4-D4 <<<<
			UInt32 MR             = memoryManager.Read<UInt32>(address + 0xD4);   // D4-D8 MR
			UInt32 MRExperience   = memoryManager.Read<UInt32>(address + 0xD8);   // D8-DC MR Experience

			UInt32 steam = memoryManager.Read<UInt32>(address + 0x102FE0);
        */
        public MonsterHunterWorldMemoryManager ( ) : base("MonsterHunterWorld") {
            Console.WriteLine($"Initialized MonsterHunterWorldMemoryManager");
        }

        public void dump_const_table () {
            // MonsterHunterWorld.exe+506FA00~5072450
            // 5072498

            UInt64 const_table = BaseAddress + 0x506FA00;
            for ( UInt64 address = const_table ; address < BaseAddress + 0x5072450 ; address += 8 ) {
                UInt64 points_to = ReadAbsolute<UInt64>(address);
                String s = ReadString(points_to, 64);
                Console.WriteLine($"{address:X}: {s}");
            }

            UInt64 pl_skill_param_address = ReadAbsolute<UInt64>(const_table + 0x50);
            UInt64 pl_skill_param_string_address = ReadAbsolute<UInt64>(const_table + 0x58);

            String pl_skill_param_string = ReadString(pl_skill_param_string_address, 64);
            Console.WriteLine($"{pl_skill_param_string} at {pl_skill_param_address:X}");

        }
    }
}
