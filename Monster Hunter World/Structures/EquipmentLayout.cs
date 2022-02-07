using System;

namespace MonsterHunterWorld {

    class EquipmentLayout {
        Byte[] data;

        public EquipmentLayout ( Byte[] data, Int32 offset = 0 ) {
            this.data = new Byte[0x2A4];
            Array.Copy(data, offset, this.data, 0, 0x2A4);
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
