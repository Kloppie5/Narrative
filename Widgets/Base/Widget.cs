using System;
using System.Drawing;
using System.Windows.Forms;

namespace Narrative {

  public abstract class Widget : IDisposable {

    public Widget ( ) { }

    public Boolean enabled { get; protected set; } = false;
    public virtual Boolean EnableCondition ( ) {
      return true;
    }
    public Boolean TryEnable ( ) {
      if ( !EnableCondition() )
        return false;
      Enable();
      return true;
    }
    public void Enable ( ) {
      if ( enabled )
        return;
      enabled = true;
      OnEnable();
    }
    public virtual void OnEnable ( ) { }

    public virtual Boolean DisableCondition ( ) {
      return false;
    }
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
