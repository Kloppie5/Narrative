using System;
using System.Text;

namespace MonsterHunterWorld {

    class Guildcard {
        Byte[] data;

        UInt64 steamID; // [0x0000 - 0x0008]
        UInt64 timestampCreated; // [0x0008 - 0x0010]
        Byte UNKNOWN_10_11; // [0x0010 - 0x0011]
        UInt32 HR; // [0x0011 - 0x0015]
        UInt32 MR; // [0x0015 - 0x0019]
        UInt32 playtime; // [0x0019 - 0x001D]
        UInt64 timestampUpdated; // [0x001D - 0x0025]
        String name; // [0x0025 - 0x0065] 64
        String namePrimaryGroup; // [0x0065 - 0x009B] 54

        Byte[] UNKNOWN_009B_00AF; // [0x009B - 0x00AF] 20

        HunterAppearance hunterAppearance; // [0x00AF - 0x0153] <0xA4>

        PalicoAppearance palicoAppearance; // [0x0153 - 0x17F] <0x2C>

        /*
      Register("UNKNOWN_193_92",					"Unknown",		0x193,	    92); // |    193 ][      92 unknown, looks like TYPE-ID, followed by 9 IDs (empty save is full F TYPE-ID, 0 IDs)

			Register("PalicoName",						"String",		0x1EF,	    64); // |    1EF ][      64 palicoName
			Register("PalicoRank",						"UInt32",		0x22F,		 4); // |    22F ][       4 palicoRank
			Register("PalicoHealth",					"UInt32",		0x233,		 4); // |    233 ][       4 palicoHealth
			Register("PalicoAttackMelee",				"UInt32",		0x237,		 4); // |    237 ][       4 palicoAttM
			Register("PalicoAttackRanged",				"UInt32",		0x23B,		 4); // |    23B ][       4 palicoAttR
			Register("PalicoAffinity",					"UInt32",		0x23F,		 4); // |    23F ][       4 palicoAffinity
			Register("PalicoDefence",					"UInt32",		0x243,		 4); // |    243 ][       4 palicoDef
			Register("PalicoVSFire",					"Int32",		0x247,		 4); // |    247 ][       4 palicoVsFire
			Register("PalicoVSWater",					"Int32",		0x24B,		 4); // |    24B ][       4 palicoVsWater
			Register("PalicoVSThunder",					"Int32",		0x24F,		 4); // |    24F ][       4 palicoVsThunder
			Register("PalicoVSIce",						"Int32",		0x253,		 4); // |    253 ][       4 palicoVsIce
			Register("PalicoVSDragon",					"Int32",		0x257,		 4); // |    257 ][       4 palicoVsDragon
			Register("UNKNOWN_25B_1",					"Unknown",		0x25B,		 1); // |    25B ][       1 unknown realigning byte
			Register("PalicoWeaponType",				"UInt32",		0x25C,		 4); // |    25C ][       4 palicoWeaponType
			Register("PalicoWeaponID",					"UInt32",		0x260,		 4); // |    260 ][       4 palicoWeaponID
			Register("PalicoHeadArmorType",				"UInt32",		0x264,		 4); // |    264 ][       4 palicoHeadArmorType
			Register("PalicoHeadArmorID",				"UInt32",		0x268,		 4); // |    268 ][       4 palicoHeadArmorID
			Register("PalicoBodyArmorType",				"UInt32",		0x26C,		 4); // |    26C ][       4 palicoBodyArmorType
			Register("PalicoBodyArmorID",				"UInt32",		0x270,		 4); // |    270 ][       4 palicoBodyArmorID
			Register("PalicoGadgetType",				"UInt32",		0x274,		 4); // |    274 ][       4 palicoGadgetType
			Register("PalicoGadgetID",					"UInt32",		0x278,		 4); // |    278 ][       4 palicoGadgetID
			Register("UNKNOWN_27C_4",					"Unknown",		0x27C,		 4); // |    27C ][       4 unknown
			Register("PalicoVigorWasp",					"UInt8",		0x280,		 1); // |    280 ][       1 palicoVigorwasp
			Register("PalicoFlashfly",					"UInt8",		0x281,		 1); // |    281 ][       1 palicoFlashfly
			Register("PalicoShieldspire",				"UInt8",		0x282,		 1); // |    282 ][       1 palicoShieldspire
			Register("PalicoCoral",						"UInt8",		0x283,		 1); // |    283 ][       1 palicoCoral
			Register("PalicoPlunderblade",				"UInt8",		0x284,		 1); // |    284 ][       1 palicoPlunderblade
			Register("PalicoCocktail",					"UInt8",		0x285,		 1); // |    285 ][       1 palicoCocktail
			Register("PalicoUnity",						"UInt32",		0x286,		 4); // |    286 ][       4 palicoUnity
			Register("PADDING_28A_16",					"Padding",		0x28A,	    16); // |    28A ][      16 padding

			Register("QuestsLowRank",					"UInt16",		0x29A,		 2); // |    29A ][       2 questsLowRank
			Register("QuestsHighRank",					"UInt16",		0x29C,		 2); // |    29C ][       2 questsHighRank
			Register("QuestsInvestigations",			"UInt16",		0x29E,		 2); // |    29E ][       2 questsInvestigations
			Register("QuestsArena",						"UInt16",		0x2A0,		 2); // |    2A0 ][       2 questsArena
			Register("Unity1",							"UInt32",		0x2A2,		 4); // |    2A2 ][       4 unityProtectors
			Register("Unity2",							"UInt32",		0x2A6,		 4); // |    2A6 ][       4 unityTroupers
			Register("Unity3",							"UInt32",		0x2AA,		 4); // |    2AA ][       4 unityPlunderers
			Register("Unity4",							"UInt32",		0x2AE,		 4); // |    2AE ][       4 unityLinguists
			Register("Unity5",							"UInt32",		0x2B2,		 4); // |    2B2 ][       4 unitySomethingElse
			Register("PADDING_2B6_15",					"Padding",		0x2B6,	    15); // |    2B6 ][      15 padding

        */
        UInt32 weaponType; // [0x017F - 0x0183]
        UInt32 weaponID; // [0x0183 - 0x0187]
        UInt32 headArmorType; // [0x0187 - 0x018B]
        UInt32 headArmorID; // [0x018B - 0x018F]
        UInt32 chestArmorType; // [0x018F - 0x0193]
        UInt32 chestArmorID; // [0x0193 - 0x0197]
        UInt32 armArmorType; // [0x0197 - 0x019B]
        UInt32 armArmorID; // [0x019B - 0x019F]
        UInt32 waistArmorType; // [0x019F - 0x01A3]
        UInt32 waistArmorID; // [0x01A3 - 0x01A7]
        UInt32 legArmorType; // [0x01A7 - 0x01AB]
        UInt32 legArmorID; // [0x01AB - 0x01AF]
        UInt32 charmType; // [0x01AF - 0x01B3]
        UInt32 charmID; // [0x01B3 - 0x01B7]
        UInt32 tool1Type; // [0x01B7 - 0x01BB]
        UInt32 tool1ID; // [0x01BB - 0x01BF]
        UInt32 tool2Type; // [0x01BF - 0x01C3]
        UInt32 tool2ID; // [0x01C3 - 0x01C7]

