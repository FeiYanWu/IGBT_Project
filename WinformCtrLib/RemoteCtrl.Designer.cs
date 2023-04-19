namespace WinformCtrLib
{
    partial class RemoteCtrl
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RemoteCtrl));
            this.axMsRDPClient = new AxMSTSCLib.AxMsRdpClient11NotSafeForScripting();
            ((System.ComponentModel.ISupportInitialize)(this.axMsRDPClient)).BeginInit();
            this.SuspendLayout();
            // 
            // axMsRDPClient
            // 
            this.axMsRDPClient.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.axMsRDPClient.Enabled = true;
            this.axMsRDPClient.Location = new System.Drawing.Point(3, 3);
            this.axMsRDPClient.Name = "axMsRDPClient";
            this.axMsRDPClient.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMsRDPClient.OcxState")));
            this.axMsRDPClient.Size = new System.Drawing.Size(794, 447);
            this.axMsRDPClient.TabIndex = 0;
            // 
            // RemoteCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.axMsRDPClient);
            this.Name = "RemoteCtrl";
            this.Size = new System.Drawing.Size(800, 450);
            this.DoubleClick += new System.EventHandler(this.RemoteCtrl_DoubleClick);
            ((System.ComponentModel.ISupportInitialize)(this.axMsRDPClient)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxMSTSCLib.AxMsRdpClient11NotSafeForScripting axMsRDPClient;
    }
}
