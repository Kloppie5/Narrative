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

      manager.injector.ExtractTabletopSphere();

      g.DrawString("Situations", font, brush, x, y);
      y += dy;
      foreach ( var situation in manager.injector.situationsDict ) {
        g.DrawString(situation.Key, font, brush, x+10, y);
        y += dy;
      }

      g.DrawString("Element Stacks", font, brush, x, y);
      y += dy;
      foreach ( var elementStack in manager.injector.elementStacksDict ) {
        g.DrawString(elementStack.Key, font, brush, x+10, y);
        y += dy;
      }
    }
  }
}
