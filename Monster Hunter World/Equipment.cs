using System;

namespace MonsterHunterWorld {

    class Equipment {
        Byte[] data;

        Int32 index;
        Int32 category;
        Int32 type;
        Int32 id;

        UInt32 level;
        UInt32 points;

        Int32 deco0;
        Int32 deco1;
        Int32 deco2;

        Int32 bgmod0;
        Int32 bgmod1;
        Int32 bgmod2;
        Int32 bgmod3;
        Int32 bgmod4;

        UInt32 HRaugment0;
        UInt32 HRaugment1;
        UInt32 HRaugment2;

        Byte[] unknown_44_4F;

        Byte MRaugmentExt;
        Byte MRaugmentAtt;
        Byte MRaugmentAff;
        Byte MRaugmentDef;
        Byte MRaugmentUp;
        Byte MRaugmentHp;
        Byte MRaugmentEle;

        Byte[] unknown_57_61;
        Byte[] unknown_62_71;
        Byte[] unknown_72_7D;

        public Equipment ( Byte[] data, Int32 offset = 0 ) {
            this.data = new Byte[0x7E];
            Array.Copy(data, offset, this.data, 0, 0x7E);
            Deserialize();
        }

        public void Deserialize () {
            index = BitConverter.ToInt32(data, 0x00);
            category = BitConverter.ToInt32(data, 0x04);
            type = BitConverter.ToInt32(data, 0x08);
            id = BitConverter.ToInt32(data, 0x0C);

            level = BitConverter.ToUInt32(data, 0x10);
            points = BitConverter.ToUInt32(data, 0x14);

            deco0 = BitConverter.ToInt32(data, 0x18);
            deco1 = BitConverter.ToInt32(data, 0x1C);
            deco2 = BitConverter.ToInt32(data, 0x20);

            bgmod0 = BitConverter.ToInt32(data, 0x24);
            bgmod1 = BitConverter.ToInt32(data, 0x28);
            bgmod2 = BitConverter.ToInt32(data, 0x2C);
            bgmod3 = BitConverter.ToInt32(data, 0x30);
            bgmod4 = BitConverter.ToInt32(data, 0x34);

            HRaugment0 = BitConverter.ToUInt32(data, 0x38);
            HRaugment1 = BitConverter.ToUInt32(data, 0x3C);
            HRaugment2 = BitConverter.ToUInt32(data, 0x40);

            unknown_44_4F = new Byte[0x0C];
            Array.Copy(data, 0x44, unknown_44_4F, 0, 0xC);

            MRaugmentExt = data[0x50];
            MRaugmentAtt = data[0x51];
            MRaugmentAff = data[0x52];
            MRaugmentDef = data[0x53];
            MRaugmentUp = data[0x54];
            MRaugmentHp = data[0x55];
            MRaugmentEle = data[0x56];

            unknown_57_61 = new Byte[0x0B];
            Array.Copy(data, 0x57, unknown_57_61, 0, 0x0B);
            unknown_62_71 = new Byte[0x0C];
            Array.Copy(data, 0x62, unknown_62_71, 0, 0x0C);
            unknown_72_7D = new Byte[0x0C];
            Array.Copy(data, 0x72, unknown_72_7D, 0, 0x0C);
        }

        public void Dump () {
            if ( category == -1 )
                return;
            Console.WriteLine($"{index} {category} {type} {id}");
            Console.WriteLine($"  Level: {level} Points: {points}");
            Console.WriteLine($"  Deco: {deco0} {deco1} {deco2}");
            Console.WriteLine($"  Bgmod: {bgmod0} {bgmod1} {bgmod2} {bgmod3} {bgmod4}");
            Console.WriteLine($"  HRaugment: {HRaugment0} {HRaugment1} {HRaugment2}");
            Logger.hex_dump(unknown_44_4F);
            Console.WriteLine($"  MRaugment: {MRaugmentExt} {MRaugmentAtt} {MRaugmentAff} {MRaugmentDef} {MRaugmentUp} {MRaugmentHp} {MRaugmentEle}");
            Logger.hex_dump(unknown_57_61);
            Logger.hex_dump(unknown_62_71);
            Logger.hex_dump(unknown_72_7D);
        }
    }
}
