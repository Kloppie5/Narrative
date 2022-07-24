// using System;
// using System.Collections.Generic;
// using System.Drawing;
// using System.Threading;
// using System.Windows.Forms;

// using Narrative;

// using UInt8 = System.Byte;

// namespace IdleSpiral {

//     public class ProvidenceWidget : Widget {

//         ProcessManager64 manager;
//         bool findingIdleSystem = false;
//         UInt64 IdleSystem = 0;

//         public ProvidenceWidget ( ) {
//             manager = new ProcessManager64();
//         }

//         private void FindIdleSystem ( ) {
//             if ( IdleSystem != 0 )
//                 return;
//             if ( findingIdleSystem )
//                 return;
//             findingIdleSystem = true;

//             new Thread(() => {
//                 UInt64 monoModule = PEHelper.GetModule(manager, "mono.dll", "mono-2.0-bdwgc.dll");
//                 Console.WriteLine($"monoModule: {monoModule:X}");
//                 Dictionary<String, UInt64> exportedFunctions = PEHelper.GetExportedFunctions(manager, monoModule);
//                 foreach ( KeyValuePair<String, UInt64> exportedFunction in exportedFunctions ) {
//                     if ( exportedFunction.Key.Contains("get") )
//                         Console.WriteLine($"{exportedFunction.Key} {exportedFunction.Value:X}");
//                 }

//                 UInt64 rootDomain = manager.ExtractRootDomain(exportedFunctions["mono_get_root_domain"]);

//                 UInt64 AssemblyCSharpAssembly = manager.GetAssemblyInDomain(rootDomain, "Assembly-CSharp");
//                 UInt64 IdleSpiralDomainAssembly = manager.GetAssemblyInDomain(rootDomain, "IdleSpiralDomain");

//                 UInt64 IdleSpiralDomainImage = manager.ReadAbsolute<UInt64>(IdleSpiralDomainAssembly + 0x60);

//                 UInt64 IdleSystemClass = manager.GetClassInImage(IdleSpiralDomainImage, "IdleSystem");
//                 UInt64 IdleSystemClassRuntimeInfo = manager.ReadAbsolute<UInt64>(IdleSystemClass + 0xD0);
//                 UInt64 IdleSystemVTable = manager.ReadAbsolute<UInt64>(IdleSystemClassRuntimeInfo + 0x8);
//                 Console.WriteLine($"   IdleSystemVTable: {IdleSystemVTable:X}");

//                 UInt64 RClass = manager.GetClassInImage(IdleSpiralDomainImage, "R");
//                 UInt64 RClassRuntimeInfo = manager.ReadAbsolute<UInt64>(RClass + 0xD0);
//                 UInt64 RVTable = manager.ReadAbsolute<UInt64>(RClassRuntimeInfo + 0x8);
//                 Console.WriteLine($"   RVTable: {RVTable:X}");

//                 List<UInt64> IdleSystemInstanceCanditates = manager.FindInstancesOfClass(IdleSystemVTable);
//                 Console.WriteLine($"   - InstanceCanditates: {IdleSystemInstanceCanditates.Count}");
//                 foreach ( UInt64 IdleSystemInstanceCanditate in IdleSystemInstanceCanditates ) {
//                     UInt64 distance = manager.ReadAbsolute<UInt64>(IdleSystemInstanceCanditate + 0x10);
//                     UInt64 distanceVTable = manager.ReadAbsolute<UInt64>(distance + 0x0);
//                     if ( distanceVTable == RVTable ) {
//                         Console.WriteLine($"     - {IdleSystemInstanceCanditate:X}");
//                         IdleSystem = IdleSystemInstanceCanditate;
//                         break;
//                     }
//                 }
//             }).Start( );
//         }

//         public override void Paint ( PaintEventArgs e ) {
//             if (!manager.CheckConnected())
//                 return;

//             IntPtr ptr = manager.process.MainWindowHandle;
//             Rect windowRect = new Rect();
//             ProcessManager64.GetWindowRect(ptr, ref windowRect);
//             e.Graphics.DrawRectangle(new Pen(Color.White, 2), windowRect.Left, windowRect.Top, windowRect.Right - windowRect.Left, windowRect.Bottom - windowRect.Top);

