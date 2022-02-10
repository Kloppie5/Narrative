using System;

namespace MonsterHunterWorld {

    class Investigation {
        Byte[] data;

        public Investigation ( Byte[] data, Int32 offset = 0 ) {
            this.data = new Byte[0x2A];
            Array.Copy(data, offset, this.data, 0, 0x2A);
            Deserialize();
        }

        public void Deserialize () {
            // TODO
        }

        public void Dump () {
            // TODO
        }

    }
}
