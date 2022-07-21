using System;

namespace Narrative {
  public class AddressRange<T> {

    public String name;
    public T start;
    public T end;

    public AddressRange ( T start, T end = default(T) ) {
      this.name = "";
      this.start = start;
      this.end = end;
    }
    public AddressRange ( String name, T start, T end = default(T) ) {
      this.name = name;
      this.start = start;
      this.end = end;
    }
  }
}