        Byte[] UNKNOWN_01C7_0227; // [0x01C7 - 0x0227]

        String palicoName; // [0x0227 - 0x0267]
        UInt32 palicoLevel; // [0x0267 - 0x026B]
        UInt32 palicoHp; // [0x026B - 0x026F]
        UInt32 palicoAttMelee; // [0x026F - 0x0273]
        UInt32 palicoAttRanged; // [0x0273 - 0x0277]
        UInt32 palicoAff; // [0x0277 - 0x027B]
        UInt32 palicoDef; // [0x027B - 0x027F]
        Int32 palicoVSFire; // [0x027F - 0x0283]
        Int32 palicoVSWater; // [0x0283 - 0x0287]
        Int32 palicoVSThunder; // [0x0287 - 0x028B]
        Int32 palicoVSIce; // [0x028B - 0x028F]
        Int32 palicoVSDragon; // [0x028F - 0x0293]

        Byte[] UNKNOWN_02A3_032D; // [0x02A3 - 0x032D]

        UInt16[] weaponUsageLR; // [0x032D - 0x0349] 14*<0x2>
        UInt16[] weaponUsageHR; // [0x0349 - 0x0365] 14*<0x2>
        UInt16[] weaponUsageInvestigation; // [0x0365 - 0x0381] 14*<0x2>
        UInt16[] weaponUsageMasterRank; // [0x0381 - 0x039D] 14*<0x2>
        UInt16[] weaponUsageGuidingLands; // [0x039D - 0x03B9] 14*<0x2>

