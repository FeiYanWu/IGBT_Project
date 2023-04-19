using System;
using System.Text;
using System.Threading;

namespace Wolei_485Trans
{
    /// <summary>
    /// 协议数据
    /// </summary>
    public class ProtocolData
    {
        public const int ProtocolHeaderLength = 33;
        public const byte StartPrefix1 = 0XAA;
        public const byte StartPrefix2 = 0XBB;

        internal long _frameId;

        public long FrameId => Volatile.Read(ref this._frameId);

        private int _validFlag = 0;
        public bool IsValid => Volatile.Read(ref this._validFlag) != 0;

        public string CardName { get; set; }

        public byte FunctionCode { get; set; }

        public byte SubFunctionCode { get; set; }

        public byte CompatibilityCode { get; set; }

        public byte[] Reserved { get; set; }

        public uint DataBlockSize { get; set; }

        public byte[] Data { get; set; }

        public byte CheckDigit { get; set; }

        public byte OverDigit { get; set; }

        public ProtocolData()
        {
            this.Reserved = new byte[4];
            this._validFlag = 0;
            this._frameId = -1;
            this.OverDigit = 0;
        }

        public ProtocolData(string cardName)
        {
            this.CardName = cardName;
            this.Reserved = new byte[4];
            this.OverDigit = 0;
        }

        internal void Reset(string cardName, byte[] header)
        {
            this._validFlag = 0;
            this.CardName = cardName;
            this.FunctionCode = header[22];
            this.SubFunctionCode = header[23];
            this.CompatibilityCode = header[24];
            this.DataBlockSize = BitConverter.ToUInt32(header, 29);
            if (null == this.Data || this.Data.Length != this.DataBlockSize)
            {
                this.Data = new byte[this.DataBlockSize];
            }
            for (int i = 0; i < this.DataBlockSize; i++)
            {
                this.Data[i] = 0;
            }
            this.CheckDigit = 0;
            Interlocked.Increment(ref this._frameId);
        }

        internal bool CheckData(int checkDigit)
        {
            if (checkDigit < 0)
            {
                this._validFlag = 0;
                this.CheckDigit = 0;
                return false;
            }
            this.CheckDigit = (byte) checkDigit;
            byte checksum = this.CalculateChecksum();
            bool isCheckSumRight = checksum == this.CheckDigit;
            this._validFlag = isCheckSumRight ? 1 : 0;
            return isCheckSumRight;
        }

        internal byte CalculateChecksum()
        {
            byte checksum = 0;
            checksum += StartPrefix1;
            checksum += StartPrefix2;
            byte[] nameBytes = Encoding.ASCII.GetBytes(this.CardName);
            foreach (byte nameByte in nameBytes)
            {
                checksum += nameByte;
            }

            checksum += this.FunctionCode;
            checksum += this.SubFunctionCode;
            checksum += this.CompatibilityCode;
            byte[] blockBytes = BitConverter.GetBytes(this.DataBlockSize);
            foreach (byte blockByte in blockBytes)
            {
                checksum += blockByte;
            }

            foreach (byte dataByte in this.Data)
            {
                checksum += dataByte;
            }

            return checksum;
        }

        public ProtocolData Clone()
        {
            ProtocolData protocolData = new ProtocolData()
            {
                CardName = this.CardName,
                FunctionCode = this.FunctionCode,
                SubFunctionCode = this.SubFunctionCode,
                CompatibilityCode = this.CompatibilityCode,
                DataBlockSize = this.DataBlockSize,
                Data = new byte[this.Data.Length],
                CheckDigit = this.CheckDigit,
                OverDigit = this.OverDigit,
                _frameId = this._frameId
            };
            Buffer.BlockCopy(this.Reserved, 0, protocolData.Reserved, 0, this.Reserved.Length);
            Buffer.BlockCopy(this.Data, 0, protocolData.Data, 0, this.Data.Length);
            return protocolData;
        }

        public void Clone(ProtocolData protocolData)
        {
            protocolData.CardName = this.CardName;
            protocolData.FunctionCode = this.FunctionCode;
            protocolData.SubFunctionCode = this.SubFunctionCode;
            protocolData.CompatibilityCode = this.CompatibilityCode;
            protocolData.DataBlockSize = this.DataBlockSize;
            if (protocolData.Data == null || (this.Data != null && protocolData.Data.Length != this.Data.Length))
            {
                protocolData.Data = new byte[this.Data.Length];
                Buffer.BlockCopy(this.Data, 0, protocolData.Data, 0, this.Data.Length);
            }
            protocolData.CheckDigit = this.CheckDigit;
            protocolData.OverDigit = this.OverDigit;
            protocolData._frameId = this._frameId;
            Buffer.BlockCopy(this.Reserved, 0, protocolData.Reserved, 0, this.Reserved.Length);
        }

        public override bool Equals(object obj)
        {
            ProtocolData comparer = obj as ProtocolData;
            if (null == comparer) return false;
            if (this.FrameId != comparer.FrameId) return false;
            if (this.CheckDigit != comparer.CheckDigit) return false;
            if (this.DataBlockSize != comparer.DataBlockSize) return false;
            if (this.FunctionCode != comparer.FunctionCode) return false;
            if (this.SubFunctionCode != comparer.SubFunctionCode) return false;
            if (this.OverDigit != comparer.OverDigit) return false;
            if (this.Data.Length != comparer.Data.Length) return false;
            for (int i = 0; i < this.Data.Length; i++)
            {
                if (this.Data[i] != comparer.Data[i]) return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            long sum = this.FrameId + this.CheckDigit + this.DataBlockSize + this.FunctionCode + this.SubFunctionCode + this.OverDigit;
            foreach (byte element in this.Data)
            {
                sum += element;
            }
            return sum.GetHashCode();
        }

        public int GetLength()
        {
            // 后面的1是结尾字符，用于避免数据重叠
            return (int) (ProtocolHeaderLength + 1 + this.DataBlockSize + 1);
        }

        public void WriteToBytes(byte[] buffer)
        {
            const int nameBlockSize = 20;
            int writeIndex = 0;
            buffer[writeIndex++] = StartPrefix1;
            buffer[writeIndex++] = StartPrefix2;
            byte[] nameBytes = Encoding.ASCII.GetBytes(this.CardName);
            foreach (byte nameByte in nameBytes)
            {
                buffer[writeIndex++] = nameByte;
            }
            for (int i = nameBytes.Length; i < nameBlockSize; i++)
            {
                buffer[writeIndex++] = 0;
            }

            buffer[writeIndex++] = this.FunctionCode;
            buffer[writeIndex++] = this.SubFunctionCode;
            buffer[writeIndex++] = this.CompatibilityCode;
            foreach (byte reservedByte in this.Reserved)
            {
                buffer[writeIndex++] = reservedByte;
            }
            byte[] dataBytes = BitConverter.GetBytes(this.DataBlockSize);
            foreach (byte dataByte in dataBytes)
            {
                buffer[writeIndex++] = dataByte;
            }
            for (int i = 0; i < this.DataBlockSize; i++)
            {
                buffer[writeIndex++] = this.Data[i];
            }
            buffer[writeIndex++] = this.CheckDigit;
            buffer[writeIndex] = this.OverDigit;
        }
    }
}
