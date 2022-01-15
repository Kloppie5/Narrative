using System;
using System.IO;
using System.Text;

namespace MonsterHunterWorld {
    class Character {
        Byte[] data;

        Byte[] UNKNOWN; // [0x0 - 0x4]
        String name; // [0x4 - 0x44]
        UInt32 HR; // [0x44 - 0x48]
        UInt32 MR; // [0x48 - 0x4C]
        UInt32 zenny; // [0x4C - 0x50]
        UInt32 researchPoints; // [0x50 - 0x54]
        UInt32 HRXP; // [0x54 - 0x58]
        UInt32 MRXP; // [0x58 - 0x5C]
        UInt32 playtime; // [0x5C - 0x60]
        Byte[] hunterAppearance; // [0x60 - 0xDC] <0x7C>
        Byte[] UNKNOWN_0000DC_0002B2; // [0xDC - 0x2B2]
        Byte[] guildcard; // [0x2B2 - 0x211D] <0x1E6B>
        Byte[] guildcards; // [0x211D - 0xC02E9] 100*<0x1E6B>
        Byte[] guildcardIndices; // [0xC02E9 - 0xC03B1] 100*<0x2>
        Byte[] UNKNOWN_0C03B1_0F3510; // [0xC03B1 - 0x0F3510]
        Byte[] itemLoadouts; // [0x0F3510 - 0x10A710] 80*<0x4A0>
        Byte[] itemLoadoutIndices; // [0x10A710 - 0x10A760] 80
        Byte[] UNKNOWN_10A760_116098; // [0x10A760 - 0x116098]
        Byte[] itemPouch; // [0x116098 - 0x116158] 24*<0x8>
        Byte[] ammoPouch; // [0x116158 - 0x1161D8] 16*<0x8>
        Byte[] materialPouch; // [0x1161D8 - 0x116298] 24*<0x8>
        Byte[] specialPouch; // [0x116298 - 0x116300] 13*<0x8>
        Byte[] itemBox; // [0x116300 - 0x116940] 200*<0x8>
        Byte[] ammoBox; // [0x116940 - 0x116F80] 200*<0x8>
        Byte[] materialBox; // [0x116F80 - 0x119690] 1250*<0x8>
        Byte[] decorationBox; // [0x119690 - 0x11A630] 500*<0x8>
        Byte[] equipmentBox; // [0x11A630 - 0x1674A8] 2500*<0x7E>
        Byte[] invalidEquipment; // [0x1674A8 - 0x176FAC] 510*<0x7E>
        Byte[] emptyEquipment; // [0x176FAC - 0x19D6E8] 1250*<0x7E>
        Byte[] equipmentBoxIndices; // [0x19D6E8 - 0x19FDF8] 2500*<0x4>
        Byte[] EMPTY_19FDF8_1A05C8; // [0x19FDF8 - 0x1A05C8] 2500*<0x4>
        Byte[] emptyEquipmentIndices; // [0x1A05C8 - 0x1A194C] 1250*<0x4>
        Byte[] UNKNOWN_1A194C_1A8D48; // [0x1A194C - 0x1A8D48]
        Byte[] NPCConversations; // [0x1A8D48 - 0x1ACD48] 2048*<0x8>
        Byte[] UNKNOWN_1ACD48_1AD5DF; // [0x1ACD48 - 0x1AD5DF]
        Byte[] investigations; // [0x1AD5DF - 0x1B177F] 400*<0x2A>
        Byte[] UNKNOWN_1B177F_1B21B1; // [0x1B177F - 0x1B21B1]
        Byte[] UNKNOWN_1B21B1_1B60B1; // [0x1B21B1 - 0x1B60B1] 128*<0x7E>
        Byte[] UNKNOWN_1B60B1_1B6955; // [0x1B60B1 - 0x1B6955]
        Byte[] equipmentLayouts; // [0x1B6955 - 0x1DB8D5] 224*<0x2A4>
        Byte[] UNKNOWN_1DB8D5_1E4315; // [0x1DB8D5 - 0x1E4315] 112*<0x13C>
        Byte[] roomConfigurations; // [0x1E4315 - 0x1E5ED5] 24*<0x128>
        Byte[] UNKNOWN_1E5ED5_2098C0; // [0x1E5ED5 - 0x2098C0]
        Byte[] hash; // [0x2098C0 - 0x209AC0]

