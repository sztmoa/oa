using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMT.SaaS.OA.DAL;
using FAMDataModel;
using System.Linq.Dynamic;

using SMT.SaaS.CustomModel;
using SMT.SaaS.GlobalFunction;
namespace SMT.SaaS.OA.BLL
{
    /// <summary>
    /// 任务维护档案
    /// </summary>
    public class TaskMainBLL : BaseBll<T_TASK_TASKMAIN>
    {
        private TaskMainDAL taskmainDll = new TaskMainDAL();
        private TaskCategoryDAL taskCategoryDal = new TaskCategoryDAL();
        private TaskParticipantsDAL taskParticipantsDal = new TaskParticipantsDAL();
        private FAMDataModelContext FamDMC = new FAMDataModelContext();
        private List<T_TASK_TASKMAIN> ListTask;
        #region 新增任务维护档案
        /// <summary>
        /// 新增任务维护档案
        /// </summary>
        /// <param name="taskmain"></param>
        /// <param name="customPerfix"></param>
        /// <returns></returns>
        public bool AddTaskMain(T_TASK_TASKMAIN taskmain, List<T_TASK_PARTICIPANTS> taskPants, List<T_TASK_FLOWTRIGGER> taskFlowTrigger, CustomPerfix customPerfix, ref string ID)
        {
            try
            {
           
                string taskId = "";
                taskmain.CREATEDATE = GlobalFunction.GlobalFunction.Date();
                taskmain.CREATETIME = GlobalFunction.GlobalFunction.Time();
                taskmain.TASKID = AssetsPrefixBLL.Instance.PrefixNo(customPerfix);//生成字首ID`
                BeginTransaction();
                DataContext.AddObject(typeof(T_TASK_TASKMAIN).Name, taskmain);//
                taskId = taskmain.TASKID;
                ID = taskmain.TASKID;
                taskPants.ForEach(taskList =>
                {
                    taskList.TASKID = taskId;
                    taskList.CREATEDATE = GlobalFunction.GlobalFunction.Date();
                    taskList.CREATETIME = GlobalFunction.GlobalFunction.Time();
                    DataContext.AddObject(typeof(T_TASK_PARTICIPANTS).Name, taskList);
                });

                CustomPerfix perfix = new CustomPerfix();
                perfix.PrefixTypeId = "FlowTrigger";
                perfix.PrefixId = "FT";
                taskFlowTrigger.ForEach(triggerList =>
                {
                    triggerList.TASKID = taskId;
                    triggerList.CUSTOMID = AssetsPrefixBLL.Instance.PrefixNo(perfix);
                    triggerList.CREATEDATE = GlobalFunction.GlobalFunction.Date();
                    triggerList.CREATETIME = GlobalFunction.GlobalFunction.Time();
                    DataContext.AddObject(typeof(T_TASK_FLOWTRIGGER).Name, triggerList);
                });

                int taskMainAdd = DataContext.SaveChanges();
                if (taskMainAdd > 0)
                {

                    //发消息给执行人
                    string strXml = GlobalFunction.DataObjectToXml<T_TASK_TASKMAIN>.ObjListToXml(taskmain, "");
                    GlobalFunction.EngineMsgTrigger<T_TASK_TASKMAIN>.SendMsg(strXml, taskmain.EXECUTEUSERID, taskId, "TASKSUBMIT");
                    if (taskPants.Count > 0)
                    {
                        var v = from c in taskPants
                                where c.PARTICIPATEUSERID != taskmain.EXECUTEUSERID && c.PARTICIPATEUSERID != taskmain.PROMOTERID
                                select c;
                        if (v.Count() > 0)
                        {
                            List<T_TASK_PARTICIPANTS> List = v.ToList();
                            string[] strUser = new string[v.Count()];
                            for (int i = 0; i < List.Count; i++)
                            {
                                strUser[i] = List[i].PARTICIPATEUSERID;
                            }
                            //发消息给相关人
                            GlobalFunction.EngineMsgTrigger<T_TASK_TASKMAIN>.SendMsg(strXml, strUser, taskId, "TASKRELETEDSUBMIT");
                        }
                    }


                    CommitTransaction();
                    return true;
                }
            }
            catch (Exception ex)
            {
                RollbackTransaction();
                return false;
            }
            return false;
        }
        #endregion

