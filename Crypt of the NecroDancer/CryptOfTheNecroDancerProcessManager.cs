using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using Narrative;

namespace CryptOfTheNecroDancer {
    class CryptOfTheNecroDancerProcessManager : ProcessManager {

        /*
            Crypt of the NecroDancer v2.59
        */
        Mapping<UInt32> mapping = new Mapping<UInt32>();
        public void Init_Mapping ( ) {
            // 433468 Stats
            // 433954 numStats
            // 433958 musicIndex2
            // 43395C musicIndex

            // [...]

            // Level variables:
            // > 433EC4 Array<Boolean> allCharsCompletion
            // > 433EC8 Array<Boolean> allCharsCompletionDLC
            // > 433ECC String todaysRandSeedString
            // > 433ED0 String randSeedString
            // > 433ED4 String justUnlocked
            // > 433ED8 Array<Single> constMapLightValues
            // > 433EDC Array<Single> mapLightValues

            // 433EE0 String GAMEDATA_VERSION
            mapping.Add("GAMEDATA_VERSION", new AddressRange<UInt32>("Memory", 0x433EE0));

            // Player variables:
            // > 433EE4 Array<Int32> AltHeadWidths
            // > 433EE8 Array<Int32> AltHeadHeights
            // > 433EEC String lastDeadlyDamageSource  <<<<< Cause of death

            // Audio variables:
            // > 433EF0 String songName
            // > 433EF4 String beatDataString
            // > 433EF8 Array<Int32> beatIndicatorData
            // > 433EFC Array<Int32> beatIndicatorFade
            // > 433F00 String beatDataString2
            // > 433F04 String lastSongName
            // > 433F08 String customPlayList

            // Input variables:
            mapping.Add("stickLeft",  new AddressRange<UInt32>("Memory", 0x433F0C)); // Array<Boolean>
            mapping.Add("stickRight", new AddressRange<UInt32>("Memory", 0x433F10)); // Array<Boolean>
            mapping.Add("stickUp",    new AddressRange<UInt32>("Memory", 0x433F14)); // Array<Boolean>
            mapping.Add("stickDown",  new AddressRange<UInt32>("Memory", 0x433F18)); // Array<Boolean>
            // > 433F1C Array<Single> lastJoyX
            // > 433F20 Array<Single> lastJoyY
            // > 433F24 Array<Boolean> stickLeft2
            // > 433F28 Array<Boolean> stickRight2
            // > 433F2C Array<Boolean> stickUp2
            // > 433F30 Array<Boolean> stickDown2
            // > 433F34 Array<Single> lastJoyX2
            // > 433F38 Array<Single> lastJoyY2
            mapping.Add("movementBuffer", new AddressRange<UInt32>("Memory", 0x433F3C)); // Array<Int32>
            // > 433F40 Array<Int32> movementBufferFrame
            // > 433F44 Array<Int32> offbeatMovementBuffer
            // > 433F48 Array<Int32> offbeatMovementBufferFrame
            // > 433F4C Array<Int32> lastBeatMovedOn
            // > 433F50 Array<Int32> lastOffbeatMovedOn
            // > 433F54 Array<Int32> lastBeatMissed
            // > 433F58 Array<Int32> punishmentBeatToSkip
            // > 433F5C Array<Int32> punishmentBeatToSkipQueue
            mapping.Add("keysHitLastFrame", new AddressRange<UInt32>("Memory", 0x433F60)); // Array<Boolean>
            // > 433F64 Array<Boolean> keysHit2FramesAgo

            mapping.Add("bb_controller_game_players", new AddressRange<UInt32>("Memory", 0x433F68)); // Array<Player>

            // Item variables:
            // > 433F6C String lastChestItemClass1
            // > 433F70 String lastChestItemClass2
            // > 433F74 Array<Item> itemPoolChest
            // > 433F78 Array<Item> itemPoolChest2
            // > 433F7C Array<Item> itemPoolLockedChest
            // > 433F80 Array<Item> itemPoolLockedChest2
            // > 433F84 Array<Item> itemPoolAnyChest
            // > 433F88 Array<Item> itemPoolAnyChest2
            // > 433F8C Array<Item> itemPoolShop
            // > 433F90 Array<Item> itemPoolShop2
            // > 433F94 Array<Item> itemPoolLockedShop
            // > 433F98 Array<Item> itemPoolLockedShop2
            // > 433F9C Array<Item> itemPoolUrn
            // > 433FA0 Array<Item> itemPoolUrn2

            // 433FA4 String lastSaleItemClass1
            // 433FA8 String lastSaleItemClass2

            // 433FAC Array<Boolean> waitingForFirstMovement

            // Spells:
            // > 433FB0 String spellSlot1
            // > 433FB4 String spellSlot2
            // > 433FB8 Array<Sprite> fireballInWorld

            // 433FBC Array<Int32> bb_controller_game_beatData
            // 433FC0 String lastSavedReplayFile
            // 433FC4 Array<Int32> bb_controller_game_lastPlayerMoveBeat
            // 433FC8 NIL

            // [...]

            // 435578 List dopplegangers
            // 43557C Boolean showingBossIntro
            // 4355C8 List bombList
            // 4355CC List arrowList
            // 4355DC List allBatteries << Conductor boss
            // 43560C Map spellCoolKills
            // 435614 List currentSaleChests
            // 43561C Boolean anyPlayerHaveRingOfLuckCached
            // 43561D Boolean anyPlayerHaveRingOfShadowsCached
            // 43561E Boolean anyPlayerHaveCompassCached
            // 43561F Boolean anyPlayerHaveZoneMapCached
            // 435628 Boolean anyPlayerHaveMonocleCached
            // 435629 Boolean lastShrineVal
            // 43562A Boolean castingFireBall << Dragon
            // 43562B Boolean killingAllEnemies << WHAT?!
            // 43562C List pendingTilesList
            // 435630 List floorRecededList
            // 435634 List floorRisingList
            // 435670 Single camera_y
            // 435678 Single camera_x
            // 43569C Int32 lastGrooveLevel
            // 4356A0 Int32 lastGrooveColor
            // 4356AC List crateList
            // 4356B8 Int curExplosionKills
            // 4356CC List npcList
            // 4356D0 List currentSaleItems
            // 4356D4 List itemPoolRandom2
            // 4356D8 List itemPoolRandom
            // 4356DC List seenItems
            // 4356E4 List familiarList
            // 4356E8 List particleSystems
            // 4356EC List particlePool
            // 4356F4 Single CHARISMA_DISCOUNT
            // 4356F8 List shrineList
            // 4356FC Int32 usedShrinerInZone
            // 435700 IntSet usedShrines
            // 435704 Player noReturnShrinePlayer
            // 435708 Boolean noReturnShrineActive
            // 435709 Boolean warShrineActive
            mapping.Add("bb_controller_game_DEBUG_ALL_TILES_VISIBLE", new AddressRange<UInt32>("Memory", 0x43570A)); // Bool
            // 435769 Boolean bb_necrodancergame_CHRISTMAS_MODE
            mapping.Add("showMinimap", new AddressRange<UInt32>("Memory", 0x43576A)); // Bool
            // 43576C Boolean bb_necrodancergame_DEBUG_STOP_ENEMY_MOVEMENT
            // 43576D Boolean riskShrineActive
            // 435770 Player lastActor
            // 435774 Player riskShrinePlayer
            // 435791 Boolean isMonstrous << Shopkeeper
            // 435792 Boolean rhythmShrineActive
            // 435798 Int32 shopkeeperStartX
            // 43579C Int32 shopkeeperStartY
            // 43589A Boolean anyPlayerHaveNazarCharmCached
            // 43589B Boolean anyPlayerHaveWallsTorchCached

            // 4358C8 Boolean anyPlayerHaveForesightTorchCached
            // 4358C9 Boolean anyPlayerHaveGlassTorchCached
            // 4358CA Boolean anyPlayerHaveCircletCached

            // 4358E0 Int32 bb_controller_game_totalPlaytimeMilliseconds

            // 4355D8 KingConga
            // 4358A8 Conductor theConductor
            // 4358C4 Nightmare nightmare
            // 4359DC Necrodancer necrodancer
            // 435A14 Shriner shriner


            // 435A8A Boolean seenLeprechaun
            // 435AA0 IntMap tiles
            // 435AE8 currentFloorRNG
            // 435AEC wholeRunRNG
            // 435AF4 UInt32 randSeed
            // 435B40 Boolean shopkeeperDead
            // 435B54 Int32 currentScaleYOff
            // 435B58 Int32 currentScaleXOff
            // 435B5C Single currentScaleFactor
            // 435BB0 List familiarList <<< again?


            mapping.Add("bb_controller_game_player1", new AddressRange<UInt32>("Memory", 0x435BF0)); // UInt32
            // 435C0C Int32 bb_controller_game_currentZone
            // 435C10 String bb_controller_game_currentLevel

            // 435C20 UInt32 something
            // + 0x18 give management

            /*
            ECX = object
            EAX = vtable
            call EAX + 0xc8
            mov EAX [object + 18]
            mov EAX [eax + esi*4 + 14]
            */

            // 433DE4
            // 433DEC
            // 0x433F68 Game
            // ] 0x14 + i * 0x4: Player
            // ] ] 0x14 Position X
            // ] ] 0x18 Position Y
            // ] ] 0x11C Character ID
            // ] ] 0x134 ] 0x10 Inventory red black tree
            // ] ] 0x15C Health Component
            // ] ] ] 0x1C Health Max
            // ] ] ] 0x20 Health Temp
            // ] ] ] 0x24 Heatlh Current
            // ] ] 0x19C Kills
            // ] ] 0x214 Bombs

            mapping.Add("SongTime",             new AddressRange<UInt32>("Memory", 0x435808, 0x43580C));
            mapping.Add("PlayerTime",           new AddressRange<UInt32>("Memory", 0x435810, 0x435814));

            mapping.Add("WorldTime",            new AddressRange<UInt32>("Memory", 0x435864, 0x435868));
            mapping.Add("WallsVisible",         new AddressRange<UInt32>("Memory", 0x43589B, 0x43589C)); // "ReadOnly"
            mapping.Add("SessionMaxGold",       new AddressRange<UInt32>("Memory", 0x4358AC, 0x4358B0));
			      mapping.Add("CoinXOR",              new AddressRange<UInt32>("Memory", 0x4358B0, 0x4358B4));
            mapping.Add("Gold",                 new AddressRange<UInt32>("Memory", 0x4358B4, 0x4358B8));
            mapping.Add("EnemiesVisible",       new AddressRange<UInt32>("Memory", 0x4358CA, 0x4358CB)); // "ReadOnly"
            mapping.Add("EntityCount",          new AddressRange<UInt32>("Memory", 0x4358CC, 0x4358D0));
            mapping.Add("EntityList",           new AddressRange<UInt32>("Memory", 0x4358D0, 0x4358D4));
            mapping.Add("DeadEntityList",       new AddressRange<UInt32>("Memory", 0x4358DC, 0x4358E0));

            mapping.Add("DarknessShrineActive", new AddressRange<UInt32>("Memory", 0x4358E4, 0x4358E5));
            mapping.Add("PaceShrineActive",     new AddressRange<UInt32>("Memory", 0x4358E5, 0x4358E6));
            mapping.Add("ChestList",            new AddressRange<UInt32>("Memory", 0x435938, 0x43593C));
            mapping.Add("SpaceShrineActive",    new AddressRange<UInt32>("Memory", 0x43596E, 0x43596F));
            mapping.Add("BossShrineActive",     new AddressRange<UInt32>("Memory", 0x43596F, 0x435970));
            mapping.Add("NumDiamonds",          new AddressRange<UInt32>("Memory", 0x435970, 0x435974));
            mapping.Add("PickupList",           new AddressRange<UInt32>("Memory", 0x435978, 0x43597C));
            mapping.Add("TrapList",             new AddressRange<UInt32>("Memory", 0x43597C, 0x435980));
            mapping.Add("EnemyList",            new AddressRange<UInt32>("Memory", 0x4359E0, 0x4359E4));

            mapping.Add("Flawless",             new AddressRange<UInt32>("Memory", 0x435A13, 0x435A14));
            mapping.Add("SpecialRoomEntranceY", new AddressRange<UInt32>("Memory", 0x435A18, 0x435A1C));
            mapping.Add("SpecialRoomEntranceX", new AddressRange<UInt32>("Memory", 0x435A1C, 0x435A20));
            mapping.Add("LevelConstraintH",     new AddressRange<UInt32>("Memory", 0x435A54, 0x435A58));
            mapping.Add("LevelConstraintW",     new AddressRange<UInt32>("Memory", 0x435A58, 0x435A5C));
            mapping.Add("LevelConstraintY",     new AddressRange<UInt32>("Memory", 0x435A5C, 0x435A60));
            mapping.Add("LevelConstraintX",     new AddressRange<UInt32>("Memory", 0x435A60, 0x435A64));

            mapping.Add("CoinMultiplierStreak", new AddressRange<UInt32>("Memory", 0x435AA4, 0x435AA8));

            mapping.Add("LowPercentage",        new AddressRange<UInt32>("Memory", 0x435AC2, 0x435AC3));

            mapping.Add("MapSeed",              new AddressRange<UInt32>("Memory", 0x435AF4, 0x435AF8));

            mapping.Add("MinLevelY",            new AddressRange<UInt32>("Memory", 0x435BCC, 0x435BD0));
            mapping.Add("MaxLevelY",            new AddressRange<UInt32>("Memory", 0x435BD0, 0x435BD4));
            mapping.Add("MinLevelX",            new AddressRange<UInt32>("Memory", 0x435BD4, 0x435BD8));
            mapping.Add("MaxLevelX",            new AddressRange<UInt32>("Memory", 0x435BD8, 0x435BDC));

            mapping.Add("RenderableObjectList", new AddressRange<UInt32>("Memory", 0x435BEC, 0x435BF0));
        }

