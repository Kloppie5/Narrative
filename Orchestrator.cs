using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Narrative {

    class Orchestrator {

        Overlay overlay;
        TextWidget console;

        Dictionary<String, List<Type>> widgetRegistry = new Dictionary<String, List<Type>>();

        public Orchestrator ( Overlay overlay ) {
            this.overlay = overlay;

            console = new TextWidget();
            console.Bind(overlay);

            console.AddLine(() => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            RegisterWidget("Leaf Blower Revolution", typeof(LeafBlowerRevolution.ProvidenceWidget));

            CheckProcesses();
        }

        public void CheckProcesses ( ) {
            foreach ( var process in Process.GetProcesses() ) {
                if ( !widgetRegistry.ContainsKey(process.MainWindowTitle) )
                    continue;
                Console.WriteLine($"Found process: {process.MainWindowTitle}");
                foreach ( var widgetType in widgetRegistry[process.MainWindowTitle] ) {
                    if ( !overlay.HasWidget(widgetType) ) {
                        overlay.AddWidget(widgetType);
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

        public void RegisterWidget ( String processName, Type widgetType ) {
            if ( !widgetRegistry.ContainsKey(processName) )
                widgetRegistry.Add(processName, new List<Type>());
            widgetRegistry[processName].Add(widgetType);
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
