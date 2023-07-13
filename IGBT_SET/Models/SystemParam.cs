using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Controls;

namespace IGBT_SET.Models
{
    public class SystemParam:ViewModelBase
    {
        private string productModel;
        private string productName;

        private byte[] smuChannelMap;
        private byte[] gpChannelMap;
        private byte[] gdChannelMap;
        private byte[] hvChannelMap;
        private byte[] ccsChannelMap;
        private byte[] rt01ChannelMap;
        private byte[] rt02ChannelMap;
        private byte[] brigeLocation;



        public string ProductModel
        {
            get { return productModel; }
            set
            {
                productModel = value;
                RaisePropertyChanged();
            }
        }

        public string ProductName
        {
            get { return productName; }
            set
            {
                productName = value;
                RaisePropertyChanged();
            }
        }
    }
}
