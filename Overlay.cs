using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Narrative {
    public partial class Overlay : Form {
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow( IntPtr hWnd );
        [DllImport("user32.dll")]
        public static extern int SetWindowLong( IntPtr hWnd, int nIndex, int dwNewLong );
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong( IntPtr hWnd, int nIndex );

        public Overlay ( ) {
            Rectangle rect = Screen.PrimaryScreen.Bounds;
            TransparencyKey = Color.Turquoise;
            BackColor = Color.Turquoise;
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            DoubleBuffered = true;
            Location = new Point(rect.Left, rect.Top);
            Size = rect.Size;
            TopMost = true;
            SetForegroundWindow(Handle);

            ConsoleWidget.PermanentLine(() => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            ConsoleWidget.TemporaryLine(() => $"Initialized Overlay", 2000);
        }

        Dictionary<String, Widget> widgets = new Dictionary<String, Widget>() {
            { "IncrementalAdventures", new IncrementalAdventuresWidget() }
        };

        protected void OnTick ( ) {
            foreach ( var (widgetName, widget) in widgets ) {
                if ( !widget.enabled )
                    widget.TryEnable();
                else
                    widget.Update();
            }
            Invalidate();
        }
        protected override void OnPaint( PaintEventArgs e ) {
            base.OnPaint(e);

            foreach ( var (widgetName, widget) in widgets )
                widget.Render(e);
            ConsoleWidget.Render(e);
        }

        protected override void OnLoad ( EventArgs e ) {
            base.OnLoad(e);
            var style = GetWindowLong(Handle, -20); // GWL_EXSTYLE
            SetWindowLong(Handle, -20, style | 0x80020); // WS_EX_LAYERED, WS_EX_TRANSPARENT

            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += ( sender, e ) => OnTick();
            timer.Start();
        }
    }
}
