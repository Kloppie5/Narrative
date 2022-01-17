using System;
using System.Collections.Generic;

namespace Narrative {

    using MappingType = Dictionary<String,
        Tuple<Tuple<UInt32, UInt32>,
        Tuple<UInt32, UInt32>
    > >;
    class Mapping {

        MappingType dict;
        public Mapping ( ) {
            dict = new MappingType();
        }

        public void Add ( String key, UInt32 savefile_offset, UInt32 savefile_size, UInt32 memory_offset, UInt32 memory_size ) {
            dict.Add(
              key, new Tuple<Tuple<UInt32, UInt32>,Tuple<UInt32, UInt32>>(new Tuple<UInt32, UInt32>(
              savefile_offset,
              savefile_size), new Tuple<UInt32, UInt32>(
              memory_offset,
              memory_size))
            );
        }

        public MappingType Access() {
            return dict;
        }

        public Tuple<UInt32, UInt32> GetSaveTuple ( String key ) {
            return dict[key].Item1;
        }
    }
}
