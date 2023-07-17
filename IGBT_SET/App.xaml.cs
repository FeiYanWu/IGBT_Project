using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace IGBT_SET
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        System.Threading.Mutex mutex;

        public App()
        {
            this.Startup += new StartupEventHandler(App_Startup);
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }
        void App_Startup(object sender, StartupEventArgs e)
        {
            bool ret;
            mutex = new System.Threading.Mutex(true, Process.GetCurrentProcess().ProcessName, out ret);

            if (!ret)
            {
                MessageBox.Show("已经启动请先关闭之后再打开！");
                Environment.Exit(0);
            }
        }
    }
}
