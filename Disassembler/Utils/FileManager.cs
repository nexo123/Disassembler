using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace Disassembler.Utils
{
    public sealed class FileManager
    {
        private IOStream iostream;
        private static FileManager instance;
        private FileManager()
        {
            iostream = new IOStream();
        }

        public static FileManager GetInstance()
        {
            if (instance == null)
            {
                instance = new FileManager();
            }
            return instance;
        }

        public bool Open(string path)
        {
            if (CheckIfFileExists(path))
            {
                if (iostream.OpenFile(path))
                {
                    return true;
                }
            }
            else
            {
                MessageBox.Show("Opening the file failed! Check if file exists", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }

        public bool CheckIfFileExists(string path)
        {
            if (File.Exists(path))
            {
                return true;
            }
            return false;
        }

        public bool CheckIfFileOpen()
        {
            return iostream.IsFileOpen();
        }

        private bool CheckIfEXE()
        {
            var tmp = iostream.ReadFromFile(0, 2);
            if (tmp != null && tmp.Length == 2)
            {
                //Debug.WriteLine(ASCIIEncoding.UTF8.GetString(tmp));
                if (ASCIIEncoding.UTF8.GetString(tmp).Equals("MZ"))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Method that gets the header length if it is an .EXE file.
        /// </summary>
        /// <returns>Integer with .EXE header lenght in bytes or -1 if getting the header fails.</returns>
        public int GetHeaderSize()
        {
            if (CheckIfEXE())
            {
                var tmp = iostream.ReadFromFile(8, 2);
                if (tmp != null && tmp.Length == 2)
                {
                    ushort paragraphs = BitConverter.ToUInt16(tmp, 0);
                    return paragraphs * 16;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        }

        public string FileToHex()
        {
            var tmp = iostream.ReadFromFile(0, 0);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < tmp.Length; i++)
            {
                if (i % 16 == 15)
                {
                    sb.Append(Convert.ToString(tmp[i], 16).ToUpper().PadLeft(2,'0') + Environment.NewLine);
                }
                else
                {
                    sb.Append(Convert.ToString(tmp[i], 16).ToUpper().PadLeft(2, '0') + " ");
                }
            }
            return sb.ToString();
        }

        public bool WriteFile(string path, string data)
        {
            return iostream.Write(path, data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="num_bytes"></param>
        /// <returns></returns>
        public byte[] GetMachineCode(int offset, int num_bytes)
        {
            byte[] machine_code = null;
            if (offset < 0)
            {
                int header_lengt = GetHeaderSize();
                if (header_lengt < 0)
                {
                    MessageBox.Show("Determining the header length failed! Possibly not an .EXE file. Consider setting the code segment manually.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                try
                {
                    machine_code = iostream.ReadFromFile(header_lengt, num_bytes);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Reading machine code failed! Offset: " + offset + ", num_bytes: " + num_bytes + Environment.NewLine + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                try
                {
                    machine_code = iostream.ReadFromFile(offset, num_bytes);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Reading machine code failed! Offset: " + offset + ", num_bytes: " + num_bytes + Environment.NewLine + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return machine_code;
        }
            
    }
}
