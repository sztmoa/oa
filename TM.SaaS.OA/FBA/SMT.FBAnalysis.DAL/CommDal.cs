using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMT.Foundation.Core;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Reflection;
using System.Configuration;
using SMT.Foundation.Log;
using System.Data;


namespace SMT.FBAnalysis.DAL
{
    public class CommDal<TEntity> : BaseDAL
    {
        private static string conn = ConfigurationManager.AppSettings["ConnectionString"].ToString();
        

        public object ExecuteStoredProcedure(string Sqlstring, ParameterCollection prameters)
        {           
            OracleDAO dao = new OracleDAO(conn);
            //prameters = new ParameterCollection();
            //prameters.Add("parme1", null);
            return dao.ExecuteScalar("", System.Data.CommandType.StoredProcedure, prameters);           
        }

        public object ExecuteCustomerSql(string Sqlstring, ParameterCollection prameters)
        {            
            OracleDAO dao = new OracleDAO(conn);
            //prameters = new ParameterCollection();
            //prameters.Add("parme1", null);
            return dao.ExecuteScalar(Sqlstring, System.Data.CommandType.Text, prameters);

            //return object obj=dao.ExecuteScalar(Sqlstring, System.Data.CommandType.Text);
            //prameters = new ParameterCollection();
            //object obj1 = dao.GetDataTable(Sqlstring, System.Data.CommandType.Text, prameters);
        }

        public object ExecuteCustomerSql(string Sqlstring)
        {
            OracleDAO dao = new OracleDAO(conn);          
            object obj=dao.ExecuteScalar(Sqlstring, System.Data.CommandType.Text);
            //ParameterCollection prameters = new ParameterCollection();
            //object obj1 = dao.GetDataTable(Sqlstring, System.Data.CommandType.Text, prameters);
            return obj;
        }

        public DataTable GetDataTableByCustomerSql(string Sqlstring)
        {
            try
            {
                OracleDAO dao = new OracleDAO(conn);
                DataTable dt = dao.GetDataTable(Sqlstring, System.Data.CommandType.Text);
                return dt;
            }
            catch (Exception ex)
            {
                Tracer.Debug(ex.ToString());
                return null; 
            }finally{

                Tracer.Debug(Sqlstring);
            }

        }


        //public bool Delete(TEntity entity)
        //{
        //    try
        //    {
        //        base.Delete(entity);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //        throw (ex);
        //    }
        //}

        /// <summary>
        /// 修改实体审核状态
        /// </summary>
        /// <param name="strEntityName">实体名</param>
        /// <param name="EntityKeyName">主键名</param>
        /// <param name="EntityKeyValue">主键值</param>
        /// <param name="CheckState">审核状态</param>
        public int UpdateCheckState(string strEntityName, string EntityKeyName, string EntityKeyValue, string CheckState)
        {
            int UpdateCont = 0;
            string qualifiedEntitySetName = ConfigurationManager.AppSettings["DBContextName"] + ".";
            Dictionary<object, object> Prameters = new Dictionary<object, object>();
            if (!string.IsNullOrEmpty(CheckState))
            {
                Prameters.Add("CHECKSTATES", System.Convert.ToDecimal(CheckState));
            }
            try
            {
                Tracer.Debug("更新到了修改状态------------进入try");
                System.Data.EntityKey entityKey = new System.Data.EntityKey(qualifiedEntitySetName + strEntityName, EntityKeyName, EntityKeyValue);
                object obj = GetObjectByEntityKey(entityKey);
                if (obj != null)
                {
                    Type a = obj.GetType();

                    PropertyInfo[] infos = a.GetProperties();
                    foreach (PropertyInfo prop in infos)
                    {
                        if (Prameters.ContainsKey(prop.Name))
                        {
                            prop.SetValue(obj, Prameters[prop.Name], null);
                        }
                    }
                    UpdateCont = base.Update(obj);
                    Tracer.Debug("更新到了修改状态" + UpdateCont.ToString());
                }
            }
            catch (Exception ex)
            {
                Tracer.Debug("捕捉到异常信息Message：" + ex.Message + "，Source：" + ex.Source + "，StackTrace：" + ex.StackTrace+"，DateTime:"+System.DateTime.Now.ToString());
                //throw (ex);
            }
            return UpdateCont;
        }


        public IQueryable<TEntity> GetTable()
        {
            return base.GetTable<TEntity>();
        }

        public ObjectQuery<TEntity> GetObjects()
        {
            return base.GetObjects<TEntity>();
        }       

    }   
}