        public CryptOfTheNecroDancerProcessManager ( ) : base("Necrodancer") {
            Init_Mapping();

            Cheat();


            //UInt32 keyNumber = 37; // 37 = Left Arrow
            //WriteAbsolute<Byte>(mapping.GetNamed("keysHitLastFrame", "Memory").start + 0x14 + keyNumber, 0x1);

            // PrintEntityList();
        }

        public void Cheat ( ) {
            // WriteRelative<Byte>(mapping.GetNamed("EnemiesVisible", "Memory").start, 0x1); // nope
            UInt32 GameAddress = ReadRelative<UInt32>(mapping.GetNamed("bb_controller_game_players", "Memory").start);
            UInt32 Player1Address = ReadAbsolute<UInt32>(GameAddress + 0x14);
            UInt32 Player1Bombs = ReadAbsolute<UInt32>(Player1Address + 0x214);
            Console.WriteLine("Player 1 Bombs: " + Player1Bombs);
            WriteAbsolute<UInt32>(Player1Address + 0x214, 10);
        }

        public void ProcessRenderableObjectList ( ) {
            UInt32 RenderableObjectList = ReadRelative<UInt32>(mapping.GetNamed("RenderableObjectList", "Memory").start);
            Console.WriteLine($"RenderableObjectList: {RenderableObjectList:X}");
            UInt32 head = ReadAbsolute<UInt32>(RenderableObjectList + 0x10);
            Dictionary<UInt32, List<UInt32>> RenderableObjects = new Dictionary<UInt32, List<UInt32>>();
            for (
              UInt32 current = ReadAbsolute<UInt32>(head + 0x10), entity ;
              ( entity = ReadAbsolute<UInt32>(current + 0x18) ) != 0 ;
              current = ReadAbsolute<UInt32>(current + 0x10)
            ) {
                UInt32 VTable = ReadAbsolute<UInt32>(entity);
                Int32 unknown_10 = ReadAbsolute<Int32>(entity + 0x10);
                Int32 X = ReadAbsolute<Int32>(entity + 0x14);
                Int32 Y = ReadAbsolute<Int32>(entity + 0x18);
                Int32 unknown_1C = ReadAbsolute<Int32>(entity + 0x1C);
                // Int32 unknown_20 = ReadAbsolute<Int32>(entity + 0x20); // X ?
                // Int32 unknown_24 = ReadAbsolute<Int32>(entity + 0x24); // Y ? Maybe difference between "physical" and "displayed" location
                Int32 unknown_28 = ReadAbsolute<Int32>(entity + 0x28);
                Int32 unknown_2C = ReadAbsolute<Int32>(entity + 0x2C);
                Int32 unknown_30 = ReadAbsolute<Int32>(entity + 0x30);
                Int32 unknown_34 = ReadAbsolute<Int32>(entity + 0x34);
                Int32 unknown_38 = ReadAbsolute<Int32>(entity + 0x38);
                Int32 unknown_3C = ReadAbsolute<Int32>(entity + 0x3C);
                Int32 unknown_40 = ReadAbsolute<Int32>(entity + 0x40);
                Int32 unknown_44 = ReadAbsolute<Int32>(entity + 0x44);
                Single unknown_48 = ReadAbsolute<Single>(entity + 0x48);
                Int32 unknown_4C = ReadAbsolute<Int32>(entity + 0x4C);
                Int32 unknown_50 = ReadAbsolute<Int32>(entity + 0x50);
                Single unknown_54 = ReadAbsolute<Single>(entity + 0x54);

                RenderableObjects.TryGetValue(VTable, out List<UInt32> list);
                if (list == null) {
                    list = new List<UInt32>();
                    RenderableObjects.Add(VTable, list);
                } else {
                    list.Add(entity);
                }

                if ( unknown_10 != 0)
                  Console.WriteLine($"{{{VTable:X}}} type entity at {current:X} ({X}, {Y}) {unknown_10:X8} | {unknown_1C:X8} {unknown_28:X8} {unknown_2C:X8} {unknown_30:X8} {unknown_34:X8} {unknown_38:X8} {unknown_3C:X8} {unknown_40:X8} {unknown_44:X8} ({unknown_48}) {unknown_4C:X8} {unknown_50:X8} ({unknown_54})");
            }
            foreach (KeyValuePair<UInt32, List<UInt32>> kvp in RenderableObjects) {
                Console.WriteLine($"{kvp.Key:X} : {kvp.Value.Count}");
            }
        }

