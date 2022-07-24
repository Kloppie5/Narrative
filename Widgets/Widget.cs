using System;
using System.Drawing;
using System.Windows.Forms;

namespace Narrative {

  public abstract class Widget : IDisposable {

    Overlay overlay;
    Boolean enabled = false;

    public Widget ( ) { }

    public void Bind ( Overlay overlay ) {
      this.overlay = overlay;
    }

    public void Enable ( ) {
      if ( enabled )
        return;
      enabled = true;
      OnEnable();
    }
    public virtual void OnEnable ( ) { }
    public void Disable ( ) {
      if ( !enabled )
        return;
      enabled = false;
      OnDisable();
    }
    public virtual void OnDisable ( ) { }
        
    public virtual void Update ( ) { }
    public virtual void Render ( PaintEventArgs e ) { }

    private bool disposed = false;
    public void Dispose ( ) {
      Dispose(true);
      GC.SuppressFinalize(this);
    }
    protected virtual void Dispose ( bool disposing = false ) {
      if ( disposed )
        return;
            
      if ( disposing ) {
        Console.WriteLine($"Disposed {GetType().Name}");
      }

      disposed = true;
    }
    ~Widget() {
      Dispose();
    }
  }
}
