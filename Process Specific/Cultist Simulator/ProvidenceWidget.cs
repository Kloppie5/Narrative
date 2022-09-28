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

      TestingCheat_GetAllTheStats();
    }

    public override void Render ( PaintEventArgs e ) {
      base.Render(e);

      if ( !manager.CheckConnected() )
        return;

      var g = e.Graphics;
      var font = new Font("Consolas", 18);
      var brush = new SolidBrush(Color.White);

      var x = 30;
      var y = 30;
      var dy = 24;

      // HornedAxe
      //   Hour
      // Config
      // Concursum
      // MetaInfo
      // StageHand
      //   Scene changes
      // Limbo
      // Notifier
      // Dice
      //   RNG
      // StorfrontServicesProvider
      // AchievementsChronicler
      // ModManager
      // Compendium
      //   Dictionary<Type, EntityStore> entityStores;
      //     Recipe, Element, Verb, Legacy
      // LanguageManager
      // PrefabFactory
      // Stable
      //   Player
      // ScreenResolutionAdapter
      // HintPanel
      // SecretHistory
      //   Debug output
      // UIController
      // BackgroundMusic
      // CamOperator
      //   Camera movements
      // TabletopBackground
      // DebugTools
      // Heart
      // Meniscate
      //   Dragging tokens
      // TabletopImageBurner
      // CSChronicler
      // Xamanek
      // StatusBar
      // TabletopFadeOverlay
      // Autosaver
      // DealersTable
      // GameGateway
      // Numa

      manager.ExtractTabletopSphere();

      g.DrawString("Tokens", font, brush, x, y);
      y += dy;

      foreach ( var token in manager.tokens ) {
        g.DrawString(manager.TokenToPrettyString(token), font, brush, x, y);
        y += dy;
      }

      foreach ( var token in manager.tokens ) {
        Int32 payload = MemoryHelper.ReadAbsolute<Int32>(manager, token + 0x28);
        Int32 payloadVTable = MemoryHelper.ReadAbsolute<Int32>(manager, payload);
        Int32 payloadClass = MemoryHelper.ReadAbsolute<Int32>(manager, payloadVTable);
        String payloadClassName = MemoryHelper.ReadAbsoluteUTF8String(manager, MemoryHelper.ReadAbsolute<Int32>(manager, payloadClass + 0x2C));

        if ( payloadClassName == "Situation" ) {
          manager.Open(payload);
        }
      }
    }

    public void TestingCheat_GetAllTheStats ( ) {
      foreach ( var token in manager.tokens ) {
        Int32 payload = MemoryHelper.ReadAbsolute<Int32>(manager, token + 0x28);
        Int32 payloadVTable = MemoryHelper.ReadAbsolute<Int32>(manager, payload);
        Int32 payloadClass = MemoryHelper.ReadAbsolute<Int32>(manager, payloadVTable);
        String payloadClassName = MemoryHelper.ReadAbsoluteUTF8String(manager, MemoryHelper.ReadAbsolute<Int32>(manager, payloadClass + 0x2C));

        if ( payloadClassName == "ElementStack" ) {
          Int32 element = MemoryHelper.ReadAbsolute<Int32>(manager, payload + 0x14);
          Int32 label = MemoryHelper.ReadAbsolute<Int32>(manager, element + 0x1C);
          String labelString = MemoryHelper.ReadAbsoluteMonoWideString(manager, label);
          Int32 quantity = MemoryHelper.ReadAbsolute<Int32>(manager, payload + 0x40);

          if ( labelString == "Funds" && quantity < 50 ) {
            Console.WriteLine($"Setting Funds to 999999");
            MemoryHelper.WriteAbsolute<Int32>(manager, payload + 0x40, 999999);
          }
          if ( (labelString == "Health" || labelString == "Reason" || labelString == "Passion") && quantity < 50 ) {
            Console.WriteLine($"Setting {labelString} to 99");
            MemoryHelper.WriteAbsolute<Int32>(manager, payload + 0x40, 99);
          }
        }
      }
    }
  }
}
