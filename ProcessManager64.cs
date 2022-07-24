using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Narrative {
  public class ProcessManager64 {
        public Process process;

        public String ProcessName;
        public UInt64 BaseAddress => (UInt64) process.MainModule.BaseAddress;

        public ProcessManager64 ( String processName ) {
            ProcessName = processName;
            CheckConnected();
        }
        public Boolean CheckConnected ( ) {
            if ( process == null )
                return TryConnect();

            process.Refresh();
            if ( process.HasExited ) {
                process = null;
                return TryConnect();
            }
            return true;
        }
        public Boolean TryConnect ( ) {
            foreach ( var process in Process.GetProcesses() ) {
                if ( process.MainWindowTitle == ProcessName ) {
                    this.process = process;
                    return true;
                }
            }
            return false;
        }

        #region Imports
        [DllImport("user32.dll")]
        public static extern Boolean SetForegroundWindow( IntPtr hWnd );
        [DllImport("user32.dll")]
        public static extern Int32 SetWindowLong( IntPtr hWnd, Int32 nIndex, Int32 dwNewLong );
        [DllImport("user32.dll", SetLastError = true)]
        public static extern Int32 GetWindowLong( IntPtr hWnd, Int32 nIndex );
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);
     
        #endregion

    }
}
