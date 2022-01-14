using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MonsterHunterWorld {

    class MonsterHunterWorldSaveFileEditor {

        Byte[] data;

        public MonsterHunterWorldSaveFileEditor ( ) {
            Console.WriteLine($"Initialized MonsterHunterWorldSaveFileEditor");
        }
        public void LoadSaveFile ( String filePath ) {
            Console.WriteLine($"Loaded Save File: {filePath}");
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
            smart_dump_character(data, start, end);
        }
        public static void smart_dump_character ( String filename, UInt64 start = 0, UInt64 end = 0 ) {
            Console.WriteLine($"Dumping Character {filename}");
            Byte[] data = File.ReadAllBytes(filename);
            smart_dump_character(data, start, end);
        }
        public static void dump_equipment ( Byte[] data, UInt64 offset ) {
            Console.WriteLine(
                $" {BitConverter.ToInt32(data, (int) (offset + 0x00))}: "
                + $"id: ({BitConverter.ToInt32(data, (int) (offset + 0x04))}, {BitConverter.ToInt32(data, (int) (offset + 0x08))}, {BitConverter.ToInt32(data, (int) (offset + 0x0C))}) "
                + $"level: ({BitConverter.ToUInt32(data, (int) (offset + 0x10))}, {BitConverter.ToUInt32(data, (int) (offset + 0x14))}) " // TODO figure out safi and kulve weapon levels
                + $"decos: ({BitConverter.ToInt32(data, (int) (offset + 0x18))}, {BitConverter.ToInt32(data, (int) (offset + 0x1C))}, {BitConverter.ToInt32(data, (int) (offset + 0x20))}) "
                + $"bowgun mods: ({BitConverter.ToInt32(data, (int) (offset + 0x24))}, {BitConverter.ToInt32(data, (int) (offset + 0x28))}, {BitConverter.ToInt32(data, (int) (offset + 0x2C))}, {BitConverter.ToInt32(data, (int) (offset + 0x30))}, {BitConverter.ToInt32(data, (int) (offset + 0x34))}) "
                + $"HR augments: ({BitConverter.ToUInt32(data, (int) (offset + 0x38))}, {BitConverter.ToUInt32(data, (int) (offset + 0x3C))}, {BitConverter.ToUInt32(data, (int) (offset + 0x40))}) "
                + $"unknown: ({BitConverter.ToInt32(data, (int) (offset + 0x44))}, {BitConverter.ToInt32(data, (int) (offset + 0x48))}, {BitConverter.ToInt32(data, (int) (offset + 0x4C))}) "
                + $"MR augments: (ext:{data[offset+0x50]}, att:{data[offset+0x51]}, aff:{data[offset+0x52]}, def:{data[offset+0x53]}, up:{data[offset+0x54]}, hp:{data[offset+0x55]}, ele:{data[offset+0x56]}) "
                + $"unknown/padding?: ({data[offset+0x57]}, {data[offset+0x58]}, {data[offset+0x59]}, {data[offset+0x5A]}, {data[offset+0x5B]}, {data[offset+0x5C]}, {data[offset+0x5D]}, {data[offset+0x5E]}, {data[offset+0x5F]}, {data[offset+0x60]}, {data[offset+0x61]}) "

                + $"unknown: ({BitConverter.ToInt32(data, (int) (offset + 0x62))}, {BitConverter.ToInt32(data, (int) (offset + 0x66))}, {BitConverter.ToInt32(data, (int) (offset + 0x6A))}, {BitConverter.ToInt32(data, (int) (offset + 0x6E))}) "
                + $"unknown: ({data[offset+0x72]}, {data[offset+0x73]}, {data[offset+0x74]}, {data[offset+0x75]}, {data[offset+0x76]}, {data[offset+0x77]}, {data[offset+0x78]}, {data[offset+0x79]}, {data[offset+0x7A]}, {data[offset+0x7B]}, {data[offset+0x7C]}, {data[offset+0x7D]}) "
                + $"checksum?: ({data[offset+0x7E]}) "
            );
        }
        public static void smart_dump_character ( Byte[] data, UInt64 start = 0, UInt64 end = 0 ) {
            if (end == 0)
                end = (UInt64) data.Length;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  UNKNOWN [{start:X} - {start + 0x4:X}]");
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine($"  Name [{start + 0x4:X} - {start + 0x44:X}]");
            Console.WriteLine($"  HR [{start + 0x44:X} - {start + 0x48:X}]");
            Console.WriteLine($"  MR [{start + 0x48:X} - {start + 0x4C:X}]");
            Console.WriteLine($"  Zenny [{start + 0x4C:X} - {start + 0x50:X}]");
            Console.WriteLine($"  Research Points [{start + 0x50:X} - {start + 0x54:X}]");
            Console.WriteLine($"  HR XP [{start + 0x54:X} - {start + 0x58:X}]");
            Console.WriteLine($"  MR XP [{start + 0x58:X} - {start + 0x5C:X}]");
            Console.WriteLine($"  Playtime [{start + 0x5C:X} - {start + 0x60:X}]");
            Console.WriteLine($"  Hunter Appearance [{start + 0x60:X} - {start + 0xDC:X}] <0x7C>");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  UNKNOWN [{start + 0x0000DC:X} - {start + 0x0002B2:X}]");
            Console.ForegroundColor = ConsoleColor.White;
            /*
            hex_dump(data, start + 0x0000DC, start + 0x0002B2, 32, (_) => {return ConsoleColor.Yellow;});
            // Palico appearance
            Console.ReadLine();
            */
            Console.WriteLine($"  Guildcard [{start + 0x0002B2:X} - {start + 0x00211D:X}] <0x1E6B>");
            Console.WriteLine($"  Guildcards [{start + 0x00211D:X} - {start + 0x0C02E9:X}] | 100*<0x1E6B>");
            Console.WriteLine($"  Guildcard indices [{start + 0x0C02E9:X} - {start + 0x0C03B1:X}] | 100*<0x2>");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  UNKNOWN [{start + 0x0C03B1:X} - {start + 0x0F3510:X}]");
            Console.ForegroundColor = ConsoleColor.White;
            /*
            hex_dump(data, start + 0x0C03B1, start + 0x0F3510, 32, (_) => {return ConsoleColor.Yellow;});
            // 20 slots of something, same length as guildcards, so maybe a buffer for received guildcards?
            // multiple giant tables
            // << small monster kills
            // many slots of something else
            // padding
            // large float holding structure
            Console.ReadLine();
            */
            Console.WriteLine($"  Item Loadouts [{start + 0x0F3510:X} - {start + 0x10A710:X}] | 80*<0x4A0>");
            Console.WriteLine($"  Item Loadout indices [{start + 0x10A710:X} - {start + 0x10A760:X}] | 80*<0x1>");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  UNKNOWN [{start + 0x10A760:X} - {start + 0x116098:X}]");
            Console.ForegroundColor = ConsoleColor.White;
            /*
            hex_dump(data, start + 0x10A760, start + 0x116098, 32, (_) => {return ConsoleColor.Yellow;});
            // completely empty
            // followed by a bit increment till 77 followed by FF, possibly something like progress tracking <done on complete save>
            // Ooh, maybe some loadouts and indices?
            Console.ReadLine();
            */
            Console.WriteLine($"  Item Pouch [{start + 0x116098:X} - {start + 0x116158:X}] | 24*<0x8>");
            Console.WriteLine($"  Ammo Pouch [{start + 0x116158:X} - {start + 0x1161D8:X}] | 16*<0x8>");
            Console.WriteLine($"  Material Pouch [{start + 0x1161D8:X} - {start + 0x116298:X}] | 24*<0x8>");
            Console.WriteLine($"  Special Pouch [{start + 0x116298:X} - {start + 0x116300:X}] | 13*<0x8> << TODO CHECK");
            Console.WriteLine($"  Item Box [{start + 0x116300:X} - {start + 0x116940:X}] | 200*<0x8>");
            Console.WriteLine($"  Ammo Box [{start + 0x116940:X} - {start + 0x116F80:X}] | 200*<0x8>");
            Console.WriteLine($"  Material Box [{start + 0x116F80:X} - {start + 0x119690:X}] | 1250*<0x8>");
            Console.WriteLine($"  Decoration Box [{start + 0x119690:X} - {start + 0x11A630:X}] | 500*<0x8>");
            Console.WriteLine($"  Equipment Box [{start + 0x11A630:X} - {start + 0x1674A8:X}] | 2500*<0x7E>");
            Console.WriteLine($"  -1 Equipment, last byte different though [{start + 0x1674A8:X} - {start + 0x176FAC:X}] | 510*<0x7E>");
            Console.WriteLine($"  Empty Equipment [{start + 0x176FAC:X} - {start + 0x19D6E8:X}] | 1250*<0x7E>");
            /*
            50 pieces with identification
            everything else is empty
            */
            Console.WriteLine($"  Equipment Box indices [{start + 0x19D6E8:X} - {start + 0x19FDF8:X}] | 2500*<0x4>");
            Console.WriteLine($"  Zeroes [{start + 0x19FDF8:X} - {start + 0x1A05C8:X}] | 2500*<0x4>");
            Console.WriteLine($"  Empty Equipment Box indices [{start + 0x1A05C8:X} - {start + 0x1A194C:X}] | 1250*<0x4>");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  UNKNOWN [{start + 0x1A194C:X} - {start + 0x1A8D48:X}]");
            Console.ForegroundColor = ConsoleColor.White;
            /*
            hex_dump(data, start + 0x19D6E0, start + 0x1A8D48, 32, (_) => {return ConsoleColor.Yellow;});
            // Weapon notice flags
            Console.ReadLine();
            */
            Console.WriteLine($"  NPC Conversations [{start + 0x1A8D48:X} - {start + 0x1ACD48:X}] | 2048*<0x8>");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  UNKNOWN [{start + 0x1ACD48:X} - {start + 0x1AD5DF:X}]");
            Console.ForegroundColor = ConsoleColor.White;
            /*
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
            */
            Console.WriteLine($"  Investigations [{start + 0x1AD5DF:X} - {start + 0x1B177F:X}] | 400*<0x2A>");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  UNKNOWN [{start + 0x1B177F:X} - {start + 0x1B21D1:X}]");
            Console.ForegroundColor = ConsoleColor.White;
            /*
            hex_dump(data, start + 0x1B177F, start + 0x1B21D1, 32, (_) => {return ConsoleColor.Yellow;});
            // Map notice flags
            Console.ReadLine();
            */
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  UNKNOWN [{start + 0x1B21D1:X} - {start + 0x1B60D1:X}] | 128*<0x7E>");
            Console.WriteLine($"  UNKNOWN [{start + 0x1B60D1:X} - {start + 0x1B6955:X}]");
            Console.ForegroundColor = ConsoleColor.White;
            /**
            hex_dump(data, start + 0x1B60D1, start + 0x1B6955, 32, (_) => {return ConsoleColor.Yellow;});
            Console.ReadLine();
            */
            Console.WriteLine($"  Equipment Layouts [{start + 0x1B6955:X} - {start + 0x1DB8D5:X}] | 224*<0x2A4>");

            /**
            Console.WriteLine($"  Equipment Layout indices? [{start + 0x1DB8D5:X} - {start + 0x0:X}] | 224 Int16");
            for (UInt64 i = 0; i < 244; i++) {
                UInt64 offset = start + 0x1DB8D5 + i * 2;
                Int16 index = BitConverter.ToInt16(data, (int)offset);
                Console.WriteLine($"    Equipment Loadout {i} [{offset:X} - {offset + 2:X}] | {index}");
            }
            Console.ReadLine();
            */

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  UNKNOWN [{start + 0x1DB8D5:X} - {start + 0x1E4315:X}] | 112*<0x13C>");
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine($"  Room Configurations [{start + 0x1E4315:X} - {start + 0x1E5ED5:X}] | 24*<x128>");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  UNKNOWN [{start + 0x1E5ED5:X} - {start + 0x1E7082:X}]");
            Console.ForegroundColor = ConsoleColor.White;
            /*
            hex_dump(data, start + 0x1E5ED5, start + 0x1E7082, 32, (_) => {return ConsoleColor.Yellow;});
            // Strange names, some from guildcards, some unknown
            // looks like 12 slots of maybe guildcard linked palico helpers you can find?
            // Guildcard titles and medals notics flags
            Console.ReadLine();
            */
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  UNKNOWN [{start + 0x1E7082:X} - {start + 0x1EA202:X}] | 96*<0x84>");

            Console.WriteLine($"  UNKNOWN [{start + 0x1EA202:X} - {start + 0x2098C0:X}]");
            Console.ForegroundColor = ConsoleColor.White;
            /*
            hex_dump(data, start + 0x1EA202, start + 0x2098C0, 32, (_) => {return ConsoleColor.Yellow;});
            // very empty with the occasional structure
            Console.ReadLine();
            */
            Console.WriteLine($"  Hash [{start + 0x2098C0:X} - {start + 0x209AC0:X}]");
        }

        public void SaveCharacter ( int character, String filename ) {
            Int32 start = 0x3010D8 + character * 0x209AC0;

            Byte[] data = new Byte[0x209AC0];
            Array.Copy(this.data, start, data, 0, 0x209AC0);

            File.WriteAllBytes(filename, data);
        }
        public void LoadCharacter ( int character, String filename ) {
            Int32 start = 0x3010D8 + character * 0x209AC0;

            Byte[] data = File.ReadAllBytes(filename);
            Array.Copy(data, 0, this.data, start, 0x209AC0);
        }
        public static void diff_bytes ( Byte[] data1, Byte[] data2, UInt64 start, UInt64 end, UInt64 line_length = 32 ) {
            for ( UInt64 i = start ; i < end ; i += line_length ) {
                Boolean line_diff = false;
                List<Action> act_1_hex = new List<Action>();
                List<Action> act_1_char = new List<Action>();
                List<Action> act_2_hex = new List<Action>();
                List<Action> act_2_char = new List<Action>();

                for ( UInt64 j = 0 ; j < line_length && i + j < end ; ++j ) {
                    Byte c1 = data1[i + j];
                    Byte c2 = data2[i + j];
                    Boolean diff = c1 != c2;
                    if ( c1 != c2 )
                        diff = true;

                    act_1_hex.Add(() => {Console.ForegroundColor = diff ? ConsoleColor.Red : ConsoleColor.White;});
                    act_1_hex.Add(() => {Console.Write(c1.ToString("X2").Replace("0", "-"));});
                    act_1_char.Add(() => {Console.ForegroundColor = diff ? ConsoleColor.Red : ConsoleColor.White;});
                    act_1_char.Add(() => {Console.Write(c1 >= 32 && c1 <= 126 ? (Char) c1 : '.');});
                    act_2_hex.Add(() => {Console.ForegroundColor = diff ? ConsoleColor.Red : ConsoleColor.White;});
                    act_2_hex.Add(() => {Console.Write(c2.ToString("X2").Replace("0", "-"));});
                    act_2_char.Add(() => {Console.ForegroundColor = diff ? ConsoleColor.Red : ConsoleColor.White;});
                    act_2_char.Add(() => {Console.Write(c2 >= 32 && c2 <= 126 ? (Char) c2 : '.');});

                    if ( diff )
                        line_diff = true;
                }

                if ( line_diff ) {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"{i:X6}: ");
                    act_1_hex.ForEach(cmd => cmd());
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($" | ");
                    act_2_hex.ForEach(cmd => cmd());
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($" || ");
                    act_1_char.ForEach(cmd => cmd());
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($" | ");
                    act_2_char.ForEach(cmd => cmd());
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine();
                }
            }
        }
        public static void diff_characters ( String filename1, String filename2, UInt64 line_length = 32 ) {
            Console.WriteLine($"Diffing {filename1} and {filename2}");
            Byte[] data1 = File.ReadAllBytes(filename1);
            Byte[] data2 = File.ReadAllBytes(filename2);
            diff_bytes(data1, data2, 0, 0x2098C0, line_length);
        }
        public static void smart_diff_characters ( String filename1, String filename2, UInt64 line_length = 32 ) {
            Console.WriteLine($"Diffing {filename1} and {filename2}");
            Byte[] data1 = File.ReadAllBytes(filename1);
            Byte[] data2 = File.ReadAllBytes(filename2);

            Console.ReadLine();
            diff_bytes(data1, data2, 0x000000, 0x000004, line_length);
            Console.WriteLine($"  Unknown signature thing");

            Console.WriteLine($"  Hunter Appearance <0x7C> IGNORED");
            Console.ReadLine();
            diff_bytes(data1, data2, 0x0000DC, 0x0002B2, line_length);
            Console.WriteLine($"  UNKNOWN");

            Console.WriteLine($"  Guildcard <0x1E6B> IGNORED");
            Console.WriteLine($"  Guildcards 100*<0x1E6B> IGNORED");
            Console.ReadLine();
            diff_bytes(data1, data2, 0x0C02E9, 0x0F3510, line_length);
            // 20 slots of something, same length as guildcards, so maybe a buffer for received guildcards?
            // multiple giant tables
            // << small monster kills
            // many slots of something else
            // padding
            // large float holding structure
            /**
            Diffing character_p07_nq_jagras_of_the_ancient_forest.sav and character_p08_jagras_murder.sav
            0E67D9: ------------------------91------ | ------------------------9D------ || ................ | ................
            0E6A19: -------------------------------- | -----------------A-------------- || ................ | ................
            0E8089: A------------------------------- | A-----6---1---1--8-----------6-- || ................ | ...`............
            */
            Console.WriteLine($"  UNKNOWN");

            Console.WriteLine($"  Item Loadouts 80*<0x4A0> IGNORED");
            Console.ReadLine();
            diff_bytes(data1, data2, 0x10A710, 0x116098, line_length);
            // completely empty
            // followed by a bit increment till 77 followed by FF, possibly something like progress tracking <done on complete save>
            Console.WriteLine($"  UNKNOWN");

            Console.WriteLine($"  Item Pouch 24*<0x8> IGNORED");
            Console.WriteLine($"  Ammo Pouch 16*<0x8> IGNORED");
            Console.WriteLine($"  Material Pouch 24*<0x8> IGNORED");
            Console.WriteLine($"  Special Pouch 12*<0x8> IGNORED");
            Console.WriteLine($"  Item Box 200*<0x8> IGNORED");
            Console.WriteLine($"  Ammo Box 200*<0x8> IGNORED");
            Console.WriteLine($"  Material Box 1250*<0x8> IGNORED");
            Console.WriteLine($"  Decoration Box 500*<0x8> IGNORED");
            Console.WriteLine($"  Equipment Box 2500*<0x7E> IGNORED");
            Console.ReadLine();
            diff_bytes(data1, data2, 0x1674A0, 0x176FA4, line_length);
            // Some differences
            Console.WriteLine($"  -1 Equipment 510*<0x7E>");

            Console.ReadLine();
            diff_bytes(data1, data2, 0x176FA4, 0x19D6E0, line_length);
            Console.WriteLine($"  Different Equipment somewhere 1250*<0x7E>");

            Console.ReadLine();
            diff_bytes(data1, data2, 0x19D6E0, 0x1A8D48, line_length);
            // Weapon notice flags
            /**
            Notice hbgs
            1A4690: -------------------------------- | ------------3-------C----------- || ................ | ......0.........
            1A46A0: -------------------------------- | -----------2-------------------- || ................ | ................
            */
            /**
            Diffing character_p07_nq_jagras_of_the_ancient_forest.sav and character_p08_jagras_murder.sav
            1A5AA0: ----------------89-------------- | ----------------89------8A------ || ................ | ................
            1A5AB0: -------------------------------- | 8C------8D------8F------92------ || ................ | ................
            1A5AC0: -------------------------------- | 95------9D------A8------A7------ || ................ | ................
            1A5D70: -------------------------------- | 89--------------8A------8C------ || ................ | ................
            1A5D80: -------------------------------- | --------8D---------------------- || ................ | ................
            1A5D90: -------------------------------- | 8F----------------------92------ || ................ | ................
            1A5DB0: -------------------------------- | ------------------------95------ || ................ | ................
            1A5DE0: -------------------------------- | --------A8------9D-------------- || ................ | ................
            */
            Console.WriteLine($"  UNKNOWN");

            Console.WriteLine($"  NPC Conversations 2048*<0x8> IGNORED");
            Console.ReadLine();
            diff_bytes(data1, data2, 0x1ACD48, 0x1AD5DF, line_length);
            // Palico name is in here, so probably palico tool levels and xp
            // Quest notics flags

            /**
            1ACDC8: 7-D9-1--19-22---8--------------- | 66FC-1--39-A2---8--------------- || p..... ......... | f...9. .........
            1ACDF8: -------------------------------- | ------------------------1------- || ................ | ................
            1ACE28: -------------------------------- | -----4-------------------------- || ................ | ................
            1AD008: -------------------------------- | ----------------------64-------- || ................ | ...........d....
            1AD098: -------------------------------- | -------1------------------------ || ................ | ................
            1AD108: -------1---------------1-------- | -------1-------1-------1-------- || ................ | ................
            1AD118: -------1------------------------ | -------1-------1---------------- || ................ | ................
            1AD148: ------5E9C9A8F2142B1D25E28935F38 | ------5E9C9A8FCCD7F22854B4AA5ADB || ...^...!B..^(._8 | ...^......(T..Z.
            1AD158: 8439255-73-8CA-3AB93BE6-469D9F-5 | 6FD-275-73-8CA-3AB93BE6-469D9F-5 || .9%Ps......`F... | o.'Ps......`F...
            1AD168: 2F8DC6545D173A3755B4578CC5D36898 | 2F8DC6545D173A3755B4578CC5D368EC || /..T].:7U.W...h. | /..T].:7U.W...h.
            1AD178: 5D636FD-A63A4B3537C7DB3FA87359F8 | 5-69AEFBF1ADF13537C7DB3FA87359F8 || ]co..:K57..?.sY. | Pi.....57..?.sY.
            1AD198: 64B9AD-------------------------- | 64B9AD------------------------1- || d............... | d...............
            1AD1B8: -------------------------------- | ---1---------------------------- || ................ | ................
            1AD1D8: -------------------------------- | ------------------------------11 || ................ | ................
            */

            // 1ACDC8: Unlock quest jagras; A8D8 | 7-D9-

            // 1AD228:1 Notice flag; The Great Glutton
			// 1AD227:40 Notice flag; Bird-Brained Bandit
			// 1AD227:10 Notice flag; A Thicket of Thugs
			// 1AD227:8 Notice flag; Butting Heads with Nature
			// 1AD227:4 Notice flag; The Great Jagras Hunt
			// 1AD227:2 Notice flag; A Kestadon Kerfuffle
			// 1AD227:1 Notice flag; Jagras of the Great Forest <<

			// 1AD249:4 Notice flag; Learning the Clutch
            Console.WriteLine($"  UNKNOWN");

            Console.WriteLine($"  Investigations 400*<0x2A> IGNORED");
            Console.ReadLine();
            diff_bytes(data1, data2, 0x1B177F, 0x1B21D1, line_length);
            // Map notice flags
            /**
            1B177F: ---------------------7------1B-- | -------------7------1B---------- || ................ | ................
            1B178F: -----3-2--------1E------1B------ | -----4---7--------------18------ || ................ | ................
            1B179F: -----7------1E-------1-------5-1 | -1-11E------18-----------------2 || ................ | ................
            1B17AF: 1B------15-------C---------115-- | 15-------C------1B-------3-21B-- || ................ | ................
            1B17BF: -----9------1D---------2-C------ | ----18------15-------5--1B------ || ................ | ................
            1B17CF: 1D------15-------4--2-------1C-- | -9------15-------1-11C------1F-- || .......... ..... | ................
            1B17DF: ----1F-------5--1F------1C------ | ----2----------11C------1F------ || ................ | .. .............
            1B17EF: 22---------11C------1F------2--- | 2--------5-21F------2-------1C-- || "............. . |  ......... .....
            1B17FF: -----5-221------23------FFFFFFFF | -----5--23------21------FFFFFFFF || ....!...#....... | ....#...!.......
            1B180F: -5-222------21------27-------3-- | ----21------22------27-------3-1 || .."...!...'..... | ..!..."...'.....
            1B181F: 23------21------22-------1-1FFFF | 22------21------23-------1-2FFFF || #...!..."....... | "...!...#.......
            1B182F: FFFFFFFFFFFFFFFFFFFF-2-113------ | FFFFFFFFFFFFFFFFFFFF-5-225------ || ................ | ............%...
            1B183F: 16------25-------5-216------13-- | 16------13-------2--16------25-- || ....%........... | ..............%.
            1B184F: ----25-------5--FFFFFFFFFFFFFFFF | ----13-------1-1FFFFFFFFFFFFFFFF || ..%............. | ................
            1B185F: FFFFFFFF-1--FFFFFFFFFFFFFFFFFFFF | FFFFFFFF-1-1FFFFFFFFFFFFFFFFFFFF || ................ | ................
            1B186F: FFFF-1-14D------FFFFFFFFFFFFFFFF | FFFF-5-24D------FFFFFFFFFFFFFFFF || ....M........... | ....M...........
            1B187F: -5-24--C-1---1-------1-------8-- | -5--4--C-1---1-------1-------8-- || ..@............. | ..@.............
            1B18AF: -------------------------------- | ----------1-2-8-------8-2--4--2- || ................ | ...... ..... ..
            1B1BAF: -------------------------------- | ---------------------------4-4-- || ................ | ................
            1B1BBF: -------------------------------- | ---8--------2------818---8--2--- || ................ | ...... ....... .
            */
            Console.WriteLine($"  UNKNOWN");

            Console.ReadLine();
            diff_bytes(data1, data2, 0x1B21D1, 0x1B60D1, 0x7E);
            // Only real difference is some flag per object and some data at the end
            Console.WriteLine($"  UNKNOWN 128*<0x7E>");

            Console.ReadLine();
            diff_bytes(data1, data2, 0x1B60D1, 0x1B6955, line_length);
            // Seems like more flags
            /**
            1B6911: -------------------------------- | --5----------------------------- || ................ | .P..............
            1B6921: -------------------------------- | -------------------------------8 || ................ | ................
            */
            Console.WriteLine($"  UNKNOWN");

            Console.WriteLine($"  Equipment Layouts 224*<0x2A4> IGNORED");
            Console.ReadLine();
            diff_bytes(data1, data2, 0x1DB8D5, 0x1E4315, line_length);
            // No difference; maybe something I never touched upon
            Console.WriteLine($"  UNKNOWN 112*<0x13C>");

            Console.WriteLine($"  Room Configurations 24*<0x128> IGNORED");
            Console.ReadLine();
            diff_bytes(data1, data2, 0x1E5ED5, 0x1E7082, line_length);
            // Strange names, some from guildcards, some unknown
            // looks like 12 slots of maybe guildcard linked palico helpers you can find?
            // Guildcard titles and medals notics flags
            /**
            1E60C5: FFFFFFFFFFFFFFFF---6-------6---- | FFFFFFFFFFFFFFFF---5-------2---- || ................ | ................
            1E60D5: ---6-------2-------------------- | ---1---------------------------- || ................ | ................
            1E6185: ----------------------------985D | ----------------------------EC5- || ...............] | ...............P
            1E6195: 636F---------------------------- | 69AE---------------------------- || co.............. | i...............
            */
            Console.WriteLine($"  UNKNOWN");

            Console.ReadLine();
            diff_bytes(data1, data2, 0x1E7082, 0x1EA202, line_length);
            // Some shoutout texts, so probably gestures poses etc.
            Console.WriteLine($"  UNKNOWN 96*<0x84>");

            Console.ReadLine();
            diff_bytes(data1, data2, 0x1EA202, 0x2098C0, line_length);
            // very empty with the occasional structure
            // 206222: Multiplayer tutorial: -----------------8-------------- | -----------------C--------------
            /**
            1EA512: -------------------------------- | -------------------1------------ || ................ | ................
            1EA522: -------------------------------- | -------------------1------------ || ................ | ................
            1EA532: -------------------------------- | -------------------1------------ || ................ | ................
            1EAD92: -------------------------------- | ---1---------------------------- || ................ | ................
            1EB2C2: FF------------------------------ | FF7E------C--------------------- || ................ | .~..............
            1EB2D2: -------------------------------- | --7E------C--------------------- || ................ | .~..............
            206232: -------------------------------- | ----------8--------------------- || ................ | ................
            2063B2: ------------------------727245-7 | ----------------4-------AA77B693 || ............rrE. | ........@....w..
            206912: --FF------1C------1E------4E---- | --FF------21-------9------15---- || .............N.. | .....!..........
            206922: ---2---------------2-------1---- | ---3-------1-------1-------1---- || ................ | ................
            206932: ---1-------1-------1-------3---- | ---1-------1------FEFFFFFFFDFFFF || ................ | ................
            206942: --FDFFFFFF----------------FFFFFF | FF------------------------FFFFFF || ................ | ................
            206E32: EC-7-------7-------7-------7---- | EC-8-------8-------8-------8---- || ................ | ................
            206E42: ---7-------7-------7-------7---- | ---8-------8-------8-------8---- || ................ | ................
            206E52: ---7-------7-------7-------7---- | ---8-------8-------8-------8---- || ................ | ................
            206E62: ---7-------7-------7-------7---- | ---8-------8-------8-------8---- || ................ | ................
            */
            Console.WriteLine($"  UNKNOWN");

            Console.WriteLine($"  Hash IGNORED");

            Console.ReadLine();
        }

        public static void hex_dump ( Byte[] bytes, UInt64 start = 0, UInt64 end = 0, UInt64 line_length = 32, Func<UInt64, ConsoleColor> color_func = null ) {
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
                for ( UInt64 j = 0; j < line_length && i + j < end; ++j ) {
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