        public Character ( String filename ) {
            LoadRaw( filename );
        }
        public Character ( Byte[] data, int offset = 0 ) {
            this.data = new Byte[0x209AC0];
            Array.Copy(data, offset, this.data, 0, 0x209AC0);
            CharacterEncryption.DecryptCharacter(this.data);
        }

        public void Deserialize ( ) {
            UNKNOWN = new Byte[0x4];
            Array.Copy(data, 0x0, UNKNOWN, 0x0, 0x4);
            name = Encoding.ASCII.GetString(data, 0x4, 0x44);
            HR = BitConverter.ToUInt32(data, 0x44);
            MR = BitConverter.ToUInt32(data, 0x48);
            zenny = BitConverter.ToUInt32(data, 0x4C);
            researchPoints = BitConverter.ToUInt32(data, 0x50);
            HRXP = BitConverter.ToUInt32(data, 0x54);
            MRXP = BitConverter.ToUInt32(data, 0x58);
            playtime = BitConverter.ToUInt32(data, 0x5C);
            hunterAppearance = new Byte[0x7C];
            Array.Copy(data, 0x60, hunterAppearance, 0x0, 0x7C);
            UNKNOWN_0000DC_0002B2 = new Byte[0x2B2];
            Array.Copy(data, 0xDC, UNKNOWN_0000DC_0002B2, 0x0, 0x2B2);
            guildcard = new Byte[0x1E6B];
            Array.Copy(data, 0x2B2, guildcard, 0x0, 0x1E6B);
            guildcards = new Byte[100 * 0x1E6B];
            for ( int i = 0; i < 100; ++i )
                Array.Copy(data, 0x211D + (i * 0x1E6B), guildcards, (i * 0x1E6B), 0x1E6B);
            guildcardIndices = new Byte[0x2 * 100];
            Array.Copy(data, 0xC02E9, guildcardIndices, 0x0, 0x2 * 100);
            UNKNOWN_0C03B1_0F3510 = new Byte[0x0F3510 - 0xC03B1];
            Array.Copy(data, 0xC03B1, UNKNOWN_0C03B1_0F3510, 0x0, 0x0F3510 - 0xC03B1);
            itemLoadouts = new Byte[80 * 0x4A0];
            for ( int i = 0; i < 80; ++i )
                Array.Copy(data, 0x0F3510 + (i * 0x4A0), itemLoadouts, (i * 0x4A0), 0x4A0);
            itemLoadoutIndices = new Byte[0x2 * 80];
            Array.Copy(data, 0x10A710, itemLoadoutIndices, 0x0, 0x2 * 80);
            UNKNOWN_10A760_116098 = new Byte[0x116098 - 0x10A760];
            Array.Copy(data, 0x10A760, UNKNOWN_10A760_116098, 0x0, 0x116098 - 0x10A760);
            itemPouch = new Byte[24 * 0x8];
            for ( int i = 0; i < 24; ++i )
                Array.Copy(data, 0x116098 + (i * 0x8), itemPouch, (i * 0x8), 0x8);
            ammoPouch = new Byte[16 * 0x8];
            for ( int i = 0; i < 16; ++i )
                Array.Copy(data, 0x116158 + (i * 0x8), ammoPouch, (i * 0x8), 0x8);
            materialPouch = new Byte[24 * 0x8];
            for ( int i = 0; i < 24; ++i )
                Array.Copy(data, 0x1161D8 + (i * 0x8), materialPouch, (i * 0x8), 0x8);
            specialPouch = new Byte[13 * 0x8];
            for ( int i = 0; i < 13; ++i )
                Array.Copy(data, 0x116298 + (i * 0x8), specialPouch, (i * 0x8), 0x8);
            itemBox = new Byte[200 * 0x8];
            for ( int i = 0; i < 200; ++i )
                Array.Copy(data, 0x116300 + (i * 0x8), itemBox, (i * 0x8), 0x8);
            ammoBox = new Byte[200 * 0x8];
            for ( int i = 0; i < 200; ++i )
                Array.Copy(data, 0x116940 + (i * 0x8), ammoBox, (i * 0x8), 0x8);
            materialBox = new Byte[1250 * 0x8];
            for ( int i = 0; i < 1250; ++i )
                Array.Copy(data, 0x116F80 + (i * 0x8), materialBox, (i * 0x8), 0x8);
            decorationBox = new Byte[500 * 0x8];
            for ( int i = 0; i < 500; ++i )
                Array.Copy(data, 0x119690 + (i * 0x8), decorationBox, (i * 0x8), 0x8);
            equipmentBox = new Byte[2500 * 0x7E];
            for ( int i = 0; i < 2500; ++i )
                Array.Copy(data, 0x11A630 + (i * 0x7E), equipmentBox, (i * 0x7E), 0x7E);
            invalidEquipment = new Byte[510 * 0x7E];
            for ( int i = 0; i < 510; ++i )
                Array.Copy(data, 0x1674A8 + (i * 0x7E), invalidEquipment, (i * 0x7E), 0x7E);
            emptyEquipment = new Byte[1250 * 0x7E];
            for ( int i = 0; i < 1250; ++i )
                Array.Copy(data, 0x176FAC + (i * 0x7E), emptyEquipment, (i * 0x7E), 0x7E);
            equipmentBoxIndices = new Byte[2500 * 0x4];
            Array.Copy(data, 0x19D6E8, equipmentBoxIndices, 0x0, 0x4 * 2500);
            EMPTY_19FDF8_1A05C8 = new Byte[0x1A05C8 - 0x19FDF8];
            Array.Copy(data, 0x19FDF8, EMPTY_19FDF8_1A05C8, 0x0, 0x1A05C8 - 0x19FDF8);
            emptyEquipmentIndices = new Byte[1250 * 0x4];
            Array.Copy(data, 0x1A05C8, emptyEquipmentIndices, 0x0, 0x4 * 1250);
            UNKNOWN_1A194C_1A8D48 = new Byte[0x1A8D48 - 0x1A194C];
            Array.Copy(data, 0x1A194C, UNKNOWN_1A194C_1A8D48, 0x0, 0x1A8D48 - 0x1A194C);
            NPCConversations = new Byte[0x1ACD48 - 0x1A8D48];
            Array.Copy(data, 0x1A8D48, NPCConversations, 0x0, 0x1ACD48 - 0x1A8D48);
            UNKNOWN_1ACD48_1AD5DF = new Byte[0x1AD5DF - 0x1ACD48];
            Array.Copy(data, 0x1ACD48, UNKNOWN_1ACD48_1AD5DF, 0x0, 0x1AD5DF - 0x1ACD48);
            investigations = new Byte[400 * 0x2A];
            for ( int i = 0; i < 400; ++i )
                Array.Copy(data, 0x1AD5DF + (i * 0x2A), investigations, (i * 0x2A), 0x2A);
            UNKNOWN_1B177F_1B21B1 = new Byte[0x1B21B1 - 0x1B177F];
            Array.Copy(data, 0x1B177F, UNKNOWN_1B177F_1B21B1, 0x0, 0x1B21B1 - 0x1B177F);
            UNKNOWN_1B21B1_1B60B1 = new Byte[0x1B60B1 - 0x1B21B1];
            Array.Copy(data, 0x1B21B1, UNKNOWN_1B21B1_1B60B1, 0x0, 0x1B60B1 - 0x1B21B1);
            UNKNOWN_1B60B1_1B6955 = new Byte[0x1B6955 - 0x1B60B1];
            Array.Copy(data, 0x1B60B1, UNKNOWN_1B60B1_1B6955, 0x0, 0x1B6955 - 0x1B60B1);
            equipmentLayouts = new Byte[224 * 0x2A4];
            for ( int i = 0; i < 224; ++i )
                Array.Copy(data, 0x1B6955 + (i * 0x2A4), equipmentLayouts, (i * 0x2A4), 0x2A4);
            UNKNOWN_1DB8D5_1E4315 = new Byte[0x1E4315 - 0x1DB8D5];
            Array.Copy(data, 0x1DB8D5, UNKNOWN_1DB8D5_1E4315, 0x0, 0x1E4315 - 0x1DB8D5);
            roomConfigurations = new Byte[0x1E5ED5 - 0x1E4315];
            Array.Copy(data, 0x1E4315, roomConfigurations, 0x0, 0x1E5ED5 - 0x1E4315);
            UNKNOWN_1E5ED5_2098C0 = new Byte[0x2098C0 - 0x1E5ED5];
            Array.Copy(data, 0x1E5ED5, UNKNOWN_1E5ED5_2098C0, 0x0, 0x2098C0 - 0x1E5ED5);
            hash = new Byte[0x209AC0 - 0x2098C0];
            Array.Copy(data, 0x2098C0, hash, 0x0, 0x209AC0 - 0x2098C0);
        }
        public void Serialize ( ) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("TODO");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void Save ( String filename ) {
            Serialize();
            CharacterEncryption.EncryptCharacter(data);
            File.WriteAllBytes( filename, data );
        }
        public void SaveRaw ( String filename ) {
            Serialize();
            File.WriteAllBytes( filename, data );
        }
        public void Load ( String filename ) {
            data = File.ReadAllBytes( filename );
            CharacterEncryption.DecryptCharacter(data);
            Deserialize();
        }
        public void LoadRaw ( String filename ) {
            data = File.ReadAllBytes( filename );
            Deserialize();
        }

