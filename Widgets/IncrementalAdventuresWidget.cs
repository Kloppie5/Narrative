using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Narrative {

  public class IncrementalAdventuresWidget : Widget {

    Process process;

    public IncrementalAdventuresWidget ( ) : base ( ) {
      ConsoleWidget.TemporaryLine(() => $"Incremental Adventures Widget Initiated");
    }

    public override Boolean EnableCondition ( ) {
      foreach ( Process process in Process.GetProcesses() ) {
        if ( process.ProcessName == "IncrementalAdventures"
          && process.MainWindowTitle == "Incremental Adventures" )
            return true;
      }
      return false;
    }
    public override void OnEnable ( ) {
      foreach ( Process process in Process.GetProcesses() ) {
        if ( process.ProcessName == "IncrementalAdventures"
        && process.MainWindowTitle == "Incremental Adventures" ) {
          this.process = process;
          break;
        }
      }
      ConsoleWidget.TemporaryLine(() => $"Incremental Adventures Widget Connected");
    }
    public override Boolean DisableCondition ( ) {
      if ( process == null )
        return false;
      process.Refresh();
      if ( process.HasExited )
        return true;
      return false;
    }
    public override void OnDisable ( ) {
      ConsoleWidget.TemporaryLine(() => $"Incremental Adventures Widget Disconnected");
      process = null;
    }
  }
}
