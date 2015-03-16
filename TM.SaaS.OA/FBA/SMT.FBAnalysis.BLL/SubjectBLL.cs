
/*
 * 文件名：SubjectBLL.cs
 * 作  用：T_FB_SUBJECT 业务逻辑类
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
    public class SubjectBLL
    {
        public SubjectBLL()
        { }

        #region 获取数据

        /// <summary>
        /// 获取T_FB_SUBJECT信息
        /// </summary>
        /// <param name="strSubjectId">主键索引</param>
        /// <returns></returns>
        public T_FB_SUBJECT GetSubjectByID(string strSubjectId)
        {
            if (string.IsNullOrEmpty(strSubjectId))
            {
                return null;
            }

            SubjectDAL dalSubject = new SubjectDAL();
            StringBuilder strFilter = new StringBuilder();
            List<string> objArgs = new List<string>();

            if (!string.IsNullOrEmpty(strSubjectId))
            {
                strFilter.Append(" SUBJECTID == @0");
                objArgs.Add(strSubjectId);
            }

            T_FB_SUBJECT entRd = dalSubject.GetSubjectRdByMultSearch(strFilter.ToString(), objArgs.ToArray());
            return entRd;
        }

        /// <summary>
        /// 根据条件，获取T_FB_SUBJECT信息
        /// </summary>
        /// <param name="strVacName"></param>
        /// <param name="strVacYear"></param>
        /// <param name="strCountyType"></param>
        /// <param name="strSortKey"></param>
        /// <returns></returns>
        public static IQueryable<T_FB_SUBJECT> GetAllSubjectRdListByMultSearch(string strFilter, List<object> objArgs, string strSortKey)
        {
            SubjectDAL dalSubject = new SubjectDAL();
            string strOrderBy = string.Empty;

            if (!string.IsNullOrEmpty(strSortKey))
            {
                strOrderBy = strSortKey;
            }
            else
            {
                strOrderBy = " SUBJECTID ";
            }

            var q = dalSubject.GetSubjectRdListByMultSearch(strOrderBy, strFilter, objArgs.ToArray());
            return q;
        }

        /// <summary>
        /// 根据条件，获取T_FB_SUBJECT信息,并进行分页
        /// </summary>
        /// <param name="strFilter">查询条件</param>
        /// <param name="objArgs">查询参数</param>
        /// <param name="strSortKey">排序字段</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="pageSize">页码大小</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>T_FB_SUBJECT信息</returns>
        public IQueryable<T_FB_SUBJECT> GetSubjectRdListByMultSearch(string strFilter, List<object> objArgs,
            string strSortKey, int pageIndex, int pageSize, ref int pageCount)
        {
            var q = GetAllSubjectRdListByMultSearch(strFilter, objArgs, strSortKey);

            return Utility.Pager<T_FB_SUBJECT>(q, pageIndex, pageSize, ref pageCount);
        }

        /// <summary>
        /// 获取所有 SUBJECT。
        /// </summary>
        /// <returns>返回结果列表。</returns>
        public IQueryable<T_FB_SUBJECT> GetAllSubject(string strOwnerID)
        {
            SubjectDAL dalSubject = new SubjectDAL();

            string filterString = string.Empty;
            List<object> objArgs = new List<object>();

            if (!string.IsNullOrEmpty(strOwnerID))
            {
                SMT.SaaS.BLLCommonServices.Utility ul = new SaaS.BLLCommonServices.Utility();
                ul.SetOrganizationFilter(ref filterString, ref objArgs, strOwnerID, "T_FB_SUBJECT");
            }

            return dalSubject.GetAllSubject();
        }

        /// <summary>
        /// 根据机构ID及其组织架构类型返回该机构下的所有科目类型
        /// </summary>
        /// <param name="strOwnerID"></param>
        /// <param name="strOrgType"></param>
        /// <param name="strOrgId"></param>
        /// <returns></returns>
        public List<T_FB_SUBJECT> GetSubjectListByOrgType(string strOwnerID, string strOrgType, string strOrgId)
        {
            List<T_FB_SUBJECT> entResList = new List<T_FB_SUBJECT>();
            if (string.IsNullOrWhiteSpace(strOwnerID) || string.IsNullOrWhiteSpace(strOrgType) || string.IsNullOrWhiteSpace(strOrgId))
            {
                return entResList;
            }

            switch (strOrgType.ToUpper())
            {
                case "COMPANY":
                    GetSubjectByCompany(strOwnerID, strOrgId, "T_FB_SUBJECTCOMPANY", ref entResList);
                    break;
                case "DEPARTMENT":
                    GetSubjectByDeptment(strOwnerID, strOrgId, "T_FB_SUBJECTDEPTMENT", ref entResList);
                    break;
            }

            return entResList;
        }

        /// <summary>
        /// 根据公司ID及其组织架构类型返回该公司下的所有科目类型
        /// </summary>
        /// <param name="strOwnerID"></param>
        /// <param name="strOrgId"></param>
        /// <param name="strModelCode"></param>
        /// <param name="entResList"></param>
        private void GetSubjectByCompany(string strOwnerID, string strOrgId, string strModelCode, ref List<T_FB_SUBJECT> entResList)
        {
            SubjectDAL dalSubject = new SubjectDAL();
            decimal dActived = 1;
            var ents = from v in dalSubject.GetObjects<T_FB_SUBJECTCOMPANY>().Include("T_FB_SUBJECT")
                       where v.ACTIVED == dActived && v.OWNERCOMPANYID == strOrgId
                       select v;

            string filterString = string.Empty;
            List<object> objArgs = new List<object>();

            SMT.SaaS.BLLCommonServices.Utility ul = new SaaS.BLLCommonServices.Utility();
            ul.SetOrganizationFilter(ref filterString, ref objArgs, strOwnerID, "T_FB_SUBJECTCOMPANY");

            ents = ents.Where(filterString, objArgs.ToArray());

            if (ents == null)
            {
                return;
            }

            if (ents.Count() == 0)
            {
                return;
            }

            var res = ents.Select(t => t.T_FB_SUBJECT);

            if (res == null)
            {
                return;
            }

            entResList = res.ToList();
        }

        /// <summary>
        /// 根据部门ID及其组织架构类型返回该部门下的所有科目类型
        /// </summary>
        /// <param name="strOwnerID"></param>
        /// <param name="strOrgId"></param>
        /// <param name="strModelCode"></param>
        /// <param name="entRes"></param>
        private void GetSubjectByDeptment(string strOwnerID, string strOrgId, string strModelCode, ref List<T_FB_SUBJECT> entResList)
        {
            SubjectDAL dalSubject = new SubjectDAL();
            decimal dActived = 1;
            var ents = from v in dalSubject.GetObjects<T_FB_SUBJECTDEPTMENT>().Include("T_FB_SUBJECT")
                       where v.ACTIVED == dActived && v.OWNERDEPARTMENTID == strOrgId
                       select v;

            string filterString = string.Empty;
            List<object> objArgs = new List<object>();

            SMT.SaaS.BLLCommonServices.Utility ul = new SaaS.BLLCommonServices.Utility();
            ul.SetOrganizationFilter(ref filterString, ref objArgs, strOwnerID, "T_FB_SUBJECTDEPTMENT");

            if (ents == null)
            {
                return;
            }

            if (ents.Count() == 0)
            {
                return;
            }

            var res = ents.Select(t => t.T_FB_SUBJECT);

            if (res == null)
            {
                return;
            }

            entResList = res.ToList();
        }
        #endregion
    }
}

