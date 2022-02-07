using System;

namespace MonsterHunterWorld {

    class InventorySlot<T> where T : Enum {
        Byte[] data;

        T id;
        UInt32 amount;

        public InventorySlot ( Byte[] data, Int32 offset = 0 ) {
            this.data = new Byte[0x8];
            Array.Copy(data, offset, this.data, 0, 0x8);
            Deserialize();
        }

        public void Deserialize () {
            id = (T) (Object) BitConverter.ToUInt32(data, 0); // casting abuse
            amount = BitConverter.ToUInt32(data, 4);
        }

        public void Dump () {
            if ( amount == 0 )
                return;
            Console.WriteLine($"  {amount} * {id}");
        }

    }

}
