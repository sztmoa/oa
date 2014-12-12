using SMT_OA_EFModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SMT.AttendanceLogs.UI
{
    public partial class FormTravelSolution : Form
    {
        bool checke1 = false;
        bool checke2 = false;
        private SMT_OA_EFModelContext dal;
        public FormTravelSolution()
        {
            InitializeComponent();
            dal = new SMT_OA_EFModelContext();
        }
        System.Data.Common.DbTransaction tran = null;
        private void BtnStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (!checke1 || !checke2)
                {
                    MessageBox.Show("请先检查出差方案，请重试");
                    return;
                }
                  var dr = MessageBox.Show("确认开始吗？请提前备份好数据再开始", "确认", MessageBoxButtons.YesNo);
                  if (dr == DialogResult.Yes)
                  {
                      if (dal.Connection.State == System.Data.ConnectionState.Closed)
                      {
                          dal.Connection.Open();
                      }
                      tran = dal.Connection.BeginTransaction();
                      string strSourceSolutionId = labelFromSolutionId.Text;
                      string strToSolutionId = labelToSolutionID.Text;

                      var areaAll = (from ent in dal.T_OA_AREADIFFERENCE.Include("T_OA_TRAVELSOLUTIONS")
                                     where ent.T_OA_TRAVELSOLUTIONS.TRAVELSOLUTIONSID == strSourceSolutionId//神州通集团出差方案
                                     select ent).ToList();
                      var solutionitem = (from ent in dal.T_OA_TRAVELSOLUTIONS
                                          where ent.TRAVELSOLUTIONSID == strToSolutionId
                                          select ent).FirstOrDefault();
                      //复制城市分类
                      foreach (var area in areaAll)
                      {
                          StarCopy(solutionitem, area, strSourceSolutionId);
                      }

                      dal.SaveChanges();
                      tran.Commit();
                  }
            }
            catch (Exception ex)
            {
                tran.Rollback();
                MessageBox.Show(ex.ToString());

            }
        }

        private void BtnCheck1_Click(object sender, EventArgs e)
        {
            var ent = (from a in dal.T_OA_TRAVELSOLUTIONS
                      where a.PROGRAMMENAME==(txtsourceSolution.Text)
                      select a).FirstOrDefault();
            if(ent!=null)
            {
                labelFromSolutionId.Text = ent.TRAVELSOLUTIONSID;
                checke1 = true;
            }else
            {
                MessageBox.Show("未找到出差方案，请重试");
                checke1 = false;
            }
        }

        private void BtnCheck2_Click(object sender, EventArgs e)
        {
            var ent = (from a in dal.T_OA_TRAVELSOLUTIONS
                       where a.PROGRAMMENAME==(TxtSolution.Text)
                       select a).FirstOrDefault();
            if (ent != null)
            {
                labelToSolutionID.Text = ent.TRAVELSOLUTIONSID;
                checke2 = true;
            }
            else
            {
                MessageBox.Show("未找到出差方案，请重试");
                checke2 = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtSolutionAll.Text = string.Empty;
            var ent = from a in dal.T_OA_TRAVELSOLUTIONS
                      select a;
            if (ent.Count()>0)
            {
                foreach(var item in ent)
                {
                    txtSolutionAll.Text += item.PROGRAMMENAME + System.Environment.NewLine;
                }
            }
        }

        private void btnCopyAll_Click(object sender, EventArgs e)
        {

            try
            {
                var dr = MessageBox.Show("确认开始吗？请提前备份好数据再开始", "确认", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    string strSourceSolutionId = labelFromSolutionId.Text;
                    if (dal.Connection.State == System.Data.ConnectionState.Closed)
                    {
                        dal.Connection.Open();
                    }
                    tran = dal.Connection.BeginTransaction();
                    //string strToSolutionId = labelToSolutionID.Text;

                    var areaAll = (from ent in dal.T_OA_AREADIFFERENCE.Include("T_OA_TRAVELSOLUTIONS")
                                   where ent.T_OA_TRAVELSOLUTIONS.TRAVELSOLUTIONSID == strSourceSolutionId//神州通集团出差方案
                                   select ent).ToList();
                    var solution = (from ent in dal.T_OA_TRAVELSOLUTIONS
                                    where ent.TRAVELSOLUTIONSID != strSourceSolutionId
                                    select ent).ToList();
                    foreach (var solutionitem in solution)
                    {
                        //复制城市分类
                        foreach (var area in areaAll)
                        {
                            StarCopy(solutionitem, area, strSourceSolutionId);
                        }
                    }
                    dal.SaveChanges();
                    tran.Commit();
                    MessageBox.Show("同步成功 ！");
                }
            }
            catch (Exception ex)
            {
                tran.Rollback();
                MessageBox.Show(ex.ToString());

            }
        }



        private void StarCopy(T_OA_TRAVELSOLUTIONS solutionitem, T_OA_AREADIFFERENCE area, string strSourceSolutionId)
        {
            var checkArea = (from ent in dal.T_OA_AREADIFFERENCE
                             where ent.T_OA_TRAVELSOLUTIONS.TRAVELSOLUTIONSID == solutionitem.TRAVELSOLUTIONSID
                             && ent.AREACATEGORY == area.AREACATEGORY + "(" + solutionitem.PROGRAMMENAME + ")"
                             select ent).FirstOrDefault();
            //复制城市分类
            T_OA_AREADIFFERENCE areaNew = new T_OA_AREADIFFERENCE();
            if (checkArea != null)
            {
                areaNew = checkArea;
            }
            else
            {
                Utility.CloneEntity(area, areaNew);

                areaNew.T_OA_TRAVELSOLUTIONSReference.EntityKey =
                   new System.Data.EntityKey("SMT_OA_EFModelContext.T_OA_TRAVELSOLUTIONS", "TRAVELSOLUTIONSID", solutionitem.TRAVELSOLUTIONSID);
                areaNew.AREACATEGORY = area.AREACATEGORY + "(" + solutionitem.PROGRAMMENAME + ")";
                areaNew.AREADIFFERENCEID = Guid.NewGuid().ToString();
                areaNew.CREATEDATE = DateTime.Now;
                areaNew.OWNERCOMPANYID = solutionitem.OWNERCOMPANYID;
                areaNew.CREATECOMPANYID = solutionitem.OWNERCOMPANYID;
                areaNew.CREATEUSERID = "系统复制";
                areaNew.CREATEUSERNAME = "系统复制";
                dal.AddToT_OA_AREADIFFERENCE(areaNew);//添加城市分类
                dal.SaveChanges();
            }
            //1复制城市分类关联的城市
            var cityall = (from ent in dal.T_OA_AREACITY
                           where ent.T_OA_AREADIFFERENCE.AREADIFFERENCEID == area.AREADIFFERENCEID
                           select ent).ToList();
            foreach (var city in cityall)
            {
                var checkCity = (from ent in dal.T_OA_AREACITY
                                 where ent.T_OA_AREADIFFERENCE.AREADIFFERENCEID == areaNew.AREADIFFERENCEID
                                 && ent.CITY == city.CITY
                                 select ent).FirstOrDefault();
                T_OA_AREACITY citynew = new T_OA_AREACITY();
                if (checkCity != null)
                {
                    citynew = checkCity;
                }
                else
                {
                    Utility.CloneEntity(city, citynew);
                    citynew.AREACITYID = Guid.NewGuid().ToString();
                    citynew.T_OA_AREADIFFERENCEReference.EntityKey =
                   new System.Data.EntityKey("SMT_OA_EFModelContext.T_OA_AREADIFFERENCE", "AREADIFFERENCEID", areaNew.AREADIFFERENCEID);
                    citynew.CREATEUSERID = "系统复制";
                    dal.AddToT_OA_AREACITY(citynew);//添加城市     
                }
            }


            //2.修改补帖分类 查找集团所有补贴
            var allowanceOldAll = (from ent in dal.T_OA_TRAVELSOLUTIONS 
                                   join b in dal.T_OA_AREADIFFERENCE on ent.TRAVELSOLUTIONSID equals b.T_OA_TRAVELSOLUTIONS.TRAVELSOLUTIONSID
                                   join c in dal.T_OA_AREAALLOWANCE on b.AREADIFFERENCEID equals c.T_OA_AREADIFFERENCE.AREADIFFERENCEID
                                   where ent.TRAVELSOLUTIONSID== strSourceSolutionId
                                && b.AREADIFFERENCEID == area.AREADIFFERENCEID
                                   select c).ToList();
            if (allowanceOldAll != null)
            {
                foreach (var allowanceOld in allowanceOldAll)
                {
                    var checkEnt = (from ent in dal.T_OA_AREAALLOWANCE
                                    where ent.POSTLEVEL == allowanceOld.POSTLEVEL
                                    && ent.T_OA_TRAVELSOLUTIONS.TRAVELSOLUTIONSID
                                    == solutionitem.TRAVELSOLUTIONSID
                                    && ent.T_OA_AREADIFFERENCE.AREADIFFERENCEID == areaNew.AREADIFFERENCEID
                                    select ent).FirstOrDefault();
                    if (checkEnt != null)//如果已经设置，直接拷贝并删除旧的
                    {
                        T_OA_AREAALLOWANCE allowanceNew = new T_OA_AREAALLOWANCE();
                        Utility.CloneEntity(checkEnt, allowanceNew);
                        allowanceNew.AREAALLOWANCEID = Guid.NewGuid().ToString();
                        allowanceNew.T_OA_TRAVELSOLUTIONSReference.EntityKey =
                        new System.Data.EntityKey("SMT_OA_EFModelContext.T_OA_TRAVELSOLUTIONS", "TRAVELSOLUTIONSID", solutionitem.TRAVELSOLUTIONSID);
                        allowanceNew.T_OA_AREADIFFERENCEReference.EntityKey =
                        new System.Data.EntityKey("SMT_OA_EFModelContext.T_OA_AREADIFFERENCE", "AREADIFFERENCEID", areaNew.AREADIFFERENCEID);
                        allowanceNew.CREATEDATE = DateTime.Now;
                        allowanceNew.CREATEUSERID = "系统复制";
                        dal.AddToT_OA_AREAALLOWANCE(allowanceNew);
                        dal.DeleteObject(checkEnt);
                    }
                    else
                    {
                        //如果没有补贴且集团已设置，拷贝
                        T_OA_AREAALLOWANCE allowanceNew = new T_OA_AREAALLOWANCE();
                        Utility.CloneEntity(allowanceOld, allowanceNew);
                        allowanceNew.AREAALLOWANCEID = Guid.NewGuid().ToString();
                        allowanceNew.T_OA_TRAVELSOLUTIONSReference.EntityKey =
                      new System.Data.EntityKey("SMT_OA_EFModelContext.T_OA_TRAVELSOLUTIONS", "TRAVELSOLUTIONSID", solutionitem.TRAVELSOLUTIONSID);
                        allowanceNew.T_OA_AREADIFFERENCEReference.EntityKey =
                    new System.Data.EntityKey("SMT_OA_EFModelContext.T_OA_AREADIFFERENCE", "AREADIFFERENCEID", areaNew.AREADIFFERENCEID);
                        allowanceNew.CREATEDATE = DateTime.Now;
                        allowanceNew.CREATEUSERID = "系统复制集团方案";
                        dal.AddToT_OA_AREAALLOWANCE(allowanceNew);
                    }
                }
            }
        }

    }
}
