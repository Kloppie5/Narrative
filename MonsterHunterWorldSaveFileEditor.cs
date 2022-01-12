using System;
using System.IO;

namespace SaveFileEditor {
    class MonsterHunterWorldSaveFileEditor : SaveFileEditor {
        public MonsterHunterWorldSaveFileEditor(String filePath) : base(filePath) {
            Console.WriteLine($"Initialized MonsterHunterWorldSaveFileEditor");
        }

        public void debug_dump() {
            Console.WriteLine($"Dumping MonsterHunterWorldSaveFileEditor");
            Console.WriteLine($"filePath: {filePath}");

            // Read file
            Byte[] fileBytes = File.ReadAllBytes(filePath);
            hex_dump(fileBytes, 0, (UInt64) fileBytes.Length, 32);
        }

        public void hex_dump ( Byte[] bytes, UInt64 start = 0, UInt64 end = 0, UInt64 line_length = 32 ) {
            if (end == 0)
                end = (UInt64)bytes.Length;

            for ( UInt64 i = start; i < end; i += line_length ) {
                Console.Write($"{i:X5} : ");
                for ( UInt64 j = 0; j < line_length; j++ )
                    Console.Write(
                        i + j < end ?
                        $"{bytes[i + j]:X2} " :
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
