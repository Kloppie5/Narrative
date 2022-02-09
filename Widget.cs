using System.Drawing;
using System.Windows.Forms;

namespace Narrative {

    public class Widget {

        Overlay overlay;

        public Widget ( Overlay overlay ) {
            this.overlay = overlay;
            overlay.AddWidget(this);
        }

        public void Remove () {
            overlay.RemoveWidget(this);
        }

        public virtual void Paint ( PaintEventArgs e ) {
            e.Graphics.DrawRectangle(new Pen(Color.White, 2), 0, 0, overlay.Width - 1, overlay.Height - 1);
        }
    }
}
