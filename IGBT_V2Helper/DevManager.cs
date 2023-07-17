using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static IGBT_V2Helper.IGBStructs;

namespace IGBT_V2Helper
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
    public class DevManager
    {
        public SiemensS1200Helper siemensS1200Helper;
        public WL751301Helper wL751301Helper;
        public WL751302Helper wL751302Helper;
        public WL7020Helper wl7020Helper;
        public WL7016Helper wl7016Helper;
        public WL7010Helper wl7010Helper;
        public WL7001Helper wl7001Helper;
        public WL7005Helper wl7005Helper;
        public WL7505Helper wl7505Helper;
        public WL7011Helper wl7011Helper;


        public TcpClientHandle tcpDC_High ;
        public DCPowerHandle DcPowerHandle ;
        public DCPowerHandle DcPowerHandleConfig;

        public OscilloScopeHelper oscilloScopeHelper;

        public Dictionary<string, DevInfo> devInfos = new Dictionary<string, DevInfo>();

        public DevManager()
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

            siemensS1200Helper = new SiemensS1200Helper(devInfos["S1200"].DevIP);
            wL751301Helper = new WL751301Helper(devInfos["WL7513"].DevIP);
            wL751302Helper = new WL751302Helper(devInfos["WL7513_02"].DevIP);
            wl7020Helper   = new WL7020Helper(devInfos["WL7020"].DevIP);
            wl7016Helper   = new WL7016Helper(devInfos["WL7016"].DevIP);
            wl7010Helper   = new WL7010Helper(devInfos["WL7010"].DevIP);


            wl7001Helper   = new WL7001Helper(devInfos["WL7001"].DevIP);
            wl7005Helper   = new WL7005Helper(devInfos["WL7005"].DevIP);
            wl7505Helper   = new WL7505Helper(devInfos["WL7505"].DevIP);
            wl7011Helper   = new WL7011Helper(devInfos["WL7011"].DevIP);

            oscilloScopeHelper = new OscilloScopeHelper(devInfos["Oscilloscope"].DevIP);

            tcpDC_High = new TcpClientHandle(devInfos["DC_High"].DevIP, devInfos["DC_High"].Port);
            DcPowerHandle = new DCPowerHandle(devInfos["DC_Low"].DevIP, devInfos["DC_Low"].Port);
            DcPowerHandleConfig = new DCPowerHandle(devInfos["DC_Low"].DevIP, devInfos["DC_Low"].Port);
        }


        public void ClearAllFault()
        {
            wL751301Helper.ClearFault();
            wL751302Helper.ClearFault();
            wl7020Helper.ClearFault();
            wl7016Helper.ClearFault();
            wl7010Helper.ClearFault();
            wl7001Helper.ClearFault();
            wl7005Helper.ClearFault();
            wl7505Helper.ClearFault();
            wl7011Helper.ClearFault();
        }
        public void CloseAllDevice()
        {
            wL751301Helper?.Dispose();
            wL751302Helper?.Dispose();
            wl7020Helper?.Dispose();
            wl7016Helper?.Dispose();
            wl7010Helper?.Dispose();
            wl7001Helper?.Dispose();
            wl7005Helper?.Dispose();
            wl7505Helper?.Dispose();
            wl7011Helper?.Dispose();
            siemensS1200Helper?.Dispose();
            tcpDC_High?.Dispose();

            DcPowerHandle?.Dispose();
            DcPowerHandleConfig?.Dispose();
        }
        public bool LoadParam(ref igbt_fix_para_t igbtPara)
        {
            if (wl7016Helper.SetIGBTPara(ref igbtPara) &&
                wl7505Helper.SetIGBTPara(ref igbtPara) &&
                wl7001Helper.SetIGBTPara(ref igbtPara) &&
                wl7010Helper.SetIGBTPara(ref igbtPara) &&
                wl7001Helper.SetIGBTPara(ref igbtPara) &&
                wl7011Helper.SetIGBTPara(ref igbtPara) &&
                wL751301Helper.SetIGBTPara(ref igbtPara) &&
                wL751302Helper.SetIGBTPara(ref igbtPara) &&
                wl7005Helper.SetIGBTPara(ref igbtPara))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