        Byte poseID; // [0x03B9]
        Byte expressionID; // [0x03BA]
        Byte backgroundID; // [0x03BB]
        Byte stickerID; // [0x03BC]

        String greeting; // [0x03BD - 0x04BD] 256
        String title; // [0x04BD - 0x05BD] 256

        Byte[] UNKNOWN_05BD_1B0B; // [0x05BD - 0x1B0B]

        UInt16[] monsterCaptured; // [0x1B0B - 0x1BCB] 96*<0x2>
        UInt16[] monsterSlain; // [0x1BCB - 0x1C8B] 96*<0x2>
        UInt16[] monsterLarge; // [0x1C8B - 0x1D4B] 96*<0x2>
        UInt16[] monsterSmall; // [0x1D4B - 0x1E0B] 96*<0x2>
        Byte[] monsterResearch; // [0x1E0B - 0x1E6B] 96*<0x1>

        public Guildcard ( Byte[] data, Int32 offset = 0 ) {
            this.data = new Byte[0x1E6B];
            Array.Copy(data, offset, this.data, 0, 0x1E6B);
            Deserialize();
        }

        public void Deserialize () {
            steamID = BitConverter.ToUInt64(data, 0x0000);
            timestampCreated = BitConverter.ToUInt64(data, 0x0008);
            UNKNOWN_10_11 = data[0x0010];
            HR = BitConverter.ToUInt32(data, 0x0011);
            MR = BitConverter.ToUInt32(data, 0x0015);
            playtime = BitConverter.ToUInt32(data, 0x0019);
            timestampUpdated = BitConverter.ToUInt64(data, 0x001D);
            name = Encoding.UTF8.GetString(data, 0x0025, 0x40);
            namePrimaryGroup = Encoding.UTF8.GetString(data, 0x0065, 0x2C);

            UNKNOWN_009B_00AF = new Byte[0x00AF - 0x009B];
            Array.Copy(data, 0x009B, UNKNOWN_009B_00AF, 0, 0x00AF - 0x009B);

            hunterAppearance = new HunterAppearance(data, 0x00AF);

            palicoAppearance = new PalicoAppearance(data, 0x0153);

            weaponType = BitConverter.ToUInt32(data, 0x017F);
            weaponID = BitConverter.ToUInt32(data, 0x0183);
            headArmorType = BitConverter.ToUInt32(data, 0x0187);
            headArmorID = BitConverter.ToUInt32(data, 0x018B);
            chestArmorType = BitConverter.ToUInt32(data, 0x018F);
            chestArmorID = BitConverter.ToUInt32(data, 0x0193);
            armArmorType = BitConverter.ToUInt32(data, 0x0197);
            armArmorID = BitConverter.ToUInt32(data, 0x019B);
            waistArmorType = BitConverter.ToUInt32(data, 0x019F);
            waistArmorID = BitConverter.ToUInt32(data, 0x01A3);
            legArmorType = BitConverter.ToUInt32(data, 0x01A7);
            legArmorID = BitConverter.ToUInt32(data, 0x01AB);
            charmType = BitConverter.ToUInt32(data, 0x01AF);
            charmID = BitConverter.ToUInt32(data, 0x01B3);
            tool1Type = BitConverter.ToUInt32(data, 0x01B7);
            tool1ID = BitConverter.ToUInt32(data, 0x01BB);
            tool2Type = BitConverter.ToUInt32(data, 0x01BF);
            tool2ID = BitConverter.ToUInt32(data, 0x01C3);

            UNKNOWN_01C7_0227 = new Byte[0x0227 - 0x01C7];
            Array.Copy(data, 0x01C7, UNKNOWN_01C7_0227, 0, 0x0227 - 0x01C7);

            palicoName = Encoding.UTF8.GetString(data, 0x0227, 0x40);
            palicoLevel = BitConverter.ToUInt32(data, 0x0267);
            palicoHp = BitConverter.ToUInt32(data, 0x026B);
            palicoAttMelee = BitConverter.ToUInt32(data, 0x026F);
            palicoAttRanged = BitConverter.ToUInt32(data, 0x0273);
            palicoAff = BitConverter.ToUInt32(data, 0x0277);
            palicoDef = BitConverter.ToUInt32(data, 0x027B);
            palicoVSFire = BitConverter.ToInt32(data, 0x027F);
            palicoVSWater = BitConverter.ToInt32(data, 0x0283);
            palicoVSThunder = BitConverter.ToInt32(data, 0x0287);
            palicoVSIce = BitConverter.ToInt32(data, 0x028B);
            palicoVSDragon = BitConverter.ToInt32(data, 0x028F);

            UNKNOWN_02A3_032D = new Byte[0x032D - 0x02A3];
            Array.Copy(data, 0x02A3, UNKNOWN_02A3_032D, 0, 0x032D - 0x02A3);

            weaponUsageLR = new UInt16[14];
            for ( Int32 i = 0; i < 14; i++ )
                weaponUsageLR[i] = BitConverter.ToUInt16(data, 0x032D + i * 2);
            weaponUsageHR = new UInt16[14];
            for ( Int32 i = 0; i < 14; i++ )
                weaponUsageHR[i] = BitConverter.ToUInt16(data, 0x0349 + i * 2);
            weaponUsageInvestigation = new UInt16[14];
            for ( Int32 i = 0; i < 14; i++ )
                weaponUsageInvestigation[i] = BitConverter.ToUInt16(data, 0x0365 + i * 2);
            weaponUsageMasterRank = new UInt16[14];
            for ( Int32 i = 0; i < 14; i++ )
                weaponUsageMasterRank[i] = BitConverter.ToUInt16(data, 0x0381 + i * 2);
            weaponUsageGuidingLands = new UInt16[14];
            for ( Int32 i = 0; i < 14; i++ )
                weaponUsageGuidingLands[i] = BitConverter.ToUInt16(data, 0x039D + i * 2);

            poseID = data[0x03B9];
            expressionID = data[0x03BA];
            backgroundID = data[0x03BB];
            stickerID = data[0x03BC];

            greeting = Encoding.UTF8.GetString(data, 0x03BD, 0x100);
            title = Encoding.UTF8.GetString(data, 0x04BD, 0x100);

            UNKNOWN_05BD_1B0B = new Byte[0x1B0B - 0x05BD];
            Array.Copy(data, 0x05BD, UNKNOWN_05BD_1B0B, 0, 0x1B0B - 0x05BD);

            monsterCaptured = new UInt16[96];
            for ( Int32 i = 0; i < 96; i++ )
                monsterCaptured[i] = BitConverter.ToUInt16(data, 0x1B0B + i * 2);

            monsterSlain = new UInt16[96];
            for ( Int32 i = 0; i < 96; i++ )
                monsterSlain[i] = BitConverter.ToUInt16(data, 0x1BCB + i * 2);

            monsterLarge = new UInt16[96];
            for ( Int32 i = 0; i < 96; i++ )
                monsterLarge[i] = BitConverter.ToUInt16(data, 0x1C8B + i * 2);

            monsterSmall = new UInt16[96];
            for ( Int32 i = 0; i < 96; i++ )
                monsterSmall[i] = BitConverter.ToUInt16(data, 0x1D4B + i * 2);

            monsterResearch = new Byte[96];
            for ( Int32 i = 0; i < 96; i++ )
                monsterResearch[i] = data[0x1E0B + i];
        }

