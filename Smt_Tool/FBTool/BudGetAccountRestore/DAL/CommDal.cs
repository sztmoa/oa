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


namespace DAL
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

        public DataTable GetDataTable(string Sqlstring)
        {
            OracleDAO dao = new OracleDAO(conn); 
            DataTable dt = dao.GetDataTable(Sqlstring);
            return dt;
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
            Prameters.Add("CHECKSTATE", CheckState);
            try
            {
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
                }
            }
            catch (Exception ex)
            {
                Tracer.Debug("Message：" + ex.Message + "，Source：" + ex.Source + "，StackTrace：" + ex.StackTrace+"，DateTime:"+System.DateTime.Now.ToString());
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
