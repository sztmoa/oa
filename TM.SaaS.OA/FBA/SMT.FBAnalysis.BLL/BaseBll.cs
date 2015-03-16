using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TM_SaaS_OA_EFModel;
using System.Linq.Dynamic;
using System.Data.Objects;
using System.Configuration;
using SMT.SaaS.BLLCommonServices.FlowWFService;
using System.Data.Objects.DataClasses;
using OrganizationWS = SMT.SaaS.BLLCommonServices.OrganizationWS;
using PermissionWS = SMT.SaaS.BLLCommonServices.PermissionWS;
using FlowServiceWS = SMT.SaaS.BLLCommonServices.FlowWFService;
using System.ServiceModel.Description;
using System.Reflection;
using System.Resources;

using BLLCommonServices = SMT.SaaS.BLLCommonServices;
using SMT.FBAnalysis.DAL;
using SMT.SaaS.BLLCommonServices.PublicInterfaceWS;

namespace SMT.FBAnalysis.BLL
{
    public class BaseBll<TEntity> : IDisposable where TEntity : class
    {
        public static string qualifiedEntitySetName = ConfigurationManager.AppSettings["DBContextName"] + ".";
        public CommDal<TEntity> dal = new CommDal<TEntity>();
        public static SMT.SaaS.BLLCommonServices.Utility UtilityClass;
        protected static OrganizationWS.OrganizationServiceClient OrgClient;
        protected static PermissionWS.PermissionServiceClient PermClient;
        protected static PublicServiceClient PublicInterfaceClient;

        protected static BLLCommonServices.BllCommonUserPermissionWS.BllCommonPermissionServicesClient BllPermClient;
        public BaseBll()
        {
            if (OrgClient == null)
            {
                OrgClient = new SMT.SaaS.BLLCommonServices.OrganizationWS.OrganizationServiceClient();
            }
            if (PermClient == null)
            {
                PermClient = new SMT.SaaS.BLLCommonServices.PermissionWS.PermissionServiceClient();
            }
            if (BllPermClient == null)
            {
                BllPermClient = new SMT.SaaS.BLLCommonServices.BllCommonUserPermissionWS.BllCommonPermissionServicesClient();
            }
            if (UtilityClass == null)
            {
                UtilityClass = new SMT.SaaS.BLLCommonServices.Utility();
            }

        }

