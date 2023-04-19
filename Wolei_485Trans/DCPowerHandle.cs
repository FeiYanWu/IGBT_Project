using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Wolei_485Trans
{
    public class DCPowerHandle
    {
        private readonly byte[] _readBuffer;
        private readonly byte[] _writeBuffer;

        private ReaderWriterLockSlim _lock= null;
        private TcpClient _tcpClient;
        private NetworkStream _networkStream;
        private StreamReader _sr;
        private Encoding _defaultEncoding;


        public string DefaultEncoding
        {
            get => this._defaultEncoding.EncodingName;
            set => this._defaultEncoding = Encoding.GetEncoding(value);
        }

        public int SendTimeout
        {
            get => this._tcpClient.SendTimeout;
            set => this._tcpClient.SendTimeout = value;
        }

        public int ReadTimeout
        {
            get => this._networkStream.ReadTimeout;
            set => this._networkStream.ReadTimeout = value;
        }

        public DCPowerHandle(string ipAddress, int port, int readBufferSize = 512, int writeBufferSize = 512) 
        {
            this._tcpClient = new TcpClient();
            this._tcpClient.Connect(ipAddress, port);
            this._networkStream = this._tcpClient.GetStream();
            this.ReadTimeout = 1000;
            this._readBuffer = new byte[readBufferSize];
            this._writeBuffer = new byte[writeBufferSize];
            this._defaultEncoding = Encoding.ASCII;
            this._lock = new ReaderWriterLockSlim();
            this._sr = new StreamReader(_networkStream);
        }

        public void WriteCommand(string command)
        {
            //_lock.EnterWriteLock();
            //try
            //{
            //    WriteLine(command, "");
            //}
            //finally
            //{
            //    _lock.ExitWriteLock();
            //}
            lock (this)
            {
                WriteLine(command, "");
            }
        }
        public string ReadCommand(string command)
        {
            //_lock.EnterWriteLock();
            //try
            //{
            //    WriteLine(command);
            //    Thread.Sleep(1000);
            //    _sr = new StreamReader(_networkStream);
            //    return _sr.ReadLine();
            //}
            //finally
            //{
            //    _lock.ExitWriteLock();
            //}

            lock (this) 
            {
                string result = string.Empty;
                WriteLine(command);
                Thread.Sleep(1000);
                try
                {
                    result = this._sr.ReadLine();
                }
                catch (Exception)
                {
                    result = this._sr.ReadLine();
                }
                return result;
            }
        }
        private void WriteLine(string text, string encodingName = "")
        {
            Encoding encoding = string.IsNullOrWhiteSpace(encodingName) ? this._defaultEncoding : Encoding.GetEncoding(encodingName);
            byte[] textBytes = encoding.GetBytes(text + "\r\n");
            this._networkStream.Write(textBytes, 0, textBytes.Length);
        }
    }
}
