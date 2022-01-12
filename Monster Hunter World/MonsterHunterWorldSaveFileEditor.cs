using System;
using System.IO;

namespace MonsterHunterWorld {

    class MonsterHunterWorldSaveFileEditor {

        String filePath;
        Byte[] data;

        public MonsterHunterWorldSaveFileEditor(String filePath) {
            Console.WriteLine($"Initialized MonsterHunterWorldSaveFileEditor");
            this.filePath = filePath;
            data = File.ReadAllBytes(filePath);
        }

        public Boolean DecryptGlobal() {
            Console.WriteLine($"Decrypting MonsterHunterWorldSaveFile");
            SaveFileEncryption.Decrypt(data);
            return true;
        }
        public Boolean DecryptCharacters() {
            CharacterEncryption.DecryptCharacter(data, 0x3010D8);
            CharacterEncryption.DecryptCharacter(data, 0x50AB98);
            CharacterEncryption.DecryptCharacter(data, 0x714658);
            return true;
        }

        public void debug_dump() {
            Console.WriteLine($"Dumping MonsterHunterWorldSaveFileEditor");
            Console.WriteLine($"filePath: {filePath}");

            Console.WriteLine($"SaveFileHeader[0x0 - 0x60]");
            Console.WriteLine($"  SaveFileSignature [0x0 - 0x4] = 01 00 00 00");
            Console.WriteLine($"  UNKNOWN [0x4 - 0xC]");
            Console.WriteLine($"  DataHash [0xC - 0x20]");
            Console.WriteLine($"  DataSize [0x20 - 0x28] = 0xAC30A0");
            Console.WriteLine($"  SteamID [0x28 - 0x30]");
            Console.WriteLine($"  PADDING [0x30 - 0x40] = 00 ... 00");
            Console.WriteLine($"  OffsetControls [0x40 - 0x48] = 0x60");
            Console.WriteLine($"  OffsetOptions [0x48 - 0x50] = 0x300070");
            Console.WriteLine($"  OffsetSection2 [0x50 - 0x58] = 0x301080");
            Console.WriteLine($"  OffsetCharacters [0x58 - 0x60] = 0x3010C8");

            Console.WriteLine($"ControlsRegion [0x60 - 0x300070]");
            Console.WriteLine($"  ControlsSignature [0x60 - 0x64]");
            Console.WriteLine($"  UNKNOWN [0x64 - 0x68]");
            Console.WriteLine($"  ControlsSize [0x68 - 0x70]");
            Console.WriteLine($"  ControlsData [0x70 - 0x300070]");

            Console.WriteLine($"OptionsRegion [0x300070 - 0x301080]");
            Console.WriteLine($"  OptionsSignature [0x300070 - 0x300074]");
            Console.WriteLine($"  UNKNOWN [0x300074 - 0x300078]");
            Console.WriteLine($"  OptionsSize [0x300078 - 0x300080]");
            Console.WriteLine($"  OptionsData [0x300080 - 0x301080]");

            Console.WriteLine($"Section2Region [0x301080 - 0x3010C8]");
            Console.WriteLine($"  Section2Signature [0x301080 - 0x301084]");
            Console.WriteLine($"  UNKNOWN [0x301084 - 0x301088]");
            Console.WriteLine($"  Section2Size [0x301088 - 0x301090]");
            Console.WriteLine($"  Section2Data [0x301090 - 0x3010C8]");

            Console.WriteLine($"CharactersRegion [0x3010C8 - 0xAC30E0]");
            Console.WriteLine($"  CharactersSignature [0x3010C8 - 0x3010CC]");
            Console.WriteLine($"  UNKNOWN [0x3010CC - 0x3010D0]");
            Console.WriteLine($"  CharactersSize [0x3010D0 - 0x3010D8]");
            Console.WriteLine($"  CharactersData [0x3010D8 - 0xAC30D8]");
            Console.WriteLine($"    Character1 [0x3010D8 - 0x50AB98]");
            Console.WriteLine($"    Character2 [0x50AB98 - 0x714658]");
            Console.WriteLine($"    Character3 [0x714658 - 0x91E118]");
            Console.WriteLine($"    PADDING [0x91E118 - 0xAC30D8]");
            Console.WriteLine($"    PADDING [0xAC30D8 - 0xAC30E0]");

            hex_dump(data);
        }

