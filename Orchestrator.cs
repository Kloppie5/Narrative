using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Narrative {

    class Orchestrator {

        Overlay overlay;
        TextWidget console;

        Dictionary<String, Dictionary<String, Type>> widgetRegistry = new Dictionary<string, Dictionary<string, Type>>();

        public Orchestrator ( Overlay overlay ) {
            this.overlay = overlay;

            console = new TextWidget();
            console.Bind(overlay);

            console.AddLine(() => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            //RegisterWidget("Leaf Blower Revolution", "Providence", typeof(LeafBlowerRevolution.ProvidenceWidget));
            //RegisterWidget("The Perfect Tower II", "Placeholder", typeof(ThePerfectTowerII.ProvidenceWidget));
            RegisterWidget("Crypt of the NecroDancer v3.1.2-b2834", "Providence", typeof(CryptOfTheNecroDancer.ProvidenceWidget));
            //RegisterWidget("Cultist Simulator", "Providence", typeof(CultistSimulator.ProvidenceWidget));

            CheckProcesses();
        }

        public void CheckProcesses ( ) {
            foreach ( var process in Process.GetProcesses() ) {
                if ( !widgetRegistry.ContainsKey(process.MainWindowTitle) )
                    continue;
                Console.WriteLine($"Found process: {process.MainWindowTitle}");
                foreach ( var (widgetName, widgetType) in widgetRegistry[process.MainWindowTitle] ) {
                    if ( !overlay.HasWidget(widgetName) ) {
                        overlay.AddWidget(widgetName, widgetType);
                        Console.WriteLine($"Added widget: {widgetType.Name}");
                    }
                }
            }
        }

        Process GetProcess ( String processName ) {
            foreach ( Process process in Process.GetProcesses() )
                if ( process.MainWindowTitle == processName )
                    return process;
            return null;
        }

        public void RegisterWidget ( String processName, String widgetName, Type widgetType ) {
            if ( !widgetRegistry.ContainsKey(processName) )
                widgetRegistry.Add(processName, new Dictionary<String, Type>());
            widgetRegistry[processName].Add($"[{processName}]-[{widgetName}]", widgetType);
        }

        /**
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
        */
    }
}
