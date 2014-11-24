using SMT_HRM_EFModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SMT.Foundation.Core;
using System.Configuration;
using SMT.HRM.BLL;
using System.Threading;


namespace SMT.AttendanceLogs.UI
{
    public partial class SalaryStandGenerator : BaseForm
    {

        public IDAL dal;
        public static string qualifiedEntitySetName = ConfigurationManager.AppSettings["DBContextName"] + ".";
        public SalaryStandGenerator()
        {
            InitializeComponent();
            dal = new SMT_HRM_EFModel.SMT_HRM_EFModelContext();
            PermissionWS.PermissionServiceClient pclient = new PermissionWS.PermissionServiceClient();
            var ents = pclient.GetDictionaryByCategoryArray(new string[] { "POSTLEVEL" });

            foreach (var item in ents)
            {
                postlevels.Add(item.DICTIONARYNAME, item.DICTIONARYVALUE.ToString());
            }
        }
        private T_HR_SALARYSOLUTION solution;
        private void button1_Click(object sender, EventArgs e)
        {
        
            var q =from ent in dal.GetObjects<T_HR_SALARYSOLUTION>().Include("T_HR_SALARYSYSTEM")
               where ent.SALARYSOLUTIONNAME.Contains(txtSolution.Text)
               select ent;
            if(q.Count()==1)
            {
                solution = q.FirstOrDefault();
                MessageBox.Show("薪资方案检查正确。");
            }else
            {
                MessageBox.Show("薪资方案不明确，请指定明确的薪资方案名");
            }
        }
        Dictionary<string, string> postlevels = new Dictionary<string, string>();
        T_HR_SALARYSTANDARD standtmp;
        private void button2_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(CreateSalaryStandBatch);
            t.Start();           
        }

