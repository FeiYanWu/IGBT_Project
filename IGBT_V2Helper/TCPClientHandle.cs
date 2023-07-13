using IGBT_V2Helper.common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IGBT_V2Helper
{
    public class TcpClientHandle : IDisposable
    {
       
            private readonly byte[] _readBuffer;
            private readonly byte[] _writeBuffer;
            private TcpClient _tcpClient;
            private NetworkStream _networkStream;
            private Encoding _defaultEncoding;
            private StreamReader _sr;
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

            public TcpClientHandle(string ipAddress, int port, int readBufferSize = 512, int writeBufferSize = 512)
            {
                try
                {
                    this._tcpClient = new TcpClient();
                    this._tcpClient.Connect(ipAddress, port);
                    this._networkStream = this._tcpClient.GetStream();
                    this.ReadTimeout = 100;
                    this._readBuffer = new byte[readBufferSize];
                    this._writeBuffer = new byte[writeBufferSize];
                    this._defaultEncoding = Encoding.ASCII;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("TCP_服务器连接失败");
                    Environment.Exit(0);
                }
            }

            public void WriteString(string text, string encodingName = "")
            {
                Encoding encoding = string.IsNullOrWhiteSpace(encodingName) ? this._defaultEncoding : Encoding.GetEncoding(encodingName);
                int encodingLength = encoding.GetBytes(text, 0, text.Length, this._writeBuffer, 0);
                this._networkStream.Write(this._writeBuffer, 0, encodingLength);
            }

            public void WriteStringCrossThread(string text, string encodingName = "")
            {
                Encoding encoding = string.IsNullOrWhiteSpace(encodingName) ? this._defaultEncoding : Encoding.GetEncoding(encodingName);
                byte[] textBytes = encoding.GetBytes(text);
                this._networkStream.Write(textBytes, 0, textBytes.Length);
            }

            public void WriteBytes(byte[] bytes, int startIndex, int count)
            {
                this._networkStream.Write(bytes, startIndex, count);
            }

            public void WriteByte(byte byteValue)
            {
                this._networkStream.WriteByte(byteValue);
            }

            public void WriteLine(string text, string encodingName = "")
            {
                Encoding encoding = string.IsNullOrWhiteSpace(encodingName) ? this._defaultEncoding : Encoding.GetEncoding(encodingName);
                byte[] textBytes = encoding.GetBytes(text + "\r\n");
                this._networkStream.Write(textBytes, 0, textBytes.Length);
            }

            public string ReadLine()
            {
                string resutl = string.Empty;
                _sr = new StreamReader(_networkStream);
                resutl = _sr.ReadLine();
                return resutl;
            }

            public string ReadText(int readLength, bool isReadExactCount = false, string encodingName = "")
            {
                Encoding encoding = string.IsNullOrWhiteSpace(encodingName) ? this._defaultEncoding : Encoding.GetEncoding(encodingName);
                int readCount = 0;
                if (!isReadExactCount)
                {
                    readCount = this._networkStream.Read(this._readBuffer, 0, readLength);
                    return encoding.GetString(this._readBuffer, 0, readCount);
                }
                while (readCount < readLength)
                {
                    readCount += this._networkStream.Read(this._readBuffer, readCount, readLength - readCount);
                }
                return encoding.GetString(this._readBuffer, 0, readCount);
            }

            public string ReadTextCrossThread(int readLength, bool isReadExactCount = false, string encodingName = "")
            {
                Encoding encoding = string.IsNullOrWhiteSpace(encodingName) ? this._defaultEncoding : Encoding.GetEncoding(encodingName);
                byte[] dataBuffer = new byte[readLength];
                int readCount = 0;
                if (isReadExactCount)
                {
                    while (readCount < readLength)
                    {
                        readCount += this._networkStream.Read(dataBuffer, readCount, readLength - readCount);
                    }
                }
                else
                {
                    readCount = this._networkStream.Read(dataBuffer, 0, readLength);
                }
                return encoding.GetString(dataBuffer, 0, readCount);
            }

            public string[] ReadMessages(byte[] messagePrefix, byte[] messagePostfix, bool includePrefixAndPostfix, string encodingName = "")
            {
                Encoding encoding = string.IsNullOrWhiteSpace(encodingName) ? this._defaultEncoding : Encoding.GetEncoding(encodingName);
                int postfixCount = messagePostfix.Length;
                int minReadLength = messagePrefix.Length + postfixCount;
                // 找到起始位置匹配的节点
                int readCount = this._networkStream.Read(this._readBuffer, 0, this._readBuffer.Length);
                List<string> messages = new List<string>(4);
                int readIndex = 0;
                while (readIndex < readCount)
                {
                    int startIndex = GetByteMetIndex(this._readBuffer, readCount, readIndex, messagePrefix);
                    if (startIndex < 0)
                    {
                        break;
                    }
                    int endIndex = GetByteMetIndex(this._readBuffer, readCount, startIndex, messagePostfix);
                    // TODO 暂时跳过不处理未接收完成的消息
                    if (endIndex < 0)
                    {
                        break;
                    }
                    readIndex = endIndex + messagePostfix.Length;
                    if (!includePrefixAndPostfix)
                    {
                        startIndex += messagePrefix.Length;
                    }
                    else
                    {
                        endIndex += messagePostfix.Length;
                    }
                    messages.Add(encoding.GetString(this._readBuffer, startIndex, endIndex - startIndex));
                }
                return messages.ToArray();
            }

            /// <summary>
            /// 读取没有控制字符的数据
            /// </summary>
            /// <param name="messagePrefix">消息前缀</param>
            /// <param name="messagePostfix">消息后缀</param>
            /// <param name="includePrefixAndPostfix">是否需要包含前后缀</param>
            /// <param name="encodingName">编码类型</param>
            public string[] ReadMessagesWithNoControl(byte[] messagePrefix, byte[] messagePostfix, bool includePrefixAndPostfix, string encodingName = "")
            {
                Encoding encoding = string.IsNullOrWhiteSpace(encodingName) ? this._defaultEncoding : Encoding.GetEncoding(encodingName);
                int postfixCount = messagePostfix.Length;
                int minReadLength = messagePrefix.Length + postfixCount;
                List<string> messages = new List<string>(4);

                while (messages.Count == 0)
                {
                    // 找到起始位置匹配的节点
                    int readCount = this._networkStream.Read(this._readBuffer, 0, this._readBuffer.Length);
                    int readIndex = 0;
                    while (readIndex < readCount)
                    {
                        int startIndex = GetByteMetIndex(this._readBuffer, readCount, readIndex, messagePrefix);
                        if (startIndex < 0)
                        {
                            break;
                        }
                        int endIndex = GetByteMetIndex(this._readBuffer, readCount, startIndex, messagePostfix);
                        // TODO 暂时跳过不处理未接收完成的消息
                        if (endIndex < 0)
                        {
                            break;
                        }
                        readIndex = endIndex + messagePostfix.Length;
                        if (!includePrefixAndPostfix)
                        {
                            startIndex += messagePrefix.Length;
                        }
                        else
                        {
                            endIndex += messagePostfix.Length;
                        }

                        string message = encoding.GetString(this._readBuffer, startIndex, endIndex - startIndex);
                        // 如果包含控制字符则重新扫描
                        if (StringUtil.HasControlChar(message))
                        {
                            continue;
                        }
                        messages.Add(message);
                    }
                }

                return messages.ToArray();
            }

            private static int GetByteMetIndex(byte[] source, int bufferLength, int compareStartIndex, byte[] compareBytes)
            {
                if (bufferLength - compareStartIndex < compareBytes.Length) return -1;
                for (int i = compareStartIndex; i <= bufferLength - compareBytes.Length; i++)
                {
                    bool isByteEqual = true;
                    int sourceIndex = i;
                    for (int j = 0; j < compareBytes.Length; j++)
                    {
                        if (source[sourceIndex++] != compareBytes[j])
                        {
                            isByteEqual = false;
                            break;
                        }
                    }
                    if (isByteEqual) return i;
                };
                return -1;
            }

            private static void AlignHeaderPrefixToStart(int prefixStartIndex, int headBufferLength, byte[] headerBuffer)
            {
                int writeIndex = 0;
                for (int i = prefixStartIndex; i < headBufferLength; i++)
                {
                    headerBuffer[writeIndex++] = headerBuffer[i];
                }
            }

            public byte[] ReadBytes(int readCount, out int actualReadCount, bool isReadExactCount = false)
            {
                if (!isReadExactCount)
                {
                    actualReadCount = this._networkStream.Read(this._readBuffer, 0, readCount);
                    return this._readBuffer;
                }
                actualReadCount = 0;
                while (actualReadCount < readCount)
                {
                    actualReadCount += this._networkStream.Read(this._readBuffer, actualReadCount, readCount - actualReadCount);
                }
                return this._readBuffer;
            }

            public byte[] ReadBytesCrossThread(int readCount, out int actualReadCount, bool isReadExactCount)
            {
                byte[] readBuffer = new byte[readCount];
                if (!isReadExactCount)
                {
                    actualReadCount = this._networkStream.Read(readBuffer, 0, readCount);
                    return readBuffer;
                }
                actualReadCount = 0;
                while (actualReadCount < readCount)
                {
                    actualReadCount += this._networkStream.Read(readBuffer, actualReadCount, readCount - actualReadCount);
                }
                return readBuffer;
            }

            public int ReadByte()
            {
                return this._networkStream.ReadByte();
            }

            public void Dispose()
            {
                this._networkStream?.Dispose();
                this._sr?.Dispose();
                this._tcpClient?.Dispose();
            }
        }
}