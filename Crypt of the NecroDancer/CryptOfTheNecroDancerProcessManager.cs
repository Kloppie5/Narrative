using System;
using System.Text;

using Narrative;

namespace CryptOfTheNecroDancer {
    class CryptOfTheNecroDancerProcessManager : ProcessManager {

        /*
            Crypt of the NecroDancer v2.59
        */
        Mapping<UInt32> mapping = new Mapping<UInt32>();
        public void Init_Mapping ( ) {
            mapping.Add("Gold",       new AddressRange<UInt32>("Memory", 0x4358B4, 0x4358B8));
            mapping.Add("SongTime",   new AddressRange<UInt32>("Memory", 0x435808, 0x43580C));
            mapping.Add("PlayerTime", new AddressRange<UInt32>("Memory", 0x435810, 0x435814));
            mapping.Add("WorldTime",  new AddressRange<UInt32>("Memory", 0x435864, 0x435868));
            mapping.Add("MapSeed",    new AddressRange<UInt32>("Memory", 0x435AF4, 0x435AF8));
        }

        public CryptOfTheNecroDancerProcessManager ( ) : base("Necrodancer") {
            Init_Mapping();

        }

        public void Dump ( ) {
            Console.WriteLine("Dumping Crypt of the NecroDancer...");

            // Gold
            UInt32 gold = ReadRelative<UInt32>(mapping.GetNamed("Gold", "Memory").start);
            Console.WriteLine($"Gold: {gold}");

            // SongTime
            UInt32 songTime = ReadRelative<UInt32>(mapping.GetNamed("SongTime", "Memory").start);
            Console.WriteLine($"SongTime: {songTime / 60000}:{(songTime / 1000) % 60}.{songTime % 1000}");

            // PlayerTime
            UInt32 playerTime = ReadRelative<UInt32>(mapping.GetNamed("PlayerTime", "Memory").start);
            Console.WriteLine($"PlayerTime: {playerTime / 60000}:{(playerTime / 1000) % 60}.{playerTime % 1000}");

            // WorldTime
            UInt32 worldTime = ReadRelative<UInt32>(mapping.GetNamed("WorldTime", "Memory").start);
            Console.WriteLine($"WorldTime: {worldTime / 60000}:{(worldTime / 1000) % 60}.{worldTime % 1000}");

            // MapSeed
            UInt32 mapSeed = ReadRelative<UInt32>(mapping.GetNamed("MapSeed", "Memory").start);
            Console.WriteLine($"MapSeed: {mapSeed}");
        }
    }
}
