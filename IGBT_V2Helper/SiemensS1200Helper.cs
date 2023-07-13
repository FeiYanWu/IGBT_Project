using HslCommunication;
using HslCommunication.Profinet.Siemens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IGBT_V2Helper
{
    public class SiemensS1200Helper
    {
        private SiemensS7Net siemens;

        public SiemensS1200Helper(string ipAddress)
        {
            siemens = new SiemensS7Net(SiemensPLCS.S1200);
            siemens.IpAddress = ipAddress;
            if (!Connect())
            {
                MessageBox.Show("PLC链接失败");
                Environment.Exit(0);
            }
        }

        public bool Connect()
        {
            OperateResult connectResult = siemens.ConnectServer();
            return connectResult.IsSuccess;
        }

        public bool Disconnect()
        {
            siemens.ConnectClose();
            return true;
        }

        public bool ReadBoolData(string address)
        {
            OperateResult<bool> readResult = siemens.ReadBool(address);
            if (readResult.IsSuccess)
            {
                return readResult.Content;
            }
            else
            {
                throw new Exception("Read failed: " + readResult.Message);
            }
        }

        public void WriteBoolData(string address, bool value)
        {
            OperateResult writeResult = siemens.Write(address, value);
            if (!writeResult.IsSuccess)
            {
                throw new Exception("Write failed: " + writeResult.Message);
            }
        }

        public void WritInt32(string address, int value)
        {
            OperateResult writeResult = siemens.Write(address, value);
            if (!writeResult.IsSuccess)
            {
                throw new Exception("Write failed: " + writeResult.Message);
            }
        }

        public int ReadInt32(string address)
        {
            OperateResult<int> readResult = siemens.ReadInt32(address);
            if (readResult.IsSuccess)
            {
                return readResult.Content;
            }
            else
            {
                throw new Exception("Read failed: " + readResult.Message);
            }
        }


        public void WritInt16(string address, short value)
        {
            OperateResult writeResult = siemens.Write(address, value);
            if (!writeResult.IsSuccess)
            {
                throw new Exception("Write failed: " + writeResult.Message);
            }
        }

        public short ReadInt16(string address)
        {
            OperateResult<short> readResult = siemens.ReadInt16(address);
            if (readResult.IsSuccess)
            {
                return readResult.Content;
            }
            else
            {
                throw new Exception("Read failed: " + readResult.Message);
            }
        }

        public float ReadFloat(string address)
        {
            OperateResult<float> readResult = siemens.ReadFloat(address);
            if (readResult.IsSuccess)
            {
                return readResult.Content;
            }
            else
            {
                throw new Exception("Read failed: " + readResult.Message);
            }
        }
        /// <summary>
        /// 写入单精浮点
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="value">值</param>
        public void WriteFloat(string address, float value)
        {
            OperateResult writeResult = siemens.Write(address, value);
            if (!writeResult.IsSuccess)
            {
                throw new Exception("Write failed: " + writeResult.Message);
            }
        }

        public double ReadDouble(string address)
        {
            OperateResult<double> readResult = siemens.ReadDouble(address);
            if (readResult.IsSuccess)
            {
                return readResult.Content;
            }
            else
            {
                throw new Exception("Read failed: " + readResult.Message);
            }
        }
        /// <summary>
        /// 写入单精浮点
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="value">值</param>
        public void WriteDouble(string address, double value)
        {
            OperateResult writeResult = siemens.Write(address, value);
            if (!writeResult.IsSuccess)
            {
                throw new Exception("Write failed: " + writeResult.Message);
            }
        }

        public void Dispose()
        {
            siemens?.ConnectClose();
        }
    }
}