        public void Dump () {
            Console.WriteLine($"SteamID: {steamID}");
            Console.WriteLine($"Timestamp created: {timestampCreated}");
            Console.WriteLine($"UNKNOWN_10_11: {UNKNOWN_10_11}");
            Console.WriteLine($"HR: {HR}");
            Console.WriteLine($"MR: {MR}");
            Console.WriteLine($"Playtime: {playtime}");
            Console.WriteLine($"Timestamp updated: {timestampUpdated}");
            Console.WriteLine($"Name: {name}");
            Console.WriteLine($"Name primary group: {namePrimaryGroup}");

            Console.WriteLine($"UNKNOWN_009B_00AF: ");
            Logger.hex_dump(UNKNOWN_009B_00AF);

            hunterAppearance.Dump();
            palicoAppearance.Dump();

            Console.WriteLine($"Weapon: {weaponType} {weaponID}");
            Console.WriteLine($"Head armor: {headArmorType} {headArmorID}");
            Console.WriteLine($"Chest armor: {chestArmorType} {chestArmorID}");
            Console.WriteLine($"Arm armor: {armArmorType} {armArmorID}");
            Console.WriteLine($"Waist armor: {waistArmorType} {waistArmorID}");
            Console.WriteLine($"Leg armor: {legArmorType} {legArmorID}");
            Console.WriteLine($"Charm: {charmType} {charmID}");
            Console.WriteLine($"Tool 1: {tool1Type} {tool1ID}");
            Console.WriteLine($"Tool 2: {tool2Type} {tool2ID}");

            Console.WriteLine($"UNKNOWN_01C7_0227: ");
            Logger.hex_dump(UNKNOWN_01C7_0227);

            Console.WriteLine($"Palico name: {palicoName}");
            Console.WriteLine($"Palico level: {palicoLevel}");
            Console.WriteLine($"Palico HP: {palicoHp}");
            Console.WriteLine($"Palico Attack Melee: {palicoAttMelee}");
            Console.WriteLine($"Palico Attack Ranged: {palicoAttRanged}");
            Console.WriteLine($"Palico Affinity: {palicoAff}");
            Console.WriteLine($"Palico VS Fire: {palicoVSFire}");
            Console.WriteLine($"Palico VS Water: {palicoVSWater}");
            Console.WriteLine($"Palico VS Thunder: {palicoVSThunder}");
            Console.WriteLine($"Palico VS Ice: {palicoVSIce}");
            Console.WriteLine($"Palico VS Dragon: {palicoVSDragon}");

            Console.WriteLine($"UNKNOWN_02A3_032D: ");
            Logger.hex_dump(UNKNOWN_02A3_032D);

            Console.WriteLine($"Weapon usage: ");
            for ( Int32 i = 0; i < 14; i++ )
                Console.WriteLine($"\t{i}: {weaponUsageLR[i]} / {weaponUsageHR[i]} / {weaponUsageInvestigation[i]} / {weaponUsageMasterRank[i]} / {weaponUsageGuidingLands[i]}");

            Console.WriteLine($"Pose ID: {poseID}");
            Console.WriteLine($"Expression ID: {expressionID}");
            Console.WriteLine($"Background ID: {backgroundID}");
            Console.WriteLine($"Sticker ID: {stickerID}");

            Console.WriteLine($"Greeting: {greeting}");
            Console.WriteLine($"Title: {title}");

            Console.WriteLine($"UNKNOWN_05BD_1B0B: ");
            Logger.hex_dump(UNKNOWN_05BD_1B0B);

            /*
              3020 (10 * (2 + 60 * 5)) arenaRecords
              // UInt16	unknown;
              // 5 times:
              //// UInt32 time;
              //// String(32) namePartner;
              //// UInt64 steamIDPartner;
              //// UInt64 timestampCreatedPartner;
              //// UInt64 date;
            */

            Console.WriteLine($"Monster stats: ");
            for ( UInt16 i = 0; i < 71; i++ ) {
                Console.WriteLine($"\t{i}: {monsterCaptured[i]} captured, {monsterSlain[i]} slain, {monsterLarge[i]} large, {monsterSmall[i]} small, {monsterResearch[i]} research");
            }
        }
    }
}