//             if ( IdleSystem == 0 ) {
//                 FindIdleSystem();
//                 Console.WriteLine("Scanning for IdleSystem...");
//             } else {
//                 Console.WriteLine($"IdleSystem: {IdleSystem:X}");
//                 UInt64 distance = manager.ReadAbsolute<UInt64>(IdleSystem + 0x10);
//                 Console.WriteLine($"   R: {manager.ReadAbsolute<Double>(distance + 0x40)}");
//                 UInt64 z = manager.ReadAbsolute<UInt64>(IdleSystem + 0x18);
//                 Console.WriteLine($"   Z: {manager.ReadAbsolute<Double>(z + 0x40)}");
//                 UInt64 lineNumber = manager.ReadAbsolute<UInt64>(IdleSystem + 0x20);
//                 UInt64 softPrestige = manager.ReadAbsolute<UInt64>(IdleSystem + 0x28);
//                 UInt64 prestigePoint = manager.ReadAbsolute<UInt64>(IdleSystem + 0x30);
//                 UInt64 prestige = manager.ReadAbsolute<UInt64>(IdleSystem + 0x38);
//                 UInt64 nullPrestigePoint = manager.ReadAbsolute<UInt64>(IdleSystem + 0x40);
//                 UInt64 tornadoPrestigePoint = manager.ReadAbsolute<UInt64>(IdleSystem + 0x48);
//                 UInt64 upgradeContainer = manager.ReadAbsolute<UInt64>(IdleSystem + 0x50);
//                 UInt64 upgradeLevels = manager.ReadAbsolute<UInt64>(IdleSystem + 0x58);
//                 UInt64 parameterContainer = manager.ReadAbsolute<UInt64>(IdleSystem + 0x60);
//                 UInt64 spiralEquipmentContainer = manager.ReadAbsolute<UInt64>(IdleSystem + 0x68);
//                 UInt64 distanceMultiplier = manager.ReadAbsolute<UInt64>(IdleSystem + 0x70);
//                 UInt64 z_multiplier = manager.ReadAbsolute<UInt64>(IdleSystem + 0x78);
//                 UInt64 infiniteSpiralDomain = manager.ReadAbsolute<UInt64>(IdleSystem + 0x80);
//                 UInt64 challengeContainer = manager.ReadAbsolute<UInt64>(IdleSystem + 0x88);
//                 UInt64 challengeManager = manager.ReadAbsolute<UInt64>(IdleSystem + 0x90);
//                 UInt64 achievementManager = manager.ReadAbsolute<UInt64>(IdleSystem + 0x98);
//                 UInt64 achievementPoints = manager.ReadAbsolute<UInt64>(IdleSystem + 0xA0);
//                 UInt64 dailySpiral = manager.ReadAbsolute<UInt64>(IdleSystem + 0xA8);
//                 UInt64 battleCtrl = manager.ReadAbsolute<UInt64>(IdleSystem + 0xB0);
//                 UInt64 ally = manager.ReadAbsolute<UInt64>(IdleSystem + 0xB8);
//                 UInt64 enemyContainer = manager.ReadAbsolute<UInt64>(IdleSystem + 0xC0);
//                 UInt64 rewardContainer = manager.ReadAbsolute<UInt64>(IdleSystem + 0xC8);

//                 PrintRewards(rewardContainer);

//                 UInt64 battleUpgrade = manager.ReadAbsolute<UInt64>(IdleSystem + 0xD0);
//                 UInt64 exp = manager.ReadAbsolute<UInt64>(IdleSystem + 0xD8);
//                 UInt64 spiralDesignEffect = manager.ReadAbsolute<UInt64>(IdleSystem + 0xE0);
//                 UInt64 reactorContainer = manager.ReadAbsolute<UInt64>(IdleSystem + 0xE8);
//                 UInt64 reactorManager = manager.ReadAbsolute<UInt64>(IdleSystem + 0xF0);
//                 UInt64 agent = manager.ReadAbsolute<UInt64>(IdleSystem + 0xF8);
//                 UInt64 timeManager = manager.ReadAbsolute<UInt64>(IdleSystem + 0x100);
//                 UInt64 zCurrencyManager = manager.ReadAbsolute<UInt64>(IdleSystem + 0x108);
//                 UInt64 dto = manager.ReadAbsolute<UInt64>(IdleSystem + 0x110);
//             }
//             // manager.EnumImageClassCache(IdleSpiralDomainImage);
//         }

