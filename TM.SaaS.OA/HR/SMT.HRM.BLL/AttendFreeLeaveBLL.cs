
/*
 * 文件名：AttendFreeLeaveBLL.cs
 * 作  用：考勤方案带薪假 业务逻辑类
 * 创建人：吴鹏
 * 创建时间：2010-3-25 15:36:56
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
using SMT.HRM.DAL;

namespace SMT.HRM.BLL
{
    public class AttendFreeLeaveBLL : BaseBll<T_HR_ATTENDFREELEAVE>
    {
        public AttendFreeLeaveBLL()
        { }

        #region 获取数据

        /// <summary>
        /// 获取考勤方案带薪假信息
        /// </summary>
        /// <param name="strAttendFreeLeaveId">主键索引</param>
        /// <returns></returns>
        public T_HR_ATTENDFREELEAVE GetAttendFreeLeaveByID(string strAttendFreeLeaveId)
        {
            if (string.IsNullOrEmpty(strAttendFreeLeaveId))
            {
                return null;
            }

            AttendFreeLeaveDAL dalAttendFreeLeave = new AttendFreeLeaveDAL();
            StringBuilder strfilter = new StringBuilder();
            List<string> objArgs = new List<string>();

            if (!string.IsNullOrEmpty(strAttendFreeLeaveId))
            {
                strfilter.Append(" ATTENDFREELEAVEID == @0");
                objArgs.Add(strAttendFreeLeaveId);
            }

            T_HR_ATTENDFREELEAVE entRd = dalAttendFreeLeave.GetAttendFreeLeaveRdByMultSearch(strfilter.ToString(), objArgs.ToArray());
            return entRd;
        }

        /// <summary>
        /// 根据条件，获取考勤方案带薪假信息
        /// </summary>
        /// <param name="strAttendanceSolutionID"></param>
        /// <param name="strFreeLeaveDaySetID"></param>
        /// <param name="strSortKey"></param>
        /// <returns></returns>
        public IQueryable<T_HR_ATTENDFREELEAVE> GetAllAttendFreeLeaveRdListByMultSearch(string strAttendanceSolutionID, string strFreeLeaveDaySetID, string strSortKey)
        {
            AttendFreeLeaveDAL dalAttendFreeLeave = new AttendFreeLeaveDAL();

            StringBuilder strfilter = new StringBuilder();
            List<string> objArgs = new List<string>();
            string strOrderBy = string.Empty;
            int iIndex = 0;

            if (!string.IsNullOrEmpty(strAttendanceSolutionID))
            {
                strfilter.Append(" T_HR_ATTENDANCESOLUTION.ATTENDANCESOLUTIONID = @0");
                objArgs.Add(strAttendanceSolutionID);
            }

            if (!string.IsNullOrEmpty(strFreeLeaveDaySetID))
            {
                if (!string.IsNullOrEmpty(strfilter.ToString()))
                {
                    strfilter.Append(" &&");
                }

                if (objArgs.Count() > 0)
                {
                    iIndex = objArgs.Count();
                }

                strfilter.Append(" T_HR_ATTENDANCESOLUTION.FREELEAVEDAYSETID == @" + iIndex.ToString());
                objArgs.Add(strFreeLeaveDaySetID);
            }

            if (!string.IsNullOrEmpty(strSortKey))
            {
                strOrderBy = strSortKey;
            }
            else
            {
                strOrderBy = " ATTENDFREELEAVEID ";
            }

            var q = dalAttendFreeLeave.GetAttendFreeLeaveRdListByMultSearch(strOrderBy, strfilter.ToString(), objArgs.ToArray());
            return q;
        }

        /// <summary>
        /// 根据条件，获取考勤方案带薪假信息,并进行分页
        /// </summary>
        /// <param name="strAttendFreeLeaveName">考勤方案编号</param>
        /// <param name="strFreeLeaveDaySetID">带薪假设置编号</param>
        /// <param name="strSortKey">排序字段</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="pageSize">页码大小</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>考勤方案带薪假信息</returns>
        public IQueryable<T_HR_ATTENDFREELEAVE> GetAttendFreeLeaveRdListByMultSearch(string strAttendanceSolutionID, string strFreeLeaveDaySetID,
            string strSortKey, int pageIndex, int pageSize, ref int pageCount)
        {
            var q = GetAllAttendFreeLeaveRdListByMultSearch(strAttendanceSolutionID, strFreeLeaveDaySetID, strSortKey);

            return Utility.Pager<T_HR_ATTENDFREELEAVE>(q, pageIndex, pageSize, ref pageCount);
        }

        #endregion

        #region 操作

        /// <summary>
        /// 新增考勤方案带薪假信息
        /// </summary>
        /// <param name="entTemp"></param>
        /// <returns></returns>
        public string AddAttendFreeLeave(T_HR_ATTENDFREELEAVE entTemp)
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

                strFilter.Append(" T_HR_ATTENDANCESOLUTION.ATTENDANCESOLUTIONID == @0");
                strFilter.Append(" && T_HR_LEAVETYPESET.LEAVETYPESETID == @1");

                objArgs.Add(entTemp.T_HR_ATTENDANCESOLUTION.ATTENDANCESOLUTIONID);
                objArgs.Add(entTemp.T_HR_LEAVETYPESET.LEAVETYPESETID);

                AttendFreeLeaveDAL dalAttendFreeLeave = new AttendFreeLeaveDAL();
                flag = dalAttendFreeLeave.IsExistsRd(strFilter.ToString(), objArgs.ToArray());

                if (flag)
                {
                    return "{ALREADYEXISTSRECORD}";
                }

                T_HR_ATTENDFREELEAVE ent = new T_HR_ATTENDFREELEAVE();
                Utility.CloneEntity<T_HR_ATTENDFREELEAVE>(entTemp, ent);

                ent.T_HR_ATTENDANCESOLUTIONReference.EntityKey =
                    new System.Data.EntityKey("TM_SaaS_OA_EFModelContext.T_HR_ATTENDANCESOLUTION", "ATTENDANCESOLUTIONID", entTemp.T_HR_ATTENDANCESOLUTION.ATTENDANCESOLUTIONID);

                ent.T_HR_LEAVETYPESETReference.EntityKey =
                    new System.Data.EntityKey("TM_SaaS_OA_EFModelContext.T_HR_LEAVETYPESET", "LEAVETYPESETID", entTemp.T_HR_LEAVETYPESET.LEAVETYPESETID);
                
                dalAttendFreeLeave.Add(ent);

                strMsg = "{SAVESUCCESSED}";

            }
            catch (Exception ex)
            {
                strMsg = ex.Message.ToString();
            }

            return strMsg;
        }

        /// <summary>
        /// 修改考勤方案带薪假信息
        /// </summary>
        /// <param name="entTemp"></param>
        /// <returns></returns>
        public string ModifyAttendFreeLeave(T_HR_ATTENDFREELEAVE entTemp)
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

                strFilter.Append(" ATTENDFREELEAVEID == @0");

                objArgs.Add(entTemp.ATTENDFREELEAVEID);

                AttendFreeLeaveDAL dalAttendFreeLeave = new AttendFreeLeaveDAL();
                flag = dalAttendFreeLeave.IsExistsRd(strFilter.ToString(), objArgs.ToArray());

                if (!flag)
                {
                    return "{NOTFOUND}";
                }

                T_HR_ATTENDFREELEAVE entUpdate = dalAttendFreeLeave.GetAttendFreeLeaveRdByMultSearch(strFilter.ToString(), objArgs.ToArray());
                entUpdate.T_HR_ATTENDANCESOLUTION = entTemp.T_HR_ATTENDANCESOLUTION;
                entUpdate.T_HR_LEAVETYPESET = entTemp.T_HR_LEAVETYPESET;
                entUpdate.REMARK = entTemp.REMARK;
                entUpdate.CREATEUSERID = entTemp.CREATEUSERID;
                entUpdate.CREATEDATE = entTemp.CREATEDATE;
                entUpdate.UPDATEUSERID = entTemp.UPDATEUSERID;
                entUpdate.UPDATEDATE = entTemp.UPDATEDATE;

                dalAttendFreeLeave.Update(entUpdate);
                strMsg = "{SAVESUCCESSED}";
            }
            catch (Exception ex)
            {
                strMsg = ex.Message.ToString();
            }

            return strMsg;
        }

        /// <summary>
        /// 根据主键索引，删除考勤方案带薪假信息(注：暂定为物理删除)
        /// </summary>
        /// <param name="strAttendFreeLeaveId">主键索引</param>
        /// <returns></returns>
        public string DeleteAttendFreeLeave(string strAttendFreeLeaveId)
        {
            string strMsg = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(strAttendFreeLeaveId))
                {
                    return "{REQUIREDFIELDS}";
                }

                bool flag = false;
                StringBuilder strFilter = new StringBuilder();
                List<string> objArgs = new List<string>();

                strFilter.Append(" ATTENDFREELEAVEID == @0");

                objArgs.Add(strAttendFreeLeaveId);

                AttendFreeLeaveDAL dalAttendFreeLeave = new AttendFreeLeaveDAL();
                flag = dalAttendFreeLeave.IsExistsRd(strFilter.ToString(), objArgs.ToArray());

                if (!flag)
                {
                    return "{NOTFOUND}";
                }

                T_HR_ATTENDFREELEAVE entDel = dalAttendFreeLeave.GetAttendFreeLeaveRdByMultSearch(strFilter.ToString(), objArgs.ToArray());
                dalAttendFreeLeave.Delete(entDel);

                strMsg = "{DELETESUCCESSED}";
            }
            catch (Exception ex)
            {
                strMsg = ex.Message.ToString();
            }

            return strMsg;
        }

        #endregion

        /// <summary>
        /// 根据条件，获取考勤方案带薪假信息
        /// </summary>
        /// <param name="strAttendSolID"></param>
        /// <returns></returns>
        public IQueryable<T_HR_ATTENDFREELEAVE> GetAttendFreeLeaveByAttendSolID(string strAttendSolID)
        {
            if (string.IsNullOrEmpty(strAttendSolID))
            {
                return null;
            }

            AttendFreeLeaveDAL dalAttendFreeLeave = new AttendFreeLeaveDAL();
            StringBuilder strfilter = new StringBuilder();
            List<string> objArgs = new List<string>();

            if (!string.IsNullOrEmpty(strAttendSolID))
            {
                strfilter.Append(" T_HR_ATTENDANCESOLUTION.ATTENDANCESOLUTIONID == @0");
                objArgs.Add(strAttendSolID);
            }

            string strOrderBy = " ATTENDFREELEAVEID ";

            var q = dalAttendFreeLeave.GetAttendFreeLeaveRdListByMultSearch(strOrderBy, strfilter.ToString(), objArgs.ToArray());
            return q;
        }

        /// <summary>
        /// 根据考勤方案ID删除考勤方案带薪假记录
        /// </summary>
        /// <param name="strAttendSolID"></param>
        public void ModifyAttendFreeLeaveByAttSolID(string strAttendSolID)
        {
            try
            {
                IQueryable<T_HR_ATTENDFREELEAVE> ents = GetAttendFreeLeaveByAttendSolID(strAttendSolID);
                if (ents != null)
                {
                    foreach (T_HR_ATTENDFREELEAVE ent in ents)
                    {
                        DeleteAttendFreeLeave(strAttendSolID);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    Utility.SaveLog(ex.Message);
                }
                else
                {
                    Utility.SaveLog(ex.InnerException.Message);
                }
            }
        }

        /// <summary>
        /// 初始化五四三八数据
        /// </summary>
        public void InitYouth()
        {
            try
            {
                var ents = from ent in dal.GetObjects<T_HR_ATTENDANCESOLUTION>()
                           where ent.CHECKSTATE == "2"
                           orderby ent.CREATEDATE ascending
                           select ent;
                SMT.Foundation.Log.Tracer.Debug("总数量为：" + ents.Count().ToString());
                int intCount = 0;
                foreach (var ent in ents)
                {
                    //if (ent.ATTENDANCESOLUTIONID == "1214E853-3536-456F-BBC9-66A059A903A6")
                    //{
                    //    continue;
                    //}
                    #region 三八
                    T_HR_LEAVETYPESET typeset = new T_HR_LEAVETYPESET();
                    typeset.LEAVETYPESETID = Guid.NewGuid().ToString();
                    typeset.LEAVETYPENAME = ent.ATTENDANCESOLUTIONNAME + "三八妇女节";
                    typeset.LEAVETYPEVALUE = "13";
                    typeset.ISFREELEAVEDAY = "2";
                    typeset.MAXDAYS = (decimal)0.53;
                    typeset.FINETYPE = "1";
                    typeset.SEXRESTRICT = "0";
                    typeset.ENTRYRESTRICT = "1";
                    typeset.OWNERCOMPANYID = ent.OWNERCOMPANYID;
                    typeset.OWNERDEPARTMENTID = ent.OWNERDEPARTMENTID;
                    typeset.OWNERPOSTID = ent.OWNERPOSTID;
                    typeset.OWNERID = ent.OWNERID;
                    typeset.CREATEDATE = DateTime.Now;
                    typeset.UPDATEDATE = DateTime.Now;
                    typeset.UPDATEUSERID = ent.OWNERID;
                    typeset.CREATECOMPANYID = ent.OWNERCOMPANYID;
                    typeset.CREATEDEPARTMENTID = ent.OWNERDEPARTMENTID;
                    typeset.CREATEPOSTID = ent.OWNERPOSTID;
                    typeset.CREATEUSERID = ent.OWNERID;
                    typeset.REMARK = "系统产生三八妇女节";
                    var entExist = from ent1 in dal.GetObjects<T_HR_LEAVETYPESET>()
                                   where ent1.LEAVETYPEVALUE == "13" && ent1.OWNERCOMPANYID == ent.OWNERCOMPANYID
                                   select ent1;
                    LeaveTypeSetBLL bllLeaveTypeSet = new LeaveTypeSetBLL();
                    if (entExist.Count() == 0)
                    {

                        string aaa = bllLeaveTypeSet.AddLeaveTypeSet(typeset);
                        if (aaa == "{SAVESUCCESSED}")
                        {
                            SMT.Foundation.Log.Tracer.Debug(ent.ATTENDANCESOLUTIONNAME + "产生了三八节");
                        }
                        else
                        {
                            SMT.Foundation.Log.Tracer.Debug(ent.ATTENDANCESOLUTIONNAME + "没有产生三八节");
                            continue;
                        }
                    }
                    else
                    {
                        typeset = entExist.FirstOrDefault();
                    }
                    //int iii = dal.Add(typeset);
                    //if (iii > 0)
                    //{
                    //    SMT.Foundation.Log.Tracer.Debug(ent.ATTENDANCESOLUTIONNAME +"产生了三八节");
                    //}
                    //dal.SaveContextChanges();
                    //dal.AddToContext(typeset);
                    T_HR_FREELEAVEDAYSET freeTypeSet = new T_HR_FREELEAVEDAYSET();
                    freeTypeSet.FREELEAVEDAYSETID = Guid.NewGuid().ToString();
                    freeTypeSet.T_HR_LEAVETYPESET = typeset;
                    //freeTypeSet.T_HR_LEAVETYPESETReference.EntityKey =
                    //new System.Data.EntityKey("TM_SaaS_OA_EFModelContext.T_HR_LEAVETYPESET", "LEAVETYPESETID", typeset.LEAVETYPESETID);
                    freeTypeSet.MINIMONTH = 0;
                    freeTypeSet.MAXMONTH = 9999;
                    freeTypeSet.LEAVEDAYS = (decimal)0.53;
                    freeTypeSet.ISPERFECTATTENDANCEFACTOR = "1";
                    freeTypeSet.OFFESTTYPE = "1";
                    freeTypeSet.OWNERCOMPANYID = ent.OWNERCOMPANYID;
                    freeTypeSet.OWNERDEPARTMENTID = ent.OWNERDEPARTMENTID;
                    freeTypeSet.OWNERPOSTID = ent.OWNERPOSTID;
                    freeTypeSet.OWNERID = ent.OWNERID;
                    freeTypeSet.CREATEDATE = DateTime.Now;
                    freeTypeSet.UPDATEDATE = DateTime.Now;
                    freeTypeSet.UPDATEUSERID = ent.OWNERID;
                    freeTypeSet.CREATECOMPANYID = ent.OWNERCOMPANYID;
                    freeTypeSet.CREATEDEPARTMENTID = ent.OWNERDEPARTMENTID;
                    freeTypeSet.CREATEPOSTID = ent.OWNERPOSTID;
                    freeTypeSet.CREATEUSERID = ent.OWNERID;
                    freeTypeSet.REMARK = "系统产生三八妇女节带薪假";
                    //dal.AddToContext(freeTypeSet);
                    FreeLeaveDaySetBLL bllFreeLeaveDaySet = new FreeLeaveDaySetBLL();
                    string aa2 = bllFreeLeaveDaySet.AddFreeLeaveDaySet(freeTypeSet);
                    if (aa2 == "{SAVESUCCESSED}")
                    {
                        SMT.Foundation.Log.Tracer.Debug(ent.ATTENDANCESOLUTIONNAME + "产生了带薪假三八节");
                    }
                    else
                    {
                        SMT.Foundation.Log.Tracer.Debug(ent.ATTENDANCESOLUTIONNAME + "没有产生带薪假三八节");
                        continue;
                    }
                    T_HR_ATTENDFREELEAVE attendFree = new T_HR_ATTENDFREELEAVE();
                    attendFree.ATTENDFREELEAVEID = Guid.NewGuid().ToString();
                    var entSol = from ent11 in dal.GetObjects<T_HR_ATTENDANCESOLUTION>()
                                 where ent11.ATTENDANCESOLUTIONID == ent.ATTENDANCESOLUTIONID
                                 select ent11;
                    attendFree.T_HR_ATTENDANCESOLUTION = entSol.FirstOrDefault();
                    var entSet = from ent11 in dal.GetObjects<T_HR_LEAVETYPESET>()
                                 where ent11.LEAVETYPESETID == typeset.LEAVETYPESETID
                                 select ent11;

                    attendFree.T_HR_LEAVETYPESET = entSet.FirstOrDefault();
                    attendFree.REMARK = "自动产生三八节";
                    attendFree.CREATEDATE = DateTime.Now;
                    attendFree.UPDATEDATE = DateTime.Now;
                    attendFree.CREATEUSERID = ent.OWNERID;
                    attendFree.UPDATEUSERID = ent.OWNERID;
                    string bbb = AddAttendFreeLeave(attendFree);
                    if (bbb == "{SAVESUCCESSED}")
                    {
                        SMT.Foundation.Log.Tracer.Debug(ent.ATTENDANCESOLUTIONNAME + "产生了三八节,和考勤方案关联了");
                    }
                    else
                    {
                        SMT.Foundation.Log.Tracer.Debug(ent.ATTENDANCESOLUTIONNAME + "没有@@@@@@产生三八节,和考勤方案关联了");
                        continue;
                    }
                    #endregion
                    #region 五四


                    T_HR_LEAVETYPESET typesetYouth = new T_HR_LEAVETYPESET();
                    typesetYouth.LEAVETYPESETID = Guid.NewGuid().ToString();
                    typesetYouth.LEAVETYPENAME = ent.ATTENDANCESOLUTIONNAME + "五四青年节";
                    typesetYouth.LEAVETYPEVALUE = "12";
                    typesetYouth.ISFREELEAVEDAY = "2";
                    typesetYouth.MAXDAYS = (decimal)0.53;
                    typesetYouth.FINETYPE = "1";
                    typesetYouth.SEXRESTRICT = "2";
                    typesetYouth.ENTRYRESTRICT = "1";
                    typesetYouth.OWNERCOMPANYID = ent.OWNERCOMPANYID;
                    typesetYouth.OWNERDEPARTMENTID = ent.OWNERDEPARTMENTID;
                    typesetYouth.OWNERPOSTID = ent.OWNERPOSTID;
                    typesetYouth.OWNERID = ent.OWNERID;
                    typesetYouth.CREATEDATE = DateTime.Now;
                    typesetYouth.UPDATEDATE = DateTime.Now;
                    typesetYouth.UPDATEUSERID = ent.OWNERID;
                    typesetYouth.CREATECOMPANYID = ent.OWNERCOMPANYID;
                    typesetYouth.CREATEDEPARTMENTID = ent.OWNERDEPARTMENTID;
                    typesetYouth.CREATEPOSTID = ent.OWNERPOSTID;
                    typesetYouth.CREATEUSERID = ent.OWNERID;
                    typesetYouth.REMARK = "系统产生五四青年节";
                    //dal.AddToContext(typesetYouth);
                    var entExistYouth = from ent1 in dal.GetObjects<T_HR_LEAVETYPESET>()
                                        where ent1.LEAVETYPEVALUE == "12" && ent1.OWNERCOMPANYID == ent.OWNERCOMPANYID
                                        select ent1;
                    if (entExistYouth.Count() == 0)
                    {
                        string bbb1 = bllLeaveTypeSet.AddLeaveTypeSet(typesetYouth);
                        if (bbb1 == "{SAVESUCCESSED}")
                        {
                            SMT.Foundation.Log.Tracer.Debug(ent.ATTENDANCESOLUTIONNAME + "产生了五四青年节");
                        }
                        else
                        {
                            SMT.Foundation.Log.Tracer.Debug(ent.ATTENDANCESOLUTIONNAME + "没有####产生五四青年节");
                            continue;
                        }
                    }
                    else
                    {
                        typesetYouth = entExistYouth.FirstOrDefault();
                    }
                    T_HR_FREELEAVEDAYSET freeTypeSetYouth = new T_HR_FREELEAVEDAYSET();
                    freeTypeSetYouth.FREELEAVEDAYSETID = Guid.NewGuid().ToString();
                    freeTypeSetYouth.T_HR_LEAVETYPESET = typesetYouth;
                    //freeTypeSetYouth.T_HR_LEAVETYPESETReference.EntityKey =
                    //new System.Data.EntityKey("TM_SaaS_OA_EFModelContext.T_HR_LEAVETYPESET", "LEAVETYPESETID", typesetYouth.LEAVETYPESETID);
                    freeTypeSetYouth.MINIMONTH = 0;
                    freeTypeSetYouth.MAXMONTH = 9999;
                    freeTypeSetYouth.LEAVEDAYS = (decimal)0.53;
                    freeTypeSetYouth.ISPERFECTATTENDANCEFACTOR = "1";
                    freeTypeSetYouth.OFFESTTYPE = "1";
                    freeTypeSetYouth.OWNERCOMPANYID = ent.OWNERCOMPANYID;
                    freeTypeSetYouth.OWNERDEPARTMENTID = ent.OWNERDEPARTMENTID;
                    freeTypeSetYouth.OWNERPOSTID = ent.OWNERPOSTID;
                    freeTypeSetYouth.OWNERID = ent.OWNERID;
                    freeTypeSetYouth.CREATEDATE = DateTime.Now;
                    freeTypeSetYouth.UPDATEDATE = DateTime.Now;
                    freeTypeSetYouth.UPDATEUSERID = ent.OWNERID;
                    freeTypeSetYouth.CREATECOMPANYID = ent.OWNERCOMPANYID;
                    freeTypeSetYouth.CREATEDEPARTMENTID = ent.OWNERDEPARTMENTID;
                    freeTypeSetYouth.CREATEPOSTID = ent.OWNERPOSTID;
                    freeTypeSetYouth.CREATEUSERID = ent.OWNERID;
                    freeTypeSetYouth.REMARK = "系统产生五四青年节带薪假";
                    //dal.AddToContext(freeTypeSetYouth);
                    string bb2 = bllFreeLeaveDaySet.AddFreeLeaveDaySet(freeTypeSetYouth);
                    if (bb2 == "{SAVESUCCESSED}")
                    {
                        SMT.Foundation.Log.Tracer.Debug(ent.ATTENDANCESOLUTIONNAME + "产生了带薪假五四青年节");
                    }
                    else
                    {
                        SMT.Foundation.Log.Tracer.Debug(ent.ATTENDANCESOLUTIONNAME + "没有&&&产生带薪假五四青年节");
                        continue;
                    }
                    T_HR_ATTENDFREELEAVE attendFreeYouth = new T_HR_ATTENDFREELEAVE();
                    attendFreeYouth.ATTENDFREELEAVEID = Guid.NewGuid().ToString();
                    attendFreeYouth.T_HR_ATTENDANCESOLUTION = ent;

                    attendFreeYouth.T_HR_LEAVETYPESET = typesetYouth;
                    //attendFreeYouth.T_HR_LEAVETYPESETReference.EntityKey =
                    //    new System.Data.EntityKey("TM_SaaS_OA_EFModelContext.T_HR_LEAVETYPESET", "LEAVETYPESETID", typesetYouth.LEAVETYPESETID);
                    attendFreeYouth.REMARK = "自动产生五四青年节";
                    attendFreeYouth.CREATEDATE = DateTime.Now;
                    attendFreeYouth.UPDATEDATE = DateTime.Now;
                    attendFreeYouth.CREATEUSERID = ent.OWNERID;
                    attendFreeYouth.UPDATEUSERID = ent.OWNERID;
                    string cc = AddAttendFreeLeave(attendFreeYouth);
                    if (cc == "{SAVESUCCESSED}")
                    {
                        SMT.Foundation.Log.Tracer.Debug(ent.ATTENDANCESOLUTIONNAME + "产生了五四节,和考勤方案关联了");
                    }
                    else
                    {
                        SMT.Foundation.Log.Tracer.Debug(ent.ATTENDANCESOLUTIONNAME + "没有********了五四节,和考勤方案关联了");
                        continue;
                    }
                    #endregion
                    SMT.Foundation.Log.Tracer.Debug(ent.ATTENDANCESOLUTIONNAME + "执行完了");
                    intCount += 1;
                    SMT.Foundation.Log.Tracer.Debug("执行了：" + intCount.ToString());
                }
            }
            catch (Exception ex)
            {
                SMT.Foundation.Log.Tracer.Debug("初始化五四三八出现错误" + ex.ToString());
            }
        }
    }
}