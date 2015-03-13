using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.Generic;
using System.Text;
using TM_SaaS_OA_EFModel;
using SMT.SaaS.OA.BLL;
namespace SMT.SaaS.OA.Services
{
    public partial class SmtOAPersonOffice
    {

        [OperationContract]
        public List<T_OA_WORKRECORD> GetWorkerCordListByUserId(int pageIndex, int pageSize, string sort, string filterString, object[] paras, ref int pageCount, string userId)
        {
            WorkerCordManagementBll workerCordBll = new WorkerCordManagementBll();
            //using (WorkerCordManagementBll workerCordBll = new WorkerCordManagementBll())
            //{
            IQueryable<T_OA_WORKRECORD> workerCordList = workerCordBll.GetWorkerCodeListByUserID(pageIndex, pageSize, sort, filterString, paras, ref  pageCount, userId);
            if (workerCordList == null)
            {
                return null;
            }
            else
            {
                return workerCordList.ToList();
            }
            //}
        }

        [OperationContract]
        public int DeleteWorkerCordList(List<T_OA_WORKRECORD> objList)
        {
            WorkerCordManagementBll workerCordBll = new WorkerCordManagementBll();
            //using (WorkerCordManagementBll workerCordBll = new WorkerCordManagementBll())
            //{
            foreach (T_OA_WORKRECORD obj in objList)
            {
                if (!workerCordBll.DeleteWorkCord(obj.WORKRECORDID))
                {
                    return -1;
                }
            }
            return 1;
            //}
        }


        [OperationContract]
        public int WorkerCordAdd(T_OA_WORKRECORD obj)
        {
            WorkerCordManagementBll workerCordBll = new WorkerCordManagementBll();
            //using (WorkerCordManagementBll workerCordBll = new WorkerCordManagementBll())
            //{
            bool sucess = workerCordBll.AddWorkCord(obj);
            if (sucess == false)
            {
                return -1;
            }
            return 1;
            //}
        }

        [OperationContract]
        public bool WorkerCordDel(T_OA_WORKRECORD obj)
        {
            WorkerCordManagementBll workerCordBll = new WorkerCordManagementBll();
            //using (WorkerCordManagementBll workerCordBll = new WorkerCordManagementBll())
            //{
            return workerCordBll.DeleteWorkCord(obj.WORKRECORDID);
            //}
        }

        [OperationContract]
        public void WorkerCordUpdate(T_OA_WORKRECORD obj)
        {
            WorkerCordManagementBll workerCordBll = new WorkerCordManagementBll();
            //using (WorkerCordManagementBll workerCordBll = new WorkerCordManagementBll())
            //{
            workerCordBll.UpdateWorkCord(obj);
            //}
        }

        #region 手机版调用服务
        /// <summary>
        /// 添加工作日志
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="employeeID">员工ID</param>
        /// <param name="strTime">工作日志时间</param>
        /// <returns>成功返回为空</returns>
        [OperationContract]
        public string AddWorkCordForMobile(string title,string content,string employeeID,string strTime,ref string workRecordID)
        {            
            using (WorkerCordManagementBll workerCordBll = new WorkerCordManagementBll())
            {
                return workerCordBll.AddWorkCordForMobile(title,content,employeeID,strTime,ref  workRecordID);            
            }
        }

        /// <summary>
        /// 修改工作日志信息
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="employeeID">员工ID</param>
        /// <param name="strTime">工作日志时间</param>
        /// <param name="workRecordID">工作日志ID</param>
        /// <returns>成功为空</returns>
        [OperationContract]
        public string UpdateWorkCordForMobile(string title, string content, string employeeID, string strTime, string workRecordID)
        {
            using (WorkerCordManagementBll workerCordBll = new WorkerCordManagementBll())
            {
                return workerCordBll.UpdateWorkCordForMobile(title, content, employeeID, strTime,workRecordID);
            }
        }

        /// <summary>
        /// 获取工作日志实体信息
        /// </summary>
        /// <param name="workRecordID">工作日志ID</param>
        /// <returns>返回工作日志实体</returns>
        [OperationContract]
        public T_OA_WORKRECORD GetWorkCordByID(string workRecordID)
        {
            using (WorkerCordManagementBll workerCordBll = new WorkerCordManagementBll())
            {
                return workerCordBll.GetWorkCordByID(workRecordID);
            }
        }

        /// <summary>
        /// 删除工作日志
        /// </summary>
        /// <param name="workRecordID">工作日志ID</param>
        /// <returns>成功true</returns>
        [OperationContract]
        public bool DelWorkerCordByID(string workRecordID)
        {            
            using (WorkerCordManagementBll workerCordBll = new WorkerCordManagementBll())
            {
                return workerCordBll.DeleteWorkCord(workRecordID);
            }
        }

        /// <summary>
        /// 获取员工在1个月中的日期集合
        /// </summary>
        /// <param name="employeeID">员工ID</param>
        /// <param name="strDate">日期</param>
        /// <returns>返回日期集合</returns>
        [OperationContract]
        public List<string> GetDateByEmployeeIDAndDate(string employeeID, string strDate)
        {
            using (WorkerCordManagementBll workerCordBll = new WorkerCordManagementBll())
            {
                return workerCordBll.GetDateByEmployeeIDAndDate(employeeID,strDate);
            }
        }

        /// <summary>
        /// 获取员工在1个月中的工作日志集合
        /// </summary>
        /// <param name="employeeID">员工ID</param>
        /// <param name="strDate">日期</param>
        /// <returns>返回1个月的工作日志集合</returns>
        [OperationContract]
        public List<T_OA_WORKRECORD> GetWorkRecordsByEmployeeIDAndDate(string employeeID, string strDate)
        {
            using (WorkerCordManagementBll workerCordBll = new WorkerCordManagementBll())
            {
                return workerCordBll.GetWorkRecordsByEmployeeIDAndDate(employeeID, strDate);
            }
        }

        /// <summary>
        /// 根据员工ID和当前日期返回该员工的日志实体
        /// </summary>
        /// <param name="employeeID"></param>
        /// <param name="strDate"></param>
        /// <returns></returns>
        [OperationContract]
        public T_OA_WORKRECORD GetWorkRecordsByEmployeeIDAndCurrentDay(string employeeID, string strDate)
        {
            using (WorkerCordManagementBll workerCordBll = new WorkerCordManagementBll())
            {
                return workerCordBll.GetWorkRecordsByEmployeeIDAndCurrentDay(employeeID, strDate);
            }
        }
        #endregion
    }
}