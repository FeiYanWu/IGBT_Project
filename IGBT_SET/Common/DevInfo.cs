using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.MobileControls;
using System.Xml.Linq;

namespace IGBT_SET.Common
{
    public  class DevInfo
    {
        /// <summary>
        /// 设备名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 设备IP
        /// </summary>
        public  string DevIP { get; set; }
        /// <summary>
        /// 设备端口
        /// </summary>
        public  int Port { get; set; }
    }

    public class DevCollocation
    {
        public  Dictionary <string, DevInfo> devInfos = new Dictionary<string, DevInfo>();
        public  DevCollocation()
        {
            string Path = "./Config\\DevIpCfg.xml";
            XDocument document = XDocument.Load(Path);
            XElement Devadress = document.Root;
            foreach(XElement dev in Devadress.Elements())
            {
                DevInfo tmp = new DevInfo();
                tmp.Name = dev.Name.ToString();
                tmp.DevIP = dev.Attribute("IP").Value;
                tmp.Port = int.Parse(dev.Attribute("Port").Value);
                devInfos.Add(tmp.Name,tmp);
            }
        }
    }
}
