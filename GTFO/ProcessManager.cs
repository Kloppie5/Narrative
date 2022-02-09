using System;
using System.Collections.Generic;
using Mono;

namespace GTFO {

    class ProcessManager : MonoProcessManager {
        public ProcessManager ( ) : base("GTFO") {
            /**
            Dictionary<String, UInt64> modules = GetModules64();
            UInt64 BaseAddress = 0;
            foreach ( KeyValuePair<String, UInt64> module in modules ) {
                Console.WriteLine($"{module.Key} {module.Value:X}");
            }
            foreach ( KeyValuePair<String, UInt64> module in modules ) {
                Dictionary<String, UInt64> exportedFunctions = GetExportedFunctions64(BaseAddress);
                foreach ( KeyValuePair<String, UInt64> exportedFunction in exportedFunctions ) {
                    Console.WriteLine($"{exportedFunction.Key} {exportedFunction.Value:X}");
                }
            }
            */
        }
    }
}
