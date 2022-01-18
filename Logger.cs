using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Narrative;
class Logger {

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

    public static void hex_dump ( Byte[] bytes, AddressRange<UInt32> range, UInt32 line_length = 32, Func<UInt32, ConsoleColor> color_func = null ) {
        hex_dump(bytes, range.start, range.end, line_length, color_func);
    }
    public static void hex_dump ( Byte[] bytes, AddressRange<UInt64> range, UInt32 line_length = 32, Func<UInt32, ConsoleColor> color_func = null ) {
        hex_dump(bytes, (UInt32) range.start, (UInt32) range.end, line_length, color_func);
    }
    public static void hex_dump ( Byte[] bytes, UInt32 start = 0, UInt32 end = 0, UInt32 line_length = 32, Func<UInt32, ConsoleColor> color_func = null ) {
        if (end == 0)
            end = (UInt32)bytes.Length;

        if (color_func == null)
            color_func = (_) => ConsoleColor.White;

        for ( UInt32 i = start; i < end; i += line_length ) {
            Console.Write($"{i:X6} | {i-start:X6} : ");
            for ( UInt32 j = 0; j < line_length; j++ ) {
                Console.ForegroundColor = color_func(i + j);
                Console.Write(
                    i + j < end ?
                    $"{bytes[i + j]:X2} ".Replace("0", "-") :
                    "   "
                );
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("  ");
            for ( UInt32 j = 0; j < line_length && i + j < end; ++j ) {
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
