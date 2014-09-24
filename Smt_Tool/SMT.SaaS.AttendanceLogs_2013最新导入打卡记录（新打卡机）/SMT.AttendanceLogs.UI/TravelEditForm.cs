using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.Objects.DataClasses;
using System.Threading;
using System.Reflection;
using System.Data;
using System.Data.Objects;
using SMT.Foundation.Core;
using SMT.Foundation.Log;
using SMT_OA_EFModel;

namespace SMT.AttendanceLogs.UI
{
    public partial class TravelEditForm : Form
    {
        private SMT_OA_EFModelContext dal;
        public TravelEditForm()
        {
            InitializeComponent();
            dal = new SMT_OA_EFModelContext();
        }

        private void btnSerch_Click(object sender, EventArgs e)
        {
            ShowMessage("查询数据中......");
            try
            {
                getTravelMaster();
                getTraveDetail();
            }
            catch (Exception ex) { }
            finally
            {
              ShowMessage("");
            }
           
        }
        delegate void DelShow(String Msg); //代理
        //将对控件的操作写到一个函数中 
        public void ShowMessage(String para)
        {
            if (!labMsg.InvokeRequired) //不需要唤醒，就是创建控件的线程
            //如果是创建控件的线程，直接正常操作 
            {
                labMsg.Text = para ;
            }
            else //非创建线程，用代理进行操作
            {
                DelShow ds = new DelShow(ShowMessage);
                //唤醒主线程，可以传递参数，也可以为null，即不传参数
                Invoke(ds, new object[] { para });
            }
        }

        private string strBusneessTripid;
        private void getTravelMaster()
        {
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("请输入员工姓名");
                return;
            }
            DateTime dtStart = new DateTime(2014 - 1 - 1);
            DateTime.TryParse(this.dtFrom.Value.ToString("yyyy-MM-dd"), out dtStart);



            var ents = from ent in dal.T_OA_BUSINESSTRIP.Include("T_OA_BUSINESSTRIPDETAIL")
                       where ent.OWNERNAME.Contains(txtName.Text)
                       && ent.STARTDATE >= dtStart
                       select new
                       {
                           ent.BUSINESSTRIPID,
                           ent.OWNERNAME,
                           ent.STARTDATE,
                           ent.ENDDATE,
                           ent.STARTCITYNAME,
                           ent.ENDCITYNAME,
                           ent.CHECKSTATE,
                           ent.CONTENT
                       };
            if (ents.Count() < 1) return;
            dtBussnissTrip.DataSource = ents.ToList();
            strBusneessTripid = ents.FirstOrDefault().BUSINESSTRIPID;
        }

        private void dtBussnissTrip_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewColumn column = dtBussnissTrip.Columns[e.ColumnIndex];
                strBusneessTripid = dtBussnissTrip.Rows[e.RowIndex].Cells["BUSINESSTRIPID"].EditedFormattedValue.ToString();
                getTraveDetail();

            }
        }

        private void getTraveDetail()
        {
            var ents = from ent in dal.T_OA_BUSINESSTRIPDETAIL
                       where ent.T_OA_BUSINESSTRIP.BUSINESSTRIPID == strBusneessTripid
                       select new
                       {
                           ent.BUSINESSTRIPDETAILID,
                           ent.STARTDATE,
                           ent.ENDDATE,
                           ent.STARTCITYNAME,
                           ent.ENDCITYNAME
                       };

            try
            {
                DataTable dt = new System.Data.DataTable();
                dt = LinqToDataTable(ents);
                dtTravel.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        public DataTable LinqToDataTable(IEnumerable list)
{
            DataTable table = new DataTable();
            bool schemaIsBuild = false;
            PropertyInfo[] props = null;
            foreach (object item in list)
            {
                if (!schemaIsBuild)
                {
                    props = item.GetType().GetProperties();
                    foreach (var pi in props)
                        table.Columns.Add(new DataColumn(pi.Name, typeof(String)));
                    schemaIsBuild = true;
                }
                var row = table.NewRow();
                foreach (var pi in props)
                {
                    if (pi.GetValue(item, null) == null) row[pi.Name] = string.Empty;
                    else row[pi.Name] = pi.GetValue(item, null);
                }
                table.Rows.Add(row);
            }
            table.AcceptChanges();
            return table;
}

        private void btnEdit_Click(object sender, System.EventArgs e)
        {
            try { 
            if(dtTravel.Rows.Count>0)
            {

                var entsBusnessTrip = (from ent in dal.T_OA_BUSINESSTRIP
                                       where ent.BUSINESSTRIPID == strBusneessTripid
                                       select ent).FirstOrDefault();

                for(int i=0;i<dtTravel.Rows.Count;i++)
                {
                    string strid = dtTravel.Rows[i].Cells["BUSINESSTRIPDETAILID"].EditedFormattedValue.ToString();
                    var ents = (from ent in dal.T_OA_BUSINESSTRIPDETAIL
                              where ent.BUSINESSTRIPDETAILID == strid
                              select ent).FirstOrDefault();

                    DateTime dtStart;
                    string strdtStart = dtTravel.Rows[i].Cells["STARTDATE"].EditedFormattedValue.ToString();
                    DateTime.TryParse(strdtStart, out dtStart);
                    if(dtStart==null)
                    {
                        MessageBox.Show("出差明细开始时间设置错误，请检查！");
                        return;
                    }
                   
                    DateTime dtend;
                    string strdtend = dtTravel.Rows[i].Cells["ENDDATE"].EditedFormattedValue.ToString();
                    DateTime.TryParse(strdtend, out dtend);
                    if (dtend == null)
                    {
                        MessageBox.Show("出差明细结束时间设置错误，请检查！");
                        return;
                    }

                    if (i == 0) entsBusnessTrip.STARTDATE = dtStart;
                    if (i == dtTravel.Rows.Count - 1) entsBusnessTrip.ENDDATE = dtend;
                    ents.STARTDATE = dtStart;
                    ents.ENDDATE = dtend;
                }
                if(!string.IsNullOrEmpty(txtCheckState.Text))
                {
                    try
                    {
                        int checkstate = int.Parse(txtCheckState.Text);
                        if (checkstate == 0 || checkstate == 1 || checkstate == 2 || checkstate == 3)
                        {
                            entsBusnessTrip.CHECKSTATE = checkstate.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("审核状态输入错误");
                    }
                }

                dal.SaveChanges();
                MessageBox.Show("修改成功！");
                getTravelMaster();
                getTraveDetail();
            }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dtTravel_CellClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dtTravel_SelectionChanged(object sender, System.EventArgs e)
        {
            dtTravel.ReadOnly = false;
            dtTravel.BeginEdit(true);
        }
      
        private void btnInitTravelSolution_Click(object sender, System.EventArgs e)
        {
            
        }

    }
}
