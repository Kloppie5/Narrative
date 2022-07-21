using System;
using System.Runtime.InteropServices;

namespace Narrative {
  [StructLayout(LayoutKind.Sequential)]
  public struct IMAGE_DATA_DIRECTORY {
    public UInt32 VirtualAddress;
    public UInt32 Size;
  }
}
