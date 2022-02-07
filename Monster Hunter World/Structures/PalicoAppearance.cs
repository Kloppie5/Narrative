using System;

namespace MonsterHunterWorld {

    class PalicoAppearance {
        Byte[] data;

        UInt32 patternColor1; // [0x00 - 0x04]
        UInt32 patternColor2; // [0x04 - 0x08]
        UInt32 patternColor3; // [0x08 - 0x0C]
        UInt32 furColor; // [0x0C - 0x10]
        UInt32 leftEyeColor; // [0x10 - 0x14]
        UInt32 rightEyeColor; // [0x14 - 0x18]
        UInt32 clothingColor; // [0x18 - 0x1C]
        Single furLength; // [0x1C - 0x20]
        Single furThickness; // [0x20 - 0x24]
        Byte patternType; // [0x24]
        Byte eyeType; // [0x25]
        Byte earType; // [0x26]
        Byte tailType; // [0x27]
        Byte voiceType; // [0x28]
        Byte voicePitch; // [0x29]
        Byte[] UNKNOWN_2A_2B; // [0x2A - 0x2B]

        public PalicoAppearance ( Byte[] data, Int32 offset = 0 ) {
            this.data = new Byte[0x2C];
            Array.Copy(data, offset, this.data, 0, 0x2C);
            Deserialize();
        }

        public void Deserialize () {
            patternColor1 = BitConverter.ToUInt32(data, 0x00);
            patternColor2 = BitConverter.ToUInt32(data, 0x04);
            patternColor3 = BitConverter.ToUInt32(data, 0x08);
            furColor = BitConverter.ToUInt32(data, 0x0C);
            leftEyeColor = BitConverter.ToUInt32(data, 0x10);
            rightEyeColor = BitConverter.ToUInt32(data, 0x14);
            clothingColor = BitConverter.ToUInt32(data, 0x18);
            furLength = BitConverter.ToSingle(data, 0x1C);
            furThickness = BitConverter.ToSingle(data, 0x20);
            patternType = data[0x24];
            eyeType = data[0x25];
            earType = data[0x26];
            tailType = data[0x27];
            voiceType = data[0x28];
            voicePitch = data[0x29];
            UNKNOWN_2A_2B = new Byte[0x2];
            Array.Copy(data, 0x2A, UNKNOWN_2A_2B, 0, 0x2);
        }

        public void Dump () {
            Console.WriteLine("Palico Appearance:");
            Console.WriteLine($"\tColors: {patternColor1:X8} {patternColor2:X8} {patternColor3:X8} {furColor:X8} {leftEyeColor:X8} {rightEyeColor:X8} {clothingColor:X8}");
            Console.WriteLine($"\tFur: {furLength} {furThickness}");
            Console.WriteLine($"\tTypes: {patternType} {eyeType} {earType} {tailType} {voiceType} {voicePitch} {BitConverter.ToString(UNKNOWN_2A_2B)}");
        }
    }
}
