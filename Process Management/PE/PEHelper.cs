using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Narrative {
  public static class PEHelper {
    public static Boolean debug = false;

    [DllImport("psapi.dll")]
    public static extern Boolean EnumProcessModulesEx ( IntPtr hProcess, [Out] IntPtr[] lphModule, Int32 cb, ref Int32 lpcbNeeded, Int32 dwFilterFlag );
    [DllImport("psapi.dll")]
    public static extern Int32 GetModuleFileNameEx( IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName, Int32 nSize );

    public static Dictionary<String, Int64> GetModules64 ( ProcessManager64 manager ) {
      Dictionary<String, Int64> modules = new Dictionary<String, Int64>();
      IntPtr[] hModules = new IntPtr[1024];
      Int32 needed = 0;
      EnumProcessModulesEx(manager.process.Handle, hModules, 1024, ref needed, 0x3);
      for ( Int32 i = 0; i < needed / IntPtr.Size; i++ ) {
        StringBuilder sb = new StringBuilder(1024);
        GetModuleFileNameEx(manager.process.Handle, hModules[i], sb, 1024);
        if ( modules.ContainsKey(sb.ToString()) )
          continue;
        modules.Add(sb.ToString(), (Int64) hModules[i].ToInt64());
      }

      return modules;
    }
    public static void DumpModules64 ( ProcessManager64 manager ) {
      Dictionary<String, Int64> modules = GetModules64(manager);
      foreach ( var (moduleName, moduleBase) in modules ) {
        Console.WriteLine($"{moduleName} @ {moduleBase:X}");
      }
    }
    public static Int64 GetModule ( ProcessManager64 manager, params String[] moduleNames ) {
      Dictionary<String, Int64> modules = GetModules64(manager);
      foreach ( KeyValuePair<String, Int64> module in modules )
        foreach ( String moduleName in moduleNames )
          if ( module.Key.Contains(moduleName) )
            return module.Value;
      throw new Exception($"Could not find module matching {String.Join(", ", moduleNames)}");
    }
    public static void DumpModule ( ProcessManager64 manager, Int64 moduleBase ) {
      ReadPEHeaders(manager, moduleBase, out IMAGE_DOS_HEADER dosHeader, out IMAGE_FILE_HEADER fileHeader, out IMAGE_OPTIONAL_HEADER32 optionalHeader32, out IMAGE_OPTIONAL_HEADER64 optionalHeader64, out IMAGE_SECTION_HEADER[] sectionHeaders);
      dosHeader.DumpToConsole();
      fileHeader.DumpToConsole();
      optionalHeader32.DumpToConsole();
      optionalHeader64.DumpToConsole();
      foreach ( IMAGE_SECTION_HEADER sectionHeader in sectionHeaders ) {
        sectionHeader.DumpToConsole();
      }
    }

    public static void ReadPEHeaders (
      ProcessManager64 manager,
      Int64 imageBase,
      out IMAGE_DOS_HEADER dosHeader,
      out IMAGE_FILE_HEADER fileHeader,
      out IMAGE_OPTIONAL_HEADER32 optionalHeader32,
      out IMAGE_OPTIONAL_HEADER64 optionalHeader64,
      out IMAGE_SECTION_HEADER[] imageSectionHeaders
    ) {
      dosHeader = MemoryHelper.ReadAbsolute<IMAGE_DOS_HEADER>( manager, imageBase );
      if ( debug )
        dosHeader.DumpToConsole();
      fileHeader = MemoryHelper.ReadAbsolute<IMAGE_FILE_HEADER>( manager, imageBase + dosHeader.e_lfanew );
      if ( debug )
        fileHeader.DumpToConsole();

      optionalHeader32 = MemoryHelper.ReadAbsolute<IMAGE_OPTIONAL_HEADER32>( manager, imageBase + dosHeader.e_lfanew + 0x18 );
      if ( debug )
        optionalHeader32.DumpToConsole();
      optionalHeader64 = MemoryHelper.ReadAbsolute<IMAGE_OPTIONAL_HEADER64>( manager, imageBase + dosHeader.e_lfanew + 0x18 );
      if ( debug )
        optionalHeader64.DumpToConsole();

      Int16 optionalHeaderMagic = MemoryHelper.ReadAbsolute<Int16>( manager, imageBase + dosHeader.e_lfanew + 0x18 );
      Int64 imageSectionHeadersAddress = 0;
      if ( optionalHeaderMagic == 0x10B ) {
        imageSectionHeadersAddress = imageBase + dosHeader.e_lfanew + 0xF8;
      }
      if ( optionalHeaderMagic == 0x20B ) {
        imageSectionHeadersAddress = imageBase + dosHeader.e_lfanew + 0x108;
      }

      imageSectionHeaders = new IMAGE_SECTION_HEADER[fileHeader.NumberOfSections];
      for ( Int32 i = 0; i < fileHeader.NumberOfSections; i++ ) {
        imageSectionHeaders[i] = MemoryHelper.ReadAbsolute<IMAGE_SECTION_HEADER>( manager, imageSectionHeadersAddress + ( 0x28 * i ) );
        if ( debug )
          imageSectionHeaders[i].DumpToConsole();
      }
    }

    public static Dictionary<String, Int64> GetExportedFunctions ( ProcessManager64 manager, Int64 imageBase ) {
      ReadPEHeaders(manager, imageBase, out IMAGE_DOS_HEADER dosHeader, out IMAGE_FILE_HEADER fileHeader, out IMAGE_OPTIONAL_HEADER32 optionalHeader32, out IMAGE_OPTIONAL_HEADER64 optionalHeader64, out IMAGE_SECTION_HEADER[] sectionHeaders);

      Dictionary<String, Int64> exportedFunctions = new Dictionary<String, Int64>();

      Int16 optionalHeaderMagic = MemoryHelper.ReadAbsolute<Int16>( manager, imageBase + dosHeader.e_lfanew + 0x18 );
      Int64 exportTableAddress = 0;
      if ( optionalHeaderMagic == 0x10B ) {
        exportTableAddress = imageBase + optionalHeader32.ExportTable.VirtualAddress;
      }
      if ( optionalHeaderMagic == 0x20B ) {
        exportTableAddress = imageBase + optionalHeader64.ExportTable.VirtualAddress;
      }
      IMAGE_EXPORT_DIRECTORY_TABLE ExportTable = MemoryHelper.ReadAbsolute<IMAGE_EXPORT_DIRECTORY_TABLE>(manager, exportTableAddress);
      for ( Int32 i = 0; i < ExportTable.AddressTableEntries; ++i ) {
        Int32 FunctionNameOffset = MemoryHelper.ReadAbsolute<Int32>(manager, imageBase + ExportTable.NamePointerRva + i * 4);
        String FunctionName = MemoryHelper.ReadAbsoluteUTF8String(manager, imageBase + FunctionNameOffset);
        Int32 FunctionOffset = MemoryHelper.ReadAbsolute<Int32>(manager, imageBase + ExportTable.ExportAddressTableRva + i * 4);
        exportedFunctions.Add(FunctionName, imageBase + FunctionOffset);
      }

      return exportedFunctions;
    }
    public static void DumpExportedFunctions ( ProcessManager64 manager, Int64 imageBase ) {
      Dictionary<String, Int64> exportedFunctions = GetExportedFunctions(manager, imageBase);
      foreach ( var (exportedFunctionName, exportedFunctionAddress) in exportedFunctions ) {
        Console.WriteLine($"{exportedFunctionName} = {exportedFunctionAddress:X}");
      }
    }
    public static Int64 FindExportedFunction ( ProcessManager64 manager, Int64 imageBase, String exportedFunctionName ) {
      Dictionary<String, Int64> exportedFunctions = GetExportedFunctions(manager, imageBase);
      if ( exportedFunctions.ContainsKey(exportedFunctionName) )
        return exportedFunctions[exportedFunctionName];
      throw new Exception($"Could not find exported function {exportedFunctionName}");
    }
  }
}
