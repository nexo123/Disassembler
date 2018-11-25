using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Disassembler.Core
{
    class Decoder
    {
        private Dictionary<string, string> registers = new Dictionary<string, string>()
        {
            { "0000", "AL" },
            { "0010", "CL" },
            { "0100", "DL" },
            { "0110", "BL" },
            { "1000", "AH" },
            { "1010", "CH" },
            { "1100", "DH" },
            { "1110", "BH" },
            { "0001", "AX" },
            { "0011", "CX" },
            { "0101", "DX" },
            { "0111", "BX" },
            { "1000", "SP" },
            { "1010", "BP" },
            { "1100", "SI" },
            { "1110", "DI" }
        };

        private Dictionary<string, string> segment_registers = new Dictionary<string, string>()
        {
            { "000", "ES" },
            { "001", "CS" },
            { "010", "SS" },
            { "110", "DS" }
        };



        private byte[] machine_code;

        public Decoder()
        {

        }


    }
}
