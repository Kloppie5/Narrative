using System;
using System.Drawing;
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

        public Overlay() {
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

            Console.WriteLine($"Initialized Overlay");
        }

        protected override void OnPaint( PaintEventArgs e ) {
            base.OnPaint(e);

            e.Graphics.DrawRectangle(new Pen(Color.White, 2), 0, 0, Width - 1, Height - 1);
        }

        protected override void OnLoad ( EventArgs e ) {
            base.OnLoad(e);
            var style = GetWindowLong(Handle, -20); // GWL_EXSTYLE
            SetWindowLong(Handle, -20, style | 0x80020); // WS_EX_LAYERED, WS_EX_TRANSPARENT

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 5000;
            timer.Tick += new System.EventHandler((sender, ee) => {
                Invalidate();
            });
            timer.Start();
        }
    }
}