        public void Dump ( Boolean verbose = false, Boolean halting = false ) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  UNKNOWN [{0:X} - {0x4:X}] = {BitConverter.ToString(UNKNOWN)}");
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine($"  Name [{0x4:X} - {0x44:X}] = {name}");
            Console.WriteLine($"  HR [{0x44:X} - {0x48:X}] = {HR}");
            Console.WriteLine($"  MR [{0x48:X} - {0x4C:X}] = {MR}");
            Console.WriteLine($"  Zenny [{0x4C:X} - {0x50:X}] = {zenny}");
            Console.WriteLine($"  Research Points [{0x50:X} - {0x54:X}] = {researchPoints}");
            Console.WriteLine($"  HR XP [{0x54:X} - {0x58:X}] = {HRXP}");
            Console.WriteLine($"  MR XP [{0x58:X} - {0x5C:X}] = {MRXP}");
            Console.WriteLine($"  Playtime [{0x5C:X} - {0x60:X}] = {playtime}");
            Console.WriteLine($"  Hunter Appearance [{0x60:X} - {0xDC:X}] <0x7C>");
            if ( verbose )
                Logger.hex_dump(hunterAppearance);
            if ( halting )
                Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  UNKNOWN [{0x0000DC:X} - {0x0002B2:X}]");
            Console.ForegroundColor = ConsoleColor.White;
            if ( verbose )
                Logger.hex_dump(UNKNOWN_0000DC_0002B2);
            if ( halting )
                Console.ReadLine();

