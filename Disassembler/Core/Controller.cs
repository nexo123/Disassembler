using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Disassembler.Utils;

namespace Disassembler.Core
{
    public sealed class Controller
    {
        private static Controller instance;
        private Decoder decoder;
        private Dictionary<int, string> labels = new Dictionary<int, string>();

        private Controller()
        {
            decoder = new Decoder();
        }

        public static Controller GetInstance()
        {
            if (instance == null)
            {
                instance = new Controller();
            }
            return instance;
        }

        /// <summary>
        /// When a file is opened, initialize the controller so it is ready to disassemble.
        /// </summary>
        /// <param name="file_path">Path to the file the user is trying to disassemble.</param>
        /// <returns></returns>
        public string Init(string file_path)
        {
            FileManager.GetInstance().Open(file_path);
            return FileManager.GetInstance().FileToHex();
        }

        private void SetInstructionPointer(ref int instruction_pointer)
        {
            instruction_pointer = FileManager.GetInstance().GetHeaderSize();
        }


        /// <summary>
        /// Main loop, handles disassembling.
        /// </summary>
        /// <param name="offset">Offset from start.</param>
        /// <param name="num_bytes">How many bytes to disassemble.</param>
        public List<string> Disassemble(int offset, int num_bytes)
        {
            bool disassemble = true;
            bool potential_end = false;
            int instruction_pointer = 0;
            int buffer_lenght = 0;
            Instruction decoded_instruction = new Instruction();
            List<Instruction> decoded_instructions_list = new List<Instruction>();

            labels.Clear();

            //check if there is a file open
            if (!FileManager.GetInstance().CheckIfFileOpen())
            {
                MessageBox.Show("Disassembly failed! File not opened! Please open a file first.", "Error");
                return null;
            }
            byte[] machine_code = FileManager.GetInstance().GetMachineCode(offset, num_bytes);
            if (machine_code == null)
            {
                //disassemble = false;
                return null;
            }
            else
            {
                buffer_lenght = machine_code.Length;
                decoder.machine_code = machine_code;
            }

            //main disassembling loop
            while (disassemble && instruction_pointer < buffer_lenght)
            {
                decoded_instruction = decoder.DecodeInstructionAt(instruction_pointer);
                //test if decoding was successfull, (decoded instruction lenght > 0)
                if (decoded_instruction.length < 1)
                {
                    //handle errors
                    MessageBox.Show("Disassembly failed!" + Environment.NewLine + "Decoder returned: " + decoded_instruction.name, "Error");
                    disassemble = false;
                }
                else
                {
                    CheckCSEnd(potential_end, decoded_instruction, ref disassemble);
                    potential_end = TestPotentialEnd(decoded_instruction);
                    decoded_instruction.address = instruction_pointer;
                    decoded_instructions_list.Add(decoded_instruction);
                    instruction_pointer += decoded_instruction.length;
                    TestForLabel(ref decoded_instruction, instruction_pointer);
                }
                
            }
            return BuildAssemblyCode(decoded_instructions_list);
        }


        private bool TestPotentialEnd(Instruction decoded_instruction)
        {
            if (decoded_instruction.name.Equals("MOV"))
            {
                if (decoded_instruction.operand1.Equals("AH") && decoded_instruction.operand2.Equals(", 4CH"))
                {
                    return true;
                }
                else if (decoded_instruction.operand1.Equals("AX") && decoded_instruction.operand2.Substring(0, 5).Equals(", 4CH"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        private void CheckCSEnd(bool potential_end, Instruction decoded_instruction, ref bool disassemble)
        {
            if (potential_end)
            {
                if (decoded_instruction.name.Equals("INT") && decoded_instruction.operand1.Equals("21H"))
                {
                    disassemble = false;
                }
            }
        }


        private void UpdateLabelAt(int address)
        {
            if (!labels.ContainsKey(address))
            {
                labels.Add(address, "Label" + labels.Count + ":");
            }
        }


        private void TestForLabel(ref Instruction instruction, int next_ip)
        {
            if (instruction.name.Contains("J") | instruction.name.Contains("LOOP"))
            {
                int jump_address = next_ip + Convert.ToInt32(instruction.operand1.Replace("H", string.Empty), 16);
                UpdateLabelAt(jump_address);
                if (labels.TryGetValue(jump_address, out string label))
                {
                    instruction.operand1 = label.Replace(":", "");
                }
            }
        }


        private List<string> BuildAssemblyCode(List<Instruction> instructions)
        {
            List<string> assembly_code = new List<string>();
            for (int i = 0; i < instructions.Count; i++)
            {
                string assembly_line = "";
                if (labels.TryGetValue(instructions[i].address, out string label))
                {
                    assembly_line += label;
                }
                assembly_line += "\t" + instructions[i].name + "\t" + instructions[i].operand1 + instructions[i].operand2 + Environment.NewLine;
                assembly_code.Add(assembly_line);
            }
            return assembly_code;
        }

    }
}
