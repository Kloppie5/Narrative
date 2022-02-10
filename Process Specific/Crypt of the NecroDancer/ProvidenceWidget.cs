
using System.Windows.Forms;

using Narrative;

namespace CryptOfTheNecroDancer {

    public class ProvidenceWidget : Widget {

        ProcessManager manager;
        public ProvidenceWidget ( Overlay overlay, ProcessManager manager ) : base(overlay) {
            this.manager = manager;
        }

        public override void Paint ( PaintEventArgs e ) {
            if (!manager.CheckConnected())
                return;


        }
    }
}
