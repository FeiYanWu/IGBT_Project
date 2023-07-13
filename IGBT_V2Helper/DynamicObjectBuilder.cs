using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IGBT_V2Helper
{
    public static class DynamicObjectBuilder
    {
        /// <summary>
        /// 加载组件对象
        /// </summary>
        /// <param name="comName">组件名称</param>
        /// <param name="serverAddress">服务器地址</param>
        /// <returns></returns>
        public static object LoadComObject(string comName, string serverAddress = "localhost")
        {
            Type comType = string.IsNullOrWhiteSpace(serverAddress) || "localhost".Equals(serverAddress, StringComparison.OrdinalIgnoreCase)
                ? Type.GetTypeFromProgID(comName, false)
                : Type.GetTypeFromProgID(comName, serverAddress, false);
            if (null == comType)
            {
                throw new ArgumentException($"The COM object with name '{comName}' cannot be found.");
            }
            object comObject;
            try
            {
                comObject = Activator.CreateInstance(comType);
                if (null == comObject)
                {
                    throw new COMException($"Initialize COM object '{comName}' error.");
                }
            }
            catch (Exception ex)
            {
                throw new COMException($"Initialize COM object '{comName}' error.", ex);
            }
            return comObject;
        }
    }
}