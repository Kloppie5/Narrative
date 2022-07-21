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

    public override void Paint ( PaintEventArgs e ) {
      if (!manager.CheckConnected())
        return;

      manager.Dump();
    }
  }
}
