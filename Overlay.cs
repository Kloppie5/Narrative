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

        protected override CreateParams CreateParams {
            get {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x80020; // WS_EX_LAYERED, WS_EX_TRANSPARENT
                return cp;
            }
        }

        private SettingsForm settingsForm = new SettingsForm();

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

            settingsForm.Show();
            settingsForm.AddBooleanSetting("ShowConsole", "Show Console", true);
        }

        Dictionary<String, Widget> widgets = new Dictionary<String, Widget>() {
            { "MonsterHunterWorld", new ProcessLinkedWidget("MonsterHunterWorld") },
            { "Neurodancer", new ProcessLinkedWidget("Crypt of the NecroDancer") },
            { "Golemancy", new ProcessLinkedWidget("Cultist Simulator") },
            { "FactorAI", new ProcessLinkedWidget("factorio") }
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
            Timer timer = new Timer { Interval = 1000 };
            timer.Tick += ( sender, e ) => OnTick();
            timer.Start();
        }
    }
}