        public bool Add(TEntity entity)
        {
            try
            {
                Utility.RefreshEntity(entity as System.Data.Objects.DataClasses.EntityObject);
                int i = dal.Add(entity);
                if (i > 0)
                {
                    SaveMyRecord(entity);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
                throw (ex);
            }
        }

        public bool Delete(TEntity entity)
        {
            try
            {
                dal.Delete(entity);
                DeleteMyRecord(entity);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw (ex);
            }
        }

        public bool Update(TEntity entity)
        {
            try
            {
                int i = dal.Update(entity);
                if (i > 0)
                {
                    SaveMyRecord(entity);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
                throw (ex);
            }
        }

        /// <summary>
        /// 将指定的单据记录存储到我的单据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string SaveMyRecord(TEntity entity)
        {
            string strTemp = string.Empty;
            try
            {
                BLLCommonServices.Utility.SubmitMyRecord<TEntity>(entity);
            }
            catch (Exception ex)
            {
                strTemp = ex.InnerException.Message;
            }
            return strTemp;
        }

        /// <summary>
        /// 将指定的单据记录从我的单据中删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string DeleteMyRecord(TEntity entity)
        {
            string strTemp = string.Empty;
            try
            {
                BLLCommonServices.Utility.RemoveMyRecord<TEntity>(entity);
            }
            catch (Exception ex)
            {
                strTemp = ex.InnerException.Message;
            }
            return strTemp;
        }

        public IQueryable<TEntity> GetTable()
        {
            //return ((ObjectQuery<TEntity>)dal.GetTable(typeof(TEntity).Name)).AsQueryable();
            return dal.GetTable<TEntity>();
        }

        public object CustomerQuery(string Sql)
        {
            return dal.CustomerQuery(Sql);

        }

        public void InitCustomResult(ref CustomResult result)
        {
            result = new CustomResult();
            result.ResultValue = 1;
        }

        public void SetCustomResult(ref CustomResult result, string message)
        {
            result.ResultValue = 0;
            result.ResultMessage = message;
        }

        /// <summary>
        /// 用于实体Grid中显示数据的分页查询
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="sort">排序字段</param>
        /// <param name="filterString">过滤条件</param>
        /// <param name="paras">过滤条件中的参数值</param>
        /// <param name="pageCount">返回总页数</param>
        /// <returns>查询结果集</returns>
        public IQueryable<TEntity> QueryWithPaging(int pageIndex, int pageSize, string sort, string filterString, IList<object> paras, ref int pageCount)
        {
            #region 实现动态查询的两个例子
            //方式一:用Dynamic Linq 查询
            //string[] paras = new string[2];
            //paras[0] = "EmpCode3";
            //paras[1] = "EmpName5";
            //IQueryable<T_HR_EMPLOYEE> ents = dal.GetTable().Where("EMPLOYEECODE==@0 or EMPLOYEENAME==@1 ", paras).OrderBy(sort).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            //return ents;


            //方式二:用原始的DynamicQueryable查询
            //IQueryable<T_HR_EMPLOYEE> ents = dal.GetTable();
            //ParameterExpression para = Expression.Parameter(typeof(T_HR_EMPLOYEE), "emp");

            ////c.City=="London"
            //Expression left = Expression.Property(para, typeof(T_HR_EMPLOYEE).GetProperty("EMPLOYEECODE"));
            //Expression right = Expression.Constant("22222");
            //Expression filter = Expression.Equal(left, right);

            //Expression pred = Expression.Lambda(filter, para);
            ////Where(c=>c.City=="London")
            //Expression expr = Expression.Call(typeof(Queryable), "Where",
            //    new Type[] { typeof(T_HR_EMPLOYEE) },
            //    Expression.Constant(ents), pred);

            ////OrderBy(ContactName => ContactName)
            //MethodCallExpression orderByCallExpression = Expression.Call(
            //typeof(Queryable), "OrderBy",
            //new Type[] { typeof(T_HR_EMPLOYEE), typeof(string) },
            //expr,
            //Expression.Lambda(Expression.Property
            //(para, "EMPLOYEENAME"), para));

            ////生成动态查询
            //IQueryable<T_HR_EMPLOYEE> query = ents.AsQueryable()
            //    .Provider.CreateQuery<T_HR_EMPLOYEE>(orderByCallExpression);


            //int count = query.Count();
            //pageCount = count/pageSize;
            //int tmp = count % pageSize;
            //pageCount = pageCount + (tmp > 0 ? 1 : 0);

            //return query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            #endregion

            IQueryable<TEntity> ents = dal.GetTable();
            if (!string.IsNullOrEmpty(filterString))
            {
                ents = ents.Where(filterString, paras.ToArray());
            }
            ents = ents.OrderBy(sort);

            ents = Utility.Pager<TEntity>(ents, pageIndex, pageSize, ref pageCount);

            return ents;
        }

        /// <summary>
        /// 获取所有系统字典
        /// </summary>
        /// <returns></returns>
        public List<PermissionWS.T_SYS_DICTIONARY> GetAllDictionary()
        {

            List<PermissionWS.T_SYS_DICTIONARY> entList = new List<PermissionWS.T_SYS_DICTIONARY>();
            try
            {
                PermissionWS.T_SYS_DICTIONARY[] ents = PermClient.GetSysDictionaryByCategory("");
                if (ents != null)
                {
                    entList = ents.ToList();
                }

                return entList;
            }
            catch(Exception ex)
            {
                SMT.Foundation.Log.Tracer.Debug(ex.ToString());
                return entList;
            }
        }

        public List<SaaS.BLLCommonServices.OrganizationWS.V_DEPARTMENT> GetAllDepartment(string strOwnerId)
        {
            List<OrganizationWS.V_DEPARTMENT> entList = new List<OrganizationWS.V_DEPARTMENT>();
            OrganizationWS.V_DEPARTMENT[] ents = OrgClient.GetAllDepartmentView(strOwnerId);
            if (ents != null)
            {
                entList = ents.ToList();
            }

            return entList;
        }

        #region 防止重复提交更新
        private static Object thisLock = new Object();
        private static HashSet<string> OrderHandling = new HashSet<string>();

        /// <summary>
        /// 锁定记录
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public static bool LockOrder(string keyValue)
        {

            bool result = true;
            lock (thisLock)
            {
                if (!OrderHandling.Contains(keyValue))
                {
                    OrderHandling.Add(keyValue);
                    result = false;
                }
            };
            return result;


        }

        /// <summary>
        /// 解锁记录
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public static bool ReleaseOrder(string keyValue)
        {
            bool result = false;
            lock (thisLock)
            {
                if (OrderHandling.Contains(keyValue))
                {
                    OrderHandling.Remove(keyValue);
                    result = true;
                }
            };
            return result;
        }
        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            dal.Dispose();
        }

        #endregion

        #region 每个业务层单据构造XML

        public SMT.SaaS.MobileXml.AutoDictionary basedata(string TableName, string Name, string Value, string Text)
        {
            string[] strlist = new string[4];
            strlist[0] = TableName;
            strlist[1] = Name;
            strlist[2] = Value;
            strlist[3] = Text;
            SMT.SaaS.MobileXml.AutoDictionary ad = new SMT.SaaS.MobileXml.AutoDictionary();
            ad.AutoDictionaryList(strlist);
            return ad;
        }

        public SMT.SaaS.MobileXml.AutoDictionary basedataForChild(string TableName, string Name, string Value, string Text, string keyValue)
        {
            string[] strlist = new string[5];
            strlist[0] = TableName;
            strlist[1] = Name;
            strlist[2] = Value;
            strlist[3] = Text;
            strlist[4] = keyValue;
            SMT.SaaS.MobileXml.AutoDictionary ad = new SMT.SaaS.MobileXml.AutoDictionary();
            ad.AutoDictionaryChiledList(strlist);
            return ad;
        }

        public string GetBusinessObject(string ModeCode)
        {
            if (PublicInterfaceClient == null)
            {
                PublicInterfaceClient = new PublicServiceClient();
            }
            string BusinessObject = PublicInterfaceClient.GetBusinessObject("FB", ModeCode);
            return BusinessObject;
        }
        #endregion
    }
}
