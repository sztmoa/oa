
/*
 * 文件名：ChargeApplyMasterBLL.cs
 * 作  用：T_FB_CHARGEAPPLYMASTER 业务逻辑类
 * 创建人：吴鹏
 * 创建时间：2010-12-15 11:47:04
 * 修改人：
 * 修改时间：
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;
using System.Data;
using System.Data.Objects.DataClasses;
using System.Linq.Dynamic;
using System.Linq.Expressions;

using TM_SaaS_OA_EFModel;
using SMT.FBAnalysis.DAL;
using SMT.FBAnalysis.CustomModel;
using SMT.Foundation.Log;
using SMT.SaaS.BLLCommonServices.FlowWFService;
using SMT.SaaS.BLLCommonServices.OrganizationWS;
using SMT.SaaS.BLLCommonServices.PersonnelWS;
using System.Configuration;


namespace SMT.FBAnalysis.BLL
{
    public class ChargeApplyMasterBLL : BaseBll<T_FB_CHARGEAPPLYMASTER>
    {
        public ChargeApplyMasterBLL()
        { }

        #region 检查记录是否存在

        /// <summary>
        /// 根据参数，检查是否存在指定记录(方便进行数据增删改)
        /// </summary>
        /// <param name="strFilter">查询语句</param>
        /// <param name="objArgs">查询参数集合</param>
        /// <returns>True/False(是/否)</returns>
        public bool IsExistsRd(string strFilter, params object[] objArgs)
        {
            bool flag = false;

            var q = from v in dal.GetObjects()
                    select v;

            if (objArgs.Count() <= 0 || string.IsNullOrEmpty(strFilter))
            {
                return flag;
            }

            q = q.Where(strFilter, objArgs);

            if (q.Count() > 0)
            {
                flag = true;
            }

            return flag;
        }
        #endregion

        #region 获取指定的T_FB_CHARGEAPPLYMASTER信息


        /// <summary>
        /// 获取指定条件的T_FB_CHARGEAPPLYMASTER信息
        /// </summary>        
        /// <param name="strFilter">查询语句</param>
        /// <param name="objArgs">查询参数集合</param>
        /// <returns>返回T_FB_CHARGEAPPLYMASTER信息</returns>
        public T_FB_CHARGEAPPLYMASTER GetChargeApplyMasterRdByMultSearch(string strFilter, params object[] objArgs)
        {
            try
            {
                var q = from v in dal.GetObjects()
                        select v;

                if (objArgs.Count() > 0 && !string.IsNullOrEmpty(strFilter))
                {
                    q = q.Where(strFilter, objArgs);
                }

                return q.First();
            }
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：GetChargeApplyMasterRdByMultSearch，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
            }
            return null;
        }
        #endregion

        #region 查询申请的费用


        /// <summary>
        /// 查询申请的费用申请。
        /// </summary>
        /// <param name="conditions">查询条件对象集。</param>
        /// <returns>返回申请的费用申请。</returns>
        public IQueryable<V_Money> GetChargeApplyBll(ExecutionConditions conditions)
        {
            try
            {
                var a = from b in dal.GetObjects<T_FB_CHARGEAPPLYDETAIL>().Include("T_FB_CHARGEAPPLYMASTER")
                        join c in dal.GetObjects<T_FB_SUBJECTCOMPANY>().Include("T_FB_SUBJECT") on b.T_FB_SUBJECT.SUBJECTID equals c.T_FB_SUBJECT.SUBJECTID
                        where b.T_FB_CHARGEAPPLYMASTER.CHECKSTATES == conditions.CheckStates
                        select b;

                #region 添加查询条件

                // 起止时间
                if (conditions.DateFrom != null && conditions.DateTo != null)
                {
                    a = a.Where(b => b.T_FB_CHARGEAPPLYMASTER.BUDGETARYMONTH >= conditions.DateFrom && b.T_FB_CHARGEAPPLYMASTER.BUDGETARYMONTH <= conditions.DateTo);
                }
                // 科目
                if (conditions.SubjectID != string.Empty)
                {
                    string strTempString = "T_FB_SUBJECT.SUBJECTID==@0 ";
                    List<object> objs = new List<object>();
                    objs.Add(conditions.SubjectID);
                    a = a.Where(strTempString, objs.ToArray());
                }
                // 机构
                if (conditions.OrgnizationType != -1)
                {
                    string strTempString = "";
                    List<object> objs = new List<object>();

                    if (conditions.OrgnizationType == 0)
                    {
                        strTempString = "T_FB_CHARGEAPPLYMASTER.OWNERCOMPANYID==@0 ";
                        objs.Add(conditions.OrgnizationID);
                    }
                    else if (conditions.OrgnizationType == 1)
                    {
                        strTempString = "T_FB_CHARGEAPPLYMASTER.OWNERDEPARTMENTID==@0 ";
                        objs.Add(conditions.OrgnizationID);
                    }
                    else if (conditions.OrgnizationType == 2)
                    {
                        strTempString = "T_FB_CHARGEAPPLYMASTER.OWNERPOSTID==@0 ";
                        objs.Add(conditions.OrgnizationID);
                    }
                    else if (conditions.OrgnizationType == 3)
                    {
                        strTempString = "T_FB_CHARGEAPPLYMASTER.OWNERID==@0 ";
                        objs.Add(conditions.OwnerID);
                    }

                    a = a.Where(strTempString, objs.ToArray());
                }
                #endregion 添加查询条件

                var v = from u in a
                        select new V_Money
                        {
                            Money = u.T_FB_CHARGEAPPLYMASTER.TOTALMONEY
                        };

                return v;
            }
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：GetChargeApplyBll，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
            }
            return null;
        }
        #endregion

        #region 查询费用申请列表


        /// <summary>
        /// 查询费用申请列表。
        /// </summary>
        /// <param name="conditions">查询条件对象集。</param>
        /// <returns>返回费用申请列表。</returns>
        public IQueryable<V_ChargeList> GetChargeApplyList(ExecutionConditions conditions)
        {
            try
            {
                var a = from b in dal.GetObjects<T_FB_CHARGEAPPLYDETAIL>().Include("T_FB_CHARGEAPPLYMASTER")
                        join c in dal.GetObjects<T_FB_SUBJECTCOMPANY>().Include("T_FB_SUBJECT") on b.T_FB_SUBJECT.SUBJECTID equals c.T_FB_SUBJECT.SUBJECTID
                        where b.T_FB_CHARGEAPPLYMASTER.CHECKSTATES == conditions.CheckStates
                        select b;

                #region 添加查询条件

                // 起止时间
                if (conditions.DateFrom != null && conditions.DateTo != null)
                {
                    try
                    {
                        a = a.Where(b => b.T_FB_CHARGEAPPLYMASTER.BUDGETARYMONTH >= conditions.DateFrom && b.T_FB_CHARGEAPPLYMASTER.BUDGETARYMONTH <= conditions.DateTo);
                    }
                    catch (Exception e)
                    {
                        Tracer.Debug(e.InnerException.Message);
                    }
                }

                // 科目
                if (conditions.SubjectID != string.Empty)
                {
                    string strTempString = "T_FB_SUBJECT.SUBJECTID==@0 ";
                    List<object> objs = new List<object>();
                    objs.Add(conditions.SubjectID);
                    try
                    {
                        a = a.Where(strTempString, objs.ToArray());
                    }
                    catch (Exception e)
                    {
                        Tracer.Debug(e.InnerException.Message);
                    }
                }

                // 机构
                if (conditions.OrgnizationType != -1)
                {
                    string strTempString = "";
                    List<object> objs = new List<object>();

                    if (conditions.OrgnizationType == 0)
                    {
                        strTempString = "T_FB_CHARGEAPPLYMASTER.OWNERCOMPANYID==@0 ";
                        objs.Add(conditions.OrgnizationID);
                    }
                    else if (conditions.OrgnizationType == 1)
                    {
                        strTempString = "T_FB_CHARGEAPPLYMASTER.OWNERDEPARTMENTID==@0 ";
                        objs.Add(conditions.OrgnizationID);
                    }
                    else if (conditions.OrgnizationType == 2)
                    {
                        strTempString = "T_FB_CHARGEAPPLYMASTER.OWNERPOSTID==@0 ";
                        objs.Add(conditions.OrgnizationID);
                    }
                    else if (conditions.OrgnizationType == 3)
                    {
                        strTempString = "T_FB_CHARGEAPPLYMASTER.OWNERID==@0 ";
                        objs.Add(conditions.CurrentOnlineUser);
                    }

                    a = a.Where(strTempString, objs.ToArray());
                }

                var t = from u in a
                        select new V_ChargeList
                        {
                            ID = u.T_FB_CHARGEAPPLYMASTER.CHARGEAPPLYMASTERCODE,
                            SubjectID = u.T_FB_SUBJECT.SUBJECTID,
                            Type = u.T_FB_CHARGEAPPLYMASTER.PAYTYPE,
                            SubjectName = u.T_FB_SUBJECT.SUBJECTNAME,
                            CreateDate = u.T_FB_CHARGEAPPLYMASTER.CREATEDATE,
                            DeptmentID = u.T_FB_CHARGEAPPLYMASTER.OWNERDEPARTMENTID,
                            DeptmentName = u.T_FB_CHARGEAPPLYMASTER.OWNERDEPARTMENTNAME,
                            CreateUserID = u.T_FB_CHARGEAPPLYMASTER.CREATEUSERID,
                            CreateUserName = u.T_FB_CHARGEAPPLYMASTER.CREATEUSERNAME,
                            TotalMoney = u.T_FB_CHARGEAPPLYMASTER.TOTALMONEY,
                            BudgetaryMonth = u.T_FB_CHARGEAPPLYMASTER.BUDGETARYMONTH,
                            ChargeType = u.CHARGETYPE.Value,
                            OperateType = "费用申请"
                        };

                return t;

                #endregion 添加查询条件
            }
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：GetChargeApplyList，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
            }
            return null;
        }

        #endregion

        #region 获取数据

        /// <summary>
        /// 获取T_FB_CHARGEAPPLYMASTER信息
        /// </summary>
        /// <param name="strChargeApplyMasterId">主键索引</param>
        /// <returns></returns>
        public T_FB_CHARGEAPPLYMASTER GetChargeApplyMasterByID(string strChargeApplyMasterId)
        {
            if (string.IsNullOrEmpty(strChargeApplyMasterId))
            {
                return null;
            }

            var ids = strChargeApplyMasterId.Split(':');
            var id = ids[0];
            if (ids.Length > 1)
            {
                return GetMasterForEdit(id);
            }
            else
            {
                return InnerGetChargeApplyMasterByID(id);;
            }           
           
          
        }

        /// <summary>
        /// 获取T_FB_CHARGEAPPLYMASTER信息
        /// </summary>
        /// <param name="strChargeApplyMasterId">主键索引</param>
        /// <returns></returns>
        public T_FB_CHARGEAPPLYMASTER InnerGetChargeApplyMasterByID(string strChargeApplyMasterId)
        {
            ChargeApplyMasterDAL dalChargeApplyMaster = new ChargeApplyMasterDAL();
            StringBuilder strFilter = new StringBuilder();
            List<string> objArgs = new List<string>();

            if (!string.IsNullOrEmpty(strChargeApplyMasterId))
            {
                strFilter.Append(" CHARGEAPPLYMASTERID == @0");
                objArgs.Add(strChargeApplyMasterId);
            }

            try
            {
                T_FB_CHARGEAPPLYMASTER entRd = dalChargeApplyMaster.GetChargeApplyMasterRdByMultSearch(strFilter.ToString(), objArgs.ToArray());
                return entRd;
            }
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：GetChargeApplyMasterByID，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
            }
            return null;
        }

        /// <summary>
        /// 根据条件，获取费用报销信息
        /// </summary>
        /// <param name="strOwnerID"></param>
        /// <param name="strDateStart"></param>
        /// <param name="strDateEnd"></param>
        /// <param name="strCheckState"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <param name="strSortKey"></param>
        /// <returns></returns>
        public static IQueryable<T_FB_CHARGEAPPLYMASTER> GetAllChargeApplyMasterRdListByMultSearch(string strOwnerID, string strDateStart,
            string strDateEnd, string strCheckState, string strFilter, List<object> objArgs, string strSortKey)
        {
            try
            {
                ChargeApplyMasterDAL dalCharge = new ChargeApplyMasterDAL();
                string strOrderBy = string.Empty;

                if (string.IsNullOrWhiteSpace(strOwnerID) || string.IsNullOrWhiteSpace(strCheckState))
                {
                    return null;
                }

                if (!string.IsNullOrEmpty(strSortKey))
                {
                    strOrderBy = strSortKey;
                }
                else
                {
                    strOrderBy = " CHARGEAPPLYMASTERID ";
                }

                SMT.SaaS.BLLCommonServices.Utility ulFoo = new SaaS.BLLCommonServices.Utility();
                if (strCheckState != Convert.ToInt32(FBAEnums.CheckStates.WaittingApproval).ToString())
                {
                    ulFoo.SetOrganizationFilter(ref strFilter, ref objArgs, strOwnerID, "T_FB_CHARGEAPPLYMASTER");
                }
                else
                {
                    string StrOld = "";
                    StrOld = strFilter;//将过滤前的字符串付给再比较
                    ulFoo.SetFilterWithflow("CHARGEAPPLYMASTERID", "T_FB_CHARGEAPPLYMASTER", strOwnerID, ref strCheckState, ref strFilter, ref objArgs);
                    if (StrOld.Equals(strFilter))
                        return null;

                    strCheckState = Convert.ToInt32(FBAEnums.CheckStates.Approving).ToString();
                }

                if (strCheckState == Convert.ToInt32(FBAEnums.CheckStates.All).ToString())
                {
                    strCheckState = string.Empty;
                }

                var ents = dalCharge.GetChargeApplyMasterRdListByMultSearch(strCheckState, strDateStart, strDateEnd, strOrderBy, strFilter, objArgs.ToArray());
                return ents;
            }
            catch (Exception ex)
            {
                Utility.SaveLog(DateTime.Now.ToString() + "调用函数GetAllChargeApplyMasterRdListByMultSearch出错， 查询人的员工ID为：" + strOwnerID + "，错误信息为：" + ex.ToString());
            }

            return null;
        }
        #region 导出报表
        /// <summary>
        /// 根据条件，获取费用报销信息
        /// </summary>
        /// <param name="strOwnerID"></param>
        /// <param name="strDateStart"></param>
        /// <param name="strDateEnd"></param>
        /// <param name="strCheckState"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <param name="strSortKey"></param>
        /// <returns></returns>
        public List<V_ChargeApplyReport> ChargeApplyMasterRdListByMultSearchReports(string strOwnerID, string strDateStart, string strDateEnd, string strCheckState, string strFilter, List<object> objArgs, string strSortKey)
        {

            //byte[] result = null;
            List<V_ChargeApplyReport> entlist = new List<V_ChargeApplyReport>();
            try
            {
                PersonnelServiceClient personnelService=new PersonnelServiceClient();
                SMT.SaaS.BLLCommonServices.PersonnelWS.T_HR_EMPLOYEE employee=null;
                V_ChargeApplyReport vChargeApply = null;
                IQueryable<T_FB_CHARGEAPPLYMASTER> chargeApplyMasterRdInfos = GetAllChargeApplyMasterRdListByMultSearch(strOwnerID,strDateStart,strDateEnd,strCheckState,strFilter,objArgs,strSortKey);
                if (chargeApplyMasterRdInfos.Count() > 0)
                {
                    foreach (var changeApplyMaster in chargeApplyMasterRdInfos)
                    {
                        employee = personnelService.GetEmployeeByID(changeApplyMaster.OWNERID.ToString());
                        vChargeApply = new V_ChargeApplyReport();
                        vChargeApply.CHARGEAPPLYMASTERCODE=changeApplyMaster.CHARGEAPPLYMASTERCODE;
                        vChargeApply.BANKCARDNUMBER=employee.BANKCARDNUMBER;
                        vChargeApply.EMPLOYEECNAME=employee.EMPLOYEECNAME;
                        vChargeApply.TOTALMONEY = changeApplyMaster.TOTALMONEY.ToString();
                        vChargeApply.BANKID = employee.BANKID;
                        vChargeApply.BANKADDRESS=string.Empty;
                        entlist.Add(vChargeApply);
                    }
                }
            }
            catch (Exception ex)
            {
                SMT.Foundation.Log.Tracer.Debug("ExportEmployeeLeftOfficeConfirmReports:" + ex.Message);

            }
            return entlist;
        }

        public static byte[] ChargeApplyMasterRdListByMultSearchStream(List<T_FB_CHARGEAPPLYMASTER> chargeApplyMasterRdInfos)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Utility.GetHeader().ToString());
            sb.Append(GetChargeApplyMasterRdListBody(chargeApplyMasterRdInfos).ToString());
            byte[] by = Encoding.UTF8.GetBytes(sb.ToString());

            return by;
        }

        public static StringBuilder GetChargeApplyMasterRdListBody(List<T_FB_CHARGEAPPLYMASTER> Collects)
        {
            StringBuilder s = new StringBuilder();
            PersonnelServiceClient personnelService=new PersonnelServiceClient();
            SMT.SaaS.BLLCommonServices.PersonnelWS.T_HR_EMPLOYEE employee=null;
            //var tmp = new SaaS.BLLCommonServices.PermissionWS.PermissionServiceClient().GetSysDictionaryByCategoryList(new string[] { "CHECKSTATE" });
            //string checkStateName = string.Empty;
            s.Append("<body>\n\r");
            s.Append("<table border=1 cellspacing=0 CELLPADDING=3 width=100% align=center>");
            s.Append("<tr>");
            s.Append("<td align=center class=\"title\" >单据编号</td>");
            s.Append("<td align=center class=\"title\" >帐号</td>");
            s.Append("<td align=center class=\"title\" >户名</td>");
            s.Append("<td align=center class=\"title\" >金额</td>");
            s.Append("<td align=center class=\"title\" >开户行</td>");
            s.Append("<td align=center class=\"title\" >开户地</td>");
            s.Append("</tr>");

            if (Collects.Count() > 0)
            {
                for (int i = 0; i < Collects.Count; i++)
                {
                    employee = personnelService.GetEmployeeByID(Collects[i].OWNERID.ToString());
                    s.Append("<tr>");
                    s.Append("<td class=\"x1282\">" + Collects[i].CHARGEAPPLYMASTERCODE + "</td>");
                    s.Append("<td class=\"x1282\">" + employee.BANKCARDNUMBER + "</td>");
                    s.Append("<td class=\"x1282\">" + employee.EMPLOYEECNAME + "</td>");
                    s.Append("<td class=\"x1282\">" + Collects[i].TOTALMONEY + "</td>");//金额
                    s.Append("<td class=\"x1282\">" + employee.BANKID + "</td>");
                    s.Append("<td class=\"x1282\">" + string.Empty + "</td>");
                    s.Append("</tr>");
                }
            }
            s.Append("</table>");
            s.Append("</body></html>");
            return s;
        }
        /// <summary>
        /// 费用报销Excel的员工导入更新
        /// </summary>
        /// <param name="strCreateUserID">创建人</param>
        /// <param name="strPhysicalPath">当前上传的Excel文件路径</param>
        /// <param name="strMsg">处理返回结果后的消息</param>
        /// <returns>返回数据列表</returns>
        public List<V_ChargeApplyReport> ImportChargeApplyByImportExcel(string strCreateUserID, string strPhysicalPath, ref string strMsg)
        {
            List<V_ChargeApplyReport> vChargeApplyReports = new List<V_ChargeApplyReport>();
            V_ChargeApplyReport vChargeApplyReport = null;
            try
            {

                Microsoft.VisualBasic.FileIO.TextFieldParser TF = new Microsoft.VisualBasic.FileIO.TextFieldParser(strPhysicalPath, Encoding.GetEncoding("GB2312"));

                TF.Delimiters = new string[] { "	" }; //设置分隔符
                string[] strLine;
                int i = 0;   //第一行标头排除
                while (!TF.EndOfData)
                {
                    try
                    {
                        vChargeApplyReport = new V_ChargeApplyReport();
                        strLine = TF.ReadFields();
                        vChargeApplyReport.CHARGEAPPLYMASTERCODE = strLine[0];
                        vChargeApplyReport.BANKCARDNUMBER = strLine[1];
                        vChargeApplyReport.EMPLOYEECNAME = strLine[2];
                        vChargeApplyReport.TOTALMONEY = strLine[3];
                        vChargeApplyReport.BANKID = strLine[4];
                        vChargeApplyReport.BANKADDRESS = strLine[5];
                        //审核通过、
                        var changeApplyMaster = (from e in dal.GetObjects()
                                                 where e.CHARGEAPPLYMASTERCODE == vChargeApplyReport.CHARGEAPPLYMASTERCODE.Trim() && e.CHECKSTATES == 2 && (e.PAYTYPE == 1 || e.PAYTYPE == 4) &&e.ISPAYED!=2
                                                 select e).FirstOrDefault();
                        if (changeApplyMaster != null&&i>0)
                        {
                            vChargeApplyReports.Add(vChargeApplyReport);
                        }
                        if (i == 0)
                        {
                            vChargeApplyReports.Add(vChargeApplyReport);
                        }
                        i++;
                    }
                    catch (Exception ex)
                    {
                        strMsg = "费用报销记录导入失败";
                        Utility.SaveLog("ImportChargeApplyByImportExcel:" + ex.ToString());
                    }
                }
                TF.Close();
                strMsg = "{IMPORTSUCCESSED}";
            }
            catch (Exception ex)
            {
                strMsg = "费用报销记录导入失败";
                Utility.SaveLog(ex.ToString());
            }

            //if (strMsg != "{SAVESUCCESSED}" && strMsg != "{ALREADYEXISTSRECORD}")
            //{
            //    return;
            //}
            Utility.DeleteUploadFile(strPhysicalPath);
            return vChargeApplyReports;
        }
        /// <summary>
        /// 更新报销状态
        /// </summary>
        /// <param name="vChargeApplys">报销实体</param>
        /// <returns>更新是否成功</returns>
        public int UptChargeApplyIsPayed(List<V_ChargeApplyReport> vChargeApplys)
        {
            int iResult = 0;
            try
            {
                dal.BeginTransaction();
                if (vChargeApplys != null && vChargeApplys.Count() > 0)
                {
                    foreach (var vChargeApply in vChargeApplys)
                    {
                        var changeApplyMaster = (from e in dal.GetObjects()
                                                 where e.CHARGEAPPLYMASTERCODE == vChargeApply.CHARGEAPPLYMASTERCODE
                                                 select e).FirstOrDefault();
                        if (changeApplyMaster != null)
                        {
                            changeApplyMaster.ISPAYED = 2;
                        }
                        iResult = dal.Update(changeApplyMaster);
                    }
                }
                dal.CommitTransaction();
            }
            catch (Exception ex)
            {
                Tracer.Debug(ex.Message);
                dal.RollbackTransaction();
            }
            return iResult;
        }
        #endregion
        /// <summary>
        /// 根据条件，获取费用报销信息(此函数用于费用报销查询分页)
        /// </summary>
        /// <param name="strOwnerID">登陆人的员工ID</param>
        /// <param name="strDateStart">查询起始时间</param>
        /// <param name="strDateEnd">查询截止时间</param>
        /// <param name="strCheckState">当前查询的审核状态</param>
        /// <param name="strFilter">查询条件</param>
        /// <param name="objArgs">查询参数</param>
        /// <param name="strSortKey">排序信息</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页显示条目数</param>
        /// <param name="pageCount">总页码</param>
        /// <returns></returns>
        public IQueryable<T_FB_CHARGEAPPLYMASTER> GetChargeApplyMasterRdListByMultSearch(string strOwnerID, string strDateStart,
            string strDateEnd, string strCheckState, string strFilter, List<object> objArgs, string strSortKey, int pageIndex,
            int pageSize, ref int pageCount)
        {
            var ents = GetAllChargeApplyMasterRdListByMultSearch(strOwnerID, strDateStart, strDateEnd, strCheckState, strFilter,
                objArgs, strSortKey);

            if (ents == null)
            {
                return null;
            }

            if (pageIndex == 0 && pageSize == 0)
            {
                return ents;
            }
            return Utility.Pager<T_FB_CHARGEAPPLYMASTER>(ents, pageIndex, pageSize, ref pageCount);
        }

        /// <summary>
        /// 查询申请的费用申请。
        /// </summary>
        /// <param name="conditions">查询条件对象集。</param>
        /// <returns>返回申请的费用申请。</returns>
        public static IQueryable<V_Money> GetChargeApply(ExecutionConditions conditions)
        {
            try
            {
                ChargeApplyMasterDAL dalChargeApplyMaster = new ChargeApplyMasterDAL();

                return dalChargeApplyMaster.GetChargeApply(conditions);
            }
            catch (Exception ex)
            {
                string ErrInfo = new ChargeApplyMasterBLL().GetType().ToString() + "：GetChargeApply，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
            }
            return null;
        }

        /// <summary>
        /// 根据条件，获取T_FB_CHARGEAPPLYMASTER信息
        /// </summary>
        /// <param name="strExtenOrderID">查询条件</param>
        /// <returns>T_FB_CHARGEAPPLYMASTER信息</returns>
        public IQueryable<T_FB_CHARGEAPPLYMASTER> GetChargeApplyMasterRdListByExtenOrderID(string strExtenOrderID)
        {
            ChargeApplyMasterDAL dalCharge = new ChargeApplyMasterDAL();
            var ents = from n in dalCharge.GetObjects().Include("T_FB_EXTENSIONALORDER")
                       where n.T_FB_EXTENSIONALORDER.EXTENSIONALORDERID == strExtenOrderID
                       select n;

            if (ents == null)
            {
                return null;
            }

            if (ents.Count() == 0)
            {
                return null;
            }

            return ents;
        }

        #endregion

        #region 写入数据
        /// <summary>
        /// 写入费用报销主从表数据  add by zl
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="detailList"></param>
        /// <returns></returns>
        public bool AddChargeApplyMasterAndDetail(T_FB_CHARGEAPPLYMASTER entity, List<T_FB_CHARGEAPPLYDETAIL> detailList,
            List<T_FB_CHARGEAPPLYREPAYDETAIL> detailRepList)
        {
            bool re;
            //从服务端获取时间
            entity.BUDGETARYMONTH = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM") + "-1");
            Tracer.Debug("T_FB_CHARGEAPPLYMASTER-CHARGEAPPLYMASTERID:" + entity.CHARGEAPPLYMASTERID);
            try
            {
                var company = OrgClient.GetCompanyById(entity.OWNERCOMPANYID);
                if (company!=null)
                {
                    entity.OWNERCOMPANYNAME = string.IsNullOrEmpty(company.BRIEFNAME) ? company.CNAME : company.BRIEFNAME;
                }
            }
            catch
            {
                Tracer.Debug("AddChargeApplyMasterAndDetail获取公司信息为空");
            }
            try
            {
                //Utility.RefreshEntity(entity);
                dal.BeginTransaction();
                var ents = from ent in dal.GetObjects<T_FB_SUBJECT>()
                           
                           select ent;
                foreach (T_FB_CHARGEAPPLYDETAIL obj in detailList)
                {
                    //添加报销明细
                    //Utility.RefreshEntity(obj);
                    Tracer.Debug("T_FB_CHARGEAPPLYDETAIL-ID:" + obj.CHARGEAPPLYDETAILID);
                    if (obj.T_FB_SUBJECT != null)
                    {
                        Tracer.Debug("T_FB_CHARGEAPPLYDETAIL-subjectID:" + obj.T_FB_SUBJECT.SUBJECTID + ";name" + obj.T_FB_SUBJECT.SUBJECTNAME);
                        //Tracer.Debug("T_FB_CHARGEAPPLYDETAIL-reference:" + obj.T_FB_SUBJECT.T_FB_SUBJECT2Reference.EntityKey.EntityKeyValues + ";name" + obj.T_FB_SUBJECT.SUBJECTNAME);
                        if (obj.T_FB_SUBJECT.T_FB_SUBJECT2Reference.EntityKey == null)
                        {
                            Tracer.Debug("T_FB_SUBJECT2Reference为空:" + obj.T_FB_SUBJECT.SUBJECTID + ";name" + obj.T_FB_SUBJECT.SUBJECTNAME);
                            var entSubject = ents.Where(s => s.SUBJECTID == obj.T_FB_SUBJECT.SUBJECTID).FirstOrDefault();
                            if (entSubject != null)
                            {
                                obj.T_FB_SUBJECT = entSubject;
                            }
                        }
                        else
                        {
                            Tracer.Debug("T_FB_SUBJECT2Reference不为空:" + obj.T_FB_SUBJECT.T_FB_SUBJECT2Reference.EntityKey);
                        }
                    }
                    else
                    {
                        Tracer.Debug("T_FB_CHARGEAPPLYDETAIL-IsubjectID 为空:");
                    }
                    //添加报销冲借款明细
                    Utility.RefreshEntity(obj);
                    Tracer.Debug("T_FB_CHARGEAPPLYDETAIL-刷新关联:");
                    entity.T_FB_CHARGEAPPLYDETAIL.Add(obj);
                }

                if (entity.PAYTYPE == 2)   //如果是冲借款则添加报销冲借款明细
                {
                    foreach (T_FB_CHARGEAPPLYREPAYDETAIL obj in detailRepList)
                    {
                        Tracer.Debug("T_FB_CHARGEAPPLYREPAYDETAIL-ID:" + obj.CHARGEAPPLYREPAYDETAILID);
                        
                        //添加报销冲借款明细
                        Utility.RefreshEntity(obj);
                        Tracer.Debug("T_FB_CHARGEAPPLYREPAYDETAIL-刷新关联:");
                        entity.T_FB_CHARGEAPPLYREPAYDETAIL.Add(obj);
                    }
                }
                Tracer.Debug("关联数目:" + entity.T_FB_CHARGEAPPLYDETAIL.Count().ToString());
                re = Add(entity);
                if (!re)
                {
                    Tracer.Debug("AddChargeApplyMasterAndDetail添加失败");
                    dal.RollbackTransaction();                    
                    return false;
                }

                dal.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：AddChargeApplyMasterAndDetail，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
                dal.RollbackTransaction();
                return false;
                //throw(ex);
            }
        }


        public bool AddChargeApplyMasterAndDetailMobile(T_FB_CHARGEAPPLYMASTER entity, List<T_FB_CHARGEAPPLYDETAIL> detailList,
            List<T_FB_CHARGEAPPLYREPAYDETAIL> detailRepList,ref string strMsg)
        {
            bool re;
            //从服务端获取时间
            entity.BUDGETARYMONTH = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM") + "-1");
            BudgetAccountBLL budgetBLL = new BudgetAccountBLL();
            budgetBLL.UpdateAccount(entity, 1);
            Tracer.Debug("AddChargeApplyMasterAndDetailMobile-CHARGEAPPLYMASTERID:" + entity.CHARGEAPPLYMASTERID);
            try
            {
                var company = OrgClient.GetCompanyById(entity.OWNERCOMPANYID);
                if (company != null)
                {
                    entity.OWNERCOMPANYNAME = string.IsNullOrEmpty(company.BRIEFNAME) ? company.CNAME : company.BRIEFNAME;
                }
            }
            catch
            {
                Tracer.Debug("AddChargeApplyMasterAndDetailMobileAddChargeApplyMasterAndDetail获取公司信息为空");
            }
            try
            {
                //Utility.RefreshEntity(entity);
                dal.BeginTransaction();
                var ents = from ent in dal.GetObjects<T_FB_SUBJECT>()

                           select ent;
                foreach (T_FB_CHARGEAPPLYDETAIL obj in detailList)
                {
                    //添加报销明细
                    //Utility.RefreshEntity(obj);
                    Tracer.Debug("AddChargeApplyMasterAndDetailMobileT_FB_CHARGEAPPLYDETAIL-ID:" + obj.CHARGEAPPLYDETAILID);
                    if (obj.T_FB_SUBJECT != null)
                    {
                        Tracer.Debug("AddChargeApplyMasterAndDetailMobileT_FB_CHARGEAPPLYDETAIL-subjectID:" + obj.T_FB_SUBJECT.SUBJECTID + ";name" + obj.T_FB_SUBJECT.SUBJECTNAME);
                        //Tracer.Debug("T_FB_CHARGEAPPLYDETAIL-reference:" + obj.T_FB_SUBJECT.T_FB_SUBJECT2Reference.EntityKey.EntityKeyValues + ";name" + obj.T_FB_SUBJECT.SUBJECTNAME);
                        if (obj.T_FB_SUBJECT.T_FB_SUBJECT2Reference.EntityKey == null)
                        {
                            Tracer.Debug("AddChargeApplyMasterAndDetailMobileT_FB_SUBJECT2Reference为空:" + obj.T_FB_SUBJECT.SUBJECTID + ";name" + obj.T_FB_SUBJECT.SUBJECTNAME);
                            var entSubject = ents.Where(s => s.SUBJECTID == obj.T_FB_SUBJECT.SUBJECTID).FirstOrDefault();
                            if (entSubject != null)
                            {
                                obj.T_FB_SUBJECT = entSubject;
                            }
                        }
                        else
                        {
                            Tracer.Debug("AddChargeApplyMasterAndDetailMobileT_FB_SUBJECT2Reference不为空:" + obj.T_FB_SUBJECT.T_FB_SUBJECT2Reference.EntityKey);
                        }
                    }
                    else
                    {
                        Tracer.Debug("AddChargeApplyMasterAndDetailMobileT_FB_CHARGEAPPLYDETAIL-IsubjectID 为空:");
                    }
                    //添加报销冲借款明细
                    Utility.RefreshEntity(obj);
                    Tracer.Debug("AddChargeApplyMasterAndDetailMobileT_FB_CHARGEAPPLYDETAIL-刷新关联:");
                    entity.T_FB_CHARGEAPPLYDETAIL.Add(obj);
                }

                if (entity.PAYTYPE == 2)   //如果是冲借款则添加报销冲借款明细
                {
                    foreach (T_FB_CHARGEAPPLYREPAYDETAIL obj in detailRepList)
                    {
                        Tracer.Debug("AddChargeApplyMasterAndDetailMobileT_FB_CHARGEAPPLYREPAYDETAIL-ID:" + obj.CHARGEAPPLYREPAYDETAILID);

                        //添加报销冲借款明细
                        Utility.RefreshEntity(obj);
                        Tracer.Debug("AddChargeApplyMasterAndDetailMobileT_FB_CHARGEAPPLYREPAYDETAIL-刷新关联:");
                        entity.T_FB_CHARGEAPPLYREPAYDETAIL.Add(obj);
                    }
                }
                Tracer.Debug("AddChargeApplyMasterAndDetailMobile关联数目:" + entity.T_FB_CHARGEAPPLYDETAIL.Count().ToString());
                string strCode = "";
                
                    try
                    {
                        strCode = new OrderCodeBLL().GetAutoOrderCode(entity);
                        entity.CHARGEAPPLYMASTERCODE = strCode;
                        string err = this.GetType().ToString() + "," + System.DateTime.Now.ToString() + entity.CHARGEAPPLYMASTERID + "：产生单据号 " + strCode;
                        Tracer.Debug(err);

                    }
                    catch (Exception ex)
                    {
                        string sr = this.GetType().ToString() + "," + System.DateTime.Now.ToString() + entity.CHARGEAPPLYMASTERID + "：产生单据号时出现异常 " + ex.Message;
                        Tracer.Debug(sr);
                        //strMsg = "产生单据号时出现异常！";                        
                    }
                    strMsg = strCode;
                re = Add(entity);
                if (!re)
                {
                    Tracer.Debug("AddChargeApplyMasterAndDetailMobile添加失败");
                    dal.RollbackTransaction();
                    return false;
                }

                dal.CommitTransaction();

                return true;
            }
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：AddChargeApplyMasterAndDetail，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
                dal.RollbackTransaction();
                return false;
                //throw (ex);
            }
        }

        #endregion

        #region 更新数据
        /// <summary>
        /// 更新费用报销主从表数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="detailList"></param>
        /// <returns></returns>
        //public void UptChargeApplyMasterAndDetail(string strActionType, T_FB_CHARGEAPPLYMASTER entity,
        //    List<T_FB_CHARGEAPPLYDETAIL> detailList, List<T_FB_CHARGEAPPLYREPAYDETAIL> detailRepList, ref string strMsg)
        //{
        //    try
        //    {
                
        //        var company = OrgClient.GetCompanyById(entity.OWNERCOMPANYID);
        //        if (company != null)
        //        {
        //            entity.OWNERCOMPANYNAME = string.IsNullOrEmpty(company.BRIEFNAME) ? company.CNAME : company.BRIEFNAME;
        //        }
        //    }
        //    catch
        //    {

        //    }
        //    bool re = false;
        //    var ent = (from ch in dal.GetObjects()
        //              where ch.CHARGEAPPLYMASTERID == entity.CHARGEAPPLYMASTERID
        //              select ch).FirstOrDefault();
        //    entity.BUDGETARYMONTH = ent.BUDGETARYMONTH;
        //    if (LockOrder(entity.CHARGEAPPLYMASTERID))
        //    {
        //        strMsg = "单据正在提交或审核中，不可重复操作！";
        //        return;
        //    }

        //    try
        //    {
        //        this.dal.BeginTransaction();
        //        FBAEnums.CheckStates dOldChecksates = FBAEnums.CheckStates.UnSubmit;
        //        T_FB_CHARGEAPPLYMASTER cha = GetChargeApplyMasterByID(entity.CHARGEAPPLYMASTERID);
        //        if (cha == null)
        //        {
        //            strMsg = "提交的单据不存在或已删除，不可继续操作！";
        //            return;
        //        }
        //        //if (IsOverYear(cha.BUDGETARYMONTH))
        //        //{
        //        //    strMsg = "不能进行跨年报销";
        //        //    Tracer.Debug(strMsg);
        //        //    return;
        //        //}

        //        object checkStatesOld = cha.CHECKSTATES;
        //        dOldChecksates = (FBAEnums.CheckStates)int.Parse(checkStatesOld.ToString());

        //        object checkStatesNew = entity.CHECKSTATES;
        //        FBAEnums.CheckStates dNewCheckStates = (FBAEnums.CheckStates)int.Parse(checkStatesNew.ToString());

        //        if ((dOldChecksates == FBAEnums.CheckStates.Approved || dOldChecksates == FBAEnums.CheckStates.UnApproved)
        //            && strActionType.ToUpper() != "RESUBMIT")
        //        {
        //            strMsg = "单据已审核完毕，不可再次操作";
        //            return;
        //        }

        //        #region 是否本月有结算

        //        bool isChecked = SystemSettingsBLL.IsChecked;
        //        // 没月结，只能处理报销。
        //        string entityType = entity.GetType().Name;
        //        string[] EntityTypes = new string[] { typeof(T_FB_BORROWAPPLYMASTER).Name, 
        //            typeof(T_FB_REPAYAPPLYMASTER).Name, typeof(T_FB_CHARGEAPPLYMASTER).Name};
        //        // 月结不可操作：1.日常报销类型的单据的审核或提交，2.所有单据的提交或重新提交
        //        if (!isChecked && (EntityTypes.Contains(entityType) || (strActionType.ToUpper() == "SUBMIT")
        //            || (strActionType.ToUpper() == "RESUBMIT")))
        //        {
        //            strMsg = "本月尚未结算,无法提交或审核!";
        //            return;
        //        }

        //        #endregion

        //        Utility.CloneEntity(entity, cha);
        //        cha.UPDATEDATE = DateTime.Now;

        //        bool n = Update(cha);
        //        if (n == false)
        //        {
        //            strMsg = "单据更新异常！";
        //            return;
        //        }

        //        ChargeApplyDetailBLL chargeDetailBLL = new ChargeApplyDetailBLL();
        //        //chargeDetailBLL.dal = this.dal;
                
        //        re = chargeDetailBLL.UpdateChargeApplyDetail(cha.CHARGEAPPLYMASTERID, detailList);//更新报销明细
        //        if (!re)
        //        {
        //            strMsg = "单据明细更新异常！";
        //            return;
        //        }

        //        //add zl 12.30   如果是冲借款则更新冲借款明细数据

        //        ChargeApplyRepayDetailBLL chargeRepayDetailBLL = new ChargeApplyRepayDetailBLL();
        //        re = chargeRepayDetailBLL.UpdateChargeApplyRepayDetail(cha.CHARGEAPPLYMASTERID, detailRepList);
        //        if (!re)
        //        {
        //            strMsg = "单据冲借款明细更新异常！";
        //            return;
        //        }

        //        //add end

        //        if (dOldChecksates == dNewCheckStates && dOldChecksates == (int)FBAEnums.CheckStates.UnSubmit
        //            && strActionType.ToUpper() == "EDIT")
        //        {
        //            strMsg = "单据更新成功！";
        //            return;
        //        }
        //        this.dal.CommitTransaction();
        //    }
        //    catch (Exception ex)
        //    {
        //        this.dal.RollbackTransaction();
        //        string ErrInfo = this.GetType().ToString() + "：UptChargeApplyMasterAndDetail，" + System.DateTime.Now.ToString() + "，" + ex.Message;
        //        Tracer.Debug(ErrInfo);
        //    }
        //    finally
        //    {
        //        ReleaseOrder(entity.CHARGEAPPLYMASTERID);
        //    }
        //}

        public void Test(string id)
        {

            var ent = (from a in dal.GetObjects()
                      where a.CHARGEAPPLYMASTERID == id
                      select a).FirstOrDefault();
            var ent2 = from a in dal.GetObjects()
                       join b in dal.GetObjects<T_FB_CHARGEAPPLYDETAIL>() on a.CHARGEAPPLYMASTERID equals b.T_FB_CHARGEAPPLYMASTER.CHARGEAPPLYMASTERID
                       where a.CHARGEAPPLYMASTERID == id
                       select b;
            List<T_FB_CHARGEAPPLYDETAIL> list = ent2.ToList();
            string strMsg="";
            this.UptChargeApplyCheckState(ent,list,null,ref strMsg);
           
        }


        /// <summary>
        /// 更新费用报销主表CHECKSTATES字段值 add by zl
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="detailList"></param>
        /// <returns></returns>
        public void UptChargeApplyCheckState(T_FB_CHARGEAPPLYMASTER entity, List<T_FB_CHARGEAPPLYDETAIL> detailList,
            List<T_FB_CHARGEAPPLYREPAYDETAIL> detailRepList, ref string strMsg)
        {
            if (LockOrder(entity.CHARGEAPPLYMASTERID))
            {
                strMsg = "单据正在提交或审核中，不可重复操作！";
                return;
            }

            try
            {
                PersonAccountBLL PerBLL = new PersonAccountBLL();
                BudgetAccountBLL budgetBLL = new BudgetAccountBLL();

                FBAEnums.CheckStates dOldChecksates = FBAEnums.CheckStates.UnSubmit;

                object checkStatesNew = entity.CHECKSTATES;
                FBAEnums.CheckStates dNewCheckStates = (FBAEnums.CheckStates)int.Parse(checkStatesNew.ToString());

                ///add 2012.12.12
                ///传入报销月份为时间去和当前时间判断，如果不在同一年
                ///说明该报销单是跨年的，则不能进行审核操作，即当年的报销单只能在当年进行报销
                if (dNewCheckStates == FBAEnums.CheckStates.Approved || dNewCheckStates == FBAEnums.CheckStates.Approving)
                {
                    if (IsOverYear(entity.BUDGETARYMONTH))
                    {
                        strMsg = "报销单跨年后只能终审不通过(财务规定)";
                        Tracer.Debug(strMsg);
                        return;
                    }
                }
                //end
                T_FB_CHARGEAPPLYMASTER cha = GetChargeApplyMasterByID(entity.CHARGEAPPLYMASTERID);
                if (cha == null)
                {
                    strMsg = entity.CHARGEAPPLYMASTERID + "费用报销单据不存在，不可继续操作！";
                    Tracer.Debug(strMsg);
                    return;
                }

                object checkStatesOld = cha.CHECKSTATES;
                dOldChecksates = (FBAEnums.CheckStates)int.Parse(checkStatesOld.ToString());
                if (dOldChecksates == FBAEnums.CheckStates.Approved && dNewCheckStates == FBAEnums.CheckStates.Approving)
                {
                    strMsg = "已审核通过的费用报销单禁止再次审核!";
                    string ErrInfo = this.GetType().ToString() + "：UptChargeApplyCheckState，" + System.DateTime.Now.ToString() + "，" + strMsg;
                    Tracer.Debug(ErrInfo);
                    return;
                }


                Utility.CloneEntity(entity, cha);

                //add zl 12.12 提交审核时产生单据号
                string strCode = "";
                if (string.IsNullOrEmpty(cha.CHARGEAPPLYMASTERCODE.Trim()))
                {
                    try
                    {
                        strCode = new OrderCodeBLL().GetAutoOrderCode(entity);
                        cha.CHARGEAPPLYMASTERCODE = strCode;
                        string err = this.GetType().ToString() + "," + System.DateTime.Now.ToString() + cha.CHARGEAPPLYMASTERID + "：产生单据号 " + strCode;
                        Tracer.Debug(err);

                    }
                    catch (Exception ex)
                    {
                        string sr = this.GetType().ToString() + "," + System.DateTime.Now.ToString() + cha.CHARGEAPPLYMASTERID + "：产生单据号时出现异常 " + ex.Message;
                        Tracer.Debug(sr);
                        strMsg = "产生单据号时出现异常！";
                        return;
                    }
                }
                string Logmsg = this.GetType().ToString() + "," + System.DateTime.Now.ToString() + "：UptChargeApplyCheckState，表单ID " + cha.CHARGEAPPLYMASTERID + ", 单据号 " + cha.CHARGEAPPLYMASTERCODE + ",审核状态：" + cha.CHECKSTATES;
                Tracer.Debug(Logmsg);
                //add end

                //add zl 审核中时检查报销金额是否大于实际结余，冲借款金额是否大于借款余额 2012.1.11
                if (cha.PAYTYPE == 2)
                {
                    if (dNewCheckStates == FBAEnums.CheckStates.Approving || dNewCheckStates == FBAEnums.CheckStates.Approved)
                    {
                        strMsg = PerBLL.CheckRepMoneyForCharge(cha, detailRepList, (int)dNewCheckStates);
                        if (!string.IsNullOrEmpty(strMsg))
                        {
                            return;
                        }
                    }
                }



                //zl 11.16
                if (dNewCheckStates == FBAEnums.CheckStates.UnSubmit)
                {
                    return;
                }
                if (dNewCheckStates == FBAEnums.CheckStates.Approving && (dOldChecksates != FBAEnums.CheckStates.UnSubmit && dOldChecksates != FBAEnums.CheckStates.UnApproved))
                {
                    return;
                }
                if (dNewCheckStates == FBAEnums.CheckStates.Approved && dOldChecksates != FBAEnums.CheckStates.Approving)
                {
                    return;
                }
                if (dNewCheckStates == FBAEnums.CheckStates.UnApproved && dOldChecksates != FBAEnums.CheckStates.Approving)
                {
                    return;
                }

                // 第一次提交
                if (dNewCheckStates == FBAEnums.CheckStates.Approving && (dOldChecksates == FBAEnums.CheckStates.UnSubmit || dOldChecksates == FBAEnums.CheckStates.UnApproved))
                {
                    cha.BUDGETARYMONTH = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                }
                else if ( dNewCheckStates == FBAEnums.CheckStates.Approving || dNewCheckStates == FBAEnums.CheckStates.Approved)
                {
                    if (cha.BUDGETARYMONTH.Year < System.DateTime.Now.Year)
                    {
                        strMsg = "单据已超过有效期，请审核不通过！";
                        return;
                    }
                }
                

                cha.UPDATEDATE = DateTime.Now;
                if (dNewCheckStates == FBAEnums.CheckStates.Approved && (cha.PAYTYPE == 1 || cha.PAYTYPE == 4))
                {
                    cha.ISPAYED = 1;//1未报销；2已报销
                }
                bool n = Update(cha);
                if (!n)
                {
                    strMsg = "单据明细更新异常！";
                    return;
                }

                //end

                //add zl 12.12   终审通过时冲借款更新PersonAccount表
                if (cha.PAYTYPE == 2)
                {
                    try
                    {
                        if (dNewCheckStates == FBAEnums.CheckStates.Approved)
                        {
                            n = PerBLL.UptPersonAccountByChar(cha, detailRepList, (int)dNewCheckStates);
                            if (!n)
                            {
                                strMsg = "更新借还总账表异常！";
                                return;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string ErrInfo = this.GetType().ToString() + "操作函数：UptPersonAccountByChar，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                        Tracer.Debug(ErrInfo);
                        strMsg = "更新借还总账表异常！";
                        return;
                    }
                }
                //add end

                try
                {
                    string chrDet = "";
                    foreach (T_FB_CHARGEAPPLYDETAIL obj in detailList)
                    {
                        chrDet += obj.T_FB_SUBJECT.SUBJECTID + ",报销金额：" + obj.CHARGEMONEY + "||";
                    }
                    string ErrInfo = this.GetType().ToString() + "费用报销更新预算总账表，" + System.DateTime.Now.ToString() + "，" + "表单ID:" + entity.CHARGEAPPLYMASTERID + "," + "所更改科目:" + chrDet;
                    Tracer.Debug(ErrInfo);
                    budgetBLL.UpdateAccount(cha, (int)dNewCheckStates);
                }
                catch (Exception ex)
                {
                    string ErrInfo = this.GetType().ToString() + "操作函数：UpdateAccount，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                    Tracer.Debug(ErrInfo);
                    strMsg = "更新预算总账表异常！";
                    return;
                }

            }
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：UptChargeApplyCheckState，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
                strMsg = "更新单据状态出现异常！";
            }
            finally
            {
                ReleaseOrder(entity.CHARGEAPPLYMASTERID);
            }
        }

        #endregion

        //#region 修改元数据
        //public string UpdateEntityXMLNumber(string Formid)
        //{
        //    try
        //    {
        //        TravelReimbursementBLL bll = new TravelReimbursementBLL();
        //        string ReplaceString = (from ent in bll.dal.GetObjects()
        //                                where ent.TRAVELREIMBURSEMENTID == Formid
        //                                select ent.NOBUDGETCLAIMS).FirstOrDefault();
        //        if (string.IsNullOrEmpty(ReplaceString))
        //        {
        //            Tracer.Debug("出差报销提交审核替换元数据单号，获取的单号为空：" + ReplaceString);
        //            return "";
        //        }
        //        else
        //        {
        //            Tracer.Debug("开始更新出差报销单号：获取的最新单号为：" + ReplaceString + " formid: " + Formid);
        //        }
        //        //更新元数据里的报销单号
        //        SMT.SaaS.BLLCommonServices.FlowWFService.ServiceClient client =
        //        new SaaS.BLLCommonServices.FlowWFService.ServiceClient();
        //        Tracer.Debug("开始调用元数据获取接口：FlowWFService.GetMetadataByFormid(" + Formid + ")");
        //        string xml = string.Empty;
        //        xml = client.GetMetadataByFormid(Formid);
        //        Tracer.Debug("获取到的元数据：" + xml);
        //        xml = xml.Replace("自动生成", ReplaceString);
        //        if (string.IsNullOrEmpty(xml))
        //        {
        //            Tracer.Debug("获取到的流程元数据为空，不更新元数据单号");
        //            return "";
        //        }
        //        bool flag = UpdateMetadataByFormid2(Formid, xml);
        //        if (flag)
        //        {
        //            Tracer.Debug("新出差报销元数据替换单号成功：" + ReplaceString);
        //            return "";
        //        }
        //        else
        //        {
        //            Tracer.Debug("出差报销元数据替换单号UpdateMetadataByFormid返回false：Formid：" + Formid
        //                + ReplaceString);
        //            return "";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Tracer.Debug(ex.ToString());
        //        return "";
        //    }

        //}

        //public static bool UpdateMetadataByFormid2(string formid, string xml)
        //{
        //    string str = ConfigurationManager.AppSettings["ConnectionString"];
        //    var conn = new OracleConnection(str);
        //    try
        //    {
        //        conn.Open();
        //        string sql = "UPDATE smtwf.FLOW_FLOWRECORDMASTER_T set BUSINESSOBJECT=:BUSINESSOBJECT where FORMID=:FORMID ";
        //        string sqlDb = "UPDATE smtwf.T_WF_DOTASK set APPXML=:APPXML where ORDERID=:FORMID ";
        //        try
        //        {
        //            #region 审核主表
        //            OracleParameter[] pageparm =
        //                { 
        //                    new OracleParameter(":FORMID",OracleType.NVarChar), 
        //                    new OracleParameter(":BUSINESSOBJECT",OracleType.Clob)                   

        //                };
        //            pageparm[0].Value = formid;//
        //            pageparm[1].Value = xml;//
        //            int n;
        //            OracleCommand cmd = new OracleCommand(sql, conn);
        //            AttachParameters(cmd, pageparm);

        //            n = cmd.ExecuteNonQuery();

        //            Tracer.Debug("UpdateMetadataByFormid2【第1次】：【审核主表FLOW_FLOWRECORDMASTER_T】[更新元数据]成功 影响记录数：" + n + ";formid＝" + formid + ";xml=" + xml);
        //            #endregion
        //            #region 待办任务
        //            OracleParameter[] pageparmDb =
        //                { 
        //                    new OracleParameter(":FORMID",OracleType.NVarChar), 
        //                    new OracleParameter(":APPXML",OracleType.Clob)                   

        //                };
        //            pageparmDb[0].Value = formid;//
        //            pageparmDb[1].Value = xml;//
        //            OracleCommand cmdDb = new OracleCommand(sqlDb, conn);
        //            AttachParameters(cmdDb, pageparmDb);
        //            int n2 = cmdDb.ExecuteNonQuery();
        //            Tracer.Debug("UpdateMetadataByFormid2【第1次】：【待办任务T_WF_DOTASK】[更新元数据]成功 影响记录数：" + n2 + ";formid＝" + formid + ";xml=" + xml);
        //            #endregion
        //            return true;

        //        }
        //        catch (Exception ex)
        //        {
        //            Tracer.Debug("更新元数据【第1次】 UpdateMetadataByFormid2 异常信息：" + ex.Message);
        //            return false;
        //        }
        //        finally
        //        {
        //            if (conn.State == System.Data.ConnectionState.Open)
        //            {
        //                conn.Close();
        //                conn.Dispose();
        //            }
        //            Tracer.Debug("更新元数据【第1次】:UpdateMetadataByFormid2-> \r\n SQL=" + sql + "\r\n SQL=" + sqlDb);

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Tracer.Debug("更新元数据【第1次】:UpdateMetadataByFormid2:异常信息：" + ex.Message);
        //        return false;
        //    }
        //}
        //#endregion

        #region 删除数据
        /// <summary>
        /// 删除费用报销主表数据  add by zl
        /// </summary>
        /// <param name="chargeMasterID"></param>
        /// <returns></returns>
        public bool DelChargeApplyMaster(string chargeMasterID)
        {
            try
            {
                var entitys = (from ent in dal.GetTable() where ent.CHARGEAPPLYMASTERID == chargeMasterID select ent);
                if (entitys.Count() > 0)
                {
                    var entity = entitys.FirstOrDefault();
                    //只有未提交的单据才可以删除
                    if (entity.CHECKSTATES == 0)
                    {
                        Delete(entity);
                        return true;
                    }
                    else
                    {
                        string ErrInfo = "删除报销单出现错误，单据审核状态为：" + entity.CHECKSTATES + " .表单ID为：" + entity.CHARGEAPPLYMASTERID;
                        Tracer.Debug(ErrInfo);
                        return false;
                    }
                    
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：DelChargeApplyMaster，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
                return false;
            }
        }

        /// <summary>
        /// 一起删除费用报销主表和明细表   add by zl
        /// </summary>
        /// <param name="chargeMasterID"></param>
        /// <returns></returns>
        public bool DelChargeApplyMasterAndDetail(string chargeMasterID)
        {
            try
            {
                ChargeApplyDetailBLL chargeDetailBLL = new ChargeApplyDetailBLL();
                ChargeApplyRepayDetailBLL chargeRepayDetailBLL = new ChargeApplyRepayDetailBLL();
                dal.BeginTransaction();
                if (!chargeDetailBLL.DelChargeApplyDetail(chargeMasterID))
                {
                    dal.RollbackTransaction();
                    return false;
                }
                if (!chargeRepayDetailBLL.DelChargeApplyRepayDetail(chargeMasterID))
                {
                    dal.RollbackTransaction();
                    return false;
                }
                if (!DelChargeApplyMaster(chargeMasterID))
                {
                    dal.RollbackTransaction();
                    return false;
                }
                dal.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：DelChargeApplyMasterAndDetail，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
                dal.RollbackTransaction();
                return false;
            }
        }


        #endregion

        #region 手机版修改审核状态  ljx 2011-8-19
        public int GetChargeApplyForMobile(string Chargeid, string StrCheckState)
        {
            T_FB_CHARGEAPPLYMASTER master = new T_FB_CHARGEAPPLYMASTER();
            master = GetChargeApplyMasterByID(Chargeid);
            master.CHECKSTATES = Convert.ToInt32(StrCheckState);
            List<object> masterids = new List<object>();
            masterids.Add(Chargeid);
            List<T_FB_CHARGEAPPLYDETAIL> entRdlist = new List<T_FB_CHARGEAPPLYDETAIL>();
            ChargeApplyDetailBLL bllChargeApplyDetail = new ChargeApplyDetailBLL();
            List<T_FB_CHARGEAPPLYREPAYDETAIL> entReplist = new List<T_FB_CHARGEAPPLYREPAYDETAIL>();
            ChargeApplyRepayDetailBLL bllChargeApplyRepayDetail = new ChargeApplyRepayDetailBLL();
            entRdlist = bllChargeApplyDetail.GetChargeApplyDetailByMasterID(masterids);
            entReplist = bllChargeApplyRepayDetail.GetChargeApplyRepayDetailByMasterID(Chargeid);
            if (entRdlist != null)
            {
                string strmsg = string.Empty;
                UptChargeApplyCheckState(master, entRdlist, entReplist, ref strmsg);
                if (string.IsNullOrWhiteSpace(strmsg))
                {
                    return 1;
                }
                else
                {
                    throw new Exception(strmsg);
                }
            }
            return 0;
        }
        #endregion

        #region  转移数据专用 zl
        /// <summary>
        /// 改变报销表状态
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UptChargeApplyMasterChkSta(T_FB_CHARGEAPPLYMASTER entity)
        {
            bool re = true;
            re = Update(entity);
            return re;
        }
        #endregion

        /// <summary>
        /// 判断传入的时间和当前时间是不是在一年
        /// </summary>
        /// <param name="dateTime">报销时间</param>
        /// <returns>不是跨年返回false，是跨年返回true</returns>
        private bool IsOverYear(DateTime dateTime)
        {
            bool flag = false;
            int bugYear = dateTime.Year;
            int nowYear = DateTime.Now.Year;
            if (bugYear != nowYear)
            {
                flag = true;
            }
            return flag;
        }


        #region 2014
        // . 优化, 费用的添加，修改
        /// <summary>
        /// 更新费用报销主从表数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="detailList"></param>
        /// <returns></returns>
        public void UptChargeApplyMasterAndDetail(string strActionType, T_FB_CHARGEAPPLYMASTER entity,
            List<T_FB_CHARGEAPPLYDETAIL> detailList, List<T_FB_CHARGEAPPLYREPAYDETAIL> detailRepList, ref string strMsg)
        {
            try
            {
                this.dal.BeginTransaction();

                if (LockOrder(entity.CHARGEAPPLYMASTERID))
                {
                    strMsg = "单据正在提交或审核中，不可重复操作！";
                    return;
                }

                var entityOld = (from ch in dal.GetObjects()
                                 where ch.CHARGEAPPLYMASTERID == entity.CHARGEAPPLYMASTERID
                                 select ch).FirstOrDefault();
                if (entityOld == null)
                {
                    strMsg = "单据不存在或已删除，不可继续操作！";
                    return;
                }

                FBAEnums.CheckStates dOldChecksates = FBAEnums.CheckStates.UnSubmit;
                object checkStatesOld = entityOld.CHECKSTATES;
                dOldChecksates = (FBAEnums.CheckStates)int.Parse(checkStatesOld.ToString());
                if (dOldChecksates == FBAEnums.CheckStates.Approving)
                {
                    strMsg = "单据已在审核中，不可再次操作！";
                    return;
                }
                else if ((dOldChecksates == FBAEnums.CheckStates.Approved) 
                    || (dOldChecksates == FBAEnums.CheckStates.UnApproved && strActionType.ToUpper() != "RESUBMIT"))
                {
                    strMsg = "单据已审核完毕，不可再次操作";
                    return;
                }
                try
                {
                    // 公司名称用短的
                    if (entityOld.OWNERCOMPANYID != entity.OWNERCOMPANYID)
                    {
                        var company = OrgClient.GetCompanyById(entity.OWNERCOMPANYID);
                        if (company != null)
                        {
                            entity.OWNERCOMPANYNAME = string.IsNullOrEmpty(company.BRIEFNAME) ? company.CNAME : company.BRIEFNAME;
                        }
                    }
                }
                catch
                {

                }
                bool re = false;
                // 获取单号
                if (string.IsNullOrWhiteSpace(entityOld.CHARGEAPPLYMASTERCODE) && strActionType.ToUpper() == "SUBMIT")
                {
                    entity.CHARGEAPPLYMASTERCODE = new OrderCodeBLL().GetAutoOrderCode(entity);
                }
                else
                {
                    entity.CHARGEAPPLYMASTERCODE = entityOld.CHARGEAPPLYMASTERCODE;
                }
                entity.BUDGETARYMONTH = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                #region 是否本月有结算
                
                bool isChecked = SystemSettingsBLL.IsChecked;
                // 没月结，只能处理报销。
                string entityType = entity.GetType().Name;
                string[] EntityTypes = new string[] { typeof(T_FB_BORROWAPPLYMASTER).Name, 
                    typeof(T_FB_REPAYAPPLYMASTER).Name, typeof(T_FB_CHARGEAPPLYMASTER).Name};
                // 月结不可操作：1.日常报销类型的单据的审核或提交，2.所有单据的提交或重新提交
                if (!isChecked && (EntityTypes.Contains(entityType) || (strActionType.ToUpper() == "SUBMIT")
                    || (strActionType.ToUpper() == "RESUBMIT")))
                {
                    strMsg = "本月尚未结算,无法提交或审核!";
                    return;
                }

                #endregion

                entity.CREATEDATE = entityOld.CREATEDATE;
                entity.CREATEUSERID = entityOld.CREATEUSERID;
                entity.CREATEUSERNAME = entityOld.CREATEUSERNAME;
                entity.CREATEPOSTID = entityOld.CREATEPOSTID;
                entity.CREATEPOSTNAME = entityOld.CREATEPOSTNAME;
                entity.CREATEDEPARTMENTID = entityOld.CREATEDEPARTMENTID;
                entity.CREATEDEPARTMENTNAME = entityOld.CREATEDEPARTMENTNAME;
                entity.CREATECOMPANYID = entityOld.CREATECOMPANYID;
                entity.CREATECOMPANYNAME = entityOld.CREATECOMPANYNAME;
                Utility.CloneEntity(entity, entityOld);

                entityOld.UPDATEDATE = DateTime.Now;
                bool n = Update(entityOld);
                if (n == false)
                {
                    strMsg = "单据更新异常！";
                    return;
                }

                ChargeApplyDetailBLL chargeDetailBLL = new ChargeApplyDetailBLL();
                //chargeDetailBLL.dal = this.dal;

                re = chargeDetailBLL.UpdateChargeApplyDetail(entityOld.CHARGEAPPLYMASTERID, detailList);//更新报销明细
                if (!re)
                {
                    strMsg = "单据明细更新异常！";
                    return;
                }

                //add zl 12.30   如果是冲借款则更新冲借款明细数据
                
                ChargeApplyRepayDetailBLL chargeRepayDetailBLL = new ChargeApplyRepayDetailBLL();
                re = chargeRepayDetailBLL.UpdateChargeApplyRepayDetail(entityOld.CHARGEAPPLYMASTERID, detailRepList);
                if (!re)
                {
                    strMsg = "单据冲借款明细更新异常！";
                    return;
                }
                if (strActionType.ToUpper() == "EDIT")
                {
                    strMsg = "OK:单据保存成功!";
                }
                else
                {
                    strMsg = "OK:" + entityOld.CHARGEAPPLYMASTERCODE;
                }
                this.dal.CommitTransaction();
                return;

            }
            catch (Exception ex)
            {
                this.dal.RollbackTransaction();
                string ErrInfo = this.GetType().ToString() + "：UptChargeApplyMasterAndDetail，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
            }
            finally
            {
                ReleaseOrder(entity.CHARGEAPPLYMASTERID);
            }
        }

        /// <summary>
        /// 费用报销修改手机版调用
        /// </summary>
        /// <param name="strActionType"></param>
        /// <param name="entity"></param>
        /// <param name="detailList"></param>
        /// <param name="detailRepList"></param>
        /// <param name="strMsg"></param>
        public void UptChargeApplyMasterAndDetailForMobile(string strActionType, T_FB_CHARGEAPPLYMASTER entity,
            List<T_FB_CHARGEAPPLYDETAIL> detailList, List<T_FB_CHARGEAPPLYREPAYDETAIL> detailRepList, ref string strMsg)
        {
            try
            {
                this.dal.BeginTransaction();

                if (LockOrder(entity.CHARGEAPPLYMASTERID))
                {
                    strMsg = "单据正在提交或审核中，不可重复操作！";
                    return;
                }

                var entityOld = (from ch in dal.GetObjects()
                                 where ch.CHARGEAPPLYMASTERID == entity.CHARGEAPPLYMASTERID
                                 select ch).FirstOrDefault();
                if (entityOld == null)
                {
                    strMsg = "单据不存在或已删除，不可继续操作！";
                    return;
                }

                FBAEnums.CheckStates dOldChecksates = FBAEnums.CheckStates.UnSubmit;
                object checkStatesOld = entityOld.CHECKSTATES;
                dOldChecksates = (FBAEnums.CheckStates)int.Parse(checkStatesOld.ToString());
                if (dOldChecksates == FBAEnums.CheckStates.Approving)
                {
                    strMsg = "单据已在审核中，不可再次操作！";
                    return;
                }
                else if ((dOldChecksates == FBAEnums.CheckStates.Approved)
                    || (dOldChecksates == FBAEnums.CheckStates.UnApproved && strActionType.ToUpper() != "RESUBMIT"))
                {
                    strMsg = "单据已审核完毕，不可再次操作";
                    return;
                }
                try
                {
                    // 公司名称用短的
                    if (entityOld.OWNERCOMPANYID != entity.OWNERCOMPANYID)
                    {
                        var company = OrgClient.GetCompanyById(entity.OWNERCOMPANYID);
                        if (company != null)
                        {
                            entity.OWNERCOMPANYNAME = string.IsNullOrEmpty(company.BRIEFNAME) ? company.CNAME : company.BRIEFNAME;
                        }
                    }
                }
                catch
                {

                }
                bool re = false;
                // 获取单号
                if (string.IsNullOrWhiteSpace(entityOld.CHARGEAPPLYMASTERCODE) )
                {
                    entity.CHARGEAPPLYMASTERCODE = new OrderCodeBLL().GetAutoOrderCode(entity);
                }
                else
                {
                    entity.CHARGEAPPLYMASTERCODE = entityOld.CHARGEAPPLYMASTERCODE;
                }
                entity.BUDGETARYMONTH = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                #region 是否本月有结算

                bool isChecked = SystemSettingsBLL.IsChecked;
                // 没月结，只能处理报销。
                string entityType = entity.GetType().Name;
                string[] EntityTypes = new string[] { typeof(T_FB_BORROWAPPLYMASTER).Name, 
                    typeof(T_FB_REPAYAPPLYMASTER).Name, typeof(T_FB_CHARGEAPPLYMASTER).Name};
                // 月结不可操作：1.日常报销类型的单据的审核或提交，2.所有单据的提交或重新提交
                if (!isChecked && (EntityTypes.Contains(entityType) || (strActionType.ToUpper() == "SUBMIT")
                    || (strActionType.ToUpper() == "RESUBMIT")))
                {
                    strMsg = "本月尚未结算,无法提交或审核!";
                    return;
                }

                #endregion

                entity.CREATEDATE = entityOld.CREATEDATE;
                entity.CREATEUSERID = entityOld.CREATEUSERID;
                entity.CREATEUSERNAME = entityOld.CREATEUSERNAME;
                entity.CREATEPOSTID = entityOld.CREATEPOSTID;
                entity.CREATEPOSTNAME = entityOld.CREATEPOSTNAME;
                entity.CREATEDEPARTMENTID = entityOld.CREATEDEPARTMENTID;
                entity.CREATEDEPARTMENTNAME = entityOld.CREATEDEPARTMENTNAME;
                entity.CREATECOMPANYID = entityOld.CREATECOMPANYID;
                entity.CREATECOMPANYNAME = entityOld.CREATECOMPANYNAME;
                Utility.CloneEntity(entity, entityOld);

                entityOld.UPDATEDATE = DateTime.Now;
                bool n = Update(entityOld);
                if (n == false)
                {
                    strMsg = "单据更新异常！";
                    return;
                }

                ChargeApplyDetailBLL chargeDetailBLL = new ChargeApplyDetailBLL();
                //chargeDetailBLL.dal = this.dal;

                re = chargeDetailBLL.UpdateChargeApplyDetail(entityOld.CHARGEAPPLYMASTERID, detailList);//更新报销明细
                if (!re)
                {
                    strMsg = "单据明细更新异常！";
                    return;
                }

                //add zl 12.30   如果是冲借款则更新冲借款明细数据

                ChargeApplyRepayDetailBLL chargeRepayDetailBLL = new ChargeApplyRepayDetailBLL();
                re = chargeRepayDetailBLL.UpdateChargeApplyRepayDetail(entityOld.CHARGEAPPLYMASTERID, detailRepList);
                if (!re)
                {
                    strMsg = "单据冲借款明细更新异常！";
                    return;
                }
                if (strActionType.ToUpper() == "EDIT")                
                {
                    strMsg = "OK:" + entityOld.CHARGEAPPLYMASTERCODE;
                }
                this.dal.CommitTransaction();
                return;

            }
            catch (Exception ex)
            {
                this.dal.RollbackTransaction();
                string ErrInfo = this.GetType().ToString() + "：UptChargeApplyMasterAndDetail，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
            }
            finally
            {
                ReleaseOrder(entity.CHARGEAPPLYMASTERID);
            }
        }

        public void UptDetails(T_FB_CHARGEAPPLYMASTER entity, List<T_FB_CHARGEAPPLYDETAIL> detailList)
        {
        }

        // 优化获取数据

        public T_FB_CHARGEAPPLYMASTER GetMasterForEdit(string masterID)
        {
            var master = dal.GetObjects().Include("T_FB_CHARGEAPPLYDETAIL").Include("T_FB_EXTENSIONALORDER").Include("T_FB_CHARGEAPPLYDETAIL.T_FB_SUBJECT")
               .FirstOrDefault(item => item.CHARGEAPPLYMASTERID == masterID);
            if (master == null || (master.T_FB_EXTENSIONALORDER != null))
            {
                return master;
            }
            // 更新费用明细数据

            using (BudgetAccountBLL bll = new BudgetAccountBLL())
            {
                var list = bll.GetBudgetAccountByPerson(master.OWNERID, master.OWNERPOSTID, master.OWNERCOMPANYID);
                var details = master.T_FB_CHARGEAPPLYDETAIL.ToList();
                
                details.ForEach(item =>
                    {
                        var find = list.Find(itemB => itemB.T_FB_SUBJECT.SUBJECTID == item.T_FB_SUBJECT.SUBJECTID && (itemB.ACCOUNTOBJECTTYPE + item.CHARGETYPE) == 4);
                        if (find != null)
                        {
                            item.USABLEMONEY = find.USABLEMONEY;
                        }
                        else
                        {
                            item.USABLEMONEY = 0; // 找不到可用的数据时，标为０.
                            // master.T_FB_CHARGEAPPLYDETAIL.Remove(item);
                        }
                    });
            }
            return master;
        }
        #endregion
        #region 添加版本控制分正式和航信
        /// <summary>
        /// 费用报销版本是否是 湖南航信
        /// 默认为 false
        /// </summary>
        /// <returns></returns>
        public bool GetVersionIsHuNan()
        {
            bool isReturn = false;
            try
            {
                string isForHuNanHangXingSalary = ConfigurationManager.AppSettings["isForHuNanHangXing"];
                 if (isForHuNanHangXingSalary == "true")
                 {
                     isReturn = true;
                 }
            }
            catch (Exception ex)
            {
                SMT.Foundation.Log.Tracer.Debug("获取版本信息出错"+ex.ToString());
            }
            return isReturn;
        }
        #endregion

        
    }
}

