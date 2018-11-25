using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public string Init(string file_path)
        {
            FileManager.GetInstance().Open(file_path);
            return FileManager.GetInstance().FileToHex();
        }

        public void Disassemble()
        {
            Debug.WriteLine(FileManager.GetInstance().GetHeaderSize());
        }
    }
}
