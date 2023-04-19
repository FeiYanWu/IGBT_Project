using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MSTSCLib;

namespace WinformCtrLib
{
    public partial class RemoteCtrl: UserControl
    {
        public FullScreenHelper fullScreenHelper;
        public string UserName { get; set; }

        public string ClearTextPassword { get; set; }

        public string Server { get; set; }

        public int Port { get; set; }

        public RemoteCtrl()
        {
            InitializeComponent();
        }

        public void InitView()
        {
            this.Height = Screen.PrimaryScreen.Bounds.Height;  //获取屏幕分辨率
            this.Width = Screen.PrimaryScreen.Bounds.Width;
            IMsTscNonScriptable securd = (IMsTscNonScriptable)axMsRDPClient.GetOcx();
            axMsRDPClient.UserName = UserName;  //远程桌面用户名
            securd.ClearTextPassword = ClearTextPassword;  //远程桌面密码
            /// 等价于axMsRDPClient.AdvancedSettings7.ClearTextPassword = "1";  //远程桌面密码
            axMsRDPClient.Server = Server;  //远程桌面计算机名或IP
            axMsRDPClient.AdvancedSettings7.RDPPort = Port;  //远程桌面服务器端口号默认为3389
            /// 此电脑需要与被远程电脑在同一局域网下
            axMsRDPClient.FullScreen = false;  //控件全屏显示
            axMsRDPClient.AdvancedSettings9.SmartSizing = true;  //控件随窗体自适应变化
            axMsRDPClient.AdvancedSettings9.NegotiateSecurityLayer = true;  //安全检查
            axMsRDPClient.AdvancedSettings7.EnableCredSspSupport = true;  //允许连接Win11系统
            axMsRDPClient.ColorDepth = 64;  //显示色彩位数
            /// 颜色位数可选 8,16,24,32,64等等，网络条件不好建议选择较低的色彩位数保证远程控制的流畅性
            axMsRDPClient.Connect();  //连接远程桌面
        }

        public void window_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            // ESC 退出全屏
            if (fullScreenHelper.IsFullScreen && e.KeyCode == System.Windows.Forms.Keys.Escape)
            {
                fullScreenHelper.FullScreen(false);
            }
        }

        public void window_Doubleclick(object sender, EventHandler e)
        {
            
        }

        public  void RemoteCtrl_DoubleClick(object sender, EventArgs e)
        {
            if (!fullScreenHelper.IsFullScreen)
            {
                fullScreenHelper.FullScreen(true);
            }
        }
    }
}
