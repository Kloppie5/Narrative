using System;
using System.Collections;
using System.Collections.Generic;

namespace Narrative {
    class Mapping<T> : IEnumerable<KeyValuePair<String, List<AddressRange<T>>>> {

        Dictionary<String, List<AddressRange<T>>> dict;
        public IEnumerator<KeyValuePair<String, List<AddressRange<T>>>> GetEnumerator ( ) {
            return dict.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator ( ) {
            return dict.GetEnumerator();
        }

        public Mapping ( ) {
            dict = new Dictionary<String, List<AddressRange<T>>>();
        }

        public void Add ( String key, params AddressRange<T>[] addressranges ) {
            if ( !dict.ContainsKey(key) )
                dict.Add(key, new List<AddressRange<T>>());
            dict[key].AddRange(addressranges);
        }

        public AddressRange<T>[] Get ( String key ) {
            if ( !dict.ContainsKey(key) )
                return new AddressRange<T>[0];
            return dict[key].ToArray();
        }

        public AddressRange<T> GetNamed ( String key, String name ) {
            if ( !dict.ContainsKey(key) )
                return null;
            foreach ( var addressrange in dict[key] )
                if ( addressrange.name == name )
                    return addressrange;
            return null;
        }
    }
}
