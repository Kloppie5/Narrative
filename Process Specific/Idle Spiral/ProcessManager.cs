using System;

namespace IdleSpiral {
    public class ProcessManager : Unity.ProcessManager64 {

        public ProcessManager ( ) : base("IdleSpiral") {
            UInt64 unityRootDomain = GetUnityRootDomain();
            Console.WriteLine($"Mono Base Address: {unityRootDomain:X}");
            UInt64 assembly = GetAssemblyInDomain(unityRootDomain, "Assembly-CSharp");
            Console.WriteLine($"Assembly: {assembly:X}");
            //UInt32 image = ReadAbsolute<UInt32>(assembly + 0x40);
            //Console.WriteLine($"Image: {image:X8}");
            //EnumImageClassCache(image);
        }
    }
}
