using System;
using System.Windows.Forms;

namespace Narrative {
    class Program {
        static void Main ( String[] args ) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Overlay overlay = new Overlay();
            
            Orchestrator orchestrator = new Orchestrator(overlay);
            Application.Run(overlay);
            Console.ReadLine();
        }
    }
}
