using System.Diagnostics;

using System;

namespace Narrative {
    public class ProcessLinkedWidget : Widget {

    Process process;
    String processName;

    public ProcessLinkedWidget ( String processName ) : base ( ) {
        ConsoleWidget.TemporaryLine(() => $"Initialized ProcessLinkedWidget for {processName}");
        this.processName = processName;
    }

    public override Boolean EnableCondition ( ) {
      foreach ( Process process in Process.GetProcesses() ) {
        if ( process.ProcessName == processName )
            return true;
      }
      return false;
    }
    public override void OnEnable ( ) {
      foreach ( Process process in Process.GetProcesses() ) {
        if ( process.ProcessName == processName ) {
          this.process = process;
          break;
        }
      }
      ConsoleWidget.TemporaryLine(() => $"ProcessLinkedWidget for {processName} Connected");
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
      ConsoleWidget.TemporaryLine(() => $"ProcessLinkedWidget for {processName} Disconnected");
      process = null;
    }
  }
}
