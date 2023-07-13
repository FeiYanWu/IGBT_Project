using ACTIVEDSOLib;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IGBT_V2Helper
{
    public class OscilloScopeHelper:IDisposable
    {
       
        public string IP;
        public OscilloScopeHelper(string ip)
        {
            this.IP = ip;
            ConnectOscilloScope();
        }


        private ActiveDSO activeDSOOBject;
        private void ConnectOscilloScope()
        {
            activeDSOOBject = (ActiveDSO)DynamicObjectBuilder.LoadComObject("LeCroy.ActiveDSOCtrl.1");

            bool success = activeDSOOBject.MakeConnection("IP:"+this.IP);
            if (!success) 
            {
                MessageBox.Show("连接示波器错误");
                Environment.Exit(0);
            }
        }

        public bool RecallSetUp(string filePath)
        {
            bool setFilePathSuccess = activeDSOOBject.WriteString("VBS ? 'app.SaveRecall.Setup.RecallSetupFilename="+ filePath,true);
            if (setFilePathSuccess)
            {
                 return activeDSOOBject.WriteString("VBS? 'app.SaveRecall.Setup.DoRecallSetupFileDoc2'",true);
            }
            return false;
        }

        public bool TriggerSingle()
        {
            return activeDSOOBject.WriteString("TRMD SINGLE", true);
        }


        public void Dispose()
        {
            activeDSOOBject?.Disconnect();
        }
    }
}