        public void CreateSalaryStandBatch()
        {
            try
            {

                EmployeeSalaryRecordBLL bll = new EmployeeSalaryRecordBLL();
                SalaryStandardItemBLL itemBLL = new SalaryStandardItemBLL();

                string strsolutionall = @"上海神达物流有限公司薪资方案（A类城市）
                                上海酷武供应链管理服务有限公司薪资方案（A类）
                                北京市神通物流有限公司薪资方案（A类城市）
                                南京红之衣物流有限公司薪资方案（B类城市）
                                南宁市神州通货运代理有限公司薪资方案（D类城市）
                                南昌神州通物流有限公司薪资方案（E类城市）
                                合肥神州通物流有限公司薪资方案（D类城市）
                                哈尔滨神达通运输有限责任公司薪资方案（D类城市）
                                天津神州通物流有限公司薪资方案（C类城市）
                                太原市神通物流有限公司薪资方案（E类城市）
                                广州市神通物流有限公司薪资方案（A类城市）
                                成都通神州物流有限公司薪资方案（D类城市）
                                昆明神州通物流有限公司薪资方案（D类城市）
                                杭州新神州通货物运输有限公司薪资方案（B类城市）
                                武汉通神州仓储服务有限责任公司薪资方案（C类城市）
                                沈阳神州通物流有限公司薪资方案（C类城市）
                                济南神州通物流有限公司薪资方案（C类城市）
                                深圳市神州通物流有限公司薪资方案（A类城市）
                                深圳市神州通物流有限公司薪资方案（B类城市）
                                深圳市神州通物流有限公司薪资方案（C类城市）
                                深圳市神州通物流有限公司薪资方案（D类城市）
                                深圳市神州通物流有限公司薪资方案（E类城市）
                                深圳市神州通物流有限公司贵阳分公司（E类城市）
                                深圳市通神州物流公司薪资方案（A类城市）
                                石家庄神州通物流有限公司薪资方案（D类城市）
                                福州市神州通货运代理有限公司薪资方案（C类城市）
                                西安市神之通物流有限公司薪资方案（D类城市）
                                贵阳市神州通物流有限公司（E类城市）
                                郑州市神州通物流有限公司薪资方案（D类城市）
                                重庆新神州通物流有限公司薪资方案（D类城市）
                                长春市神之通物流有限公司薪资方案（D类城市）
                                长沙市神通物流服务有限公司薪资方案（C类城市）
                                青海神之通电子商务有限公司薪资方案（E类）";
                var solutionAll = (from ent in dal.GetObjects<T_HR_SALARYSOLUTION>().Include("T_HR_SALARYSYSTEM")
                                   where ent.SALARYSOLUTIONNAME.Contains(strsolutionall)
                                   && ent.CHECKSTATE == "2"
                                   select ent).ToList();

                foreach (var solutionItem in solutionAll)
                {
                    solution = solutionItem;


                    standtmp = new T_HR_SALARYSTANDARD();
                    standtmp.SALARYSTANDARDID = Guid.NewGuid().ToString();
                    standtmp.CREATECOMPANYID = solution.CREATECOMPANYID;
                    standtmp.CREATEDEPARTMENTID = solution.CREATEDEPARTMENTID;
                    standtmp.CREATEPOSTID = solution.CREATEPOSTID;
                    standtmp.CREATEUSERID = solution.CREATEUSERID;

                    standtmp.OWNERCOMPANYID = solution.OWNERCOMPANYID;
                    standtmp.OWNERDEPARTMENTID = solution.OWNERDEPARTMENTID;
                    standtmp.OWNERID = solution.OWNERID;
                    standtmp.OWNERPOSTID = solution.OWNERPOSTID;
                    //loadbar.Start();

                    // 薪资方案的体系ID
                    string salarySystemID = solution.T_HR_SALARYSYSTEM.SALARYSYSTEMID;
                    //薪资体系名
                    string salarySystemName = solution.T_HR_SALARYSYSTEM.SALARYSYSTEMNAME;
                    // 岗位级别名
                    string postLevelName = "";

                    //薪资方案所属的薪资体系
                    List<T_HR_SALARYLEVEL> salarylevelList = new List<T_HR_SALARYLEVEL>();
                    //薪资方案的薪资项集合
                    List<V_SALARYITEM> salaryItems = new List<V_SALARYITEM>();

                    salarylevelList = (from c in dal.GetObjects<T_HR_SALARYLEVEL>().Include("T_HR_POSTLEVELDISTINCTION")
                                       where c.T_HR_POSTLEVELDISTINCTION.T_HR_SALARYSYSTEM.SALARYSYSTEMID == salarySystemID
                                       select c).ToList().OrderBy(m => m.SALARYLEVEL).OrderBy(x => x.T_HR_POSTLEVELDISTINCTION.POSTLEVEL).ToList();

                    salaryItems = (from n in dal.GetObjects<T_HR_SALARYITEM>()
                                   join m in dal.GetObjects<T_HR_SALARYSOLUTIONITEM>() on n.SALARYITEMID equals m.T_HR_SALARYITEM.SALARYITEMID
                                   where m.T_HR_SALARYSOLUTION.SALARYSOLUTIONID == solution.SALARYSOLUTIONID
                                   select new V_SALARYITEM
                                   {
                                       T_HR_SALARYITEM = n,
                                       ORDERNUMBER = m.ORDERNUMBER
                                   }
                                   ).ToList();

                    //方案所属地区差异补贴
                    List<T_HR_AREAALLOWANCE> areaAllowance = new List<T_HR_AREAALLOWANCE>();
                    areaAllowance = (from c in dal.GetObjects<T_HR_AREAALLOWANCE>()
                                     join b in dal.GetObjects<T_HR_SALARYSOLUTION>().Include("T_HR_AREADIFFERENCE") on c.T_HR_AREADIFFERENCE.AREADIFFERENCEID equals b.T_HR_AREADIFFERENCE.AREADIFFERENCEID
                                     where b.SALARYSOLUTIONID == solution.SALARYSOLUTIONID
                                     select c).ToList();

                    int num = 0;

                    //dal.BeginTransaction();
                    foreach (var sl in salarylevelList)
                    {
                        var ent = from c in postlevels
                                  join b in salarylevelList on c.Value equals b.T_HR_POSTLEVELDISTINCTION.POSTLEVEL.ToString()
                                  where b.SALARYLEVELID == sl.SALARYLEVELID
                                  select c.Key;
                        if (ent.Count() > 0)
                        {
                            postLevelName = ent.FirstOrDefault().ToString();
                        }
                        var ents = (from a in dal.GetObjects<T_HR_SALARYSTANDARD>()
                                    where a.T_HR_SALARYLEVEL.SALARYLEVELID == sl.SALARYLEVELID
                                    && a.SALARYSOLUTIONID == solution.SALARYSOLUTIONID
                                    select new
                                    {
                                        a.SALARYSTANDARDID,
                                        a.SALARYSTANDARDNAME,
                                        a.T_HR_SALARYLEVEL.SALARYLEVEL
                                    });
                        if (ents.Count() > 0)
                        {
                           

                            var checkEntity = ents.FirstOrDefault();
                            ShowMessage(txtMsg, "薪资标准已存在，跳过：" + solution.SALARYSOLUTIONNAME + " " + checkEntity.SALARYSTANDARDNAME + System.Environment.NewLine);
                            //已存在，跳过生成
                            continue;
                            //if (int.Parse(checkEntity.SALARYLEVEL) > 40)
                            //{
                            //    try { 
                            //    var delentity = (from a in dal.GetObjects<T_HR_SALARYSTANDARD>().Include("T_HR_SALARYSTANDARDITEM")
                            //                     where a.SALARYSTANDARDID
                            //                     == checkEntity.SALARYSTANDARDID
                            //                     select a).FirstOrDefault();
                            //    foreach (var entity in delentity.T_HR_SALARYSTANDARDITEM)
                            //    {
                            //        dal.Delete(entity);
                            //    }
                            //    dal.Delete(delentity);
                            //    ShowMessage(txtMsg, "删除薪资标准成功！" + solution.SALARYSOLUTIONNAME + " " + checkEntity.SALARYSTANDARDNAME + System.Environment.NewLine);
                            //        }catch(Exception ex)
                            //    {
                            //        ShowMessage(txtMsg, "薪资标准已被使用，跳过：" + solution.SALARYSOLUTIONNAME + " " + checkEntity.SALARYSTANDARDNAME + System.Environment.NewLine);
                            //        continue;
                            //    }
                            //}
                            //else
                            //{
                            //    ShowMessage(txtMsg, "薪资标准已存在，跳过：" + solution.SALARYSOLUTIONNAME + " " + checkEntity.SALARYSTANDARDNAME + System.Environment.NewLine);
                            //    //已存在，跳过生成
                            //    continue;
                            //}
                        }
                        // 薪资体系的每条记录都对应一个标准 根据sl生成薪资标准
                        T_HR_SALARYSTANDARD stand = new T_HR_SALARYSTANDARD();
                        //薪资标准的薪资项集合
                        List<T_HR_SALARYSTANDARDITEM> standSalaryitems = new List<T_HR_SALARYSTANDARDITEM>();
                        //获取岗位级别名


                        //薪资标准名= 薪资体系名+岗位级别名+"-"+薪资级别名
                        stand.SALARYSTANDARDNAME = salarySystemName + postLevelName + "-" + sl.SALARYLEVEL.ToString();
                        stand.SALARYSTANDARDID = Guid.NewGuid().ToString();
                        // stand.T_HR_SALARYLEVEL = new T_HR_SALARYLEVEL();
                        stand.T_HR_SALARYLEVELReference.EntityKey
                              = new System.Data.EntityKey(qualifiedEntitySetName + "T_HR_SALARYLEVEL", "SALARYLEVELID", sl.SALARYLEVELID);
                        //  stand.T_HR_SALARYLEVEL.SALARYLEVELID = sl.SALARYLEVELID;
                        stand.SALARYSOLUTIONID = solution.SALARYSOLUTIONID;
                        stand.CHECKSTATE = "2";
                        stand.BASESALARY = sl.SALARYSUM;
                        stand.CREATEDATE = System.DateTime.Now;

                        //有关权限的字段
                        stand.CREATECOMPANYID = standtmp.CREATECOMPANYID;
                        stand.CREATEDEPARTMENTID = standtmp.CREATEDEPARTMENTID;
                        stand.CREATEPOSTID = standtmp.CREATEPOSTID;
                        stand.CREATEUSERID = standtmp.CREATEUSERID;
                        stand.OWNERCOMPANYID = standtmp.OWNERCOMPANYID;
                        stand.OWNERDEPARTMENTID = standtmp.OWNERDEPARTMENTID;
                        stand.OWNERID = standtmp.OWNERID;
                        stand.OWNERPOSTID = standtmp.OWNERPOSTID;


                        //增加排序号
                        stand.PERSONALSIRATIO = num;
                        num++;


                        //标准对应的地区差异补贴（和岗位级别有关）
                        decimal? allowance = 0;
                        if (areaAllowance != null)
                        {
                            allowance = (from al in areaAllowance
                                         where al.POSTLEVEL == sl.T_HR_POSTLEVELDISTINCTION.POSTLEVEL.ToString()
                                         select al.ALLOWANCE).FirstOrDefault();
                        }
                        //计算标准的基础数据
                        decimal? BasicData = sl.SALARYSUM;

                        //按照方案的薪资项集合生成薪资标准的薪资项
                        foreach (var item in salaryItems)
                        {
                            T_HR_SALARYSTANDARDITEM standItem = new T_HR_SALARYSTANDARDITEM();
                            standItem.STANDRECORDITEMID = Guid.NewGuid().ToString();
                            standItem.T_HR_SALARYITEMReference.EntityKey
                                = new System.Data.EntityKey(qualifiedEntitySetName + "T_HR_SALARYITEM", "SALARYITEMID", item.T_HR_SALARYITEM.SALARYITEMID);
                            // standItem.T_HR_SALARYITEM.SALARYITEMID = item.T_HR_SALARYITEM.SALARYITEMID;
                            standItem.CREATEUSERID = solution.CREATEUSERID;
                            standItem.T_HR_SALARYSTANDARDReference.EntityKey
                                = new System.Data.EntityKey(qualifiedEntitySetName + "T_HR_SALARYSTANDARD", "SALARYSTANDARDID", stand.SALARYSTANDARDID);
                            // standItem.T_HR_SALARYSTANDARD.SALARYSTANDARDID = stand.SALARYSTANDARDID;
                            standItem.CREATEDATE = System.DateTime.Now;
                            standItem.ORDERNUMBER = item.ORDERNUMBER;
                            //计算类型是手动输入的 金额是薪资项设置是输入的值
                            if (item.T_HR_SALARYITEM.CALCULATORTYPE == "1" || (item.T_HR_SALARYITEM.CALCULATORTYPE == "4" && item.T_HR_SALARYITEM.GUERDONSUM != null))
                            {
                                standItem.SUM = item.T_HR_SALARYITEM.GUERDONSUM.ToString();
                            }
                            //计算类型是公式计算 而且不是在生成薪资时计算  
                            else if (item.T_HR_SALARYITEM.CALCULATORTYPE == "3" && item.T_HR_SALARYITEM.ISAUTOGENERATE == "0")
                            {
                                standItem.SUM = bll.AutoCalculateItem(item.T_HR_SALARYITEM.SALARYITEMID, Convert.ToDecimal(BasicData), allowance.ToString()).ToString();
                            }
                            //地区差异补贴
                            else if (item.T_HR_SALARYITEM.ENTITYCOLUMNCODE == "AREADIFALLOWANCE")
                            {
                                standItem.SUM = allowance.ToString();
                            }
                            standSalaryitems.Add(standItem);
                        }
                        //SalaryStandardAdd(stand);
                        //itemBLL.SalaryStandardItemsAdd(standSalaryitems);
                        AddSalaryStanderAndItems(stand, standSalaryitems);
                        ShowMessage(txtMsg, "生成薪资标准：" + solution.SALARYSOLUTIONNAME + " " + stand.SALARYSTANDARDNAME + "成功！" + System.Environment.NewLine);
                    }
                }
                //dal.CommitTransaction();
                ShowMessage(txtMsg, "生成所有薪资标准成功！" + System.Environment.NewLine);
                MessageBox.Show("生成所有薪资标准成功！");
            }
            catch (Exception ex)
            {
                //dal.RollbackTransaction();
                MessageBox.Show("生成失败，错误信息：" + ex.ToString());
                SMT.Foundation.Log.Tracer.Debug(ex.Message);
            }
        }
        public string AddSalaryStanderAndItems(T_HR_SALARYSTANDARD stand, List<T_HR_SALARYSTANDARDITEM> salaryItems)
        {
            try
            {
                salaryItems.ForEach(item =>
                {

                    stand.T_HR_SALARYSTANDARDITEM.Add(item);
                    Utility.RefreshEntity(item);
                });
                dal.Add(stand);

                return "";
            }
            catch (Exception ex)
            {
                SMT.Foundation.Log.Tracer.Debug(ex.Message);
                dal.RollbackTransaction();
                return "Error";
            }
        }

    }
    public class V_SALARYITEM
    {
        public T_HR_SALARYITEM T_HR_SALARYITEM { get; set; }
        public decimal? ORDERNUMBER { get; set; }
    }
}
