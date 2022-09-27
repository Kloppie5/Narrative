using System;
using System.Linq;
using System.Collections.Generic;

using UInt8 = System.Byte;

namespace Narrative {
    public class Assembler {

        public bool debug = true;
        public List<List<UInt8>> code = new List<List<UInt8>>();
        public UInt8[] finalize ( ) {
            UInt8[] result = code.SelectMany(x => x).ToArray();
            code.Clear();
            return result;
        }
        public void PrettyPrint ( ) {
            foreach (var line in code)
                Console.WriteLine($"  {BitConverter.ToString(line.ToArray()).Replace("-", " ")}");
        }

        public enum Register : UInt8 {
            EAX = 0,
            ECX = 1,
            EDX = 2,
            EBX = 3,
            ESP = 4,
            EBP = 5,
            ESI = 6,
            EDI = 7
        }

        public void MOVi64r ( Register r, Int64 imm ) {
            if ( debug ) Console.WriteLine($"MOVi64r {r} {imm:X}");
            List<UInt8> line = new List<UInt8>();
            line.Add(0x48); // REX.W
            line.Add((UInt8)(0xB8 + (UInt8)r)); // o270 + r
            line.AddRange(BitConverter.GetBytes(imm));
            code.Add(line);
        }
        public void MOVr0m64 ( Int64 address ) {
            if ( debug ) Console.WriteLine($"MOVr0m64 {address:X}");
            List<UInt8> line = new List<UInt8>();
            line.Add(0x48); // REX.W
            line.Add(0xA3); // o243 + r0
            line.AddRange(BitConverter.GetBytes(address));
            code.Add(line);
        }

        public void MOVi32r ( Register r, Int32 imm ) {
            if ( debug ) Console.WriteLine($"MOVi32r {r} {imm:X}");
            List<UInt8> line = new List<UInt8>();
            line.Add((UInt8)(0xB8 + (UInt8)r)); // o270 + r
            line.AddRange(BitConverter.GetBytes(imm));
            code.Add(line);
        }
        public void MOVr0m32 ( Int32 address ) {
            if ( debug ) Console.WriteLine($"MOVr0m32 {address:X}");
            List<UInt8> line = new List<UInt8>();
            line.Add(0xA3); // o243 + r0
            line.AddRange(BitConverter.GetBytes(address));
            code.Add(line);
        }

        public void RET ( ) {
            if ( debug ) Console.WriteLine($"RET");
            List<UInt8> line = new List<UInt8>();
            line.Add(0xC3); // o303
            code.Add(line);
        }

        public void CALLr ( Register r ) {
            if ( debug ) Console.WriteLine($"CALLr {r}");
            List<UInt8> line = new List<UInt8>();
            line.Add(0xFF); // o377
            line.Add((UInt8)(0xD0 + (UInt8)r)); // o320 + r
            code.Add(line);
        }
    }
}
