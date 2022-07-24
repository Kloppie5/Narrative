// using System;

// namespace MonsterHunterWorld {
//     public class ProcessManager : Narrative.ProcessManager64 {
//         /**
//         public static UInt64 QuestPointer = 0x506F240;
//         public static UInt64 HunterBasePointer = 0x5073E80;
//         public static UInt64 LobbyPointer = 0x5224BF8;
//         public static UInt64 HunterSize = 0x27E9F0;


//         define(pl_item_params,MonsterHunterWorld.exe+5072490)
//         define(pl_skill_params,MonsterHunterWorld.exe++5072498)
//         define(pl_mantle_params,MonsterHunterWorld.exe+5072588)
//         define(player_data,MonsterHunterWorld.exe+506D270)
//         define(world_data,MonsterHunterWorld.exe+5073D60)
//         define(session_data,MonsterHunterWorld.exe+5224BF8)
//         define(gathering_data,MonsterHunterWorld.exe+5072770)
//         define(monster_data,MonsterHunterWorld.exe+5074180)
//         define(armor_skills,MonsterHunterWorld.exe+5072578)
//         define(save_data,MonsterHunterWorld.exe+5073E80)

//         define(item_caps,MonsterHunterWorld.exe+5072530)
//         define(steamworks,MonsterHunterWorld.exe+4FE79C0)

//         define(session_quest,MonsterHunterWorld.exe+506F240)
//         define(coating_data,MonsterHunterWorld.exe+5072630)
//         define(world_session2,MonsterHunterWorld.exe+506E3D0)
//         define(harvestbox_data,MonsterHunterWorld.exe+5072C78)
//         define(steamworks_loottable,MonsterHunterWorld.exe+5073028)
//         define(armor_data,MonsterHunterWorld.exe+5072658)


//         // UInt32 Playtime = memoryManager.Read<UInt32>(address + 0x17FDE8);   // 17FDE8 Playtime
//             // UInt32 Quests = memoryManager.Read<UInt32>(address +	0x17FDEC);     // 17FDEC Quests
//             // UInt32 Tracks = memoryManager.Read<UInt32>(address + 0x17FDF0);     // 17FDF0 Tracks

//             UInt32 AncientForest  = memoryManager.Read<UInt32>(address + 0x27B928); // 0x27B928 Forest
//             UInt32 WildspireWaste = memoryManager.Read<UInt32>(address + 0x27B92C); // 0x27B92C Wildspire
//             UInt32 CoralHighlands = memoryManager.Read<UInt32>(address + 0x27B930); // 0x27B930 Coral
//             UInt32 RottenVale     = memoryManager.Read<UInt32>(address + 0x27B934); // 0x27B934 Rotten
//             UInt32 Volcanic       = memoryManager.Read<UInt32>(address + 0x27B938); // 0x27B938 Volcano
//             UInt32 Tundra         = memoryManager.Read<UInt32>(address + 0x27B93C); // 0x27B93C Tundra
//             UInt32 Max            = memoryManager.Read<UInt32>(address + 0x27B948); // 0x27B948 Max
//             UInt32 Sum = AncientForest + WildspireWaste + CoralHighlands + RottenVale + Volcanic + Tundra;

//             Console.WriteLine(
//                 $"Guiding Lands {Sum / 10000.0f + 6:00.0000}/{Max / 10000.0f + 12:00.0000}\n" +
//                 $"  Ancient Forest:  {AncientForest / 10000.0f + 1:0.0000}\n" +
//                 $"  Wildspire Waste: {WildspireWaste / 10000.0f + 1:0.0000}\n" +
//                 $"  Coral Highlands: {CoralHighlands / 10000.0f + 1:0.0000}\n" +
//                 $"  Rotten Vale:     {RottenVale / 10000.0f + 1:0.0000}\n" +
//                 $"  Volcanic:        {Volcanic / 10000.0f + 1:0.0000}\n" +
//                 $"  Tundra:          {Tundra / 10000.0f + 1:0.0000}\n"
//             );
//         }

//         public override String ToString ( ) {
//             String HunterName     = memoryManager.ReadString(address + 0x50, 16); // 50-90 Hunter Name
//             UInt32 HR             = memoryManager.Read<UInt32>(address + 0x90);   // 90-94 HR
//             UInt32 Zenny          = memoryManager.Read<UInt32>(address + 0x94);   // 94-98 Zenny
//             UInt32 ResearchPoints = memoryManager.Read<UInt32>(address + 0x98);   // 98-9C Research Points
//             UInt32 HRExperience   = memoryManager.Read<UInt32>(address + 0x9C);   // 9C-A0 HR Experience
//             UInt32 PlayTime       = memoryManager.Read<UInt32>(address + 0xA0);   // A0-A4 Playtime (seconds)
//                                                                                // A4-D4 <<<<
//             UInt32 MR             = memoryManager.Read<UInt32>(address + 0xD4);   // D4-D8 MR
//             UInt32 MRExperience   = memoryManager.Read<UInt32>(address + 0xD8);   // D8-DC MR Experience

//             UInt32 steam = memoryManager.Read<UInt32>(address + 0x102FE0);

