using System;
using System.Collections.Generic;

namespace Narrative {

    class Orchestrator {

        Overlay overlay;
        public Orchestrator ( Overlay overlay ) {
            this.overlay = overlay;

            TextWidget console = new TextWidget(overlay);
            console.AddLine(() => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            CryptOfTheNecroDancerOverlay();
            CultistSimulatorOverlay();
            GTFOOverlay();
            MonsterHunterWorldOverlay();
            // TimeClickersOverlay();
        }

        CryptOfTheNecroDancer.ProcessManager cotnProcessManager;
        public void CryptOfTheNecroDancerOverlay () {
            cotnProcessManager = new CryptOfTheNecroDancer.ProcessManager();
            CryptOfTheNecroDancer.ProvidenceWidget cotnWidget = new CryptOfTheNecroDancer.ProvidenceWidget(overlay, cotnProcessManager);
        }

        CultistSimulator.ProcessManager csProcessManager;
        public void CultistSimulatorOverlay () {
            csProcessManager = new CultistSimulator.ProcessManager();
            /**
            TextWidget dump = new TextWidget(overlay, 0, 200);
            dump.AddLine(() => {
                if (!csProcessManager.CheckConnected())
                    return "";

                UInt32 unityRootDomain = csProcessManager.GetUnityRootDomain();
                Console.WriteLine($"Unity Root Domain: {unityRootDomain:X}");
                UInt32 assembly = csProcessManager.GetAssemblyInDomain(unityRootDomain, "Assembly-CSharp");
                Console.WriteLine($"Assembly: {assembly:X}");
                UInt32 image = csProcessManager.ReadAbsolute<UInt32>(assembly + 0x44);
                Console.WriteLine($"Image: {image:X8}");
                csProcessManager.EnumImageClassCache(image);

                return $"CultistSimulator Image at {image:X8}";
            });
            */
        }

        GTFO.ProcessManager gtfoProcessManager;
        public void GTFOOverlay () {
            gtfoProcessManager = new GTFO.ProcessManager();
        }

        MonsterHunterWorld.ProcessManager mhwProcessManager;
        public void MonsterHunterWorldOverlay () {
            mhwProcessManager = new MonsterHunterWorld.ProcessManager();

            MonsterHunterWorld.DamageWidget mhwDamageWidget = new MonsterHunterWorld.DamageWidget(overlay, mhwProcessManager);
            MonsterHunterWorld.MonsterWidget mhwMonsterWidget = new MonsterHunterWorld.MonsterWidget(overlay, mhwProcessManager);
        }

        Unity.ProcessManager tcProcessManager;
        public void TimeClickersOverlay () {
            tcProcessManager = new Unity.ProcessManager("TimeClickers");
            UInt32 unityRootDomain = tcProcessManager.GetUnityRootDomain();
            Console.WriteLine($"Mono Base Address: {unityRootDomain:X}");
            UInt32 assembly = tcProcessManager.GetAssemblyInDomain(unityRootDomain, "Assembly-CSharp");
            Console.WriteLine($"Assembly: {assembly:X}");
            UInt32 image = tcProcessManager.ReadAbsolute<UInt32>(assembly + 0x40);
            Console.WriteLine($"Image: {image:X8}");
            tcProcessManager.EnumImageClassCache(image);
        }
    }
}
