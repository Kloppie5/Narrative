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
            IdleSpiralOverlay();
            MonsterHunterWorldOverlay();
        }

        CryptOfTheNecroDancer.ProcessManager cotnProcessManager;
        public void CryptOfTheNecroDancerOverlay () {
            cotnProcessManager = new CryptOfTheNecroDancer.ProcessManager();
            CryptOfTheNecroDancer.ProvidenceWidget cotnWidget = new CryptOfTheNecroDancer.ProvidenceWidget(overlay, cotnProcessManager);
        }

        IdleSpiral.ProcessManager64 isProcessManager;
        public void IdleSpiralOverlay () {
            isProcessManager = new IdleSpiral.ProcessManager64();
            IdleSpiral.ProvidenceWidget isWidget = new IdleSpiral.ProvidenceWidget(overlay, isProcessManager);
        }

        MonsterHunterWorld.ProcessManager mhwProcessManager;
        public void MonsterHunterWorldOverlay () {
            mhwProcessManager = new MonsterHunterWorld.ProcessManager();

            MonsterHunterWorld.DamageWidget mhwDamageWidget = new MonsterHunterWorld.DamageWidget(overlay, mhwProcessManager);
            MonsterHunterWorld.MonsterWidget mhwMonsterWidget = new MonsterHunterWorld.MonsterWidget(overlay, mhwProcessManager);
        }
    }
}
