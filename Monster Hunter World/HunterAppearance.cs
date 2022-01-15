using System;

namespace MonsterHunterWorld {
    class HunterAppearance {
        Byte[] data;

        public HunterAppearance ( Byte[] data, Int32 offset = 0 ) {
            this.data = new Byte[0x7C];
            Array.Copy(data, offset, this.data, 0, 0x7C);
            Deserialize();
        }

        public void Deserialize () {
            // TODO
        }

        public void Dump ( ) {
            Console.WriteLine("Hunter Appearance:");
            Logger.hex_dump(data);
        }
    }
}