        public void PrintBooleanArray (UInt32 address) {
            Console.WriteLine($"Array address: {address:X}");
            // 0x10: count = 4
            UInt32 count = ReadAbsolute<UInt32>(address + 0x10);
            Console.WriteLine($"Array count: {count}");
            for (UInt32 i = 0; i < count; ++i) {
                Byte b = ReadAbsolute<Byte>(address + 0x14 + i);
                Console.WriteLine($"{i}: {b}");
            }
        }
        public void PrintUInt32Array ( UInt32 address) {

            Console.WriteLine($"Array address: {address:X}");
            // 0x10: count = 4
            UInt32 count = ReadAbsolute<UInt32>(address + 0x10);
            Console.WriteLine($"Array count: {count}");
            for (UInt32 i = 0; i < count; i++) {
                UInt32 element = ReadAbsolute<UInt32>(address + 0x14 + (i * 4));
                Console.WriteLine($"Array element {i}: {element:X}");
            }
        }

        public void PrintEntityList ( ) {
            UInt32 entityCount = ReadRelative<UInt32>(mapping.GetNamed("EntityCount", "Memory").start);
            UInt32 entityList = ReadRelative<UInt32>(mapping.GetNamed("EntityList", "Memory").start);
            UInt32 deadEntityList = ReadRelative<UInt32>(mapping.GetNamed("DeadEntityList", "Memory").start);
            // Headnode is a zero node used to track the "end" of the cyclic list
            UInt32 head = ReadAbsolute<UInt32>(entityList + 0x10);

            UInt32 current = ReadAbsolute<UInt32>(head + 0x10);
            for ( Int32 i = 0 ; i < entityCount+1000 ; ++i ) {
              // 10 dll-L
              // 14 dll-R
              // 18 dll-C
              UInt32 next = ReadAbsolute<UInt32>(current + 0x10);
              UInt32 prev = ReadAbsolute<UInt32>(current + 0x14);
              UInt32 entity = ReadAbsolute<UInt32>(current + 0x18);
              if ( entity == 0 ) {
                Console.WriteLine($"Encountered {i} entities");
                // TODO: Figure out why entityCount is significantly higher than the length of the list
                break;
              }

              Int32 X = ReadAbsolute<Int32>(entity + 0x014);
              Int32 Y = ReadAbsolute<Int32>(entity + 0x018);
              Console.WriteLine($"At ({X}, {Y})");

              current = next;
            }
            head = ReadAbsolute<UInt32>(deadEntityList + 0x10);

            current = ReadAbsolute<UInt32>(head + 0x10);
            for ( Int32 i = 0 ; i < entityCount+1000 ; ++i ) {
              // 10 dll-L
              // 14 dll-R
              // 18 dll-C
              UInt32 next = ReadAbsolute<UInt32>(current + 0x10);
              UInt32 prev = ReadAbsolute<UInt32>(current + 0x14);
              UInt32 entity = ReadAbsolute<UInt32>(current + 0x18);
              if ( entity == 0 ) {
                Console.WriteLine($"Encountered {i} dead entities");
                // TODO: Figure out why entityCount is significantly higher than the length of the list
                break;
              }

              Int32 X = ReadAbsolute<Int32>(entity + 0x014);
              Int32 Y = ReadAbsolute<Int32>(entity + 0x018);
              Console.WriteLine($"Dead at ({X}, {Y})");

              current = next;
            }

        }

