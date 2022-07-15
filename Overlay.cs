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

        Dictionary<String, Widget> widgets = new Dictionary<String, Widget>();

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

        public void AddWidget( String widgetName, Type widgetType ) {
            ConstructorInfo constructorInfo = widgetType.GetConstructor(new Type[] { }); 
            if (constructorInfo == null) 
                throw new InvalidOperationException($"Tried to add widget of type {widgetType}, but could not find valid constructor.");
            Widget widget = (Widget) constructorInfo.Invoke(new Object[] { });
            widget.Bind(this);
            widgets.Add(widgetName, widget);
        }

        public Boolean HasWidget ( String widgetName ) {
            return widgets.ContainsKey(widgetName);
        }

        protected override void OnPaint( PaintEventArgs e ) {
            base.OnPaint(e);

            foreach ( var (widgetName, widget) in widgets )
                widget.Paint(e);
        }

        protected override void OnLoad ( EventArgs e ) {
            base.OnLoad(e);
            var style = GetWindowLong(Handle, -20); // GWL_EXSTYLE
            SetWindowLong(Handle, -20, style | 0x80020); // WS_EX_LAYERED, WS_EX_TRANSPARENT

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += new System.EventHandler((sender, ee) => {
                Invalidate();
            });
            timer.Start();
        }
    }
}