//             Int16 defenseboost = memoryManager.Read<Int16>(new RelativeMultiLevelPointer(0x5072498, 0xB72));
//         */
//         public ProcessManager ( ) : base("MonsterHunterWorld") {
//             Console.WriteLine($"Initialized MonsterHunterWorldMemoryManager");
//         }

//         public void debug_dump () {
//             UInt64 pl_params = ReadRelative<UInt64>(0x5072488); // const_table[3]
//             UInt64 pl_item_param = ReadRelative<UInt64>(0x5072490); // const_table[4]
//             dump_pl_skill_params();
//         }
//         public void dump_pl_skill_params () {
//             UInt64 pl_skill_param = ReadRelative<UInt64>(0x5072498); // const_table[5]

//             UInt16 HealthBoost0 = ReadAbsolute<UInt16>(pl_skill_param + 0xB58);
//             UInt16 HealthBoost1 = ReadAbsolute<UInt16>(pl_skill_param + 0xB5A);
//             UInt16 HealthBoost2 = ReadAbsolute<UInt16>(pl_skill_param + 0xB5C);
//             UInt16 HealthBoost3 = ReadAbsolute<UInt16>(pl_skill_param + 0xB5E);
//             UInt16 HealthBoost4 = ReadAbsolute<UInt16>(pl_skill_param + 0xB60);
//             UInt16 HealthBoost5 = ReadAbsolute<UInt16>(pl_skill_param + 0xB62);

//             UInt16 StaminaBoost0 = ReadAbsolute<UInt16>(pl_skill_param + 0xB64);
//             UInt16 StaminaBoost1 = ReadAbsolute<UInt16>(pl_skill_param + 0xB66);
//             UInt16 StaminaBoost2 = ReadAbsolute<UInt16>(pl_skill_param + 0xB68);

//             UInt16 AttackBoost0 = ReadAbsolute<UInt16>(pl_skill_param + 0xB6A);
//             UInt16 AttackBoost1 = ReadAbsolute<UInt16>(pl_skill_param + 0xB6C);
//             UInt16 AttackBoost2 = ReadAbsolute<UInt16>(pl_skill_param + 0xB6E);
//             UInt16 AttackBoost3 = ReadAbsolute<UInt16>(pl_skill_param + 0xB70);

//             UInt16 DefenseBoost0 = ReadAbsolute<UInt16>(pl_skill_param + 0xB72);
//             UInt16 DefenseBoost1 = ReadAbsolute<UInt16>(pl_skill_param + 0xB74);
//             UInt16 DefenseBoost2 = ReadAbsolute<UInt16>(pl_skill_param + 0xB76);
//             UInt16 DefenseBoost3 = ReadAbsolute<UInt16>(pl_skill_param + 0xB78);

//             UInt16 ElementalBoost0 = ReadAbsolute<UInt16>(pl_skill_param + 0xB7A);
//             UInt16 ElementalBoost1 = ReadAbsolute<UInt16>(pl_skill_param + 0xB7C);
//             UInt16 ElementalBoost2 = ReadAbsolute<UInt16>(pl_skill_param + 0xB7E);
//             UInt16 ElementalBoost3 = ReadAbsolute<UInt16>(pl_skill_param + 0xB80);

//             Console.WriteLine($"Health Boosts: {HealthBoost0} {HealthBoost1} {HealthBoost2} {HealthBoost3} {HealthBoost4} {HealthBoost5}");
//             Console.WriteLine($"Stamina Boosts: {StaminaBoost0} {StaminaBoost1} {StaminaBoost2}");
//             Console.WriteLine($"Attack Boosts: {AttackBoost0} {AttackBoost1} {AttackBoost2} {AttackBoost3}");
//             Console.WriteLine($"Defense Boosts: {DefenseBoost0} {DefenseBoost1} {DefenseBoost2} {DefenseBoost3}");
//             Console.WriteLine($"Elemental Boosts: {ElementalBoost0} {ElementalBoost1} {ElementalBoost2} {ElementalBoost3}");
//         }

//         public void dump_const_table () {
//             // MonsterHunterWorld.exe+506FA00~5072450
//             // 5072498

//             for ( UInt64 index = 0 ; index <= 462 ; ++index ) {
//                 UInt64 object_description_pointer = ReadRelative<UInt64>(0x506FA00 + index * 16);
//                 UInt64 name_pointer = ReadRelative<UInt64>(0x506FA00 + index * 16 + 8);
//                 String s = ReadString(name_pointer, 64);
//                 UInt64 object_pointer = ReadRelative<UInt64>(0x5072470 + index * 8);

//                 // TODO: Figure out;;
//                 /**
//                     The object pointers dont link up for index 462 ~ 674 when reaching jpn and eng text objects
//                 */
//                 UInt64 unknown_pointer = ReadAbsolute<UInt64>(object_pointer);
//                 UInt32 check_5352 = ReadAbsolute<UInt32>(object_pointer + 0x8);
//                 String s2 = ReadString(object_pointer + 0xC, 80);

//                 Console.WriteLine($"{index} : {s} / {s2} described at {object_description_pointer:X} with {unknown_pointer:X} at {object_pointer:X}");
//             }
//         }
//     }
// }