            Console.WriteLine($"  Guildcard [{0x0002B2:X} - {0x00211D:X}] <0x1E6B>");
            if ( verbose )
                Logger.hex_dump(guildcard);
            if ( halting )
                Console.ReadLine();
            Console.WriteLine($"  Guildcards [{0x00211D:X} - {0x0C02E9:X}] | 100*<0x1E6B>");
            if ( verbose )
                Logger.hex_dump(guildcards);
            if ( halting )
                Console.ReadLine();
            Console.WriteLine($"  Guildcard indices [{0x0C02E9:X} - {0x0C03B1:X}] | 100*<0x2>");
            if ( verbose )
                Logger.hex_dump(guildcardIndices);
            if ( halting )
                Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  UNKNOWN [{0x0C03B1:X} - {0x0F3510:X}]");
            Console.ForegroundColor = ConsoleColor.White;
            if ( verbose )
                Logger.hex_dump(UNKNOWN_0C03B1_0F3510);
            if ( halting )
                Console.ReadLine();
            /*
            // 20 slots of something, same length as guildcards, so maybe a buffer for received guildcards?
            // multiple giant tables
            // << small monster kills
            // many slots of something else
            // padding
            // large float holding structure
            */
            Console.WriteLine($"  Item Loadouts [{0x0F3510:X} - {0x10A710:X}] | 80*<0x4A0>");
            if ( verbose )
                Logger.hex_dump(itemLoadouts);
            if ( halting )
                Console.ReadLine();
            Console.WriteLine($"  Item Loadout indices [{0x10A710:X} - {0x10A760:X}] | 80*<0x1>");
            if ( verbose )
                Logger.hex_dump(itemLoadoutIndices);
            if ( halting )
                Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  UNKNOWN [{0x10A760:X} - {0x116098:X}]");
            Console.ForegroundColor = ConsoleColor.White;
            if ( verbose )
                Logger.hex_dump(UNKNOWN_10A760_116098);
            if ( halting )
                Console.ReadLine();
            /*
            // completely empty
            // followed by a bit increment till 77 followed by FF, possibly something like progress tracking <done on complete save>
            // Ooh, maybe some loadouts and indices?
            */
            Console.WriteLine($"  Item Pouch [{0x116098:X} - {0x116158:X}] | 24*<0x8>");
            if ( verbose )
                Logger.hex_dump(itemPouch);
            if ( halting )
                Console.ReadLine();
            Console.WriteLine($"  Ammo Pouch [{0x116158:X} - {0x1161D8:X}] | 16*<0x8>");
            if ( verbose )
                Logger.hex_dump(ammoPouch);
            if ( halting )
                Console.ReadLine();
            Console.WriteLine($"  Material Pouch [{0x1161D8:X} - {0x116298:X}] | 24*<0x8>");
            if ( verbose )
                Logger.hex_dump(materialPouch);
            if ( halting )
                Console.ReadLine();
            Console.WriteLine($"  Special Pouch [{0x116298:X} - {0x116300:X}] | 13*<0x8> << TODO CHECK");
            if ( verbose )
                Logger.hex_dump(specialPouch);
            if ( halting )
                Console.ReadLine();
            Console.WriteLine($"  Item Box [{0x116300:X} - {0x116940:X}] | 200*<0x8>");
            if ( verbose )
                Logger.hex_dump(itemBox);
            if ( halting )
                Console.ReadLine();
            Console.WriteLine($"  Ammo Box [{0x116940:X} - {0x116F80:X}] | 200*<0x8>");
            if ( verbose )
                Logger.hex_dump(ammoBox);
            if ( halting )
                Console.ReadLine();
            Console.WriteLine($"  Material Box [{0x116F80:X} - {0x119690:X}] | 1250*<0x8>");
            if ( verbose )
                Logger.hex_dump(materialBox);
            if ( halting )
                Console.ReadLine();
            Console.WriteLine($"  Decoration Box [{0x119690:X} - {0x11A630:X}] | 500*<0x8>");
            if ( verbose )
                Logger.hex_dump(decorationBox);
            if ( halting )
                Console.ReadLine();
            Console.WriteLine($"  Equipment Box [{0x11A630:X} - {0x1674A8:X}] | 2500*<0x7E>");
            if ( verbose )
                Logger.hex_dump(equipmentBox);
            if ( halting )
                Console.ReadLine();
            Console.WriteLine($"  -1 Equipment [{0x1674A8:X} - {0x176FAC:X}] | 510*<0x7E>");
            if ( verbose )
                Logger.hex_dump(invalidEquipment);
            if ( halting )
                Console.ReadLine();
            Console.WriteLine($"  Empty Equipment [{0x176FAC:X} - {0x19D6E8:X}] | 1250*<0x7E>");
            if ( verbose )
                Logger.hex_dump(emptyEquipment);
            if ( halting )
                Console.ReadLine();
            /*
            50 pieces with identification
            everything else is empty
            */
            Console.WriteLine($"  Equipment Box indices [{0x19D6E8:X} - {0x19FDF8:X}] | 2500*<0x4>");
            if ( verbose )
                Logger.hex_dump(equipmentBoxIndices);
            if ( halting )
                Console.ReadLine();
            Console.WriteLine($"  Zeroes [{0x19FDF8:X} - {0x1A05C8:X}] | 2500*<0x4>");
            if ( verbose )
                Logger.hex_dump(EMPTY_19FDF8_1A05C8);
            if ( halting )
                Console.ReadLine();
            Console.WriteLine($"  Empty Equipment Box indices [{0x1A05C8:X} - {0x1A194C:X}] | 1250*<0x4>");
            if ( verbose )
                Logger.hex_dump(emptyEquipmentIndices);
            if ( halting )
                Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  UNKNOWN [{0x1A194C:X} - {0x1A8D48:X}]");
            Console.ForegroundColor = ConsoleColor.White;
            if ( verbose )
                Logger.hex_dump(UNKNOWN_1A194C_1A8D48);
            if ( halting )
                Console.ReadLine();
            /*
            // Weapon notice flags
            */
            Console.WriteLine($"  NPC Conversations [{0x1A8D48:X} - {0x1ACD48:X}] | 2048*<0x8>");
            if ( verbose )
                Logger.hex_dump(NPCConversations);
            if ( halting )
                Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  UNKNOWN (quest notice flags) [{0x1ACD48:X} - {0x1AD5DF:X}]");
            Console.ForegroundColor = ConsoleColor.White;
            if ( verbose )
                Logger.hex_dump(UNKNOWN_1ACD48_1AD5DF);
            if ( halting )
                Console.ReadLine();
            /*
            // Palico name is in here, so probably palico tool levels and xp
            // Quest notics flags

            // 1AD228:1 Notice flag; The Great Glutton
            // 1AD227:40 Notice flag; Bird-Brained Bandit
            // 1AD227:10 Notice flag; A Thicket of Thugs
            // 1AD227:8 Notice flag; Butting Heads with Nature
            // 1AD227:4 Notice flag; The Great Jagras Hunt
            // 1AD227:2 Notice flag; A Kestadon Kerfuffle
            // 1AD227:1 Notice flag; Jagras of the Great Forest

            // 1AD249:4 Notice flag; Learning the Clutch
            */
            Console.WriteLine($"  Investigations [{0x1AD5DF:X} - {0x1B177F:X}] | 400*<0x2A>");
            if ( verbose )
                Logger.hex_dump(investigations);
            if ( halting )
                Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  UNKNOWN (map flags) [{0x1B177F:X} - {0x1B21B1:X}]");
            Console.ForegroundColor = ConsoleColor.White;
            if ( verbose )
                Logger.hex_dump(UNKNOWN_1B177F_1B21B1);
            if ( halting )
                Console.ReadLine();
            /*
            // Map notice flags
            */
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  UNKNOWN structure, maybe shift a bit [{0x1B21B1:X} - {0x1B60B1:X}] | 128*<0x7E>");
            Console.ForegroundColor = ConsoleColor.White;
            if ( verbose )
                Logger.hex_dump(UNKNOWN_1B21B1_1B60B1);
            if ( halting )
                Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  UNKNOWN [{0x1B60B1:X} - {0x1B6955:X}]");
            Console.ForegroundColor = ConsoleColor.White;
            if ( verbose )
                Logger.hex_dump(UNKNOWN_1B60B1_1B6955);
            if ( halting )
                Console.ReadLine();
            Console.WriteLine($"  Equipment Layouts [{0x1B6955:X} - {0x1DB8D5:X}] | 224*<0x2A4>");
            if ( verbose )
                Logger.hex_dump(equipmentLayouts);
            if ( halting )
                Console.ReadLine();
                /*
                1B90F1 | 000000 : -F -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- --   ................................
                1B9111 | 000020 : -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- --   ................................
                1B9131 | 000040 : -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- --   ................................
                1B9151 | 000060 : -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- --   ................................
                1B9171 | 000080 : -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- --   ................................
                1B9191 | 0000A0 : -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- --   ................................
                1B91B1 | 0000C0 : -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- --   ................................
                1B91D1 | 0000E0 : -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- --   ................................
                1B91F1 | 000100 : -- -- -- -- FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF   ................................
                1B9211 | 000120 : FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF   ................................
                1B9231 | 000140 : FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF   ................................
                1B9251 | 000160 : FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF   ................................
                1B9271 | 000180 : FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF   ................................
                1B9291 | 0001A0 : FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF   ................................
                1B92B1 | 0001C0 : FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF   ................................
                1B92D1 | 0001E0 : FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF   ................................
                1B92F1 | 000200 : FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF   ................................
                1B9311 | 000220 : FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF   ................................
                1B9331 | 000240 : FF FF FF FF FF FF FF FF -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- --   ................................
                1B9351 | 000260 : -- -- -- -- -- -- -- -- FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF   ................................
                1B9371 | 000280 : -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- --   ................................
                1B9391 | 0002A0 : FF FF FF FF                                                                                       ....
                */

