﻿using System;
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

        public void Open(string path)
        {
            if (CheckIfFileExists(path))
            {
                iostream.OpenFile(path);
            }
            else
            {
                MessageBox.Show("Opening the file failed! Check if file exists", "Error!");
            }
        }

        public bool CheckIfFileExists(string path)
        {
            if (File.Exists(path))
            {
                return true;
            }
            return false;
        }

        private bool CheckIfEXE()
        {
            var tmp = iostream.ReadFromFile(0, 2);
            if (tmp.Length == 2)
            {
                //Debug.WriteLine(ASCIIEncoding.UTF8.GetString(tmp));
                if (ASCIIEncoding.UTF8.GetString(tmp).Equals("MZ"))
                {
                    return true;
                }
            }
            return false;
        }

        public int GetHeaderSize()
        {
            if (CheckIfEXE())
            {
                var tmp = iostream.ReadFromFile(8, 2);
                ushort paragraphs = BitConverter.ToUInt16(tmp, 0);
                return paragraphs * 16;
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
            
    }
}
