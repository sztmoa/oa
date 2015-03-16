
/*
 * 文件名：SystemSettingsBLL.cs
 * 作  用：T_FB_SYSTEMSETTINGS 业务逻辑类
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

namespace SMT.FBAnalysis.BLL
{
    public class SystemSettingsBLL : BaseBll<T_FB_SYSTEMSETTINGS>
    {
        public SystemSettingsBLL()
        { }

        #region 获取数据

        #region	是否月结标记
        
        /// <summary>
        /// 当前月是否已做过月结
        /// </summary>
        public static bool IsChecked
        {
            get
            {
                #region 是否本月有结算
                SystemSettingsDAL dalSystemSettings = new SystemSettingsDAL();
                T_FB_SYSTEMSETTINGS systemSetting = dalSystemSettings.GetObjects().FirstOrDefault();
                
                var isChecked = false;
                if (systemSetting.LASTCHECKDATE != null)
                {
                    var checkDate = systemSetting.LASTCHECKDATE.Value;
                    var nowDate = System.DateTime.Now.Date;
                    if (checkDate.Year == nowDate.Year && checkDate.Month == nowDate.Month)
                    {
                        isChecked = true;
                    }
                }
                return isChecked;
                #endregion
            }
        }
        #endregion

        /// <summary>
        /// 获取T_FB_SYSTEMSETTINGS信息
        /// </summary>
        /// <param name="strSystemSettingsId">主键索引</param>
        /// <returns></returns>
        public T_FB_SYSTEMSETTINGS GetSystemSettingsByID(string strSystemSettingsId)
        {
            if (string.IsNullOrEmpty(strSystemSettingsId))
            {
                return null;
            }

            SystemSettingsDAL dalSystemSettings = new SystemSettingsDAL();
            StringBuilder strFilter = new StringBuilder();
            List<string> objArgs = new List<string>();
            
            if (!string.IsNullOrEmpty(strSystemSettingsId))
            {
                strFilter.Append(" SYSTEMSETTINGSID == @0");
                objArgs.Add(strSystemSettingsId);
            }

            T_FB_SYSTEMSETTINGS entRd = dalSystemSettings.GetSystemSettingsRdByMultSearch(strFilter.ToString(), objArgs.ToArray());
            return entRd;
        }

        /// <summary>
        /// 根据条件，获取T_FB_SYSTEMSETTINGS信息
        /// </summary>
        /// <param name="strVacName"></param>
        /// <param name="strVacYear"></param>
        /// <param name="strCountyType"></param>
        /// <param name="strSortKey"></param>
        /// <returns></returns>
        public static IQueryable<T_FB_SYSTEMSETTINGS> GetAllSystemSettingsRdListByMultSearch(string strFilter, List<object> objArgs, string strSortKey)
        {
            SystemSettingsDAL dalSystemSettings = new SystemSettingsDAL();
            string strOrderBy = string.Empty;

            if (!string.IsNullOrEmpty(strSortKey))
            {
                strOrderBy = strSortKey;
            }
            else
            {
                strOrderBy = " SYSTEMSETTINGSID ";
            }

            var q = dalSystemSettings.GetSystemSettingsRdListByMultSearch(strOrderBy, strFilter, objArgs.ToArray());
            return q;
        }

        /// <summary>
        /// 根据条件，获取T_FB_SYSTEMSETTINGS信息,并进行分页
        /// </summary>
        /// <param name="strFilter">查询条件</param>
        /// <param name="objArgs">查询参数</param>
        /// <param name="strSortKey">排序字段</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="pageSize">页码大小</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>T_FB_SYSTEMSETTINGS信息</returns>
        public IQueryable<T_FB_SYSTEMSETTINGS> GetSystemSettingsRdListByMultSearch(string strFilter, List<object> objArgs,
            string strSortKey, int pageIndex, int pageSize, ref int pageCount)
        {
            var q = GetAllSystemSettingsRdListByMultSearch(strFilter, objArgs, strSortKey);

            return Utility.Pager<T_FB_SYSTEMSETTINGS>(q, pageIndex, pageSize, ref pageCount);
        }

        #endregion
               
    }
}

