using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Narrative {

    public class TextWidget : Widget {

        Single x;
        Single y;
        Single emSize;
        Single lineHeight;
        Single lineSpacing;
        Int32 maxLines;
        List<Func<String>> lines;
        public TextWidget (
            Overlay overlay,
            Single x = 0, Single y = 0,
            Single emSize = 12, Single lineHeight = 12, Single lineSpacing = 2,
            Int32 maxLines = 10
        ) : base(overlay) {
            lines = new List<Func<String>>();
            this.x = x;
            this.y = y;
            this.emSize = emSize;
            this.lineHeight = lineHeight;
            this.lineSpacing = lineSpacing;
            this.maxLines = maxLines;
        }

        public Int32 AddLine ( Func<String> line ) {
            lines.Add(line);
            if ( lines.Count > maxLines )
                lines.RemoveAt(0);
            return lines.Count - 1;
        }
        public void SetLine ( Int32 index, Func<String> line ) {
            if ( index < 0 || index >= lines.Count )
                return;
            lines[index] = line;
        }
        public void ClearLine ( Int32 index ) {
            if ( index < 0 || index >= lines.Count )
                return;
            lines[index] = () => "";
        }

        public override void Paint ( PaintEventArgs e ) {
            Single lineY = y;
            foreach ( var line in lines ) {
                e.Graphics.DrawString(line.Invoke(), new Font("Consolas", emSize), new SolidBrush(Color.White), x, lineY);
                lineY += lineHeight + lineSpacing;
            }
        }
    }
}
