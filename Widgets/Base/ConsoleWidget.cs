using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Narrative {

  public static class ConsoleWidget {

    static Single x = 0;
    static Single y = 0;
    static Single emSize = 12;
    static Single lineHeight = 12;
    static Single lineSpacing = 10;
    static List<Func<String>> lines = new List<Func<String>>();

    public static void PermanentLine ( Func<String> line ) {
      lines.Add(line);
    }
    public static void TemporaryLine ( Func<String> line, Int32 duration = 1000 ) {
      lines.Add(line);
      Timer timer = new Timer();
      timer.Interval = duration;
      timer.Tick += ( sender, e ) => {
        lines.Remove(line);
        timer.Stop();
      };
      timer.Start();
    }

    public static void Render ( PaintEventArgs e ) {
      Single lineY = y;
      foreach ( var line in lines ) {
        e.Graphics.DrawString(line.Invoke(), new Font("Consolas", emSize), new SolidBrush(Color.White), x, lineY);
        lineY += lineHeight + lineSpacing;
      }
    }
  }
}
