using System.Text;
using System.IO;
using System.Windows.Forms;

namespace SecureShare
{
    public partial class AccessLogForm : Form
    {
      
        public AccessLogForm()
        {
            InitializeComponent();            
            InitListView();

            this.StartPosition = FormStartPosition.CenterParent;
        }

        public bool GetAccessLog()
        {
            string lastError = string.Empty;

            if (!DRMServer.GetAccessLog( listView_AccessLog, out lastError))
            {
                MessageBox.Show("Get access log failed with error:" + lastError, "GetAccessLog", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        public void InitListView()
        {
            //init ListView control
            listView_AccessLog.Clear();		//clear control
            //create column header for ListView
            listView_AccessLog.Columns.Add("Id#", 40, System.Windows.Forms.HorizontalAlignment.Left);
            listView_AccessLog.Columns.Add("AccessTime", 120, System.Windows.Forms.HorizontalAlignment.Left);
            listView_AccessLog.Columns.Add("FileName", 200, System.Windows.Forms.HorizontalAlignment.Left);            
            listView_AccessLog.Columns.Add("AccessAccount", 120, System.Windows.Forms.HorizontalAlignment.Left);
            listView_AccessLog.Columns.Add("ComputerName", 120, System.Windows.Forms.HorizontalAlignment.Left);
            listView_AccessLog.Columns.Add("AccessIp", 90, System.Windows.Forms.HorizontalAlignment.Left);
            listView_AccessLog.Columns.Add("UserName", 180, System.Windows.Forms.HorizontalAlignment.Left);
            listView_AccessLog.Columns.Add("ProcessName",200, System.Windows.Forms.HorizontalAlignment.Left);            
            listView_AccessLog.Columns.Add("Status", 200, System.Windows.Forms.HorizontalAlignment.Left);
            //DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + ";" + emailAccount + ";" + computerName + ";" + iP + ";" + processName + ";" + userName + ";" 
        }
    }
}
