using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPW_GMS
{
    public partial class Log : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            readFromLogFile();
        }
        protected void readFromLogFile()
        {
            string logPath= Path.Combine(HttpRuntime.AppDomainAppPath, "logs\\" + "2020-05-31" + ".log");
            if (File.Exists(logPath))
            {
                string text = File.ReadAllText(logPath);
                string[] lines = File.ReadAllLines(logPath);

            }
            
        }
    }
}