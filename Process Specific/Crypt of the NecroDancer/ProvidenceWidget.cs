using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using Narrative;

namespace CryptOfTheNecroDancer {

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
            Int32 centerX = (windowRect.Left + windowRect.Right) / 2;
            Int32 centerY = (windowRect.Top + windowRect.Bottom) / 2;

            Single cameraX = manager.Get<Single>("camera_x");
            Single cameraY = manager.Get<Single>("camera_y");
            Single currentScaleFactor = manager.Get<Single>("currentScaleFactor");

            Dictionary<UInt32, RenderableObject> renderableObjects = new Dictionary<UInt32, RenderableObject>();
            manager.ForEachRenderableObject(renderableObject => {
                renderableObjects.Add(renderableObject.address, renderableObject);
                e.Graphics.DrawRectangle(new Pen(Color.Red, 2),
                    centerX + (renderableObject.x * 24 - cameraX - 12) * currentScaleFactor,
                    centerY + (renderableObject.y * 24 - cameraY) * currentScaleFactor,
                    24 * currentScaleFactor,
                    24 * currentScaleFactor
                );
            });

        }
    }
}
