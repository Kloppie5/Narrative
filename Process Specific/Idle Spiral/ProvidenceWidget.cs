using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using Narrative;

namespace IdleSpiral {

    public class ProvidenceWidget : Widget {

        ProcessManager64 manager;
        public ProvidenceWidget ( Overlay overlay, ProcessManager64 manager ) : base(overlay) {
            this.manager = manager;
        }

        public override void Paint ( PaintEventArgs e ) {
            if (!manager.CheckConnected())
                return;

            IntPtr ptr = manager.process.MainWindowHandle;
            ProcessManager64.Rect windowRect = new ProcessManager64.Rect();
            ProcessManager64.GetWindowRect(ptr, ref windowRect);
            e.Graphics.DrawRectangle(new Pen(Color.White, 2), windowRect.Left, windowRect.Top, windowRect.Right - windowRect.Left, windowRect.Bottom - windowRect.Top);

            UInt64 unityRootDomain = manager.GetUnityRootDomain();
            // UInt64 assembly = manager.GetAssemblyInDomain(unityRootDomain, "Assembly-CSharp");
            UInt64 IdleSpiralDomainAssembly = manager.GetAssemblyInDomain(unityRootDomain, "IdleSpiralDomain");
            Console.WriteLine($"Assembly: {IdleSpiralDomainAssembly:X}");
            UInt64 IdleSpiralDomainImage = manager.ReadAbsolute<UInt64>(IdleSpiralDomainAssembly + 0x60);
            Console.WriteLine($"Image: {IdleSpiralDomainImage:X}");
            manager.EnumImageClassCache(IdleSpiralDomainImage);
        }
    }
}
