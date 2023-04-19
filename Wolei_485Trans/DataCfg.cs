using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Wolei_485Trans
{
    public class DevInfo
    {
        /// <summary>
        /// 设备名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 设备IP
        /// </summary>
        public string DevIP { get; set; }
        /// <summary>
        /// 设备端口
        /// </summary>
        public int Port { get; set; }
    }

    public class DataCfg
    {
        public ProtocolDataTransceiver protocolDataTransceiver;
        public TcpClientHandle tcpDC_High;
       
       
        public Dictionary<string, DevInfo> devInfos = new Dictionary<string, DevInfo>();

        public DCPowerHandle DcPowerHandle = null;
        public DCPowerHandle DcPowerHandleConfig = null;

        public DataCfg()
        {
            string Path = "./Config\\DevIpCfg.xml";
            XDocument document = XDocument.Load(Path);
            XElement Devadress = document.Root;
            foreach (XElement dev in Devadress.Elements())
            {
                DevInfo tmp = new DevInfo();
                tmp.Name = dev.Name.ToString();
                tmp.DevIP = dev.Attribute("IP").Value;
                tmp.Port = int.Parse(dev.Attribute("Port").Value);
                devInfos.Add(tmp.Name, tmp);
            }
            protocolDataTransceiver = new ProtocolDataTransceiver(devInfos["WL7016"].DevIP, devInfos["WL7016"].Port, "WL_7016_01");
            tcpDC_High = new TcpClientHandle(devInfos["DC_High"].DevIP, devInfos["DC_High"].Port);
            

            DcPowerHandle = new DCPowerHandle(devInfos["DC_Low"].DevIP, devInfos["DC_Low"].Port);
            DcPowerHandleConfig = new DCPowerHandle(devInfos["DC_Low"].DevIP, devInfos["DC_Low"].Port);
        }

        public DataCfg(string ip, int port, string CardName = "WL_IGBT_485")
        {
            protocolDataTransceiver = new ProtocolDataTransceiver(ip, port, CardName);
            tcpDC_High = new TcpClientHandle(ip, port);

        }

        /// <summary>
        /// 485板卡通信数据包配置
        /// </summary>
        /// <param name="cardtype">板卡类型 </param>
        /// <param name="fuctioncode">功能码</param>
        /// <param name="fuction">数据</param>
        public void SendProtocolDataCFG_7016( byte fuctioncode, byte[] datas)
        {
            var _sysprotocol_t = new ProtocolData("WL_7016_01");
            _sysprotocol_t.FunctionCode = 0xFE;
            _sysprotocol_t.SubFunctionCode = 0x01;
            _sysprotocol_t.CompatibilityCode = fuctioncode;
            _sysprotocol_t.Data = datas;
            _sysprotocol_t.DataBlockSize = (uint)datas.Length;
            protocolDataTransceiver.SendProtocolData(_sysprotocol_t, false);
        }

        /// <summary>
        /// 485板卡通信数据包配置
        /// </summary>
        /// <param name="cardtype">板卡类型 </param>
        /// <param name="fuctioncode">功能码</param>
        /// <param name="fuction">数据</param>
        public void SendProtocolDataCFG(byte cardtype, byte fuctioncode, byte fuction)
        {
            byte[] datas = Card485Cfg(cardtype, fuctioncode, fuction);
            var _sysprotocol_t = new ProtocolData("WL_7016_01");
            _sysprotocol_t.FunctionCode = 0x05;
            _sysprotocol_t.SubFunctionCode = 0x03;
            _sysprotocol_t.CompatibilityCode = 0x01;
            _sysprotocol_t.Data = datas;

            _sysprotocol_t.DataBlockSize = (uint)datas.Length;
            protocolDataTransceiver.SendProtocolData(_sysprotocol_t, false);
        }

        /// <summary>
        /// 485板卡通信数据包配置
        /// </summary>
        /// <param name="cardtype">板卡类型 </param>
        /// <param name="fuctioncode">功能码</param>
        /// <param name="fuction">数据</param>
        public void SendProtocolDataCFG(byte cardtype, byte fuctioncode, byte[] fuction)
        {
            try
            {
                byte[] datas = Card485Cfg(cardtype, fuctioncode, fuction);
                var _sysprotocol_t = new ProtocolData("WL_7016_01");
                _sysprotocol_t.FunctionCode = 0x05;
                _sysprotocol_t.SubFunctionCode = 0x03;
                _sysprotocol_t.CompatibilityCode = 0x01;
                _sysprotocol_t.Data = datas;
                _sysprotocol_t.DataBlockSize = (uint)datas.Length;
                protocolDataTransceiver.SendProtocolData(_sysprotocol_t, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        // 发送给PLC
        public void SendProtocolDataCFG(byte cardtype, byte fuctioncode, byte[] fuction,byte target)
        {
            try
            {
                byte[] datas = Card485Cfg(cardtype, fuctioncode, fuction,target);
                var _sysprotocol_t = new ProtocolData("WL_7016_01");
                _sysprotocol_t.FunctionCode = 0x05;
                _sysprotocol_t.SubFunctionCode = 0x03;
                _sysprotocol_t.CompatibilityCode = 0x01;
                _sysprotocol_t.Data = datas;
                _sysprotocol_t.DataBlockSize = (uint)datas.Length;
                protocolDataTransceiver.SendProtocolData(_sysprotocol_t, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// 485板卡通信数据包配置
        /// </summary>
        /// <param name="cardtype">板卡类型 </param>
        /// <param name="fuctioncode">功能码</param>
        /// <param name="fuction">数据</param>
        public byte[] Card485Cfg(byte cardtype, byte fuctioncode , byte fuction)
        {
            byte[] data = new byte[9];
            data[0] = 0xAA;
            data[1] = 0x55;
            data[2] = (byte)(data.Length-1);
            data[3] = cardtype;
            data[4] = 100;
            data[5] = (byte)(100 + cardtype);
            data[6] = fuctioncode;
            data[7] = fuction;
            data[8] = checksum(data);
            return data;
        }

        /// <summary>
        /// 485板卡通信数据包配置
        /// </summary>
        /// <param name="cardtype">板卡类型 </param>
        /// <param name="fuctioncode">功能码</param>
        /// <param name="fuction">数据数组</param>
        public byte[] Card485Cfg(byte cardtype, byte fuctioncode, byte[] fuction)
        {
            byte[] data = new byte[8 + fuction.Length];
            data[0] = 0xAA;
            data[1] = 0x55;
            data[2] = (byte)(data.Length - 1);
            data[3] = cardtype;
            data[4] = 100;
            data[5] = (byte)(100 + cardtype);
            data[6] = fuctioncode;
            for (int icnt = 0; icnt < fuction.Length; icnt++)
            {
                data[7 + icnt] = fuction[icnt];
            }
            data[data.Length-1] = checksum(data);
            return data;
        }

        /// <summary>
        /// 485板卡通信数据包配置
        /// </summary>
        /// <param name="cardtype">板卡类型 </param>
        /// <param name="fuctioncode">功能码</param>
        /// <param name="fuction">数据数组</param>
        public byte[] Card485Cfg(byte cardtype, byte fuctioncode, byte[] fuction,byte target)
        {
            byte[] data = new byte[8 + fuction.Length];
            data[0] = 0xAA;
            data[1] = 0x55;
            data[2] = (byte)(data.Length - 1);
            data[3] = cardtype;
            data[4] = 100;
            data[5] = (byte)(100 + target);
            data[6] = fuctioncode;
            for (int icnt = 0; icnt < fuction.Length; icnt++)
            {
                data[7 + icnt] = fuction[icnt];
            }
            data[data.Length - 1] = checksum(data);
            return data;
        }

        public byte checksum(byte[] data)
        {
            byte val = 0;
            for (int i = 0; i < data.Length-1; i++)
            {
                val += data[i];
            }
            return val;
        }
    }
    /// <summary>
    /// 设备类型枚举
    /// </summary>
    public enum CardType
    {
        /// <summary>
        /// 系统管理板
        /// </summary>
        SysMangerCard = 0,
        /// <summary>
        /// 开尔文板
        /// </summary>
        KelvinCard,
        /// <summary>
        /// SMU源切换测量卡
        /// </summary>
        SMUcard,
        /// <summary>
        /// 高压脉冲板卡
        /// </summary>
        HPowerPulseCard,
        /// <summary>
        /// 栅极驱动板卡
        /// </summary>
        GridDriveCard,
        /// <summary>
        /// 恒流源板卡
        /// </summary>
        ConstantFlowSourceCard,
        /// <summary>
        /// PLC
        /// </summary>
        PLCCard,
        /// <summary>
        /// 负载检测单元
        /// </summary>
        LoadDetectionUnit,
    }
    /// <summary>
    /// 功能码枚举
    /// </summary>
    public enum FuncCode
    {
        GetIP,
        NeedleContact,
        Temperature,
        IGES,
        VGEth,
        VCEsat,
        ICES,
        DoublePulse,
        SinglePulse,
        ResultJudge = 0x12,
        ParamerGet,
        ParamerSet,
        ParamerReset,
        ParamerErrRec,
        ParamerErrClr,
        StatusCheck,
        ResetOperation,
        KelvinNeedle = 0x40,
        KelvinTemperature,
        PLCCfg =0x50,
        PlcReq,
    }
    /// <summary>
    /// 通道枚举
    /// </summary>
    public enum Channel
    {
        UPipe_up = 1,
        UPipe_down,
        VPipe_up,
        VPipe_down,
        WPipe_up,
        WPipe_down,
        BBreakPipe_down,
        BBreakPipe_up
    }

    public enum Dev_7016ActionType
    {
        Read = 5,
        write = 6,
    }
}
