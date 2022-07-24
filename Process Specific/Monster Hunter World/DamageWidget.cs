// using System;
// using Narrative;

// namespace MonsterHunterWorld {

//     public class DamageWidget : TextWidget {

//         ProcessManager manager;
//         public DamageWidget ( ) {
//             manager = new ProcessManager();

//             AddLine(() => DamageString(0));
//             AddLine(() => DamageString(1));
//             AddLine(() => DamageString(2));
//             AddLine(() => DamageString(3));
//         }
//         public String DamageString ( Int64 playerIndex ) {
//             if (!manager.CheckConnected())
//                 return "";

//             UInt32 totalDamage = 0;
//             for (Int64 j = 0; j < 4; ++j)
//                 totalDamage += manager.ReadRelative<UInt32>(0x5224BF8, 0x258, 0x38, 0x450, 0x8, 0x48 + j * 0x2A0);
//             if (totalDamage <= 0)
//                 return "";

//             String PartyMemberName = manager.ReadRelativeUTF8String(0x5224BF8, 0x68, -0x22B7 + playerIndex * 0x1C0);
//             UInt16 HR              = manager.ReadRelative<UInt16>(0x5224BF8, 0x68, -0x22B7 + playerIndex * 0x1C0 + 0x27);
//             UInt16 MR              = manager.ReadRelative<UInt16>(0x5224BF8, 0x68, -0x22B7 + playerIndex * 0x1C0 + 0x29);
//             UInt32 playerDamage    = manager.ReadRelative<UInt32>(0x5224BF8, 0x258, 0x38, 0x450, 0x8, 0x48 + playerIndex * 0x2A0);

//             if ( playerDamage <= 0 )
//                 return "";

//             return $"{PartyMemberName} ({MR} | {HR}): {playerDamage} ({playerDamage * 100 / totalDamage}%)";
//         }
//     }
// }
