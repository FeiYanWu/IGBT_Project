using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGBT_SET.Common
{
    public class Product
    {
        public string Model { get; set; }
        public string SmuChannelMap { get; set; }
        public string GdChannelMap { get; set; }
        public string CcsChannelMap { get; set; }
        public string HvChannelMap { get; set; }
        public string GpChannelMap { get; set; }
        public string Rt1ChannelMap { get; set; }
        public string Rt2ChannelMap { get; set; }
    }
}