using System.Drawing;
using System.Windows.Forms;

namespace Narrative {

    public class Widget {

        Overlay overlay;

        public Widget ( ) { }

        public void Bind ( Overlay overlay ) {
            this.overlay = overlay;
        }

        public virtual void Paint ( PaintEventArgs e ) {
            if ( overlay == null )
                return;
            
            e.Graphics.DrawRectangle(new Pen(Color.White, 2), 0, 0, overlay.Width - 1, overlay.Height - 1);
        }
    }
}
