using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGBT_V2Helper
{
    public class ProtocolData
    {
       
        public static byte[] Get485Datas(byte[] datas,byte destDeviceAddress, byte functionCode1,byte functionCode2)
        {
            byte[] buff = null;
            if (datas == null)
            {
                buff = new byte[9];

                buff[0] = 0xAA;
                buff[1] = 0x55;

                buff[2] = 0;

                buff[3] = (byte)HardwareEnum.HW_MANAGE;
                buff[4] = (byte)HardwareAddressEnum.HW_ADDR_MANAGE;

                buff[5] = destDeviceAddress;
                buff[6] = functionCode1;

                buff[7] = functionCode2;
              
                buff[8] = CheckSum(buff);
            }
            else
            {
                buff = new byte[9+ datas.Length];
                buff[0] = 0xAA;
                buff[1] = 0x55;

                buff[2] = (byte)datas.Length;

                buff[3] = (byte)HardwareEnum.HW_MANAGE;
                buff[4] = (byte)HardwareAddressEnum.HW_ADDR_MANAGE;

                buff[5] = destDeviceAddress;
                buff[6] = functionCode1;

                buff[7] = functionCode2;
               

                Array.Copy(datas, 0, buff,8,datas.Length);
                buff[buff.Length - 1] = CheckSum(buff);
            }
            return buff;
        }

        public static byte CheckSum(byte[] buff)
        {
            byte checkSum = 0;
            foreach (byte blockByte in buff)
            {
                checkSum += blockByte;
            }
            return checkSum;
        }
    }
}
