using AttendRecordImportHelper.AttendanceWS;
using SMT.Foundation.Log;
using SMT.HRM.ImportAttRecordWinSV;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AttendRecordImportHelper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            dtFrom.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
            dtTo.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");

        }

        List<T_HR_EMPLOYEECLOCKINRECORD> entTempList = new List<T_HR_EMPLOYEECLOCKINRECORD>();
        private AttendRecordImportHelper.AttendanceWS.AttendanceServiceClient clientAtt;
        string TestMode = ConfigurationManager.AppSettings["TestMode"].ToString();
        public void GetAttendRecord()
        {
            try { 

            if (clientAtt == null) clientAtt = new AttendRecordImportHelper.AttendanceWS.AttendanceServiceClient();

            DateTime From = new DateTime();
            DateTime To = new DateTime();
            DateTime.TryParse(dtFrom.Text, out From);
            DateTime.TryParse(dtTo.Text, out To);

            entTempList.Clear();

            using (hrEntities db = new hrEntities())
            {

                var data = from ent in db.kqjl
                           where ent.checktime >= From
                           && ent.checktime <= To
                           select ent;
                foreach (var item in data)
                {
                    T_HR_EMPLOYEECLOCKINRECORD entTemp = new T_HR_EMPLOYEECLOCKINRECORD();
                    entTemp.CLOCKINRECORDID = System.Guid.NewGuid().ToString().ToUpper();
                    entTemp.FINGERPRINTID = item.badgenumber;
                    entTemp.CLOCKID = item.machine_sn.ToString();
                    entTemp.PUNCHDATE = item.checktime;
                    entTemp.PUNCHTIME = item.checktime.Value.Hour.ToString() + ":" + item.checktime.Value.Minute.ToString();
                    entTempList.Add(entTemp);
                }


            }
            }catch(Exception ex)
            {
                Tracer.Debug("导入打卡记录失败，失败原因为：" + ex.ToString());
            }


        }

        private void btnget_Click(object sender, EventArgs e)
        {
            btnget.Enabled = false;
            try
            {
                GetAttendRecord();
                dtRecord.DataSource = entTempList;
            }
            catch (Exception ex)
            {
                Tracer.Debug(ex.ToString());
                MessageBox.Show("获取失败" + ex.ToString());
            }
            finally
            {
                btnget.Enabled = true;
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {

                DateTime From = new DateTime();
                DateTime To = new DateTime();
                DateTime.TryParse(dtFrom.Text, out From);
                DateTime.TryParse(dtTo.Text, out To);

                DialogResult re = MessageBox.Show("是否导入" + From.ToString("yyyy-MM-dd HH:mm:ss") + "-"
                    + To.ToString("yyyy-MM-dd HH:mm:ss") + " 的打卡记录至协同办公系统？", "确认对话框", MessageBoxButtons.YesNo);
                if (re == DialogResult.No)
                {
                    return;
                }

                Tracer.Debug("航信获取打卡记录成功，下载记录数：" + entTempList.Count());
                string strMsg = string.Empty;
                string[] companyIds = ConfigurationManager.AppSettings["HangXingcompanyID"].Split(',');
                foreach (var strCompanyId in companyIds)
                {
                    if (TestMode == "true")
                    {
                        foreach (var ent in entTempList)
                        {
                            Tracer.Debug("员工指纹编码：" + ent.FINGERPRINTID + " 打卡时间：" + ent.PUNCHDATE + ":" + ent.PUNCHTIME);
                        }
                    }
                    else
                    {
                        string strCurIP = "数据库读取";

                        clientAtt.ImportClockInRdListByWSRealTime(strCompanyId, entTempList.ToArray(), From, To, strCurIP, ref strMsg);
                    }
                    Tracer.Debug("导入打卡记录成功，导入的公司ID为：" + strCompanyId);
                }
                MessageBox.Show("导入打卡记录成功!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("导入打卡记录失败，失败原因为：" + ex.ToString());
                Tracer.Debug("导入打卡记录失败，失败原因为：" + ex.ToString());
            }
        }
    }
}
