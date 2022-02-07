using System;

namespace MonsterHunterWorld {
    class HunterAppearance {
        Byte[] data;

        UInt32 makeup3Color; // [0x00 - 0x04]
        Single makeup3PosX; // [0x04 - 0x08]
        Single makeup3PosY; // [0x08 - 0x0C]
        Single makeup3Width; // [0x0C - 0x10]
        Single makeup3Height; // [0x10 - 0x14]
        Single makeup3Gloss; // [0x14 - 0x18]
        Single makeup3Metal; // [0x18 - 0x1C]
        UInt32 makeup3Lumin; // [0x1C - 0x20]
        UInt32 makeup3Type; // [0x20 - 0x24]
        UInt32 makeup2Color; // [0x24 - 0x28]
        Single makeup2PosX; // [0x28 - 0x2C]
        Single makeup2PosY; // [0x2C - 0x30]
        Single makeup2Width; // [0x30 - 0x34]
        Single makeup2Height; // [0x34 - 0x38]
        Single makeup2Gloss; // [0x38 - 0x3C]
        Single makeup2Metal; // [0x3C - 0x40]
        UInt32 makeup2Lumin; // [0x40 - 0x44]
        UInt32 makeup2Type; // [0x44 - 0x48]
        UInt32 makeup1Color; // [0x48 - 0x4C]
        Single makeup1PosX; // [0x4C - 0x50]
        Single makeup1PosY; // [0x50 - 0x54]
        Single makeup1Width; // [0x54 - 0x58]
        Single makeup1Height; // [0x58 - 0x5C]
        Single makeup1Gloss; // [0x5C - 0x60]
        Single makeup1Metal; // [0x60 - 0x64]
        UInt32 makeup1Lumin; // [0x64 - 0x68]
        UInt32 makeup1Type; // [0x68 - 0x6C]
        UInt32 leftEyeColor; // [0x6C - 0x70]
        UInt32 rightEyeColor; // [0x70 - 0x74]
        UInt32 eyeBrowColor; // [0x74 - 0x78]
        UInt32 facialHairColor; // [0x78 - 0x7C]
        Byte eyeWidth; // [0x7C]
        Byte eyeHeight; // [0x7D]
        Byte skinColorX; // [0x7E]
        Byte skinColorY; // [0x7F]
        Byte age; // [0x80]
        Byte wrinkles; // [0x81]
        Byte noseHeight; // [0x82]
        Byte mouthHeight; // [0x83]
        UInt32 gender; // [0x84 - 0x88]
        Byte browType; // [0x88]
        Byte faceType; // [0x89]
        Byte eyeType; // [0x8A]
        Byte noseType; // [0x8B]
        Byte mouthType; // [0x8C]
        Byte eyeBrowType; // [0x8D]
        Byte eyeLashLength; // [0x8E]
        Byte facialHairType; // [0x8F]
        Byte[] UNKNOWN_90_94; // [0x90 - 0x94]
        UInt32 hairColor; // [0x94 - 0x98]
        UInt32 clothingColor; // [0x99 - 0x9C]
        UInt16 hairType; // [0x9C - 0x9D]
        Byte clothingType; // [0x9E]
        Byte voice; // [0x9F]
        UInt32 expression; // [0xA0 - 0xA4]

        public HunterAppearance ( Byte[] data, Int32 offset = 0 ) {
            this.data = new Byte[0xA4];
            Array.Copy(data, offset, this.data, 0, 0xA4);
            Deserialize();
        }

