using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wolei_485Trans
{
    public class ProtocolDataTransceiver : IDisposable
    {
        private readonly TcpClient _tcpClient;
        private readonly NetworkStream _networkStream;
        private readonly CancellationTokenSource _cancellation;
        private readonly Task _continuousReadTask;
        private SpinLock _readLock;
        private SpinLock _writeLock;
        private Exception _readException;
        private readonly string _cardName;
        private readonly ProtocolData _readProtocolData;
        private readonly ProtocolData _getProtocolData;
        private byte[] _writeBuffer;

        private long _errorFrameCount;
        public long ErrorFrameCount => Volatile.Read(ref this._errorFrameCount);
        public long ReadFrameCount => this._readProtocolData.FrameId + 1;

        public event Action<ProtocolData> ProtocolDataReceived;

        public ProtocolDataTransceiver(string cardIpAddress, int port, string cardName)
        {
            try
            {
                IPAddress ipAddress = IPAddress.Parse(cardIpAddress);
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);
                this._tcpClient = new TcpClient();
                this._tcpClient.Connect(ipEndPoint);
                this._networkStream = this._tcpClient.GetStream();
                _cancellation = new CancellationTokenSource();
                _continuousReadTask = new Task(ReadTask, this._cancellation.Token);
                _continuousReadTask.Start();
                this._readLock = new SpinLock();
                this._writeLock = new SpinLock();
                this._readException = null;
                this._cardName = cardName;
                this._readProtocolData = new ProtocolData();
                _errorFrameCount = 0;
                this._writeBuffer = new byte[1024];
            }
            catch (Exception ex)
            {
                MessageBox.Show("服务器链接失败");
                //Environment.Exit(0);
            }
        }

        public ProtocolData GetCurrentProtocolData()
        {
            if (null != this._readException)
            {
                throw this._readException;
            }

            return this._readProtocolData;
        }

        public void SendProtocolData(ProtocolData protocolData, bool isCheckDigitDone)
        {
            if (!isCheckDigitDone)
            {
                protocolData.CheckDigit = protocolData.CalculateChecksum();
            }
            // TODO 暂时不处理超长的场景
            int writeLength = protocolData.GetLength();
            bool getLock = false;
            _writeLock.Enter(ref getLock);
            try
            {
                protocolData.WriteToBytes(this._writeBuffer);
                Console.WriteLine(BitConverter.ToString( this._writeBuffer).Replace("-"," "));
                this._networkStream.Write(this._writeBuffer, 0, writeLength);

            }
            finally
            {
                this._writeLock.Exit();
            }
        }

        private void ReadTask()
        {
            byte[] headerBuffer = new byte[ProtocolData.ProtocolHeaderLength];
            int headBufferLength = 0;
            bool getLock = false;
            try
            {
                while (!this._cancellation.IsCancellationRequested)
                {
                    Thread.Sleep(5);
                    int actualReadCount = this._networkStream.Read(headerBuffer, headBufferLength, headerBuffer.Length - headBufferLength);
                    headBufferLength += actualReadCount;
                    if (actualReadCount == 0 || headBufferLength < 2)
                    {
                        continue;
                    }

                    bool isHeaderDone = this.ProcessHeader(headerBuffer, ref headBufferLength);
                    if (!isHeaderDone) continue;
                    // 初始化协议头
                    getLock = false;
                    this._readLock.Enter(ref getLock);
                    _readProtocolData.Reset(this._cardName, headerBuffer);
                    this._readLock.Exit();

                    for (int i = 0; i < headerBuffer.Length; i++)
                    {
                        headerBuffer[i] = 0;
                    }
                    headBufferLength = 0;

                    // 获取数据
                    int dataReadCount = 0;
                    int bytesToRead = (int) this._readProtocolData.DataBlockSize;
                    getLock = false;
                    this._readLock.Enter(ref getLock);
                    while (dataReadCount < bytesToRead)
                    {
                        int readCount = this._networkStream.Read(this._readProtocolData.Data, dataReadCount, bytesToRead - dataReadCount);
                        dataReadCount += readCount;
                    }
                    this._readLock.Exit();
                    // 检查校验码
                    int checkDigit = -1;
                    while (!this._cancellation.IsCancellationRequested && checkDigit < 0)
                    {
                        checkDigit = this._networkStream.ReadByte();
                    }

                    getLock = false;
                    this._readLock.Enter(ref getLock);
                    bool checkPassed = this._readProtocolData.CheckData(checkDigit);
                    if (!checkPassed)
                    {
                        Interlocked.Increment(ref _errorFrameCount);
                    }
                    else
                    {
                        this.OnProtocolDataReceived();
                    }
                    this._readLock.Exit();
                }
            }
            catch (ObjectDisposedException)
            {
                // ignore
            }
            catch (Exception ex)
            {
                _readException = ex;
            }
            finally
            {
                if (getLock && this._readLock.IsHeld)
                {
                    this._readLock.Exit();
                }
            }
        }

        private bool ProcessHeader(byte[] headerBuffer, ref int headBufferLength)
        {
            int prefixStartIndex = -1;
            for (int i = 1; i < headBufferLength; i++)
            {
                if (headerBuffer[i - 1] == ProtocolData.StartPrefix1 && headerBuffer[i] == ProtocolData.StartPrefix2)
                {
                    prefixStartIndex = i - 1;
                    break;
                }
                // IGBT的板卡返回BBAA，所以加了以下代码
                if (headerBuffer[i - 1] == ProtocolData.StartPrefix2 && headerBuffer[i] == ProtocolData.StartPrefix1)
                {
                    prefixStartIndex = i - 1;
                    break;
                }
            }

            // 没有发现前缀，则继续读取数据
            if (prefixStartIndex < 0)
            {
                headBufferLength = 0;
                return false;
            }

            // 将数据头对齐到0号索引
            if (prefixStartIndex != 0)
            {
                AlignHeaderPrefixToStart(prefixStartIndex, headBufferLength, headerBuffer);
                headBufferLength -= prefixStartIndex;
            }

            // 如果头还没有读满，则继续读取
            if (headBufferLength != headerBuffer.Length) return false;
            bool isHeadFit = this.IsHeaderFit(headerBuffer);
            if (!isHeadFit)
            {
                this.FindNextAvailableHead(out headBufferLength, headerBuffer);
                return false;
            }
            return true;
        }

        private bool IsHeaderFit(byte[] headerBuffer)
        {
            if ((headerBuffer[0] == ProtocolData.StartPrefix1 && headerBuffer[1] == ProtocolData.StartPrefix2)
               || (headerBuffer[0] == ProtocolData.StartPrefix2 || headerBuffer[1] == ProtocolData.StartPrefix1))
            {
                int nameEndIndex = headerBuffer.Length;
                for (int i = 2; i < headerBuffer.Length; i++)
                {
                    if (headerBuffer[i] == 0)
                    {
                        nameEndIndex = i;
                        break;
                    }
                }

                string cardName = Encoding.ASCII.GetString(headerBuffer, 2, nameEndIndex - 2);
                return this._cardName.Equals(cardName?.TrimEnd());
            }
            return false;
        }

        private void FindNextAvailableHead(out int headBufferLength, byte[] headerBuffer)
        {
            int nextHeaderIndex = -1;
            for (int i = 3; i < headerBuffer.Length; i++)
            {
                if (headerBuffer[i - 1] == ProtocolData.StartPrefix1 && headerBuffer[i] == ProtocolData.StartPrefix2)
                {
                    nextHeaderIndex = i - 1;
                    break;
                }
            }
            if (nextHeaderIndex < 0)
            {
                headBufferLength = 0;
                return;
            }
            headBufferLength = headerBuffer.Length - nextHeaderIndex;
            AlignHeaderPrefixToStart(nextHeaderIndex, headerBuffer.Length, headerBuffer);
        }

        private static void AlignHeaderPrefixToStart(int prefixStartIndex, int headBufferLength, byte[] headerBuffer)
        {
            int writeIndex = 0;
            for (int i = prefixStartIndex; i < headBufferLength; i++)
            {
                headerBuffer[writeIndex++] = headerBuffer[i];
            }
        }

        private void OnProtocolDataReceived()
        {
            this.ProtocolDataReceived?.Invoke(this._readProtocolData);
        }

        public void Dispose()
        {
            this._cancellation?.Cancel();
            this._networkStream?.Dispose();
            this._tcpClient?.Dispose();
            this._cancellation?.Dispose();
            this._continuousReadTask.Wait(500);
            this._continuousReadTask.Dispose();
        }
    }
}

