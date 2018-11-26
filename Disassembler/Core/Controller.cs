using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Disassembler.Utils;

namespace Disassembler.Core
{
    public sealed class Controller
    {
        private static Controller instance;
        private Decoder decoder;

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

        public void Disassemble(int offset, int num_bytes)
        {
            bool disassemble = true;
            int instruction_pointer = 0;
            int buffer_lenght = 0;
            Instruction decoded_instruction = new Instruction();

            //check if there is a file open
            if (!FileManager.GetInstance().CheckIfFileOpen())
            {
                MessageBox.Show("Disassembly failed! File not opened! Please open a file first.", "Error");
                return;
            }
            byte[] machine_code = FileManager.GetInstance().GetMachineCode(offset, num_bytes);
            if (machine_code == null)
            {
                //disassemble = false;
                return;
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
                    //Debug.WriteLine(decoded_instruction.name + " " + decoded_instruction.operand1 + decoded_instruction.operand2);
                    instruction_pointer += decoded_instruction.length;
                }
                
            }
        }
    }
}
