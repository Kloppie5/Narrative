using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using Narrative;

namespace IdleSpiral {

    public class ProvidenceWidget : Widget {

        ProcessManager manager;
        public ProvidenceWidget ( Overlay overlay, ProcessManager manager ) : base(overlay) {
            this.manager = manager;
        }

        public override void Paint ( PaintEventArgs e ) {
            if (!manager.CheckConnected())
                return;

            IntPtr ptr = manager.process.MainWindowHandle;
            ProcessManager.Rect windowRect = new ProcessManager.Rect();
            ProcessManager.GetWindowRect(ptr, ref windowRect);
            e.Graphics.DrawRectangle(new Pen(Color.White, 2), windowRect.Left, windowRect.Top, windowRect.Right - windowRect.Left, windowRect.Bottom - windowRect.Top);
        }
    }
}