        public void dump_character ( int character ) {
            Console.WriteLine($"Dumping Character {character}");
            Int32 offset = 0x3010D8 + character * 0x209AC0;


            /**
            // Register("Character1",					"Region",	 0x3004DC, 1007888); // |        ][ 1007888 Character1
            // Register("Character2",					"Region",	 0x3F65EC, 1007888); // |  F6110 ][ 1007888 Character2
            // Register("Character3",					"Region",	 0x4EC6FC, 1007888); // | 1EC220 ][ 1007888 Character3
            // Register("Hunter Name",					"String",	 	  0x0,		64); // |        ][      64 Hunter Name
            // Register("Hunter Rank",					"UInt32",		 0x40,		 4); // |	  40 ][       4 HR
            // Register("Zenny",						"UInt32",		 0x44,		 4); // |	  44 ][       4 Zenny
            // Register("Research Points",				"UInt32",		 0x48,		 4); // |	  48 ][       4 Research Points
            // Register("Experience",					"UInt32",		 0x4C,		 4); // |	  4C ][       4 XP
            // Register("Playtime",						"Seconds",		 0x50,		 4); // |	  50 ][       4 Playtime
            Register("UNKNOWN",							"UInt32",		 0x54,		 4); // |	  54 ][       4 MR
            Register("Hunter Appearance",				"Region",		 0x58,	   120); // |	  58 ][     120 HunterAppearance
            Register("Palico Appearance",				"Region",		 0xD0,		44); // |	0xD0 ][      44 PalicoAppearance
            Register("Guildcard",						"Region",		 0xFC,	  4923); // |	  FC ][    4923 Guildcard
            Register("Shared Guildcards",				"Region",	   0x1437,	492300); // |	1437 ][  492300 Guildcard * 100
            Register("UNKNOWN_79743_100",				"Region",	  0x79743,	   100); // |  79743 ][     100 Unknown (counting up until full F)
            Register("UNKNOWN_797A7_500",				"Region",	  0x797A7,	   500); // |  797A7 ][     500 Unknown (100 * 5 tracking)
            Register("UNKNOWN_7999B_98460",				"Region",	  0x7999B,	 98460); // |  7999B ][   98460 Unknown (20 * 4923)
            Register("UNKNOWN_91A37_6878",				"Region",	  0x91A37,	  6878); // |  91A37 ][    6878 Unknown (20 * 4923)

                                                                // post living quarters
                                                                // Weapon upgrade and equip
                                                                    // 0x91A9F (0C -> 0A -> 13)
                                                                // A3 - Urgent Pukei-Pukei Hunt
                                                                // A3 - Sinister Shadows in the Swamp
                                                                    // 0x91BAE (08 -> 09 -> 29)
                                                                // lynian and forest 9
                                                                // camp and forest 8 10
                                                                    // 0x91BAB (9D -> DD -> FD)
                                                                // A2 - A Kestodon Kerfuffle
                                                                // A3 - The Best Kind of Quest
                                                                    // 0x91BAC (00-00 -> 01-00 -> 03-20)
                                                                // A2 - A Kestodon Kerfuffle
                                                                // A3 - Urgent Pukei-Pukei Hunt
                                                                // A3 - The Best Kind of Quest
                                                                    // 0x91BB0 (40-00 -> 60-00 -> 70-00 -> 77-20)
                                                                // Small Monster Kills
                                                                    //0x91DD3 Aptonoth
                                                                    //0x91DD7 Jagras
                                                                    //0x91DE0 Mosswine
                                                                    //0x91DE3 Gajau
                                                                    //0x91DE7 Kestadon (Male) (?)
                                                                    //0x91DEB Apceros (?)
                                                                    //0x91E37
                                                                    //0x91E77 Hornetaur
                                                                    //0x91E7B Vespoid
                                                                    //0x91E83 Kestadon (Female) (?)
                                                                    //0x91E87 Raphinos
                                                                    //0x91E8B Shamos
                                                                    //0x91E93 Girros

                                                                // ?
                                                                    //0x929DD-0x92A22

                                                            // 0x92A23-0x92E03 padding (?)
                                                                // Endemic Life Infobox Flag
                                                                        //0x92E04-0x092E5A
                                                                    //0x92E53.20 Woodland Pteryx
                                                                    //0x92E58.02 Hercudrome

                                                                // Endemic Life Capture Count
                                                                        //0x92EB9-0x93379
                                                                    //0x92EB9 Shepherd Hare
                                                                    //0x92ED9 Woodland Pteryx
                                                                    //0x92F3D Wildspire Gekko
                                                                    //0x92F79 Revolture
                                                                    //0x92F99 Omenfly (?)
                                                                    //0x93099 Hopguppy
                                                                    //0x930F9 Wiggler
                                                                    //0x9311D Giant Vigorwasp
                                                                    //0x93159 Carrier Ant
                                                                    //0x93179 Hercudrome
                                                                    //0x93199

                                                                    //0x9329D
                                                                    //0x932A5

                                                                    //0x93379 Sushifish
            Register("Item Loadouts",					"Region",	  0x93579,   63224); // |  93579 ][   63224 itemLoadouts
            Register("UNKNOWN_A2C71_8",					"Unknown",	  0xA2C71,		 8); // |  A2C71 ][	      8 unknown
            Register("Item Pouch",						"Region",	  0xA2C79,	   192); // |  A2C79 ][     192 Item Pouch (24 slots) {UInt32 id, UInt32 itemquantity}
            Register("Ammo Pouch",						"Region",	  0xA2D39,	   128); // |  A2D39 ][     128 Ammo Pouch (16 slots)
            Register("Material Pouch",					"Region",	  0xA2DB9,	   192); // |  A2DB9 ][     192 Material Pouch (24 slots)
            Register("Special Pouch",					"Region",	  0xA2E79,	    96); // |  A2E79 ][      96 Special Pouch (12 slots?)
            Register("Item Box",						"Region",	  0xA2ED9,	  1600); // |  A2ED9 ][    1600 Item box (200 slots)
            Register("Ammo Box",						"Region",	  0xA3519,	  1600); // |  A3519 ][    1600 Ammo box (200 slots)
            Register("Material Box",					"Region",	  0xA3B59,	  6400); // |  A3B59 ][    6400 Material box (800 slots)
            Register("Decoration Box",					"Region",	  0xA5459,	  1600); // |  A5459 ][    1600 Decorations (200 slots)

            Register("Equipment Box",					"Region",     0xA5A99,   68000); // |  A5A99 ][   68000 Equipment (1000 slots)
                                                                                         //// | ][ 4 SortIndex
                                                                                         //// | ][ 4 EquipmentType
                                                                                         //// | ][ 4 EquipmentType argument 1 (ArmourSlot, WeaponType, CharmPresence, KinsectPresence, None)
                                                                                         //// | ][ 4 IdInClass
                                                                                         //// | ][ 4 UpgradeLevel
                                                                                         //// | ][ 4 UpgradePoints
                                                                                         //// | ][ 4 DecoSlot1
                                                                                         //// | ][ 4 DecoSlot2
                                                                                         //// | ][ 4 DecoSlot3
                                                                                         //// | ][ 4 EquipmentType argument 2 (BowGunMod1, KinsectType)
                                                                                         //// | ][ 4 BowGunMod2
                                                                                         //// | ][ 4 BowGunMod3
                                                                                         //// | ][ 4 Augment1
                                                                                         //// | ][ 4 Augment2
                                                                                         //// | ][ 4 Augment3
                                                                                         //// | ][ 4 Unknown
                                                                                         //// | ][ 4 Unknown
            Register("UNKNOWN_B6439_34680",				"Region",     0xB6439,   34680); // |  B6439 ][   34680 Unknown (510*68)
            Register("UNKNOWN_BEBB1_68000",				"Region",     0xBEBB1,   68000); // |  BEBB1 ][   68000 Unknown (1000*68)
            Register("UNKNOWN_CF551_4000",				"Region",     0xCF551,    4000); // |  CF551 ][    4000 Unknown (counting up)
            Register("UNKNOWN_D04F1_2000",				"Region",     0xD04F1,    2000); // |  D04F1 ][    2000 Unknown (padding?)
            Register("UNKNOWN_D0CC1_4000",				"Region",     0xD0CC1,    4000); // |  D0CC1 ][    4000 Unknown (counting up)
            Register("UNKNOWN_D1C61_4096",				"Region",     0xD1C61,    4096); // |  D1C61 ][    4096 Unknown
            Register("UNKNOWN_D2C61_2048",				"Region",     0xD2C61,    2048); // |  D2C61 ][    2048 Unknown (counting up until full F)
            Register("UNKNOWN_D3461_1024",				"Region",     0xD3461,    1024); // |  D3461 ][    1024 Unknown
            Register("UNKNOWN_D3861_11392",				"Region",     0xD3861,   11392); // |  D3861 ][   11392 Unknown

            Register("NPC Conversation History",		"Region",     0xD64E1,   16384); // |  D64E1 ][   16384 NPC Conversation History (2048*8)

            Register("UNKNOWN_DA4E1_1012",				"Region",     0xDA4E1,    1012); // |  DA4E1 ][    1012 Unknown
            /**

            0DA 561 | CC -> 94 (Third_Fleet_Master, 1C128A1B / 6D7-9A16)
            0DA 562 | 69 -> 6A (Third_Fleet_Master, 1C128A1B / 6D7-9A16)

            0DA 571 | 00 -> 10 (Commander, BCCC59FC)
            0DA 577 | F1 -> F3 (Commander, BC-D2F19)

            0DA 584 | 00 -> 02 (Fiver_Bro, -C-7BB5A)
            0DA 58C | 80 -> 88 (Laid_Back_Botanist, 4633844A)

            0DA 5A3 | 30 -> 70 (Third_Fleet_Master, 1C128A1B / 6D7-9A16)
            0DA 5A7 | 0C -> 1C (Third_Fleet_Master, 1C128A1B / 6D7-9A16)
            0DA 5AB | 03 -> 07 (Third_Fleet_Master, 1C128A1B / 6D7-9A16)

            0DA 5C1 | 04 -> 06 (Meowscular_Chef, 913B8659)
            0DA 5C5 | 80 -> 90 (Second_Fleet_Master, 6AFDD861)
                0DA 5C7 | 10 -> 30 (Provisions_Stockpile, 16FAB8-B)
                0DA 5C7 | 30 -> 32 -> 36 (Housekeeper, C4A-7622 | 4A9ED1E7)
            0DA 5C8 | 00 -> 02 (Arena_Lass, FFE-8FAA)
            0DA 5CE | 00 -> 80 (Armory, 865B2B2A)

            0DA 603 | 00 -> 10 (Second_Fleet_Master, 3-9-AF53)

            0DA 815 | 02 -> 03 (Meowscular_Chef, 913B8659)



            Register("Investigations",					"Region",	  0xDA8D5,   10500); // |  DA8D5 ][   10500 Investigation (250 slots)
                                                                                         //// | ][ 0 ][ 4 filled {0x30, 0x75, 0x00, 0x00}
                                                                                         //// | ][ 4 ][ 1 selected {? 0x01 : 0x00}
                                                                                         //// | ][ 5 ][ 4 attempts
                                                                                         //// | ][ 9 ][ 4 seen (3)
                                                                                         //// | ][ 13 ][ 1 localeindex {"Ancient Forest", "Wildspire Wastes", "Coral Highlands","Rotten Vale", "Elder Recess"}
                                                                                         //// | ][ 14 ][ 1 rank byte {"Low Rank", "High Rank", "Tempered"}
                                                                                         //// | ][ 15 ][ 4 monster1 {"Anjanath", "Rathalos", "Great Jagras", "Rathian", "Pink Rathian", "Azure Rathalos", "Diablos", "Black Diablos", "Kirin", "Kushala Daora", "Lunastra", "Teostra", "Lavasioth", "Deviljho", "Barroth", "Uragaan", "Pukei-Pukei", "Nergigante", "Kulu-Ya-Ku", "Tzitzi-Ya-Ku", "Jyuratodus", "Tobi-Kadachi", "Paolumu", "Legiana", "Great Girros", "Odogaron", "Radobaan", "Vaal Hazak", "Dodogama", "Bazelgeuse", "Empty"}
                                                                                         //// | ][ 19 ][ 4 monster2
                                                                                         //// | ][ 23 ][ 4 monster3
                                                                                         //// | ][ 27 ][ 1 monster1tempered
                                                                                         //// | ][ 28 ][ 1 monster2tempered
                                                                                         //// | ][ 29 ][ 1 monster3tempered
                                                                                         //// | ][ 30 ][ 1 hp
                                                                                         //// | ][ 31 ][ 1 attack
                                                                                         //// | ][ 32 ][ 1 defense
                                                                                         //// | ][ 33 ][ 1 size
                                                                                         //// | ][ 34 ][ 1 UNKNOWN_INVESTIGATION_OFFSET_34
                                                                                         //// | ][ 35 ][ 1 flourishingindex
                                                                                         ////// {"Nothing", "Mushrooms", "Flower Beds", "Mining Outcrops","Bonepiles", "Gathering Points"}
                                                                                         ////// {"Nothing", "Cactus", "Fruit", "Mining Outcrops", "Bonepiles", "Gathering Points" }
                                                                                         ////// {"Nothing", "Conch Shells", "Pearl Oysters", "Mining Outcrops", "Bonepiles", "Gathering Points" }
                                                                                         ////// {"Nothing", "Ancient Fossils", "Crimson Fruit", "Mining Outcrops", "Bonepiles", "Gathering Points" }
                                                                                         ////// {"Nothing", "Amber Deposits", "Beryl Deposits", "Mining Outcrops", "Bonepiles", "Gathering Points"}
                                                                                         //// | ][ 36 ][ 1 time { 50, 30, 15, 50, 30, 50, 50, 50, 50, 30, 15};
                                                                                         //// | ][ 37 ][ 1 UNKNOWN_INVESTIGATION_OFFSET_37
                                                                                         //// | ][ 38 ][ 1 faints
                                                                                         //// | ][ 39 ][ 1 players
                                                                                         //// | ][ 40 ][ 1 monsterrewards
                                                                                         //// | ][ 41 ][ 1 zennymultiplier
            Register("UNKNOWN_DD1D9_237",				"Region",	  0xDD1D9,	   237); // |  DD1D9 ][     237 Unknown
            Register("UNKNOWN_DD2C6_3788",				"Region",	  0xDD2C6,	  3788); // |  DD2C6 ][    3788 Unknown
                                                // Wildspire Waste minimap flags?
                                                //	0DD2C9 | B7 -> BF WW4 Stone (or Latchberry)
                                                //  0DD2CB | 12 -> 1E WW
                                                //  0DD2CD | 10 -> 11 WW4?
                                                //  0DD2CE | 21 -> 25 WW
                                                //  0DD518 | 0C -> 0E AF1 Herb right
                                                //  0DD51E | 1C -> 5C AF1 Herb bottomright
                                                //  0DD546 | 02 -> 2E WW
                                                //  0DD547 | 00 -> 04 WW
                                                //  0DD549 | 10 -> 18 WW4 Latchberry (or stone)
                                                //  0DD54A | A0 -> E1 WW
                                                //	0DD549 | 10 -> 18 AF1 Flowfern bottommiddle
                                                //  0DD54F | 83 -> 8F WW
                                                //  0DD554 | 52 -> 53 WW
                                                //  0DD555 | 03 -> 83 WW
                                                //	0DD555 | 83 -> A3 WW4 Herb bottomleft
                                                //	0DD558 | 40 -> 60 WW4 Herb middle
                                                //  0DD55C | 20 -> 24 WW
                                                //  0DD55F | 02 -> 06 WW


            Register("Equipment Loadouts",				"Region",	  0xDE192,   60928); // |  DE192 ][   60928 Equipment Loadouts (112 slots)
            Register("UNKNOWN_ECF92_25889",				"Region",	  0xECF92,	 25889); // |  ECF92 ][   25889 Unknown
                                                                // Guild card First Title Notice Flags
                                                                    // 0F2E17-0F2E22 | AF-FF-FF-FF-FF-FF-FF-FF-FF-FF-07 -> 00-00-00-00-00-00-00-00-00-00-00
                                                                    // 0F2E6A-0F2E6C | 80-07 -> 00-00
                                                                    // 0F2E71-0F2E78 | FC-FF-FF-FF-FF-1F-1E -> 00-00-00-00-00-00-00
                                                                    // 0F2E79-0F2E7C | 7C-F0-3F -> 00-00-00
                                                                // Guild card Middle Title Notice Flags
                                                                    // 0F2F37-0F2F47 | FE-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-FF-01 -> 00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00
                                                                // Guild card Background Notice Flags
                                                                    // 0F2F5F-0F2F63 | 02-11-72-01 -> 00-00-00-00
                                                                // Guild card Pose Notice Flags
                                                                    // 0F2F6F-0F2F73 | FE-CF-01-10 -> 00-00-00-00
                                                                // Guild card Expression Notice Flags
                                                                    // 0F2F7F-0F2F83 | FE-FF-FF-01 -> 00-00-00-00
                                                        // 0F2F97 | 06-80-04 -> 00-00-00 Medal notice flags
                                                        // 0F2F9C | 50 -> 00 Medal notice flags
                                                        // 0F2F97 08 -> 00 Defender of Astera
            Register("DLC",								"Region",	  0xF34B3,     512); // |  F34B3 ][	    512 DLC
            Register("UNKNOWN_F36B3_669",				"Region",	  0xF36B3,     669); // |  F36B3 ][     669 Unknown
            Register("UNKNOWN_F3950_10176",				"Region",	  0xF3950,   10176); // |  F3950 ][   10176 Unknown
                                                                                         // |  F6110 ]
            */
            hex_dump(data, 0x3010D8, 0x50AB98, 32);
        }

        public void hex_dump ( Byte[] bytes, UInt64 start = 0, UInt64 end = 0, UInt64 line_length = 32 ) {
            if (end == 0)
                end = (UInt64)bytes.Length;

            for ( UInt64 i = start; i < end; i += line_length ) {
                Console.Write($"{i:X5} : ");
                for ( UInt64 j = 0; j < line_length; j++ )
                    Console.Write(
                        i + j < end ?
                        $"{bytes[i + j]:X2} ".Replace("0", "-") :
                        "    "
                    );
                Console.Write("  ");
                for ( UInt64 j = 0; j < line_length; j++ )
                    if ( i + j < end )
                        Console.Write(
                            bytes[i + j] >= 32 && bytes[i + j] <= 126 ?
                            $"{(char)bytes[i + j]}" :
                            "."
                        );
                Console.WriteLine();
            }
        }
    }
}
