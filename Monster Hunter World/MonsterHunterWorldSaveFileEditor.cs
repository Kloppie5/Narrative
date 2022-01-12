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

            hex_dump(data, start, end, 32, (UInt64 i) => {
                UInt64 pos = i - start;
                if (pos >= 0x000004 && pos < 0x000044) return ConsoleColor.Green; // Name
                if (pos >= 0x000044 && pos < 0x000048) return ConsoleColor.Green; // HR
                if (pos >= 0x000048 && pos < 0x00004C) return ConsoleColor.Green; // MR
                if (pos >= 0x00004C && pos < 0x000050) return ConsoleColor.Green; // Zenny
                if (pos >= 0x000050 && pos < 0x000054) return ConsoleColor.Green; // Research Points
                if (pos >= 0x000054 && pos < 0x000058) return ConsoleColor.Green; // HR XP
                if (pos >= 0x000058 && pos < 0x00005C) return ConsoleColor.Green; // MR XP
                if (pos >= 0x00005C && pos < 0x000060) return ConsoleColor.Green; // Playtime
                if (pos >= 0x000064 && pos < 0x0000DC) return ConsoleColor.Yellow; // Hunter Appearance
                // 0x0000DC - 0x0002B2
                if (pos >= 0x0002B2 && pos < 0x00211D) return ConsoleColor.Yellow; // Guildcard
                if (pos >= 0x00211D && pos < 0x0C02E9) return ConsoleColor.Yellow; // Guildcards | 100 slots
                if (pos >= 0x0F3510 && pos < 0x10A710) return ConsoleColor.Yellow; // Item Loadouts | 80 slots
                if (pos >= 0x116098 && pos < 0x116158) return ConsoleColor.Yellow; // Item Pouch | 24 slots
                if (pos >= 0x116158 && pos < 0x1161D8) return ConsoleColor.Yellow; // Ammo Pouch | 16 slots
                if (pos >= 0x1161D8 && pos < 0x116298) return ConsoleColor.Yellow; // Material Pouch | 24 slots
                if (pos >= 0x116298 && pos < 0x1162F8) return ConsoleColor.Yellow; // Special Pouch | 12 slots
                if (pos >= 0x1162F8 && pos < 0x116938) return ConsoleColor.Yellow; // Item Box | 200 slots
                if (pos >= 0x116938 && pos < 0x116F78) return ConsoleColor.Yellow; // Ammo Box | 200 slots
                if (pos >= 0x116F78 && pos < 0x119688) return ConsoleColor.Yellow; // Material Box | 1250 slots
                if (pos >= 0x119688 && pos < 0x11A628) return ConsoleColor.Yellow; // Decoration Box | 500 slots
                if (pos >= 0x11A628 && pos < 0x1674A0) return ConsoleColor.Yellow; // Equipment Box | 2500 slots
                if (pos >= 0x1674A0 && pos < 0x176FA4) return ConsoleColor.DarkYellow; // -1 Equipment | 510 slots (probably 500 and 10 of different types)
                if (pos >= 0x176FA4 && pos < 0x19D6E0) return ConsoleColor.DarkYellow; // Empty Equipment | 1250 slots (maybe used after filling the main equipment box?)

                if (pos >= 0x1A8D48 && pos < 0x1ACD48) return ConsoleColor.Yellow; // NPC Conversations | 2048 slots

                if (pos >= 0x1AD5DF && pos < 0x1B177F) return ConsoleColor.Yellow; // Investigations | 400 slots
                if (pos >= 0x1B21D1 && pos < 0x1B60D1) return ConsoleColor.DarkYellow; // Something? | 128 slots

                if (pos >= 0x1B6955 && pos < 0x1DB8D5) return ConsoleColor.Yellow; // Equipment Layouts | 224 slots
                if (pos >= 0x1DB8D5 && pos < 0x1E4315) return ConsoleColor.DarkYellow; // Something? | 112 slots
                if (pos >= 0x1E4315 && pos < 0x1E5ED5) return ConsoleColor.DarkYellow; // Something? | 24 slots
                if (pos >= 0x1E7082 && pos < 0x1EA202) return ConsoleColor.DarkYellow; // Something? | 96 slots

                if (pos >= 0x2098C0 && pos < 0x209AC0) return ConsoleColor.Green; // Hash

                return ConsoleColor.Red;
            });
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
                        "    "
                    );
                }
                Console.Write("  ");
                for ( UInt64 j = 0; j < line_length && i + j < end; j++ ) {
                    Console.ForegroundColor = color_func(i + j);
                    Console.Write(
                        bytes[i + j] >= 32 && bytes[i + j] <= 126 ?
                        $"{(char)bytes[i + j]}" :
                        "."
                    );
                }
                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
