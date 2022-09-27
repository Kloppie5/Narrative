using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using Narrative;

namespace CultistSimulator {
  public class ProvidenceWidget : Widget {

    ProcessManager manager;
    public ProvidenceWidget ( ) {
      manager = new ProcessManager();
    }

    public override void Render ( PaintEventArgs e ) {
      base.Render(e);

      if ( !manager.CheckConnected() )
        return;

      var g = e.Graphics;
      var font = new Font("Consolas", 18);
      var brush = new SolidBrush(Color.White);

      var x = 30;
      var y = 30;
      var dy = 24;

      g.DrawString($"Process: {manager.process.ProcessName}", font, brush, x, y);
      y += dy;

      foreach ( KeyValuePair<String, Int32> pair in manager.dict ) {
        g.DrawString($"{pair.Value:X08}: {pair.Key}", font, brush, x, y);
        y += dy;
      }
    }
  }
}
