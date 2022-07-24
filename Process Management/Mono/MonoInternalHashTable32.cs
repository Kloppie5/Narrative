using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Narrative {
  [StructLayout(LayoutKind.Explicit)]
  public unsafe struct MonoInternalHashTable {
    /*
    GHashFunc hash_func;
	MonoInternalHashKeyExtractFunc key_extract;
	MonoInternalHashNextValueFunc next_value;
	gint size;
	gint num_entries;
	gpointer *table;
    */
    [FieldOffset(0x00)]
    public UInt32 /* GHashFunc */ hash_func;
    [FieldOffset(0x04)]
    public UInt32 /* MonoInternalHashKeyExtractFunc */ key_extract;
    [FieldOffset(0x08)]
    public UInt32 /* MonoInternalHashNextValueFunc */ next_value;
    [FieldOffset(0x0C)]
    public Int32 size;
    [FieldOffset(0x10)]
    public Int32 num_entries;
    [FieldOffset(0x14)]
    public UInt32 /* gpointer* */ table;

    public void DumpToConsole ( ProcessManager64 manager, String prefix = "" ) {
      Console.WriteLine($"{prefix}MonoInternalHashTable;");
      Console.WriteLine($"{prefix}  hash_func: {hash_func:X}");
      Console.WriteLine($"{prefix}  key_extract: {key_extract:X}");
      Console.WriteLine($"{prefix}  next_value: {next_value:X}");
      Console.WriteLine($"{prefix}  size: {size}");
      Console.WriteLine($"{prefix}  num_entries: {num_entries}");
      Console.WriteLine($"{prefix}  table: {table:X}");
    }
  }
}
