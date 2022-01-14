using System;
using System.IO;

namespace MonsterHunterWorld {
    class SaveFile {
        Byte[] data;

        // Save file header [0x0 - 0x60]
        // Byte[] magic; // [0x0 - 0x4] 01 00 00 00
        // Byte[] UNKNOWN_4_C; // [0x4 - 0xC]
        Byte[] dataHash; // [0xC - 0x20]
        // UInt64 dataSize; // [0x20 - 0x28]
        // Byte[] steamID; // [0x28 - 0x30]
        // Byte[] PADDING_30_40; // [0x30 - 0x40]
        // UInt64 offsetControls; // [0x40 - 0x48]
        // UInt64 offsetOptions; // [0x48 - 0x50]
        // UInt64 offsetSection2; // [0x50 - 0x58]
        // UInt64 offsetCharacters; // [0x58 - 0x60]

        // ControlsRegion [0x60 - 0x300070]
        // Byte[] controlsSignature; // [0x60 - 0x64]
        // Byte[] controlsUnknown; // [0x64 - 0x68]
        // UInt64 controlsSize; // [0x68 - 0x70]
        // Byte[] controlsData; // [0x70 - 0x300070]

        // OptionsRegion [0x300070 - 0x301080]
        // Byte[] optionsSignature; // [0x300070 - 0x300074]
        // Byte[] optionsUnknown; // [0x300074 - 0x300078]
        // UInt64 optionsSize; // [0x300078 - 0x300080]
        // Byte[] optionsData; // [0x300080 - 0x301080]

        // Section2Region [0x301080 - 0x3010C8]
        // Byte[] section2Signature; // [0x301080 - 0x301084]
        // Byte[] section2Unknown; // [0x301084 - 0x301088]
        // UInt64 section2Size; // [0x301088 - 0x301090]
        // Byte[] section2Data; // [0x301090 - 0x3010C8]

        // CharactersRegion [0x3010C8 - 0xAC30E0]
        Byte[] charactersSignature; // [0x3010C8 - 0x3010CC]
        // Byte[] charactersUnknown; // [0x3010CC - 0x3010D0]
        // UInt64 charactersSize; // [0x3010D0 - 0x3010D8]

        // CharactersData [0x3010D8 - 0xAC30D8]
        Character character1; // [0x3010D8 - 0x50AB98]
        Character character2; // [0x50AB98 - 0x714658]
        Character character3; // [0x714658 - 0x91E118]
        // Byte[] PADDING_91E118_AC30D8; // [0x91E118 - 0xAC30D8]

        // Byte[] PADDING_AC30D8_AC30E0; // [0xAC30D8 - 0xAC30E0]

        public SaveFile ( String path ) {
            data = File.ReadAllBytes(path);
            SaveFileEncryption.Decrypt(data);
            character1 = new Character(data, 0x3010D8);
            character2 = new Character(data, 0x50AB98);
            character3 = new Character(data, 0x714658);
        }

        public void Dump ( Boolean verbose = false ) {
            Console.WriteLine($"SaveFileHeader[0x0 - 0x60]");
            Console.WriteLine($"  Magic [0x0 - 0x4] = {BitConverter.ToString(data, 0x0, 0x4)}");
            Console.WriteLine($"  UNKNOWN [0x4 - 0xC] = {BitConverter.ToString(data, 0x4, 0xC)}");
            Console.WriteLine($"  DataHash [0xC - 0x20] = {BitConverter.ToString(data, 0xC, 0x20)}");
            Console.WriteLine($"  DataSize [0x20 - 0x28] = 0xAC30A0");
            Console.WriteLine($"  SteamID [0x28 - 0x30] = {BitConverter.ToString(data, 0x28, 0x30)}");
            Console.WriteLine($"  PADDING [0x30 - 0x40] = 00 ... 00");
            Console.WriteLine($"  OffsetControls [0x40 - 0x48] = 0x60");
            Console.WriteLine($"  OffsetOptions [0x48 - 0x50] = 0x300070");
            Console.WriteLine($"  OffsetSection2 [0x50 - 0x58] = 0x301080");
            Console.WriteLine($"  OffsetCharacters [0x58 - 0x60] = 0x3010C8");

            Console.WriteLine($"ControlsRegion [0x60 - 0x300070]");
            Console.WriteLine($"  ControlsSignature [0x60 - 0x64] = {BitConverter.ToString(data, 0x60, 0x4)}");
            Console.WriteLine($"  UNKNOWN [0x64 - 0x68] = {BitConverter.ToString(data, 0x64, 0x8)}");
            Console.WriteLine($"  ControlsSize [0x68 - 0x70] = 0x300000");
            Console.WriteLine($"  ControlsData [0x70 - 0x300070]");
            if ( verbose )
                Console.WriteLine($"    {BitConverter.ToString(data, 0x70, 0x300000)}");

            Console.WriteLine($"OptionsRegion [0x300070 - 0x301080]");
            Console.WriteLine($"  OptionsSignature [0x300070 - 0x300074] = {BitConverter.ToString(data, 0x300070, 0x4)}");
            Console.WriteLine($"  UNKNOWN [0x300074 - 0x300078] = {BitConverter.ToString(data, 0x300074, 0x8)}");
            Console.WriteLine($"  OptionsSize [0x300078 - 0x300080] = 0x1000");
            Console.WriteLine($"  OptionsData [0x300080 - 0x301080]");
            if ( verbose )
                Console.WriteLine($"    {BitConverter.ToString(data, 0x300080, 0x1000)}");

            Console.WriteLine($"Section2Region [0x301080 - 0x3010C8]");
            Console.WriteLine($"  Section2Signature [0x301080 - 0x301084] = {BitConverter.ToString(data, 0x301080, 0x4)}");
            Console.WriteLine($"  UNKNOWN [0x301084 - 0x301088] = {BitConverter.ToString(data, 0x301084, 0x8)}");
            Console.WriteLine($"  Section2Size [0x301088 - 0x301090] = 0x38");
            Console.WriteLine($"  Section2Data [0x301090 - 0x3010C8]");
            if ( verbose )
                Console.WriteLine($"    {BitConverter.ToString(data, 0x301090, 0x38)}");

            Console.WriteLine($"CharactersRegion [0x3010C8 - 0xAC30E0]");
            Console.WriteLine($"  CharactersSignature [0x3010C8 - 0x3010CC] = {BitConverter.ToString(data, 0x3010C8, 0x4)}");
            Console.WriteLine($"  UNKNOWN [0x3010CC - 0x3010D0] = {BitConverter.ToString(data, 0x3010CC, 0x8)}");
            Console.WriteLine($"  CharactersSize [0x3010D0 - 0x3010D8] = 0x7C2000");
            Console.WriteLine($"  CharactersData [0x3010D8 - 0xAC30D8]");
            Console.WriteLine($"    Character1 [0x3010D8 - 0x50AB98]");
            Console.WriteLine($"    Character2 [0x50AB98 - 0x714658]");
            Console.WriteLine($"    Character3 [0x714658 - 0x91E118]");
            Console.WriteLine($"    PADDING [0x91E118 - 0xAC30D8]");
            Console.WriteLine($"  PADDING [0xAC30D8 - 0xAC30E0]");
        }
    }
}