        #region 检查是否存在相同记录
        /// <summary>
        /// 检查是否存在相同记录
        /// </summary>
        /// <param name="taskMainId">任务单Id</param>
        /// <param name="taskMainName">任务单名称</param>
        /// <returns></returns>
        public bool IsExistTaskMain(string taskMainId, string taskMainName)
        {
            bool IsExist = false;
            var q = from cnt in taskmainDll.GetTable()
                    where cnt.TASKID == taskMainId && cnt.TASKNAME == taskMainName
                    orderby cnt.CREATEUSERID
                    select cnt;
            if (q.Count() > 0)
            {
                IsExist = true;
            }
            return IsExist;
        }
        #endregion

        #region 检查该任务类别是否存在任务
        /// <summary>
        /// 检查该任务类别是否存在任务
        /// </summary>
        /// <param name="taskMainId">任务单Id</param>
        /// <param name="taskMainName">任务单名称</param>
        /// <returns></returns>
        public bool CheckIsExistsTask(string taskTypeID)
        {
            try
            {
                var entitys = from ent in taskmainDll.GetTable().ToList()
                              where ent.TASKTYPE == taskTypeID
                              select ent;

                if (entitys.Count() > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
                throw (ex);
            }
        }
        #endregion

        #region 修改任务维护档案
        /// <summary>
        /// 修改任务维护档案
        /// </summary>
        /// <param name="taskmain"></param>
        /// <returns></returns>
        public bool UpdateTaskMain(T_TASK_TASKMAIN taskmain, List<T_TASK_PARTICIPANTS> taskPants, List<T_TASK_FLOWTRIGGER> flowTrigger)
        {
            try
            {
                FAMDataModelContext context = taskmainDll.lbc.GetDataContext() as FAMDataModelContext;
                //删除相关人
                context.T_TASK_TASKMAIN.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                var entitys = from ent in context.T_TASK_PARTICIPANTS
                              where ent.TASKID == taskmain.TASKID
                              select ent;

                if (entitys.Count() > 0)
                {
                    foreach (var obj in entitys)
                    {
                        context.DeleteObject(obj);
                    }
                    context.SaveChanges();
                }
                //删除发起流程
                var vFlow = from c in context.T_TASK_FLOWTRIGGER
                            where c.TASKID == taskmain.TASKID
                            select c;
                if (vFlow.Count() > 0)
                {
                    foreach (var vv in vFlow)
                    {
                        context.DeleteObject(vv);
                    }
                    context.SaveChanges();
                }


                BeginTransaction();
                var v = from c in FamDMC.T_TASK_TASKMAIN
                        where c.TASKID == taskmain.TASKID
                        select c;
                if (v.Count() > 0)
                {
                    T_TASK_TASKMAIN rowobject = v.FirstOrDefault();
                    rowobject.TASKNAME = taskmain.TASKNAME;//任务单名称
                    rowobject.PROMOTERID = taskmain.PROMOTERID;//任务发起人
                    rowobject.TASKTYPE = taskmain.TASKTYPE;//任务类别
                    rowobject.STARTDATE = taskmain.STARTDATE;//开始日期
                    rowobject.ENDTDATE = taskmain.ENDTDATE;//结束日期
                    rowobject.TASKREMARK = taskmain.TASKREMARK;//任务说明
                    rowobject.EXECUTEUSERID = taskmain.EXECUTEUSERID;//执行人
                    rowobject.EXECUTEUSERNAME = taskmain.EXECUTEUSERNAME;
                    rowobject.UPDATEUSERID = taskmain.UPDATEUSERID;//修改人
                    rowobject.TASKSTATUS = taskmain.TASKSTATUS;//任务状态
                    rowobject.UPDATEDATE = GlobalFunction.GlobalFunction.Date();//修改日期
                    rowobject.UPDATETIME = GlobalFunction.GlobalFunction.Time();//修改时间
                    if (FamDMC.SaveChanges() == 1)
                    {
                        taskPants.ForEach(taskList =>
                        {
                            DataContext.AddObject(typeof(T_TASK_PARTICIPANTS).Name, taskList);
                        });
                        CustomPerfix perfix = new CustomPerfix();
                        perfix.PrefixTypeId = "FlowTrigger";
                        perfix.PrefixId = "FT";
                        flowTrigger.ForEach(flowList =>
                        {
                            flowList.TASKID = taskmain.TASKID;
                            flowList.CUSTOMID = AssetsPrefixBLL.Instance.PrefixNo(perfix);
                            flowList.EntityKey = null;
                            DataContext.AddObject(typeof(T_TASK_FLOWTRIGGER).Name, flowList);
                        });

                        int taskMainAdd = DataContext.SaveChanges();
                        CommitTransaction();
                        return true;
                    }
                }

            }
            catch (Exception ex)
            {
                RollbackTransaction();
                return false;
            }
            return false;
        }
        #endregion
        #region 更新任务进度
        public bool UpdateTaskMain(string strTaskID, string strFinishPercent)
        {
            bool result = false;
            try
            {
                var dataobjects = from ent in FamDMC.T_TASK_TASKMAIN
                                  where ent.TASKID == strTaskID
                                  select ent;

                if (dataobjects.Count() > 0)
                {
                    var rowobject = dataobjects.FirstOrDefault();
                    string CompletedFinishPercent = string.Concat((rowobject.TASKSCHEDULE.CvtInt() + strFinishPercent.CvtInt()));
                    if (CompletedFinishPercent == "100")//任务进度100%更新任务状态
                    {
                        rowobject.TASKSTATUS = "2";
                        var v = (from c in FamDMC.T_TASK_FLOWTRIGGER
                                 where c.TASKID == strTaskID
                                 select c).ToList();
                        if (v.Count() > 0)
                        {
                            CallWcf.CallWCFServer(v);//任务完成调用WCF
                        }
                        //任务完成，完毕任务消息
                        GlobalFunction.EngineMsgTrigger<T_TASK_TASKMAIN>.CloseMsg(strTaskID);
                    }
                    rowobject.TASKSCHEDULE = CompletedFinishPercent;
                    rowobject.COMPLETEDATE = DateTime.Now;
                    if (FamDMC.SaveChanges() == 1)
                    {
                        result = true;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region 中止任务
        /// <summary>
        /// 中止任务()
        /// </summary>
        /// <param name="taskMainID">任务ID</param>
        ///  <param name="strTaskStatus">状态（3：中止）</param>
        /// <returns></returns>
        public bool StopTask(string taskMainID, string strTaskStatus)
        {
            bool result = false;
            try
            {
                var dataobjects = from ent in FamDMC.T_TASK_TASKMAIN
                                  where ent.TASKID == taskMainID
                                  select ent;

                if (dataobjects.Count() > 0)
                {
                    var rowobject = dataobjects.FirstOrDefault();
                    rowobject.TASKSTATUS = strTaskStatus;
                    if (FamDMC.SaveChanges() == 1)
                    {
                        result = true;
                        if (strTaskStatus == "3")
                        {
                            GlobalFunction.EngineMsgTrigger<T_TASK_TASKMAIN>.SendMsg(rowobject, rowobject.EXECUTEUSERID, rowobject.TASKID, "TASKSTOP");
                        }
                        if (strTaskStatus == "1")
                        {
                            GlobalFunction.EngineMsgTrigger<T_TASK_TASKMAIN>.SendMsg(rowobject, rowobject.EXECUTEUSERID, rowobject.TASKID, "TASKRESTART");
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion
        #region 检查任务是否是完成状态
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strTaskID">任务ID</param>
        /// <returns></returns>
        public bool CheckTaskIsCompleted(string strTaskID)
        {
            bool result = false;
            try
            {
                var dataobjects = from ent in FamDMC.T_TASK_TASKMAIN
                                  where ent.TASKID == strTaskID
                                  select ent;

                if (dataobjects.Count() > 0)
                {
                    if (dataobjects.FirstOrDefault().TASKSCHEDULE == "2")
                    {
                        return true;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion
        #region 获取任务当前任务进度
        public string GetTaskSchedule(string strTaskID)
        {
            var dataobjects = from ent in FamDMC.T_TASK_TASKMAIN
                              where ent.TASKID == strTaskID
                              select ent;
            if (dataobjects.Count() > 0)
            {
                return dataobjects.FirstOrDefault().TASKSCHEDULE;
            }
            return "0";
        }
        #endregion

        #region 根据任务单ID查询参会人员
        /// <summary>
        /// 根据任务单ID查询参会人员
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public List<T_TASK_PARTICIPANTS> GetTaskInfoById(string taskId)
        {
            var query = from p in DataContext.T_TASK_PARTICIPANTS
                        where string.IsNullOrEmpty(taskId) || p.TASKID == taskId
                        orderby p.CREATEDATE descending
                        select p;
            if (query.Count() > 0)
            {
                return query.ToList<T_TASK_PARTICIPANTS>();
            }
            else
            {
                return null;
            }
        }
        #endregion

        /// <summary>
        /// 取得TASKID查找任务
        /// </summary>
        /// <param name="strMaintainID"></param>
        /// <returns></returns>
        public T_TASK_TASKMAIN GetTaskByID(string strTaskID)
        {
            FAMDataModelContext context = taskmainDll.lbc.GetDataContext() as FAMDataModelContext;
            var v = (from c in context.T_TASK_TASKMAIN
                     where c.TASKID == strTaskID
                     select c).FirstOrDefault();
            return v;

        }
        #region 通过任务ID，查找其所有子任务
        public List<T_TASK_TASKMAIN> GetTaskByParentID(string strTaskID)
        {
            ListTask = new List<T_TASK_TASKMAIN>();
            ListTaskTree(strTaskID);
            return ListTask;
        }
        private void ListTaskTree(string strTaskID)
        {
            FAMDataModelContext context = taskmainDll.lbc.GetDataContext() as FAMDataModelContext;
            var v = (from c in context.T_TASK_TASKMAIN
                     where c.PARENTTASKID == strTaskID
                     select c).ToList();
            if (v != null && v.Count() > 0)
            {
                foreach (T_TASK_TASKMAIN T in v)
                {
                    ListTask.Add(T);
                    ListTaskTree(T.TASKID);
                }
            }
        }
        #endregion

        #region 事务处理
        public void BeginTransaction()
        {
            dal.BeginTransaction();
        }
        public void CommitTransaction()
        {
            dal.CommitTransaction();
        }
        public void RollbackTransaction()
        {
            dal.RollbackTransaction();
        }
        #endregion

        #region 删除任务维护档案
        /// <summary>
        /// 删除任务维护档案
        /// </summary>
        /// <param name="taskmainId"></param>
        /// <returns></returns>
        public bool DeleteTaskMain(string[] taskmainId)
        {
            try
            {
                var entitys = from ent in taskmainDll.GetTable().ToList()
                              where taskmainId.Contains(ent.TASKID)
                              select ent;

                if (entitys.Count() > 0)
                {
                    TaskParticipantsBLL taskParticipantsBll = new TaskParticipantsBLL();
                    foreach (var obj in entitys)
                    {
                        int iResult = taskmainDll.Delete(obj);
                        if (iResult > 0)
                        {
                            taskParticipantsBll.DeleteTaskParticipants(obj.TASKID);//删除任务相关人
                        }
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
                throw (ex);
            }
        }
        #endregion

        #region 任务维护档案(动态查询、分页)(我安排)
        /// <summary>
        ///  任务维护档案(动态查询、分页)(我安排)
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sort"></param>
        /// <param name="filterString"></param>
        /// <param name="paras"></param>
        /// <param name="pageCount"></param>
        /// <param name="flowInfoList"></param>
        /// <param name="checkState"></param>
        /// <returns></returns>
        public List<T_TASK_TASKMAIN> MyArrangementTaskList(int pageIndex, int pageSize, string sort, string filterString, IList<object> paras, ref int pageCount, string checkState, LoginUserInfo loginUserInfo)
        {
            try
            {

                FAMDataModelContext context = taskmainDll.lbc.GetDataContext() as FAMDataModelContext;
                if (context != null)
                {
                    //context.T_TASK_TASKMAIN.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                    //var v = from c in context.T_TASK_PARTICIPANTS
                    //        where c.PARTICIPATEUSERID == loginUserInfo.userID
                    //        select c;
                    //int iAllCount = v.Count();
                    //if (iAllCount > 0)
                    //{
                    //    string[] UserID = new string[iAllCount];
                    //    foreach(
                    //}

                    var ents = from a in context.T_TASK_TASKMAIN
                               where a.OWNERCOMPANYID == loginUserInfo.companyID && a.PROMOTERID == loginUserInfo.userID  
                               select a;
                    if (ents.Count() > 0)
                    {
                        if (!string.IsNullOrEmpty(filterString))
                        {
                            ents = ents.ToList().AsQueryable().Where(filterString, paras.ToArray());
                        }
                        ents = ents.OrderBy(sort);
                        ents = Utility.Pager<T_TASK_TASKMAIN>(ents, pageIndex, pageSize, ref pageCount);
                        return ents.ToList();
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
                throw (ex);
            }
        }
        #endregion
        #region 任务维护档案(动态查询、分页)(我执行)
        /// <summary>
        ///  任务维护档案(动态查询、分页)（我执行）
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sort"></param>
        /// <param name="filterString"></param>
        /// <param name="paras"></param>
        /// <param name="pageCount"></param>
        /// <param name="flowInfoList"></param>
        /// <param name="checkState"></param>
        /// <returns></returns>
        public List<T_TASK_TASKMAIN> MyExecutionTaskList(int pageIndex, int pageSize, string sort, string filterString, IList<object> paras, ref int pageCount, string checkState, LoginUserInfo loginUserInfo)
        {
            try
            {

                FAMDataModelContext context = taskmainDll.lbc.GetDataContext() as FAMDataModelContext;
                if (context != null)
                {
                    context.T_TASK_TASKMAIN.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                    var ents = from a in context.T_TASK_TASKMAIN
                               where a.EXECUTEUSERID == loginUserInfo.userID && a.TASKSTATUS != "0"//a.OWNERCOMPANYID == loginUserInfo.companyID &&
                               select a;

                    if (ents.Count() > 0)
                    {
                        if (!string.IsNullOrEmpty(filterString))
                        {
                            ents = ents.ToList().AsQueryable().Where(filterString, paras.ToArray());
                        }
                        ents = ents.OrderBy(sort);
                        ents = Utility.Pager<T_TASK_TASKMAIN>(ents, pageIndex, pageSize, ref pageCount);
                        return ents.ToList();
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
                throw (ex);
            }
        }
        #endregion
        #region 任务维护档案(动态查询、分页)(我相关)
        /// <summary>
        ///  任务维护档案(动态查询、分页)（我相关）
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sort"></param>
        /// <param name="filterString"></param>
        /// <param name="paras"></param>
        /// <param name="pageCount"></param>
        /// <param name="flowInfoList"></param>
        /// <param name="checkState"></param>
        /// <returns></returns>
        public List<T_TASK_TASKMAIN> MyRelatedTaskList(int pageIndex, int pageSize, string sort, string filterString, IList<object> paras, ref int pageCount, string checkState, LoginUserInfo loginUserInfo)
        {


            try
            {

                FAMDataModelContext context = taskmainDll.lbc.GetDataContext() as FAMDataModelContext;
                if (context != null)
                {
                    context.T_TASK_TASKMAIN.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                    var ents = from a in context.T_TASK_TASKMAIN
                               join l in context.T_TASK_PARTICIPANTS on a.TASKID equals l.TASKID
                               where a.TASKSTATUS != "0" && l.PARTICIPATEUSERID == loginUserInfo.userID// a.OWNERCOMPANYID == loginUserInfo.companyID &&
                               select a;
                    if (ents.Count() > 0)
                    {
                        if (!string.IsNullOrEmpty(filterString))
                        {
                            ents = ents.ToList().AsQueryable().Where(filterString, paras.ToArray());
                        }
                        ents = ents.OrderBy(sort);
                        ents = Utility.Pager<T_TASK_TASKMAIN>(ents, pageIndex, pageSize, ref pageCount);
                        return ents.ToList();
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
                throw (ex);
            }
        }
        #endregion
    }
}