        public enum COMMAND {
            INVALID = 0,
            MOVE_LEFT = 1,
            MOVE_UP = 2,
            MOVE_RIGHT = 3,
            MOVE_DOWN = 4,
        }
        public void SendCommand ( COMMAND command ) {
            UInt32 something = ReadRelative<UInt32>(0x435C20); // TODO: figure out what this actually is
            UInt32 inputArray = ReadAbsolute<UInt32>(something + 0x18); // Array<UInt32>
            switch ( command ) {
                case COMMAND.MOVE_LEFT:
                    WriteAbsolute<UInt32>(inputArray + 0x14 + 0x4 * 37, 1);
                    break;
                case COMMAND.MOVE_UP:
                    WriteAbsolute<UInt32>(inputArray + 0x14 + 0x4 * 38, 1);
                    break;
                case COMMAND.MOVE_RIGHT:
                    WriteAbsolute<UInt32>(inputArray + 0x14 + 0x4 * 39, 1);
                    break;
                case COMMAND.MOVE_DOWN:
                    WriteAbsolute<UInt32>(inputArray + 0x14 + 0x4 * 40, 1);
                    break;
                default:
                    Console.WriteLine("Invalid command");
                    break;
            }
        }
        public void Dump ( ) {
            Console.WriteLine("Dumping Crypt of the NecroDancer...");

            // SongTime
            UInt32 songTime = ReadRelative<UInt32>(mapping.GetNamed("SongTime", "Memory").start);
            Console.WriteLine($"SongTime: {songTime / 60000}:{(songTime / 1000) % 60}.{songTime % 1000}");

            // PlayerTime
            UInt32 playerTime = ReadRelative<UInt32>(mapping.GetNamed("PlayerTime", "Memory").start);
            Console.WriteLine($"PlayerTime: {playerTime / 60000}:{(playerTime / 1000) % 60}.{playerTime % 1000}");

            // WorldTime
            UInt32 worldTime = ReadRelative<UInt32>(mapping.GetNamed("WorldTime", "Memory").start);
            Console.WriteLine($"WorldTime: {worldTime / 60000}:{(worldTime / 1000) % 60}.{worldTime % 1000}");

            // WallsVisible
            Byte wallsVisible = ReadRelative<Byte>(mapping.GetNamed("WallsVisible", "Memory").start);
            Console.WriteLine($"WallsVisible: {wallsVisible}");

            // Gold
            UInt32 gold = ReadRelative<UInt32>(mapping.GetNamed("Gold", "Memory").start);
            Console.WriteLine($"Gold: {gold}");

            // EnemiesVisible
            Byte enemiesVisible = ReadRelative<Byte>(mapping.GetNamed("EnemiesVisible", "Memory").start);
            Console.WriteLine($"EnemiesVisible: {enemiesVisible}");

            // EntityCount
            UInt32 entityCount = ReadRelative<UInt32>(mapping.GetNamed("EntityCount", "Memory").start);
            Console.WriteLine($"EntityCount: {entityCount}");

            // EntityList
            UInt32 entityList = ReadRelative<UInt32>(mapping.GetNamed("EntityList", "Memory").start);
            Console.WriteLine($"EntityList: {entityList:X}");

            // MapSeed
            UInt32 mapSeed = ReadRelative<UInt32>(mapping.GetNamed("MapSeed", "Memory").start);
            Console.WriteLine($"MapSeed: {mapSeed}");

            while ( true ) {
              ProcessRenderableObjectList();
              Int32 player_x = ReadRelative<Int32>(mapping.GetNamed("bb_controller_game_players", "Memory").start, 0x14, 0x14);
              Int32 player_y = ReadRelative<Int32>(mapping.GetNamed("bb_controller_game_players", "Memory").start, 0x14, 0x18);
              UInt32 player_bombs = ReadRelative<UInt32>(mapping.GetNamed("bb_controller_game_players", "Memory").start, 0x14, 0x214);
              Console.WriteLine($"Player is at ({player_x}, {player_y}) with {player_bombs} bombs");
              Thread.Sleep(1000);
            }
        }
    }
}
