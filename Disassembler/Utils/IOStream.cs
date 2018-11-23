using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Disassembler.Utils
{
    public sealed class IOStream : IDisposable
    {
        private FileStream fs;
        private bool isOpened = false;
        public IOStream()
        {

        }

        public bool OpenFile(string path)
        {
            fs = File.OpenRead(path);
            if (fs != null)
            {
                isOpened = true;
                return true;
            }
            return false;
        }

        public bool IsFileOpen()
        {
            return isOpened;
        }

        public byte[] ReadFromFile(int offset, int count)
        {
            if (fs == null)
            {
                return null;
            }
            if (count == 0)
            {
                byte[] buffer = new byte[fs.Length];
                fs.Seek(offset, SeekOrigin.Begin);
                fs.Read(buffer, 0, (int)fs.Length - count);
                return buffer;
            }
            else
            {
                byte[] buffer = new byte[count];
                fs.Seek(offset, SeekOrigin.Begin);
                fs.Read(buffer, 0, count);
                return buffer;
            }
        }

        public void CloseFile()
        {
            if (fs != null)
            {
                fs.Close();
                isOpened = false;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (fs != null)
                    {
                        fs.Dispose();
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~IOStream() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
