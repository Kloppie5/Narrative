using System;

namespace MonsterHunterWorld {

    class RoomConfiguration {
        Byte[] data;

        public RoomConfiguration ( Byte[] data, Int32 offset = 0 ) {
            this.data = new Byte[0x128];
            Array.Copy(data, offset, this.data, 0, 0x128);
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
