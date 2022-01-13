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
            Console.WriteLine($"Decrypting MonsterHunterWorldSaveFile characters");
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

        public void dump_character ( UInt64 character ) {
            UInt64 start = 0x3010D8 + character * 0x209AC0;
            UInt64 end = start + 0x209AC0;
            Console.WriteLine($"Character {character} [{start:X} - {end:X}]");

            Console.WriteLine($"  Signature [{start:X} - {start + 0x4:X}]");
            Console.WriteLine($"  Name [{start + 0x4:X} - {start + 0x44:X}]");
            Console.WriteLine($"  HR [{start + 0x44:X} - {start + 0x48:X}]");
            Console.WriteLine($"  MR [{start + 0x48:X} - {start + 0x4C:X}]");
            Console.WriteLine($"  Zenny [{start + 0x4C:X} - {start + 0x50:X}]");
            Console.WriteLine($"  Research Points [{start + 0x50:X} - {start + 0x54:X}]");
            Console.WriteLine($"  HR XP [{start + 0x54:X} - {start + 0x58:X}]");
            Console.WriteLine($"  MR XP [{start + 0x58:X} - {start + 0x5C:X}]");
            Console.WriteLine($"  Playtime [{start + 0x5C:X} - {start + 0x60:X}]");
            Console.WriteLine($"  Hunter Appearance [{start + 0x60:X} - {start + 0xDC:X}]");

            hex_dump(data, start + 0x0000DC, start + 0x0002B2, 32, (_) => {return ConsoleColor.Yellow;});
            // Palico appearance
            Console.ReadLine();

            Console.WriteLine($"  Guildcard [{start + 0x0002B2:X} - {start + 0x00211D:X}] <0x1E6B>");
            Console.WriteLine($"  Guildcards [{start + 0x00211D:X} - {start + 0x0C02E9:X}] | 100 slots <0x1E6B>");

            hex_dump(data, start + 0x0C02E9, start + 0x0F3510, 32, (_) => {return ConsoleColor.Yellow;});
            // 20 slots of something, same length as guildcards, so maybe a buffer for received guildcards?
            // multiple giant tables
            // << small monster kills
            // many slots of something else
            // padding
            // large float holding structure
            Console.ReadLine();

            Console.WriteLine($"  Item Loadouts [{start + 0x0F3510:X} - {start + 0x10A710:X}] | 80 slots");

            hex_dump(data, start + 0x10A710, start + 0x116098, 32, (_) => {return ConsoleColor.Yellow;});
            // completely empty
            // followed by a bit increment till 77 followed by FF, possibly something like progress tracking <done on complete save>
            Console.ReadLine();

            Console.WriteLine($"  Item Pouch [{start + 0x116098:X} - {start + 0x116158:X}] | 24 slots");
            Console.WriteLine($"  Ammo Pouch [{start + 0x116158:X} - {start + 0x1161D8:X}] | 16 slots");
            Console.WriteLine($"  Material Pouch [{start + 0x1161D8:X} - {start + 0x116298:X}] | 24 slots");
            Console.WriteLine($"  Special Pouch [{start + 0x116298:X} - {start + 0x1162F8:X}] | 12 slots");
            Console.WriteLine($"  Item Box [{start + 0x1162F8:X} - {start + 0x116938:X}] | 200 slots");
            Console.WriteLine($"  Ammo Box [{start + 0x116938:X} - {start + 0x116F78:X}] | 200 slots");
            Console.WriteLine($"  Material Box [{start + 0x116F78:X} - {start + 0x119688:X}] | 1250 slots");
            Console.WriteLine($"  Decoration Box [{start + 0x119688:X} - {start + 0x11A628:X}] | 500 slots");
            Console.WriteLine($"  Equipment Box [{start + 0x11A628:X} - {start + 0x1674A0:X}] | 2500 slots");
            Console.WriteLine($"  -1 Equipment [{start + 0x1674A0:X} - {start + 0x176FA4:X}] | 510 slots");
            Console.WriteLine($"  Empty Equipment [{start + 0x176FA4:X} - {start + 0x19D6E0:X}] | 1250 slots");

            hex_dump(data, start + 0x19D6E0, start + 0x1A8D48, 32, (_) => {return ConsoleColor.Yellow;});
            // Weapon notice flags
            Console.ReadLine();

            Console.WriteLine($"  NPC Conversations [{start + 0x1A8D48:X} - {start + 0x1ACD48:X}] | 2048 slots");

            hex_dump(data, start + 0x1ACD48, start + 0x1AD5DF, 32, (_) => {return ConsoleColor.Yellow;});
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

            Console.ReadLine();

            Console.WriteLine($"  Investigations [{start + 0x1AD5DF:X} - {start + 0x1B177F:X}] | 400 slots");

            hex_dump(data, start + 0x1B177F, start + 0x1B21D1, 32, (_) => {return ConsoleColor.Yellow;});
            // Map notice flags
            Console.ReadLine();

            Console.WriteLine($"  UNKNOWN [{start + 0x1B21D1:X} - {start + 0x1B60D1:X}] | 128 slots");
            hex_dump(data, start + 0x1B60D1, start + 0x1B6955, 32, (_) => {return ConsoleColor.Yellow;});
            Console.ReadLine();

            Console.WriteLine($"  Equipment Layouts [{start + 0x1B6955:X} - {start + 0x1DB8D5:X}] | 224 slot");
            Console.WriteLine($"  UNKNOWN [{start + 0x1DB8D5:X} - {start + 0x1E4315:X}] | 112 slot");
            Console.WriteLine($"  Room Configurations [{start + 0x1E4315:X} - {start + 0x1E5ED5:X}] | 24 slot");

            hex_dump(data, start + 0x1E5ED5, start + 0x1E7082, 32, (_) => {return ConsoleColor.Yellow;});
            // Strange names, some from guildcards, some unknown
            // looks like 12 slots of maybe guildcard linked palico helpers you can find?
            // Guildcard titles and medals notics flags
            Console.ReadLine();

            Console.WriteLine($"  UNKNOWN [{start + 0x1E7082:X} - {start + 0x1EA202:X}] | 96 slot");

            hex_dump(data, start + 0x1EA202, start + 0x2098C0, 32, (_) => {return ConsoleColor.Yellow;});
            // very empty with the occasional structure
            Console.ReadLine();

            Console.WriteLine($"  Hash [{start + 0x2098C0:X} - {start + 0x209AC0:X}]");
        }

        public void save_character ( int character, String filename ) {
            Int32 start = 0x3010D8 + character * 0x209AC0;

            Byte[] data = new Byte[0x209AC0];
            Array.Copy(this.data, start, data, 0, 0x209AC0);

            File.WriteAllBytes(filename, data);
        }

        public void hex_dump ( Byte[] bytes, UInt64 start = 0, UInt64 end = 0, UInt64 line_length = 32, Func<UInt64, ConsoleColor> color_func = null ) {
            if (end == 0)
                end = (UInt64)bytes.Length;

            if (color_func == null)
                color_func = (_) => ConsoleColor.White;

            for ( UInt64 i = start; i < end; i += line_length ) {
                Console.Write($"{i:X6} | {i-start:X6} : ");
                for ( UInt64 j = 0; j < line_length; j++ ) {
                    Console.ForegroundColor = color_func(i + j);
                    Console.Write(
                        i + j < end ?
                        $"{bytes[i + j]:X2} ".Replace("0", "-") :
                        "   "
                    );
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("  ");
                for ( UInt64 j = 0; j < line_length && i + j < end; j++ ) {
                    Console.ForegroundColor = color_func(i + j);
                    Console.Write(
                        bytes[i + j] >= 32 && bytes[i + j] <= 126 ?
                        $"{(char)bytes[i + j]}" :
                        "."
                    );
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