//         public void PrintRewards ( UInt64 rewardContainer ) {
//             UInt64 rewards_dict = manager.ReadAbsolute<UInt64>(rewardContainer + 0x20);
//             UInt64 entries = manager.ReadAbsolute<UInt64>(rewards_dict + 0x18);
//             UInt64 entries_size = manager.ReadAbsolute<UInt64>(entries + 0x18);
//             String[] enemy_names = new String[] {
//                 "Training",
//                 "Kappa",
//                 "Alpha",
//                 "Beta",
//                 "Pi",
//                 "Gamma",
//                 "Napier"
//             };
//             String[] reward_names = new String[] {
//                 "HP", // 0
//                 "ATK", // 1
//                 "DEF", // 2
//                 "a", // 3
//                 "b", // 4
//                 "c", // 5
//                 "d", // 6
//                 "e", // 7
//                 "k", // 8
//                 "Regenerate", // 9
//                 "Critical Rate", // 10 UNUSED
//                 "Critical Damage", // 11
//                 "a %", // 12
//                 "b %", // 13
//                 "c %", // 14
//                 "d %", // 15
//                 "e %", // 16
//                 "k %", // 17
//                 "HP %", // 18
//                 "ATK %", // 19
//                 "DEF %", // 20
//                 "n", // 21 UNUSED
//                 "v", // 22 UNUSED
//                 "gamma", // 23
//                 "sigma", // 24
//                 "Omega", // 25
//                 "Attack Speed", // 26
//                 "gamma %", // 27
//                 "sigma %", // 28
//                 "Omega %", // 29
//                 "Exp", // 30
//                 "", // 31 UNUSED
//                 "", // 32 UNUSED
//                 "S-Critical Damage" // 33
//             };
//             for ( UInt64 i = 0; i < entries_size; ++i ) {
//                 // Int32 hashCode = manager.ReadAbsolute<Int32>(entries + 0x20 + (i * 0x18));
//                 // Int32 next = manager.ReadAbsolute<Int32>(entries + 0x20 + 0x4 + (i * 0x18));
//                 // UInt64 key = manager.ReadAbsolute<UInt64>(entries + 0x20 + 0x8 + (i * 0x18));
//                 UInt64 value = manager.ReadAbsolute<UInt64>(entries + 0x20 + 0x10 + (i * 0x18));
//                 Console.WriteLine($"{enemy_names[i]}:");

//                 UInt64 reward_list = manager.ReadAbsolute<UInt64>(value + 0x10); // List
//                 UInt64 reward_list_items = manager.ReadAbsolute<UInt64>(reward_list + 0x10);
//                 Int32 reward_list_size = manager.ReadAbsolute<Int32>(reward_list + 0x18);
//                 Console.WriteLine($"   {reward_list_size} rewards:");
//                 for ( UInt64 j = 0; j < (UInt64) reward_list_size; ++j ) {
//                     UInt64 reward = manager.ReadAbsolute<UInt64>(reward_list_items + 0x20 + (j * 0x8));
//                     Double reward_value = manager.ReadAbsolute<Double>(reward + 0x20);
//                     UInt32 reward_type = manager.ReadAbsolute<UInt32>(reward + 0x28);
//                     // UInt32 reward_rarity = manager.ReadAbsolute<UInt32>(reward + 0x2C);
//                     Console.WriteLine($"   {reward_names[reward_type]} {reward_value}");
//                 }
//                 UInt64 candidate_list = manager.ReadAbsolute<UInt64>(value + 0x18); // List
//                 UInt64 candidate_list_items = manager.ReadAbsolute<UInt64>(candidate_list + 0x10);
//                 Int32 candidate_list_size = manager.ReadAbsolute<Int32>(candidate_list + 0x18);
//                 Console.WriteLine($"   {candidate_list_size} candidates:");
//                 for ( UInt64 j = 0; j < (UInt64) candidate_list_size; ++j ) {
//                     UInt64 candidate = manager.ReadAbsolute<UInt64>(candidate_list_items + 0x20 + (j * 0x8));
//                     UInt64 reward = manager.ReadAbsolute<UInt64>(candidate + 0x10);
//                     Double reward_chance = manager.ReadAbsolute<Double>(candidate + 0x20);

//                     Double reward_value = manager.ReadAbsolute<Double>(reward + 0x20);
//                     UInt32 reward_type = manager.ReadAbsolute<UInt32>(reward + 0x28);
//                     UInt32 reward_rarity = manager.ReadAbsolute<UInt32>(reward + 0x2C);

//                     Console.WriteLine($"   {reward_names[reward_type]} {reward_value} {reward_chance}");
//                 }

//             }
//         }
//     }
// }