        public void Deserialize () {
            makeup3Color = BitConverter.ToUInt32(data, 0x0);
            makeup3PosX = BitConverter.ToSingle(data, 0x4);
            makeup3PosY = BitConverter.ToSingle(data, 0x8);
            makeup3Width = BitConverter.ToSingle(data, 0xC);
            makeup3Height = BitConverter.ToSingle(data, 0x10);
            makeup3Gloss = BitConverter.ToSingle(data, 0x14);
            makeup3Metal = BitConverter.ToSingle(data, 0x18);
            makeup3Lumin = BitConverter.ToUInt32(data, 0x1C);
            makeup3Type = BitConverter.ToUInt32(data, 0x20);
            makeup2Color = BitConverter.ToUInt32(data, 0x24);
            makeup2PosX = BitConverter.ToSingle(data, 0x28);
            makeup2PosY = BitConverter.ToSingle(data, 0x2C);
            makeup2Width = BitConverter.ToSingle(data, 0x30);
            makeup2Height = BitConverter.ToSingle(data, 0x34);
            makeup2Gloss = BitConverter.ToSingle(data, 0x38);
            makeup2Metal = BitConverter.ToSingle(data, 0x3C);
            makeup2Lumin = BitConverter.ToUInt32(data, 0x40);
            makeup2Type = BitConverter.ToUInt32(data, 0x44);
            makeup1Color = BitConverter.ToUInt32(data, 0x48);
            makeup1PosX = BitConverter.ToSingle(data, 0x4C);
            makeup1PosY = BitConverter.ToSingle(data, 0x50);
            makeup1Width = BitConverter.ToSingle(data, 0x54);
            makeup1Height = BitConverter.ToSingle(data, 0x58);
            makeup1Gloss = BitConverter.ToSingle(data, 0x5C);
            makeup1Metal = BitConverter.ToSingle(data, 0x60);
            makeup1Lumin = BitConverter.ToUInt32(data, 0x64);
            makeup1Type = BitConverter.ToUInt32(data, 0x68);
            leftEyeColor = BitConverter.ToUInt32(data, 0x6C);
            rightEyeColor = BitConverter.ToUInt32(data, 0x70);
            eyeBrowColor = BitConverter.ToUInt32(data, 0x74);
            facialHairColor = BitConverter.ToUInt32(data, 0x78);
            eyeWidth = data[0x7C];
            eyeHeight = data[0x7D];
            skinColorX = data[0x7E];
            skinColorY = data[0x7F];
            age = data[0x80];
            wrinkles = data[0x81];
            noseHeight = data[0x82];
            mouthHeight = data[0x83];
            gender = BitConverter.ToUInt32(data, 0x84);
            browType = data[0x88];
            faceType = data[0x89];
            eyeType = data[0x8A];
            noseType = data[0x8B];
            mouthType = data[0x8C];
            eyeBrowType = data[0x8D];
            eyeLashLength = data[0x8E];
            facialHairType = data[0x8F];
            UNKNOWN_90_94 = new Byte[4];
            Array.Copy(data, 0x90, UNKNOWN_90_94, 0, 4);
            hairColor = BitConverter.ToUInt32(data, 0x94);
            clothingColor = BitConverter.ToUInt32(data, 0x98);
            hairType = BitConverter.ToUInt16(data, 0x9C);
            clothingType = data[0x9E];
            voice = data[0x9F];
            expression = BitConverter.ToUInt32(data, 0xA0);
        }

        public void Dump ( ) {
            Console.WriteLine("Hunter Appearance:");
            Console.WriteLine($"\tMakeup 3: {makeup3Color:X8} {makeup3PosX:F2} {makeup3PosY:F2} {makeup3Width:F2} {makeup3Height:F2} {makeup3Gloss:F2} {makeup3Metal:F2} {makeup3Lumin:X8} {makeup3Type:X8}");
            Console.WriteLine($"\tMakeup 2: {makeup2Color:X8} {makeup2PosX:F2} {makeup2PosY:F2} {makeup2Width:F2} {makeup2Height:F2} {makeup2Gloss:F2} {makeup2Metal:F2} {makeup2Lumin:X8} {makeup2Type:X8}");
            Console.WriteLine($"\tMakeup 1: {makeup1Color:X8} {makeup1PosX:F2} {makeup1PosY:F2} {makeup1Width:F2} {makeup1Height:F2} {makeup1Gloss:F2} {makeup1Metal:F2} {makeup1Lumin:X8} {makeup1Type:X8}");
            Console.WriteLine($"\tFacial Colors: {leftEyeColor:X8} {rightEyeColor:X8} {eyeBrowColor:X8} {facialHairColor:X8}");
            Console.WriteLine($"\tGeneral build: {eyeWidth} {eyeHeight} {skinColorX} {skinColorY} {age} {wrinkles} {noseHeight} {mouthHeight}");
            Console.WriteLine($"\tGender: {gender}");
            Console.WriteLine($"\tMore face: {browType} {faceType} {eyeType} {noseType} {mouthType} {eyeBrowType} {eyeLashLength} {facialHairType}");
            Console.WriteLine($"\tUnknown: {BitConverter.ToString(UNKNOWN_90_94)}");
            Console.WriteLine($"\tHair Color: {hairColor:X8}");
            Console.WriteLine($"\tClothing Color: {clothingColor:X8}");
            Console.WriteLine($"\tHair Type: {hairType}");
            Console.WriteLine($"\tClothing Type: {clothingType}");
            Console.WriteLine($"\tVoice: {voice}");
            Console.WriteLine($"\tExpression: {expression:X8}");
        }
    }
}
