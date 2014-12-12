/*
 * 文件名：LeaveTypeSetBLL.cs
 * 作  用：请假类型设置业务逻辑类
 * 创建人：吴鹏
 * 创建时间：2010年2月23日, 15:53:05
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

using SMT.HRM.CustomModel;
using SMT_HRM_EFModel;
using SMT.HRM.DAL;
using SMT.Foundation.Log;

namespace SMT.HRM.BLL
{
    public class LeaveTypeSetBLL : BaseBll<T_HR_LEAVETYPESET>, ILookupEntity
    {
        public LeaveTypeSetBLL()
        { }

        #region 获取数据
        /// <summary>
        /// 根据权限获取假期标准列表
        /// </summary>
        /// <param name="employeeID">员工ID</param>
        /// <returns>返回假期标准列表</returns>
        public List<T_HR_LEAVETYPESET> GetLeaveTypeSetAll(string employeeID)
        {
            string strFilter = "";
            List<object> queryParas = new List<object>();
            SetOrganizationFilter(ref strFilter, ref queryParas, employeeID, "T_HR_LEAVETYPESET");
            //IQueryable<T_HR_LEAVETYPESET> ents = dal.GetObjects();
            //ents = ents.Where(strFilter, queryParas.ToArray());
            IQueryable<T_HR_LEAVETYPESET> ents = null;
            
            ents = GetLeaveTypeSetRdListByEmployeeID("LEAVETYPESETID", strFilter, employeeID, queryParas.ToArray());
            //IQueryable<T_HR_LEAVETYPESET> ents = dal.GetObjects();
            //ents = ents.Where(strFilter, queryParas.ToArray());
            return ents.ToList();
        }
        /// <summary>
        /// 获取请假类型设置信息
        /// </summary>
        /// <param name="strLeaveTypeSetId">主键索引</param>
        /// <returns></returns>
        public T_HR_LEAVETYPESET GetLeaveTypeSetByID(string strLeaveTypeSetId)
        {
            try
            {
                if (string.IsNullOrEmpty(strLeaveTypeSetId))
                {
                    return null;
                }

                LeaveTypeSetDAL dalLeaveTypeSet = new LeaveTypeSetDAL();
                StringBuilder strfilter = new StringBuilder();
                List<string> objArgs = new List<string>();

                if (!string.IsNullOrEmpty(strLeaveTypeSetId))
                {
                    strfilter.Append(" LEAVETYPESETID == @0");
                    objArgs.Add(strLeaveTypeSetId);
                }

                T_HR_LEAVETYPESET entLTRd = dalLeaveTypeSet.GetLeaveTypeSetRdByMultSearch(strfilter.ToString(), objArgs.ToArray());
                return entLTRd;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 根据条件，获取请假类型设置信息
        /// </summary>
        /// <param name="strAttendanceSolutionId"></param>
        /// <param name="strSortKey"></param>
        /// <returns></returns>
        public IQueryable<T_HR_LEAVETYPESET> GetLeaveTypeSetRdListForAttendanceSolution(string strAttendanceSolutionId, string strSortKey)
        {
            var q = from af in dal.GetObjects<T_HR_ATTENDFREELEAVE>().Include("T_HR_ATTENDANCESOLUTION").Include("T_HR_LEAVETYPESET")
                    where af.T_HR_ATTENDANCESOLUTION.ATTENDANCESOLUTIONID == strAttendanceSolutionId
                    select af.T_HR_LEAVETYPESET;

            string strOrderBy = string.Empty;
            if (!string.IsNullOrEmpty(strSortKey))
            {
                strOrderBy = strSortKey;
            }
            else
            {
                strOrderBy = " LEAVETYPESETID ";
            }

            q = q.OrderBy(strOrderBy);

            return q;
        }

        /// <summary>
        /// 根据条件，获取请假类型设置信息
        /// </summary>
        /// <param name="strOwnerID">登录人的员工ID</param>
        /// <param name="strLeaveTypeValue">假期类别</param>
        /// <param name="strSortKey">排序字段</param>
        /// <returns></returns>
        public IQueryable<T_HR_LEAVETYPESET> GetAllLeaveTypeSetRdListByMultSearch(string strOwnerID, string strLeaveTypeValue, string strSortKey)
        {
            LeaveTypeSetDAL dalLeaveTypeSet = new LeaveTypeSetDAL();

            StringBuilder strfilter = new StringBuilder();
            List<object> objArgs = new List<object>();
            string strOrderBy = string.Empty;
            int iIndex = 0;

            if (!string.IsNullOrEmpty(strLeaveTypeValue))
            {
                strfilter.Append(" LEAVETYPEVALUE == @0 ");
                objArgs.Add(strLeaveTypeValue);
            }

            if (!string.IsNullOrEmpty(strSortKey))
            {
                strOrderBy = strSortKey;
            }
            else
            {
                strOrderBy = " LEAVETYPESETID ";
            }

            string filterString = strfilter.ToString();

            SetOrganizationFilter(ref filterString, ref objArgs, strOwnerID, "T_HR_LEAVETYPESET");

            var q = dalLeaveTypeSet.GetLeaveTypeSetRdListByMultSearch(strOrderBy, filterString, objArgs.ToArray());
            return q;
        }

        /// <summary>
        /// 根据条件，获取请假类型设置信息,并进行分页
        /// </summary>
        /// <param name="strOwnerID">登录人的员工ID</param>
        /// <param name="strLeaveTypeValue">假期类别</param>
        /// <param name="strSortKey">排序字段</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="pageSize">页码大小</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>请假类型信息</returns>
        public IQueryable<T_HR_LEAVETYPESET> GetLeaveTypeSetRdListByMultSearch(string strOwnerID, string strLeaveTypeValue,
            string strSortKey, int pageIndex, int pageSize, ref int pageCount)
        {
            var q = GetAllLeaveTypeSetRdListByMultSearch(strOwnerID, strLeaveTypeValue, strSortKey);

            return Utility.Pager<T_HR_LEAVETYPESET>(q, pageIndex, pageSize, ref pageCount);
        }

        #endregion

        #region 操作

        /// <summary>
        /// 新增请假类型设置信息
        /// </summary>
        /// <param name="entTemp"></param>
        /// <returns></returns>
        public string AddLeaveTypeSet(T_HR_LEAVETYPESET entTemp)
        {
            string strMsg = string.Empty;
            try
            {
                if (entTemp == null)
                {
                    return "{REQUIREDFIELDS}";
                }

                bool flag = false;
                StringBuilder strFilter = new StringBuilder();
                List<string> objArgs = new List<string>();

                strFilter.Append(" LEAVETYPENAME == @0");
                strFilter.Append(" && LEAVETYPEVALUE == @1");

                objArgs.Add(entTemp.LEAVETYPENAME);
                objArgs.Add(entTemp.LEAVETYPEVALUE);

                LeaveTypeSetDAL dalLeaveTypeSet = new LeaveTypeSetDAL();
                flag = dalLeaveTypeSet.IsExistsRd(strFilter.ToString(), objArgs.ToArray());

                if (flag)
                {
                    return "{ALREADYEXISTSRECORD}";
                }

                dalLeaveTypeSet.Add(entTemp);

                strMsg = "{SAVESUCCESSED}";

            }
            catch (Exception ex)
            {
                strMsg = ex.Message.ToString();
            }

            return strMsg;
        }

        /// <summary>
        /// 修改请假类型设置信息
        /// </summary>
        /// <param name="entTemp"></param>
        /// <returns></returns>
        public string ModifyLeaveTypeSet(T_HR_LEAVETYPESET entTemp)
        {
            string strMsg = string.Empty;
            try
            {
                if (entTemp == null)
                {
                    return "{REQUIREDFIELDS}";
                }


                bool flag = false;
                StringBuilder strFilter = new StringBuilder();
                List<string> objArgs = new List<string>();

                strFilter.Append(" LEAVETYPESETID == @0");

                objArgs.Add(entTemp.LEAVETYPESETID);

                LeaveTypeSetDAL dalLeaveTypeSet = new LeaveTypeSetDAL();
                flag = dalLeaveTypeSet.IsExistsRd(strFilter.ToString(), objArgs.ToArray());

                if (!flag)
                {
                    return "{NOTFOUND}";
                }

                T_HR_LEAVETYPESET entUpdate = dalLeaveTypeSet.GetLeaveTypeSetRdByMultSearch(strFilter.ToString(), objArgs.ToArray());
                Utility.CloneEntity(entTemp, entUpdate);
                if (entTemp.LEAVETYPEVALUE != (Convert.ToInt32(Common.LeaveTypeValue.AdjustLeave) + 1).ToString() && entTemp.LEAVETYPEVALUE != (Convert.ToInt32(Common.LeaveTypeValue.AnnualLeave) + 1).ToString())
                {
                    try
                    {
                        DateTime dtNow = DateTime.Now;
                        var entFrees = from ent in dal.GetObjects<T_HR_FREELEAVEDAYSET>()
                                       where ent.T_HR_LEAVETYPESET.LEAVETYPESETID == entTemp.LEAVETYPESETID
                                       select ent;
                        //不存在设置的情况则修改最大请假天数
                        if (entFrees.Count() == 0)
                        {
                            //格式化为当前年份
                            //dtNow = DateTime.Parse(dtNow.Year.ToString() + "-01-01");
                            var ents = from ent in dal.GetObjects<T_HR_EMPLOYEELEVELDAYCOUNT>()
                                       where ent.LEAVETYPESETID == entTemp.LEAVETYPESETID
                                       && ent.EFFICDATE.Value.Year == dtNow.Year
                                       select ent;
                            if (ents.Count() > 0)
                            {
                                foreach (var ent in ents)
                                {
                                    ent.REMARK += "原来为：" + ent.DAYS.ToString() + ";修改后重新设置天数:" + entTemp.MAXDAYS.ToString();
                                    ent.DAYS = entTemp.MAXDAYS;
                                    dal.UpdateFromContext(ent);
                                }
                                int intResult = dal.SaveContextChanges();
                                if (intResult > 0)
                                {
                                    SMT.Foundation.Log.Tracer.Debug("公共假期类型：" + entTemp.LEAVETYPENAME + "修改带薪假天数成功");
                                }
                                else
                                {
                                    SMT.Foundation.Log.Tracer.Debug("公共假期类型：" + entTemp.LEAVETYPENAME + "修改带薪假天数失败");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        SMT.Foundation.Log.Tracer.Debug("公共假期类型：" + entTemp.LEAVETYPENAME + "修改带薪假天数出现错误："+ ex.ToString()); 
                    }
                }
                    
                dalLeaveTypeSet.Update(entUpdate);
                strMsg = "{SAVESUCCESSED}";
            }
            catch (Exception ex)
            {
                strMsg = ex.Message.ToString();
            }
            return strMsg;
        }

        public decimal GetMaxDays(List<T_HR_EMPLOYEE> listEmployees, List<T_HR_EMPLOYEEENTRY> entrys, List<T_HR_EMPLOYEEPOST> employeePosts, T_HR_EMPLOYEELEVELDAYCOUNT employeeFreeCount, bool isBreak, List<T_HR_ATTENDFREELEAVE> freeLeaves)
        {
            decimal deDays = 0;
            try
            {
                //获取某个员工的入职信息
                var qw = from ent in entrys
                         where ent.T_HR_EMPLOYEE.EMPLOYEEID == employeeFreeCount.EMPLOYEEID                         
                         select ent;
                //获取员工的岗位信息
                var qp = from ent in employeePosts
                         where ent.T_HR_EMPLOYEE.EMPLOYEEID == employeeFreeCount.EMPLOYEEID
                         && ent.T_HR_EMPLOYEE.OWNERCOMPANYID == employeeFreeCount.OWNERCOMPANYID
                         select ent;
                var entEmployees = from ent in listEmployees
                                   where ent.EMPLOYEEID == employeeFreeCount.EMPLOYEEID
                                   select ent;
                
                //isBreak 为true 调用处则跳出循环
                if (qw == null)
                {
                    isBreak = true;
                }
                if (qp == null)
                {
                    isBreak = true;
                }
                if (entEmployees == null)
                {
                    isBreak = true;
                }
                T_HR_EMPLOYEE entEmployee = entEmployees.FirstOrDefault();
                T_HR_EMPLOYEEPOST entPost = qp.FirstOrDefault();
                if (entPost == null)
                {
                    isBreak = true;
                }
                T_HR_EMPLOYEEENTRY entEntry = qw.FirstOrDefault();
                if (entEntry == null)
                {
                    isBreak = true;
                }
                DateTime dtEntryDate = entEntry.ENTRYDATE.Value;
                TimeSpan tsWorkTime = DateTime.Now.Subtract(dtEntryDate);
                decimal dCurWorkAge = 0, dEmployeePostLevel = 0;
                dCurWorkAge = decimal.Round(tsWorkTime.Days / 30, 0);
                decimal.TryParse(entPost.POSTLEVEL.ToString(), out dEmployeePostLevel);
                foreach (T_HR_ATTENDFREELEAVE entAttendFreeLeave in freeLeaves)
                {
                    T_HR_LEAVETYPESET entLeaveTypeSet = entAttendFreeLeave.T_HR_LEAVETYPESET;
                    if (entLeaveTypeSet == null)
                    {                        
                        continue;
                    }

                    string strVacType = entLeaveTypeSet.LEAVETYPEVALUE;
                    string strFineType = entLeaveTypeSet.FINETYPE;

                    //如果是调休假，就不需要自动生成
                    if (strVacType == (Convert.ToInt32(Common.LeaveTypeValue.AdjustLeave) + 1).ToString())
                    {
                        continue;
                    }

                    //获取假期标准的带薪假设置记录
                    FreeLeaveDaySetBLL bllFreeLeave = new FreeLeaveDaySetBLL();
                    IQueryable<T_HR_FREELEAVEDAYSET> entFreeLeaves = bllFreeLeave.GetFreeLeaveDaySetByLeaveTypeID(entLeaveTypeSet.LEAVETYPESETID);

                    if (strFineType != (Convert.ToInt32(Common.LeaveFineType.Free) + 1).ToString())
                    {                        
                        continue;
                    }

                    decimal dPostLevelStrict = 0;
                    decimal.TryParse(entLeaveTypeSet.POSTLEVELRESTRICT, out dPostLevelStrict);

                    if (dPostLevelStrict > dEmployeePostLevel)
                    {                        
                        continue;
                    }

                    if (entLeaveTypeSet.ENTRYRESTRICT == (Convert.ToInt32(Common.IsChecked.Yes) + 1).ToString())
                    {
                        if (entEmployee.EMPLOYEESTATE != "1")
                        {                            
                            continue;
                        }
                    }

                    if (entLeaveTypeSet.SEXRESTRICT != "2")
                    {
                        if (entLeaveTypeSet.SEXRESTRICT != entEmployee.SEX)
                        {                            
                            continue;
                        }
                    }

                    if (!string.IsNullOrEmpty(entLeaveTypeSet.POSTLEVELRESTRICT))
                    {
                        decimal dPostLeavlStrict = decimal.Parse(entLeaveTypeSet.POSTLEVELRESTRICT);
                    }

                    int j = -1;
                    decimal dLeaveDay = 0;
                    string LeaveDayName = string.Empty;
                    if (entFreeLeaves.Count() > 0)
                    {
                        for (int i = 0; i < entFreeLeaves.Count(); i++)
                        {
                            if (entFreeLeaves.ToList()[i].MINIMONTH > dCurWorkAge)
                            {
                                continue;
                            }

                            if (entFreeLeaves.ToList()[i].MAXMONTH < dCurWorkAge)
                            {
                                continue;
                            }

                            dLeaveDay = entFreeLeaves.ToList()[i].LEAVEDAYS.Value;
                            LeaveDayName = entLeaveTypeSet.LEAVETYPENAME;
                            j = i;
                            break;
                        }
                    }
                    else
                    {
                        dLeaveDay = entLeaveTypeSet.MAXDAYS.Value;
                        j = 1;
                    }

                    decimal dAddDays = 0;
                    if (j > -1)
                    {
                        if (entFreeLeaves.Count() > 0)
                        {
                            if (j == 0)
                            {
                                dAddDays = dLeaveDay;

                            }
                            else
                            {
                                dAddDays = dLeaveDay - entFreeLeaves.ToList()[j - 1].LEAVEDAYS.Value;
                            }
                        }
                        else
                        {
                            dAddDays = dLeaveDay;
                        }
                    } 
                    string strNumOfDecDefault = "0.5";
                    dAddDays = RoundOff(dAddDays, strNumOfDecDefault, 1);
                    dLeaveDay = RoundOff(dLeaveDay, strNumOfDecDefault, 1);
                    deDays = dAddDays;
                }
            }
            catch (Exception ex)
            {
 
            }
            return deDays;
        }


        private decimal RoundOff(decimal dValue, string strNumOfDec, int ilength)
        {
            decimal dRes = 0;
            try
            {
                dRes = decimal.Round(dValue, ilength);

                if (!string.IsNullOrEmpty(strNumOfDec))
                {
                    decimal dNumOfDec = 0, dCheck = 0;
                    decimal.TryParse(strNumOfDec, out dNumOfDec);

                    string[] strlist = dRes.ToString().Split('.');
                    if (strlist.Length == 2)
                    {
                        decimal.TryParse("0." + strlist[1].ToString(), out dCheck);

                        if (dCheck > dNumOfDec)
                        {
                            dRes = decimal.Parse(strlist[0]) + 1;
                        }
                        else
                        {
                            dRes = decimal.Parse(strlist[0]) + dNumOfDec;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.SaveLog(ex.ToString());
            }

            return dRes;
        }

        /// <summary>
        /// 根据主键索引，删除请假类型设置信息(注：暂定为物理删除)
        /// </summary>
        /// <param name="strVacationId">主键索引</param>
        /// <returns></returns>
        public string DeleteLeaveTypeSet(string strLeaveTypeSetId)
        {
            string strMsg = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(strLeaveTypeSetId))
                {
                    return "{REQUIREDFIELDS}";
                }

                bool flag = false;
                StringBuilder strFilter = new StringBuilder();
                List<string> objArgs = new List<string>();

                strFilter.Append(" LEAVETYPESETID == @0");

                objArgs.Add(strLeaveTypeSetId);

                LeaveTypeSetDAL dalLeaveTypeSet = new LeaveTypeSetDAL();
                flag = dalLeaveTypeSet.IsExistsRd(strFilter.ToString(), objArgs.ToArray());

                if (!flag)
                {
                    return "{NOTFOUND}";
                }

                T_HR_LEAVETYPESET entDel = dalLeaveTypeSet.GetLeaveTypeSetRdByMultSearch(strFilter.ToString(), objArgs.ToArray());

                var entAL = from s in dal.GetObjects<T_HR_ATTENDFREELEAVE>().Include("T_HR_ATTENDANCESOLUTION").Include("T_HR_LEAVETYPESET")
                            where s.T_HR_LEAVETYPESET.LEAVETYPESETID == strLeaveTypeSetId
                            select s;

                if (entAL.Count() > 0)
                {
                    return "{EXISTRELATIONRECORD}";
                }

                var entLR = from r in dal.GetObjects<T_HR_EMPLOYEELEAVERECORD>().Include("T_HR_LEAVETYPESET")
                            where r.T_HR_LEAVETYPESET.LEAVETYPESETID == strLeaveTypeSetId
                            select r;

                if (entLR.Count() > 0)
                {
                    return "{EXISTRELATIONRECORD}";
                    //dal.Delete(entLR);
                }

                entDel.T_HR_FREELEAVEDAYSET.Load();

                dal.DeleteFromContext(entDel);
                dal.SaveContextChanges();

                strMsg = "{DELETESUCCESSED}";
            }
            catch (Exception ex)
            {
                strMsg = ex.Message.ToString();
            }

            return strMsg;
        }

        #endregion

        #region ILookupEntity 成员

        public EntityObject[] GetLookupData(int pageIndex, int pageSize, string sort, string filterString, IList<object> paras, ref int pageCount, string userID)
        {
            LeaveTypeSetDAL dalLeaveTypeSet = new LeaveTypeSetDAL();

            List<object> queryParas = new List<object>();
            string strOrderBy = string.Empty;
            string strEmployeeID = string.Empty;
            #region 是否是从请假处传来数据
            //只有从请假申请中才传来leaverecordemployeeid
            if (filterString != null)
            {
                if (filterString.IndexOf("leaverecordemployeeid") > 0)
                {
                    strEmployeeID = paras[0].ToString();
                    filterString = string.Empty;
                    //清空记录
                    //paras.RemoveAt(0);
                }
                else
                {
                    if (paras.Count() > 0)
                    {
                        for (int i = 0; i < paras.Count(); i++)
                        {
                            queryParas.Add(paras[i]);
                        }
                    }
                }
            }
            else
            {
                if (paras.Count() > 0)
                {
                    for (int i = 0; i < paras.Count(); i++)
                    {
                        queryParas.Add(paras[i]);
                    }
                }
            }
            #endregion
            
            

            strOrderBy = " LEAVETYPESETID ";

            //受权限限制，假如权限不够则查询条件filterString可能为空，则要判断
            if (string.IsNullOrWhiteSpace(filterString))
            {
                filterString = string.Empty;
            }
            SetOrganizationFilter(ref filterString, ref queryParas, userID, "T_HR_LEAVETYPESET");
            IQueryable<T_HR_LEAVETYPESET> ents =null;
            if (string.IsNullOrEmpty(strEmployeeID))
            {
                ents = dalLeaveTypeSet.GetLeaveTypeSetRdListByMultSearch(strOrderBy, filterString, queryParas.ToArray());
            }
            else
            {
                ents = GetLeaveTypeSetRdListByEmployeeID(strOrderBy, filterString,strEmployeeID, queryParas.ToArray());
            }
            if (ents == null)
            {
                return null;
            }
            return ents.Count() > 0 ? ents.ToArray() : null;
        }

        /// <summary>
        /// 获取指定条件的公共假期设置信息
        /// </summary>        
        /// <param name="strOrderBy">排序字段</param>
        /// <param name="strFilter">查询语句</param>
        /// <param name="objArgs">查询参数集合</param>
        /// <param name="employeeID">员工ID</param>
        /// <returns>返回请假类型</returns>
        public IQueryable<T_HR_LEAVETYPESET> GetLeaveTypeSetRdListByEmployeeID(string strOrderBy, string strFilter, string employeeID, params object[] objArgs)
        {
            var q = from l in dal.GetObjects<T_HR_LEAVETYPESET>()
                    select l;

            if (objArgs.Count() > 0 && !string.IsNullOrEmpty(strFilter))
            {
                q = q.Where(strFilter, objArgs);
            }
            EmployeeBLL employeeBLL = new EmployeeBLL();
            T_HR_EMPLOYEE employee = employeeBLL.GetEmployeeByID(employeeID);
            if (employee == null)
            {
                //不存在员工
                return null;
            }
            string strBirthDay = string.Empty;
            string strSex = string.Empty;
            if (employee.BIRTHDAY != null)
            {
                strBirthDay = employee.BIRTHDAY.ToString();
            }
            if (employee.SEX != null)
            {
                //0  女性  1 男性
                strSex = employee.SEX.ToString(); 
            }
            DateTime dtBirthday = new DateTime();
            DateTime dtYouth = new DateTime();
            DateTime.TryParse(strBirthDay, out dtBirthday);
            DateTime.TryParse(DateTime.Now.Year.ToString() + "-05-04", out dtYouth);
            
            List<T_HR_LEAVETYPESET> listSets = new List<T_HR_LEAVETYPESET>();
            foreach (var ent in q)
            {
                //五四
                if (ent.LEAVETYPEVALUE == "12")
                {
                    //小于等于28岁才有五四青年节
                    if (dtBirthday.AddYears(28) >= dtYouth)
                    {
                        listSets.Add(ent);
                    }
                }
                //三八
                else if (ent.LEAVETYPEVALUE == "13")
                {
                    if (strSex == "0")
                    {
                        listSets.Add(ent);
                    }
                }
                else
                {
                    if (ent.SEXRESTRICT != "2")
                    {
                        //性别不是为不限
                        if (ent.SEXRESTRICT == strSex)
                        {
                            listSets.Add(ent);
                        }
                    }
                    else
                    {
                        listSets.Add(ent);
                    }               
                }
            }

            return listSets.AsQueryable().OrderBy(strOrderBy);
        }

        #endregion


    }
}
