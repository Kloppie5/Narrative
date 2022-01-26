using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using MonsterHunterWorld;

namespace Narrative {
    public partial class Overlay : Form {
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow( IntPtr hWnd );
        [DllImport("user32.dll")]
        public static extern int SetWindowLong( IntPtr hWnd, int nIndex, int dwNewLong );
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong( IntPtr hWnd, int nIndex );

        MonsterHunterWorldMemoryManager manager;

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

            manager = new MonsterHunterWorldMemoryManager();

            Console.WriteLine($"Initialized Overlay");
        }

        protected override void OnPaint( PaintEventArgs e ) {
            base.OnPaint(e);

            UInt32 totalDamage = 0;
            for (Int64 i = 0; i < 4; i++)
                totalDamage += manager.ReadRelative<UInt32>(0x5224BF8, 0x258, 0x38, 0x450, 0x8, 0x48 + i * 0x2A0);

            for (Int64 i = 0; i < 4; i++) {
                String PartyMemberName = manager.ReadRelativeUTF8String(0x5224BF8, 0x68, -0x22B7 + i * 0x1C0);
                UInt16 HR              = manager.ReadRelative<UInt16>(0x5224BF8, 0x68, -0x22B7 + i * 0x1C0 + 0x27);
                UInt16 MR              = manager.ReadRelative<UInt16>(0x5224BF8, 0x68, -0x22B7 + i * 0x1C0 + 0x29);
                UInt32 playerDamage    = manager.ReadRelative<UInt32>(0x5224BF8, 0x258, 0x38, 0x450, 0x8, 0x48 + i * 0x2A0);

                if ( playerDamage > 0 && totalDamage > 0 )
                    e.Graphics.DrawString(
                        $"{PartyMemberName} ({MR} | {HR}): {playerDamage} ({playerDamage * 100 / totalDamage}%)",
                        new Font("Consolas", 12),
                        new SolidBrush(Color.White),
                        400f, 400f + i * 20f,
                        new StringFormat() { }
                    );
            }

            for (Int64 i = 0; i < 3; i++) {
                UInt64 address = manager.ReadRelative<UInt64>(0x5074180, 0xE58 + (Int64)i*0x50);
                if ( address == 0 )
                    continue;

                String emString = manager.ReadAbsoluteUTF8String(address + 0x2A0, 0x0C);
                Dictionary<String, String> emTranslate = new Dictionary<String, String>() {
                    { "em\\em001\\00\\mod\\em001_00", "Rathian" },
                    { "em\\em001\\01\\mod\\em001_01", "Pink Rathian" },
                    { "em\\em001\\02\\mod\\em001_02", "Gold Rathian" },
                    { "em\\em002\\00\\mod\\em002_00", "Rathalos" },
                    { "em\\em002\\01\\mod\\em002_01", "Azure Rathalos" },
                    { "em\\em002\\02\\mod\\em002_02", "Silver Rathalos" },
                    { "em\\em007\\00\\mod\\em007_00", "Diablos" },
                    { "em\\em007\\01\\mod\\em007_01", "Black Diablos" },
                    { "em\\em011\\00\\mod\\em011_00", "Kirin" },
                    { "em\\em013\\00\\mod\\em013_00", "Fatalis" },
                    { "em\\em018\\00\\mod\\em018_00", "Yian Garuga" },
                    { "em\\em018\\05\\mod\\em018_05", "Scarred Yian Garuga" },
                    { "em\\em023\\00\\mod\\em023_00", "Rajang" },
                    { "em\\em023\\05\\mod\\em023_05", "Furious Rajang" },
                    { "em\\em024\\00\\mod\\em024_00", "Kushala Daora" },
                    { "em\\em026\\00\\mod\\em026_00", "Lunastra" },
                    { "em\\em027\\00\\mod\\em027_00", "Teostra" },
                    { "em\\em032\\00\\mod\\em032_00", "Tigrex" },
                    { "em\\em032\\01\\mod\\em032_01", "Brute Tigrex" },
                    { "em\\em036\\00\\mod\\em036_00", "Lavasioth" },
                    { "em\\em037\\00\\mod\\em037_00", "Nargacuga" },
                    { "em\\em042\\00\\mod\\em042_00", "Barioth" },
                    { "em\\em042\\05\\mod\\em042_05", "Frostfang Barioth" },
                    { "em\\em043\\00\\mod\\em043_00", "Deviljho" },
                    { "em\\em043\\05\\mod\\em043_05", "Savage Deviljho" },
                    { "em\\em044\\00\\mod\\em044_00", "Barroth" },
                    { "em\\em045\\00\\mod\\em045_00", "Uragaan" },
                    { "em\\em050\\00\\mod\\em050_00", "Alatreon" },
                    { "em\\em057\\00\\mod\\em057_00", "Zinogre" },
                    { "em\\em057\\01\\mod\\em057_01", "Stygian Zinogre" },
                    { "em\\em063\\00\\mod\\em063_00", "Brachydios" },
                    { "em\\em063\\05\\mod\\em063_05", "Raging Brachydios" },
                    { "em\\em080\\00\\mod\\em080_00", "Glavenus" },
                    { "em\\em080\\01\\mod\\em080_01", "Acidic Glavenus" },
                    { "em\\em100\\00\\mod\\em100_00", "Anjanath" },
                    { "em\\em100\\01\\mod\\em100_01", "Fulgar Anjanath" },
                    { "em\\em101\\00\\mod\\em101_00", "Great Jagras" },
                    { "em\\em102\\00\\mod\\em102_00", "Pukei-Pukei" },
                    { "em\\em102\\01\\mod\\em102_01", "Coral Pukei-Pukei" },
                    { "em\\em103\\00\\mod\\em103_00", "Nergigante" },
                    { "em\\em103\\05\\mod\\em103_05", "Ruiner Nergigante" },
                    { "em\\em104\\00\\mod\\em104_00", "Safi'jiiva" },
                    { "em\\em105\\00\\mod\\em105_00", "Xeno'jiiva" },
                    { "em\\em106\\00\\mod\\em106_00", "Zorah Magdaros" },
                    { "em\\em107\\00\\mod\\em107_00", "Kulu-Ya-Ku" },
                    { "em\\em108\\00\\mod\\em108_00", "Jyuratodus" },
                    { "em\\em109\\00\\mod\\em109_00", "Tobi-Kadachi" },
                    { "em\\em109\\01\\mod\\em109_01", "Viper Tobi-Kadachi" },
                    { "em\\em110\\00\\mod\\em110_00", "Paolumu" },
                    { "em\\em110\\01\\mod\\em110_01", "Nightshade Paolumu" },
                    { "em\\em111\\00\\mod\\em111_00", "Legiana" },
                    { "em\\em111\\05\\mod\\em111_05", "Shrieking Legiana" },
                    { "em\\em112\\00\\mod\\em112_00", "Great Girros" },
                    { "em\\em113\\00\\mod\\em113_00", "Odogaron" },
                    { "em\\em113\\01\\mod\\em113_01", "Ebony Odogaron" },
                    { "em\\em114\\00\\mod\\em114_00", "Radobaan" },
                    { "em\\em115\\00\\mod\\em115_00", "Vaal Hazak" },
                    { "em\\em115\\05\\mod\\em115_05", "Blackveil Vaal Hazak" },
                    { "em\\em116\\00\\mod\\em116_00", "Dodogama" },
                    { "em\\em117\\00\\mod\\em117_00", "Kulve Taroth" },
                    { "em\\em118\\00\\mod\\em118_00", "Bazelgeuse" },
                    { "em\\em118\\05\\mod\\em118_05", "Seething Bazelgeuse" },
                    { "em\\em120\\00\\mod\\em120_00", "TziTzi-Ya-Ku" },
                    { "em\\em121\\00\\mod\\em121_00", "Behemoth" },
                    { "em\\em122\\00\\mod\\em122_00", "Beotodus" },
                    { "em\\em123\\00\\mod\\em123_00", "Banbaro" },
                    { "em\\em124\\00\\mod\\em124_00", "Velkhana" },
                    { "em\\em125\\00\\mod\\em125_00", "Namielle" },
                    { "em\\em126\\00\\mod\\em126_00", "Shara Ishvalda" },
                    { "em\\em127\\00\\mod\\em127_00", "Leshen" },
                    { "em\\em127\\01\\mod\\em127_01", "Ancient Leshen" },
                };
                String name = emTranslate.ContainsKey(emString) ? emTranslate[emString] : emString;

                UInt64 HPComponent = manager.ReadAbsolute<UInt64>(address + 0x7670);
                    Single MAX_HP = manager.ReadAbsolute<Single>(HPComponent + 0x60);
                    Single HP = manager.ReadAbsolute<Single>(HPComponent + 0x64);

                Single size = manager.ReadAbsolute<Single>(address + 0x188);
                Single sizeModifier = manager.ReadAbsolute<Single>(address + 0x7730);

                if ( MAX_HP > 0 )
                    e.Graphics.DrawString(
                        $"{name}:\n" +
                        $"{HP}/{MAX_HP} ({HP * 100 / MAX_HP}%)\n" +
                        $"{sizeModifier}|{size}",
                        new Font("Consolas", 12),
                        new SolidBrush(Color.White),
                        700f + i * 200f, 100f,
                        new StringFormat() { } );
            }
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