            /**
            for (UInt64 i = 0; i < 244; i++) {
                UInt64 offset = 0x1DB8D5 + i * 2;
                Int16 index = BitConverter.ToInt16(data, (int)offset);
                Console.WriteLine($"    Equipment Loadout {i} [{offset:X} - {offset + 2:X}] | {index}");
            }
            */

            Console.WriteLine($"  UNKNOWN [{0x1DB8D5:X} - {0x1E4315:X}] | 112*<0x13C>");
            if ( verbose )
                Logger.hex_dump(UNKNOWN_1DB8D5_1E4315);
            if ( halting )
                Console.ReadLine();

            Console.WriteLine($"  Room Configurations [{0x1E4315:X} - {0x1E5ED5:X}] | 24*<0x128>");
            if ( verbose )
                Logger.hex_dump(roomConfigurations);
            if ( halting )
                Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  UNKNOWN [{0x1E5ED5:X} - {0x2098C0:X}]");
            Console.ForegroundColor = ConsoleColor.White;
            if ( verbose )
                Logger.hex_dump(UNKNOWN_1E5ED5_2098C0);
            if ( halting )
                Console.ReadLine();
            /*
            // Strange names, some from guildcards, some unknown
            // looks like 12 slots of maybe guildcard linked palico helpers you can find?
            // Guildcard titles and medals notics flags
            // very empty with the occasional structure
            */
            Console.WriteLine($"  Hash [{0x2098C0:X} - {0x209AC0:X}]");
            if ( verbose )
                Logger.hex_dump(hash);
            if ( halting )
                Console.ReadLine();
        }
    }
}
