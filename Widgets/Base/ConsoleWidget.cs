using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Narrative {

  public static class ConsoleWidget {

    static Single x = 0;
    static Single y = 0;
    static List<Func<String>> lines = new List<Func<String>>();

    public static void PermanentLine ( Func<String> line ) {
      lines.Add(line);
    }
    public static void TemporaryLine ( Func<String> line, Int32 duration = 1000 ) {
      lines.Add(line);
      Timer timer = new Timer { Interval = duration };
      timer.Tick += ( sender, e ) => {
        lines.Remove(line);
        timer.Stop();
      };
      timer.Start();
    }

    public static void Render ( PaintEventArgs e ) {
      Single lineY = y;

      Graphics g = e.Graphics;
      e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
      e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
      e.Graphics.CompositingMode = CompositingMode.SourceCopy;
      g.CompositingQuality = CompositingQuality.HighQuality;
      foreach ( var line in lines ) {
        GraphicsPath p = new GraphicsPath();
        p.AddString(
            line.Invoke(),
            FontFamily.GenericMonospace,  // or any other font family
            (int) FontStyle.Regular,      // font style (bold, italic, etc.)
            g.DpiY * 14 / 72f,
            new Point((int)x, (int)lineY),
            new StringFormat());
        g.DrawPath(new Pen(Color.White, 5), p);
        g.FillPath(Brushes.Black, p);
        lineY += 26;
      }
    }
  }
}
