using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using Narrative;

namespace CryptOfTheNecroDancer {

  public class ProvidenceWidget : Widget {

    ProcessManager manager;
    public ProvidenceWidget ( ) {
      manager = new ProcessManager();
    }

    public override void OnEnable() {
      base.OnEnable();

      if ( !manager.CheckConnected() )
        return;

      manager.Dump();
    }
  }
}
