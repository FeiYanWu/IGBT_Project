using IGBT_SET.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGBT_SET.ViewModel
{
    public class SystemConfigViewModel:Notify
    {
        private string faulutInfo;
        
        public string FaulutInfo
        {
            get
            {
                return faulutInfo;
            }
            set
            {
                if (faulutInfo != value)
                {
                    faulutInfo = value;
                    RaisePropertyChanged("FaulutInfo");
                }
            }
        }

        private static SystemConfigViewModel systemConfigViewModel;


        /// <summary>
        /// 获取当前实例
        /// </summary>
        /// <returns></returns>
        public static SystemConfigViewModel GetInstance()
        {
            if (systemConfigViewModel != null)
            {
                return systemConfigViewModel;
            }
            else
            {
                return SetInstance();
            }
        }
        /// <summary>
        /// 创建当前实例
        /// </summary>
        /// <returns></returns>
        private static SystemConfigViewModel SetInstance()
        {
            systemConfigViewModel = new SystemConfigViewModel();
            return systemConfigViewModel;
        }
    }
}
