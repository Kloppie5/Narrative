using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Narrative;

namespace MonsterHunterWorld {

    class Character {

        Byte[] data;

        Mapping mapping = new Mapping();
        // [exe+0x02C95B27+r8*4]
        public void Init_Mapping ( ) {
            mapping.Add("UNKNOWN_000000_000004", 0x000000, 0x000004, 0x000048, 0x00004C);
            mapping.Add("name",                  0x000004, 0x000044, 0x000050, 0x000090); // 40
            mapping.Add("HR",                    0x000044, 0x000048, 0x000090, 0x000094);
            mapping.Add("MR",                    0x000048, 0x00004C, 0x0000D4, 0x0000D8);
            mapping.Add("zenny",                 0x00004C, 0x000050, 0x000094, 0x000098);
            mapping.Add("researchPoints",        0x000050, 0x000054, 0x000098, 0x00009C);
            mapping.Add("HRXP",                  0x000054, 0x000058, 0x00009C, 0x0000A0);
            mapping.Add("MRXP",                  0x000058, 0x00005C, 0x0000DC, 0x0000E0);
            mapping.Add("playtime",              0x00005C, 0x000060, 0x0000A0, 0x0000A4); // Seconds
            mapping.Add("UNKNOWN_000060_000064", 0x000060, 0x000064, 0x0000A4, 0x0000A8);
            mapping.Add("hunterAppearance",      0x000064, 0x000108, 0x0000E8, 0x00018C); // <0xA4>
            mapping.Add("UNKNOWN_000108_00020C", 0x000108, 0x00020C, 0x0002E8, 0x0003EC);
            mapping.Add("UNKNOWN_00020C_00020D", 0x00020C, 0x00020D, 0x0003EC, 0x0003ED);
            mapping.Add("UNKNOWN_00020D_000285", 0x00020D, 0x000285, 0x0003F8, 0x000470);
            mapping.Add("UNKNOWN_000285_000286", 0x000285, 0x000286, 0x000470, 0x000471);
            mapping.Add("palicoAppearance",      0x000286, 0x0002B2, 0x000198, 0x0001C4); // <0x2C>
            mapping.Add("guildcard",             0x0002B2, 0x00211D, 0x17FA20, 0x181AE0); // <0x1E6B>
            mapping.Add("guildcards",            0x00211D, 0x0C02E9, 0x181AE0, 0x24E5E0); // 100*<0x1E6B>
            mapping.Add("guildcardIndices",      0x0C02E9, 0x0C03B1, 0x000000, 0x000000); // 100*<0x2>
            mapping.Add("UNKNOWN_0C03B1_0C05B5", 0x0C03B1, 0x0C05B5, 0x000000, 0x000000); //
            mapping.Add("guildcardBuffer",       0x0C05B5, 0x0E6611, 0x000000, 0x000000); // 20*<0x1E6B>
            mapping.Add("UNKNOWN_0E6611_0F3510", 0x0E6611, 0x0F3510, 0x000000, 0x000000);
            // ~ [0x24E6A8 // <0x78>]
            // ~ [0x251588]
            // ~ [0x2515A0] 20*<0x20C0> <<< 20 guildcards
            // ~ [0x0002DC] <0x4>
            // ~ [0x0000A8] <0x4>
            // ~ [0x0002C8] <0x4>
            // ~ [0x0000D0] <0x4>
            // ~ [0x0F4CC2] <0x100>
            // ~ [0x0F4DC2] <0x80>
            // ~ [0x0F4E50]
            // ~ [0x0F4E68]
            // ~ [0x0F4E80]
            // ~ [0x0F4E98]
            // ~ [0x0F4EA8] <0x200>
            // ~ [0x0F50A8] <0x200>
            // ~ [0x0F52A8] <0x200>
            // ~ [0x0F54A8] <0x200>
            // ~ [0x0F56A8] <0x200>
            // ~ [0x0F58A8] <0x200>
            // ~ [0x0F64D8]
            // ~ [0x0F5AA8] <0x200>
            // ~ [0x0F5CA8] <0x200>
            // ~ [0x0F5EA8] <0x200>
            // ~ [0x0F60A8] <0x200>
            // ~ [0x0F62A8] <0x200>
            // ~ [0x0F64B0]
            // ~ [0x0F64C8]
            // ~ [0x0F6500] <0x80>

            // ~ [0x0F64F4] Single
            // ~ [0x0F6D70] <0x4>
            // ~ [0x0F6D74] <0x4>
            // ~ [0x0F6D78] <0x4>
            // ~ [0x0F6D80] <0x4>

            // ~ ?

            // ~ [0x0F6D90]
            // ~ [0x0F6D98] 400

            // ~ [0x0F6F28] 3*<0x8E0>

            // ~ [0x0F89C8] <0x470>
            // ~ [0x0F8E38] <0x470>
            // ~ [0x0F92A8] <0x200>

            // ~ [0x0F94A8]
            // ~ [0x0F99E0]
            // ~ [0x0F99E8] 24*<0x530>

            // ~ [0x101668] <0x300>
            // ~ [0x101968]
            // ~ [0x1021D0] <0x80>
            // ~ [0x102250] <0x80>
            // ~ [0x1022D8]
            // ~ [0x1022E8]
            // ~ [0x1022F8]
            // ~ [0x102310]
            // ~ [0x102328]
            // ~ [0x102340]
            // ~ [0x102358]
            // ~ [0x102380]
            // ~ [0x1023A8]
            // ~ [0x1023B8]
            // ~ [0x1023C8]
            // ~ [0x1023E0]
            // ~ [0x102400]
            // ~ [0x102410] <0x8>
            // ~ [0x102418] <0x1>
            // ~ [0x10241A]
            // ~ [0x102420]
            mapping.Add("itemLoadouts",          0x0F3510, 0x116010, 0x000738, 0x037FF8); // 120*<0x4A0>
            mapping.Add("itemLoadoutIndices",    0x116010, 0x116088, 0x000000, 0x000000); // 120*<0x1>
            mapping.Add("itemPouch",             0x116098, 0x116158, 0x038080, 0x000000); // 24*<0x8>
            mapping.Add("ammoPouch",             0x116158, 0x1161D8, 0x038200, 0x000000); // 16*<0x8>
            mapping.Add("materialPouch",         0x1161D8, 0x116298, 0x038380, 0x000000); // 24*<0x8>
            mapping.Add("specialPouch",          0x116298, 0x116300, 0x038500, 0x000000); // 13 or 5*<0x8>
            mapping.Add("itemBox",               0x116300, 0x116940, 0x038A08, 0x000000); // 200**<0x8>
            mapping.Add("ammoBox",               0x116940, 0x116F80, 0x039688, 0x000000); // 200*<0x8>
            mapping.Add("materialBox",           0x116F80, 0x119690, 0x03A308, 0x000000); // 1250*<0x8>
            mapping.Add("decorationBox",         0x119690, 0x11A630, 0x03F128, 0x000000); // 500*<0x8>
            mapping.Add("equipmentBox",          0x11A630, 0x1674A8, 0x041068, 0x000000); // 2500*<0x7E>
            mapping.Add("invalidEquipment",      0x1674A8, 0x176FAC, 0x000000, 0x000000); // 510*<0x7E> Memory copy does 3010 at once
            mapping.Add("emptyEquipment",        0x176FAC, 0x19D6E8, 0x0B0B98, 0x000000); // 1250*<0x7E>
            mapping.Add("equipmentBoxIndices",   0x19D6E8, 0x19FDF8, 0x000000, 0x000000); // 2500*<0x4>
            // ~ [0x0DF1C8] <12000>
            // ~ [0x0E20A8] <5000>
            mapping.Add("EMPTY_19FDF8_1A05C8",   0x19FDF8, 0x1A05C8, 0x000000, 0x000000); // 2500*<0x4>
            mapping.Add("emptyEquipmentIndices", 0x1A05C8, 0x1A194C, 0x000000, 0x000000); // 1250*<0x4>
            mapping.Add("UNKNOWN_1A194C_1A8D48", 0x1A194C, 0x1A8D48, 0x000000, 0x000000);
            mapping.Add("NPCConversations",      0x1A8D48, 0x1ACD48, 0x0E6430, 0x000000); // 2048*<0x8>
            mapping.Add("UNKNOWN_1ACD48_1AD5DF", 0x1ACD48, 0x1AD5DF, 0x000000, 0x000000);
            // ~ [0x0E6C30] <1024>
            // ~ [0x0E7030] <1024>
            // ~ [0x0E7430]
            // ~ [0x0E7450
            // ~ [0x0E79F0] 14*<0x40>
            // ~ [0x0E7D70] 6*<0x80>
            // ~ [0x0E8070] x*<0x40>
            // ~ [0x0E83F0] x*<0x80>
            // ~ [0x0E86F0]
            // ~ [0x0E8730] 2*<0x80>
            // ~ [0x0E8830]
            // ~ [0x0E8870] <0x80>
            // ~ [0x0E8970]
            // ~ [0x0E89B0]
            // ~ [0x0E89F0]
            // ~ [0x0E7434]
            // ~ [0x0E8A18]
            // ~ [0x0E8A58]
            // ~ [0x0E8A80] 2048
            // ~ [0x0E9284]
            // ~ [0x0E9280]
            // ~ [0x0E9288]
            // ~ [0x0E92A4]
            // ~ [0x0E92A0]
            // ~ [0x0E92A8]
            // ~ [0x0E92C4]
            // ~ [0x0E92C0]
            // ~ [0x0E92C8]
            // ~ [0x0F4938]

            // ~ [0x0EDF38] <0xA0>
            // ~ [0x0EDFD8] <0xA0>
            // ~ [0x0EE078] <0x3200>
            mapping.Add("investigations",        0x1AD5DF, 0x1B177F, 0x105898, 0x000000); // 400*<0x2A>
            mapping.Add("UNKNOWN_1B177F_1B21B1", 0x1B177F, 0x1B21B1, 0x000000, 0x000000);
            mapping.Add("UNKNOWN_1B21B1_1B60B1", 0x1B21B1, 0x1B60B1, 0x0E92E8, 0x000000); // 128*<0x7E>
            mapping.Add("UNKNOWN_1B60B1_1B6955", 0x1B60B1, 0x1B6955, 0x000000, 0x000000); // ~ [] <0x400>
            mapping.Add("equipmentLayouts",      0x1B6955, 0x1DB8D5, 0x11E758, 0x000000); // 224*<0x2A4>
            mapping.Add("UNKNOWN_1DB8D5_1E4315", 0x1DB8D5, 0x1E4315, 0x144158, 0x000000); // 112*<0x13C>
            mapping.Add("roomConfigurations",    0x1E4315, 0x1E5ED5, 0x14D0D8, 0x000000); // 24*<0x128>
            mapping.Add("UNKNOWN_1E5ED5_2098C0", 0x1E5ED5, 0x2098C0, 0x000000, 0x000000);
            // ~ [0x103028] inventory
            // ~ [0x103068] inventory
            // ~ [0x1030B8] 50 slot inventory

            // ~ [] 4*<0x540>

            // ~ [] x*<0xF8>

            // ~ [0x27B5E8] inventory

            // ~ [0x16C948] 100*<0x218>
            // ~ [0x179C38] 400*<0x218>

            // ~ [] <0x2800>
            mapping.Add("hash",                  0x2098C0, 0x209AC0, 0x000000, 0x000000);
        }

