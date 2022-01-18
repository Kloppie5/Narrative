using System;
using System.Text;

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
            // > 433F0C Array<Boolean> stickLeft
            // > 433F10 Array<Boolean> stickRight
            // > 433F14 Array<Boolean> stickUp
            // > 433F18 Array<Boolean> stickDown
            // > 433F1C Array<Single> lastJoyX
            // > 433F20 Array<Single> lastJoyY
            // > 433F24 Array<Boolean> stickLeft2
            // > 433F28 Array<Boolean> stickRight2
            // > 433F2C Array<Boolean> stickUp2
            // > 433F30 Array<Boolean> stickDown2
            // > 433F34 Array<Single> lastJoyX2
            // > 433F38 Array<Single> lastJoyY2
            // > 433F3C Array<Int32> movementBuffer
            // > 433F40 Array<Int32> movementBufferFrame
            // > 433F44 Array<Int32> offbeatMovementBuffer
            // > 433F48 Array<Int32> offbeatMovementBufferFrame
            // > 433F4C Array<Int32> lastBeatMovedOn
            // > 433F50 Array<Int32> lastOffbeatMovedOn
            // > 433F54 Array<Int32> lastBeatMissed
            // > 433F58 Array<Int32> punishmentBeatToSkip
            // > 433F5C Array<Int32> punishmentBeatToSkipQueue
            // > 433F60 Array<Boolean> keysHitLastFrame
            // > 433F64 Array<Boolean> keysHit2FramesAgo

            // 433F68 Array<Player> bb_controller_game_players

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
            // 4355D8 KingConga
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
            // 43570A Boolean bb_controller_game_DEBUG_ALL_TILES_VISIBLE
            // 435769 Boolean bb_necrodancergame_CHRISTMAS_MODE
            // 43576A Boolean showMinimap
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
            // 4358A8 Conductor theConductor

            // 4358C4 Nightmare nightmare
            // 4358C8 Boolean anyPlayerHaveForesightTorchCached
            // 4358C9 Boolean anyPlayerHaveGlassTorchCached
            // 4358CA Boolean anyPlayerHaveCircletCached

            // 4358E0 Int32 bb_controller_game_totalPlaytimeMilliseconds

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


            // 435BF0 Int32 bb_controller_game_player1
            // 435C0C Int32 bb_controller_game_currentZone
            // 435C10 String bb_controller_game_currentLevel

            // 433DE4
            // 433DEC
            // 0x433F68 Game
            // ] 0x14 Player 1
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
            // ] 0x18 Player 2

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

            PrintEntityList();
        }

        public void Cheat ( ) {
            // WriteRelative<Byte>(mapping.GetNamed("EnemiesVisible", "Memory").start, 0x1); // nope
        }

        public void PrintEntityList ( ) {
            UInt32 entityCount = ReadRelative<UInt32>(mapping.GetNamed("EntityCount", "Memory").start);
            UInt32 entityList = ReadRelative<UInt32>(mapping.GetNamed("EntityList", "Memory").start);
            UInt32 deadEntityList = ReadRelative<UInt32>(mapping.GetNamed("DeadEntityList", "Memory").start);
            // Headnode is a zero node used to track the "end" of the cyclic list
            UInt32 head = ReadAbsolute<UInt32>(entityList + 0x10);

            UInt32 current = ReadAbsolute<UInt32>(head + 0x10);
            for ( Int32 i = 0 ; i < entityCount+1000 ; ++i ) {
              // 00 vftable
              // 04 gc-L
              // 08 gc-R
              // 0C gc-C

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
              // 00 vftable
              // 04 gc-L
              // 08 gc-R
              // 0C gc-C

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
        }
    }
}
