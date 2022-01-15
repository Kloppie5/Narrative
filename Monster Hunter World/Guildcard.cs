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

        Byte[] UNKNOWN_009B_032C; // [0x009B - 0x032D]

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

            UNKNOWN_009B_032C = new Byte[0x032C - 0x009B];
            Array.Copy(data, 0x009B, UNKNOWN_009B_032C, 0, 0x032C - 0x009B);

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

            Console.WriteLine($"UNKNOWN_009B_032C: ");
            Logger.hex_dump(UNKNOWN_009B_032C);

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

        public void DumpUnknown ( ) {
            Console.WriteLine($"UNKNOWN_10_11: {UNKNOWN_10_11}");

            Console.WriteLine($"UNKNOWN_009B_032C: ");
            Logger.hex_dump(UNKNOWN_009B_032C);

            Console.WriteLine($"UNKNOWN_05BD_1B0B: ");
            Logger.hex_dump(UNKNOWN_05BD_1B0B);
        }
    }
}
