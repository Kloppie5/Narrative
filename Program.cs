using System;
using System.Windows.Forms;

namespace Narrative {
    class Program {
        static void Main ( String[] args ) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.Run(new Overlay());
        }
    }
}
