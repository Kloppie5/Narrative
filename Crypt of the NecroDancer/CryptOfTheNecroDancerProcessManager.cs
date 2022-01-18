using System;

using Narrative;

namespace CryptOfTheNecroDancer {
    class CryptOfTheNecroDancerProcessManager : ProcessManager {

        Mapping<UInt64> mapping = new Mapping<UInt64>();
        public void Init_Mapping ( ) {

        }

        public CryptOfTheNecroDancerProcessManager ( ) : base("Necrodancer") {
            Init_Mapping();
        }

        public void Dump ( ) {

        }
    }
}