        public Character ( String filename ) {
            Init_Mapping();
            LoadRaw( filename );
        }
        public Character ( Byte[] data, int offset = 0 ) {
            Init_Mapping();
            this.data = new Byte[0x209AC0];
            Array.Copy(data, offset, this.data, 0, 0x209AC0);
            CharacterEncryption.DecryptCharacter(this.data);
        }

        public void Save ( String filename ) {
            CharacterEncryption.EncryptCharacter(data);
            File.WriteAllBytes( filename, data );
        }
        public void SaveRaw ( String filename ) {
            File.WriteAllBytes( filename, data );
        }
        public void Load ( String filename ) {
            data = File.ReadAllBytes( filename );
            CharacterEncryption.DecryptCharacter(data);
        }
        public void LoadRaw ( String filename ) {
            data = File.ReadAllBytes( filename );
        }

        public void Dump ( ) {
            // UNKNOWN_000000_000004
            // Some progress tracking

            // UNKNOWN_000060_000064
            Console.WriteLine($"UNKNOWN_000060_000064 [000060 - 000064]");
            Logger.hex_dump(data, mapping.GetSaveTuple("UNKNOWN_000060_000064"));
            Console.ReadLine();

            // UNKNOWN_000108_00020C
            Console.WriteLine($"UNKNOWN_000108_00020C [000108 - 00020C]");
            Logger.hex_dump(data, mapping.GetSaveTuple("UNKNOWN_000108_00020C"));
            Console.ReadLine();

            // UNKNOWN_00020C_00020D
            // Padding or single flag

            // UNKNOWN_00020D_000285
            Console.WriteLine($"UNKNOWN_00020D_000285 [00020D - 000285]");
            Logger.hex_dump(data, mapping.GetSaveTuple("UNKNOWN_00020D_000285"));
            Console.ReadLine();

            // UNKNOWN_000285_000286
            // Padding or single flag

            // UNKNOWN_0C03B1_0C05B5
            Console.WriteLine($"UNKNOWN_0C03B1_0C05B5 [0C03B1 - 0C05B5]");
            Logger.hex_dump(data, mapping.GetSaveTuple("UNKNOWN_0C03B1_0C05B5"));
            Console.ReadLine();

            // UNKNOWN_0E6611_0F3510
            Console.WriteLine($"UNKNOWN_0E6611_0F3510 [0E6611 - 0F3510]");
            Logger.hex_dump(data, mapping.GetSaveTuple("UNKNOWN_0E6611_0F3510"));
            Console.ReadLine();

            // UNKNOWN_1A194C_1A8D48
            Console.WriteLine($"UNKNOWN_1A194C_1A8D48 [1A194C - 1A8D48]");
            Logger.hex_dump(data, mapping.GetSaveTuple("UNKNOWN_1A194C_1A8D48"));
            Console.ReadLine();

            // UNKNOWN_1ACD48_1AD5DF
            Console.WriteLine($"UNKNOWN_1ACD48_1AD5DF [1ACD48 - 1AD5DF]");
            Logger.hex_dump(data, mapping.GetSaveTuple("UNKNOWN_1ACD48_1AD5DF"));
            Console.ReadLine();

            // UNKNOWN_1B177F_1B21B1
            // Weapon notice flags?
            Console.WriteLine($"UNKNOWN_1B177F_1B21B1 [1B177F - 1B21B1]");
            Logger.hex_dump(data, mapping.GetSaveTuple("UNKNOWN_1B177F_1B21B1"));
            Console.ReadLine();

            // UNKNOWN_1B21B1_1B60B1
            // More equipment?
            Console.WriteLine($"UNKNOWN_1B21B1_1B60B1 [1B21B1 - 1B60B1]");
            Logger.hex_dump(data, mapping.GetSaveTuple("UNKNOWN_1B21B1_1B60B1"));
            Console.ReadLine();

            // UNKNOWN_1B60B1_1B6955
            Console.WriteLine($"UNKNOWN_1B60B1_1B6955 [1B60B1 - 1B6955]");
            Logger.hex_dump(data, mapping.GetSaveTuple("UNKNOWN_1B60B1_1B6955"));
            Console.ReadLine();

            // UNKNOWN_1DB8D5_1E4315
            // structures
            Console.WriteLine($"UNKNOWN_1DB8D5_1E4315 [1DB8D5 - 1E4315]");
            Logger.hex_dump(data, mapping.GetSaveTuple("UNKNOWN_1DB8D5_1E4315"));
            Console.ReadLine();

            // UNKNOWN_1E5ED5_2098C0
            Console.WriteLine($"UNKNOWN_1E5ED5_2098C0 [1E5ED5 - 2098C0]");
            Logger.hex_dump(data, mapping.GetSaveTuple("UNKNOWN_1E5ED5_2098C0"));
            Console.ReadLine();
        }
    }
}
