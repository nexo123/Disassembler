using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Disassembler.Core
{
    class Decoder
    {
        private Dictionary<string, Instruction> instruction_set;

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
            { "1001", "SP" },
            { "1011", "BP" },
            { "1101", "SI" },
            { "1111", "DI" }
        };

        private Dictionary<string, string> segment_registers = new Dictionary<string, string>()
        {
            { "000", "ES" },
            { "001", "CS" },
            { "010", "SS" },
            { "110", "DS" }
        };

        private Dictionary<string, string> GRP1 = new Dictionary<string, string>()
        {
            { "000", "ADD" },
            { "001", "OR" },
            { "010", "ADC" },
            { "011", "SBB" },
            { "100", "AND" },
            { "101", "SUB" },
            { "110", "XOR" },
            { "111", "CMP" }
        };

        private Dictionary<string, string> GRP2 = new Dictionary<string, string>()
        {
            { "000", "ROL" },
            { "001", "ROR" },
            { "010", "RCL" },
            { "011", "RCR" },
            { "100", "SHL" },
            { "101", "SHR" },
            { "111", "SAR" }
        };

        private Dictionary<string, string> GRP3A = new Dictionary<string, string>()
        {
            { "000", "TEST" },
            { "010", "NOT" },
            { "011", "NEG" },
            { "100", "MUL" },
            { "101", "IMUL" },
            { "110", "DIV" },
            { "111", "IDIV" }
        };

        private Dictionary<string, string> GRP3B = new Dictionary<string, string>()
        {
            { "000", "TEST" },
            { "010", "NOT" },
            { "011", "NEG" },
            { "100", "MUL" },
            { "101", "IMUL" },
            { "110", "DIV" },
            { "111", "IDIV" }
        };

        private Dictionary<string, string> GRP4 = new Dictionary<string, string>()
        {
            { "000", "INC" },
            { "001", "DEC" }
        };

        private Dictionary<string, string> GRP5 = new Dictionary<string, string>()
        {
            { "000", "INC" },
            { "001", "DEC" },
            { "010", "CALL" },
            { "011", "CALL" },
            { "100", "JMP" },
            { "101", "JMP" },
            { "110", "PUSH" }
        };

        private byte[] machine_code;

        public Decoder()
        {
            InitInstructionSet();
        }

        /// <summary>
        /// Simple method to parse input json file with 8086 instruction set and initialize the instruction set for the decoder.
        /// </summary>
        private void InitInstructionSet()
        {
            instruction_set = new Dictionary<string, Instruction>();
            string json = File.ReadAllText("instruction_set.json");
            var tmp = JsonConvert.DeserializeObject<List<Instruction>>(json);
            foreach (Instruction instruction in tmp)
            {
                instruction_set.Add(instruction.opcode, instruction);   
            }
        }
    }

    /// <summary>
    /// Structure defined as class, holds instruction information
    /// </summary>
    public class Instruction
    {
        public string opcode { get; set; }
        public string name { get; set; }
        public int length { get; set; }
        public string operand1 { get; set; }
        public string operand2 { get; set; }
    }
}
