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
