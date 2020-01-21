using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PentaUpdate
{
    public class Log
    {
        public static string PercorsoLog = Application.StartupPath + "\\Logs\\" + DateTime.Now.ToString("yyyyMMdd") + "-UPDATE.log";

        public void InizializzareLog()
        {
            if (!File.Exists(PercorsoLog))
            {
                if (!Directory.Exists(Application.StartupPath + "\\Logs"))
                    Directory.CreateDirectory(Application.StartupPath + "\\Logs");
                FileStream Log = File.Create(PercorsoLog);
                Log.Dispose();
            }
        }

        public static void WriteLog(string log)
        {
            File.AppendAllText(PercorsoLog, "[" + DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss") + "] --> " + log + Environment.NewLine);
        }
    }
}
