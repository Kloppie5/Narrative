using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

using Narrative;

using UInt8 = System.Byte;

namespace LeafBlowerRevolution {

    public class ProvidenceWidget : Widget {

        ProcessManager manager;
        public ProvidenceWidget ( ) {
            manager = new ProcessManager();   
        }

        public override void Paint ( PaintEventArgs e ) {
            if (!manager.CheckConnected())
                return;

            IntPtr ptr = manager.process.MainWindowHandle;
            Rect windowRect = new Rect();
            ProcessManager64.GetWindowRect(ptr, ref windowRect);
            e.Graphics.DrawRectangle(new Pen(Color.White, 2), windowRect.Left, windowRect.Top, windowRect.Right - windowRect.Left, windowRect.Bottom - windowRect.Top);

            Dictionary<String, Double> inventory = manager.GetInventory();

            e.Graphics.DrawString($"Green Leaves: {inventory["Green Leaves"]}", new Font("Consolas", 12), new SolidBrush(Color.White), windowRect.Left, windowRect.Top + 20);
        }
    }
}
