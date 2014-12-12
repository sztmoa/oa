using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using SMT.Saas.Tools.OrganizationWS;
using SMT.SaaS.LocalData.ViewModel;
using SMT.Saas.Tools.PermissionWS;
using SMT.SaaS.LocalData.Tables;
using SMT.SAAS.Main.CurrentContext;
using SMT.SAAS.Main.CurrentContextLoadData.Private;

namespace SMT.SAAS.Main.CurrentContextLoadData
{
    public partial class UserLogin
    {
        /// <summary>
        /// 部门实体集临时变量
        /// </summary>
        private List<SMT.Saas.Tools.OrganizationWS.T_HR_DEPARTMENT> allDepartments;
        /// <summary>
        /// 岗位实体集临时变量
        /// </summary>
        private List<SMT.Saas.Tools.OrganizationWS.T_HR_POST> allPosts;
        /// <summary>
        /// 公司实体集临时变量
        /// </summary>
        private List<SMT.Saas.Tools.OrganizationWS.T_HR_COMPANY> allCompanys;

        /// <summary>
        /// 组织架构加载完成事件
        /// </summary>
        public event EventHandler OnGetOrgCompleted;

        #region 组织架构加载

        /// <summary>
        /// 部门视图实体集临时变量
        /// </summary>
        private List<SMT.Saas.Tools.OrganizationWS.V_DEPARTMENT> allDepartmentsView;

        /// <summary>
        /// 岗位视图实体集临时变量
        /// </summary>
        private List<SMT.Saas.Tools.OrganizationWS.V_POST> allPostsView;

        /// <summary>
        /// 公司视图实体集临时变量
        /// </summary>
        private List<SMT.Saas.Tools.OrganizationWS.V_COMPANY> allCompanysView;

        #region 应用程序资源字典集合KEY
        //public const string RES_DICTIONARY_KEY = "SYS_DICTIONARY";

        /// <summary>
        /// 公司数据的内存缓存的键名
        /// </summary>
        public const string RESCOMPANYINFOKEY = "SYS_CompanyInfo";

        /// <summary>
        /// 部门数据的内存缓存的键名
        /// </summary>
        public const string RESDEPARTMENTINFOKEY = "SYS_DepartmentInfo";

        /// <summary>
        /// 岗位数据的内存缓存的键名
        /// </summary>
        public const string RESPOSTINFOKEY = "SYS_PostInfo";
        //public const string RES_ENTITYMENU_KEY = "SYS_EntityMenus";
        #endregion


        /// <summary>
        /// 加载公司数据
        /// </summary>
        public void LoadAllOrgInfo()
        {
            organClient.GetALLCompanyViewCompleted += (o, e) =>
            {
                if (e.Error == null)
                {
                    if (e.Result != null)
                    {
                        allCompanysView = e.Result.ToList();
                        ReFillDataToCompany();
                    }
                }
            };
          
            if (!Application.Current.Resources.Contains("SYS_CompanyInfo"))
            {
                organClient.GetALLCompanyViewAsync(SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID);
            }
            else
            {
                List<T_HR_COMPANY> ent = Application.Current.Resources["SYS_CompanyInfo"]
                 as List<T_HR_COMPANY>;
                if (ent.Count() == 0)
                {
                    organClient.GetALLCompanyViewAsync(SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID);
                }
                LoadDepartmentInfo();
            }
        }

        /// <summary>
        /// 将公司信息集合转换成标准结构的公司数据集合(V_COMPANY => T_HR_COMPANY),
        /// 并将转换后的数据集合存到内存中，然后再加载部门数据
        /// </summary>
        private void ReFillDataToCompany()
        {
            allCompanys = new List<SMT.Saas.Tools.OrganizationWS.T_HR_COMPANY>();
            var ents = allCompanysView.OrderBy(c => c.FATHERID);
            //把公司的视图集合转换为公司实体集合 
            foreach (var ent in ents)
            {
                SMT.Saas.Tools.OrganizationWS.T_HR_COMPANY company = new SMT.Saas.Tools.OrganizationWS.T_HR_COMPANY();
                company.COMPANYID = ent.COMPANYID;
                company.CNAME = ent.CNAME;
                company.ENAME = ent.ENAME;
                if (!string.IsNullOrEmpty(ent.BRIEFNAME))
                {
                    company.BRIEFNAME = ent.BRIEFNAME;
                }
                else
                {
                    company.BRIEFNAME = ent.CNAME;
                }
                company.COMPANRYCODE = ent.COMPANRYCODE;
                company.SORTINDEX = ent.SORTINDEX;
                if (!string.IsNullOrEmpty(ent.FATHERCOMPANYID))
                {
                    company.T_HR_COMPANY2 = new SMT.Saas.Tools.OrganizationWS.T_HR_COMPANY();
                    company.T_HR_COMPANY2.COMPANYID = ent.FATHERCOMPANYID;
                    //modify by 安凯航 2011年9月5日
                    //在父公司ID为空时,不做处理
                    V_COMPANY v_company = allCompanysView.Where(s => s.COMPANYID == ent.FATHERCOMPANYID).FirstOrDefault();
                    if (v_company != null)
                    {
                        company.T_HR_COMPANY2.CNAME = v_company.CNAME;
                    }
                }
                //end modify;
                company.FATHERID = ent.FATHERID;
                company.FATHERTYPE = ent.FATHERTYPE;
                company.CHECKSTATE = ent.CHECKSTATE;
                company.EDITSTATE = ent.EDITSTATE;
                allCompanys.Add(company);
            }
            //把公司实体集合存入缓存
            AddToResourceDictionary<List<SMT.Saas.Tools.OrganizationWS.T_HR_COMPANY>>(RESCOMPANYINFOKEY, allCompanys);
            LoadDepartmentInfo();
        }

        /// <summary>
        /// 加载部门数据
        /// </summary>
        private void LoadDepartmentInfo()
        {
            organClient.GetAllDepartmentViewCompleted += (o, e) =>
            {
                if (e.Error == null)
                {
                    if (e.Result != null)
                    {
                        allDepartmentsView = e.Result.ToList();
                        ReFillDataToDepartment();
                    }
                }
            };

            if (!Application.Current.Resources.Contains("SYS_DepartmentInfo"))
            {
                organClient.GetAllDepartmentViewAsync(SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID);
            }
            else
            {
                List<T_HR_DEPARTMENT> ent = Application.Current.Resources["SYS_DepartmentInfo"]
                 as List<T_HR_DEPARTMENT>;
                if (ent.Count() == 0)
                {
                    organClient.GetAllDepartmentViewAsync(SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID);

                }
                else
                {
                    LoadPostInfo();
                }

            }
        }

        /// <summary>
        /// 将部门信息集合转换成标准结构的部门数据集合(V_DEPARTMENT => T_HR_DEPARTMENT),
        /// 并将转换后的数据集合存到内存中，然后再加载岗位数据
        /// </summary>
        private void ReFillDataToDepartment()
        {
            allDepartments = new List<SMT.Saas.Tools.OrganizationWS.T_HR_DEPARTMENT>();
            var ents = allDepartmentsView.OrderBy(c => c.FATHERID);
            //把部门视图集合转换成部门实体集合
            foreach (var ent in ents)
            {
                SMT.Saas.Tools.OrganizationWS.T_HR_DEPARTMENT dep = new SMT.Saas.Tools.OrganizationWS.T_HR_DEPARTMENT();
                dep.DEPARTMENTID = ent.DEPARTMENTID;
                dep.FATHERID = ent.FATHERID;
                dep.FATHERTYPE = ent.FATHERTYPE;
                dep.T_HR_DEPARTMENTDICTIONARY = new SMT.Saas.Tools.OrganizationWS.T_HR_DEPARTMENTDICTIONARY();
                dep.T_HR_DEPARTMENTDICTIONARY.DEPARTMENTDICTIONARYID = ent.DEPARTMENTDICTIONARYID;
                dep.T_HR_DEPARTMENTDICTIONARY.DEPARTMENTNAME = ent.DEPARTMENTNAME;
                dep.T_HR_COMPANY = new SMT.Saas.Tools.OrganizationWS.T_HR_COMPANY();
                dep.T_HR_COMPANY = allCompanys.Where(s => s.COMPANYID == ent.COMPANYID).FirstOrDefault();

                dep.DEPARTMENTBOSSHEAD = ent.DEPARTMENTBOSSHEAD;
                dep.SORTINDEX = ent.SORTINDEX;
                dep.CHECKSTATE = ent.CHECKSTATE;
                dep.EDITSTATE = ent.EDITSTATE;
                allDepartments.Add(dep);
            }
            //把部门实体集合存入缓存和独立存储
            AddToResourceDictionary<List<SMT.Saas.Tools.OrganizationWS.T_HR_DEPARTMENT>>(RESDEPARTMENTINFOKEY, allDepartments);
            LoadPostInfo();
        }

        /// <summary>
        /// 加载岗位
        /// </summary>
        private void LoadPostInfo()
        {
            organClient.GetAllPostViewCompleted += (o, e) =>
            {
                if (e.Error == null)
                {
                    if (e.Result != null)
                    {
                        allPostsView = e.Result.ToList();
                        ReFillDataToPost();
                        if (OnGetOrgCompleted != null)
                        {
                            OnGetOrgCompleted(this, null);
                        }
                    }
                }
            };

            if (!Application.Current.Resources.Contains("SYS_PostInfo"))
            {
                organClient.GetAllPostViewAsync(SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID);
            }
            else
            {
                List<T_HR_POST> ent = Application.Current.Resources["SYS_PostInfo"]
                 as List<T_HR_POST>;
                if (ent.Count() == 0)
                {
                    organClient.GetAllPostViewAsync(SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID);

                }
                else
                {
                    if (OnGetOrgCompleted != null)
                    {
                        OnGetOrgCompleted(this, null);
                    }
                }
            }
        }

        /// <summary>
        /// 将岗位信息集合转换成标准结构的部岗位数据集合(V_DEPARTMENT => T_HR_DEPARTMENT),
        /// 并将转换后的数据集合存到内存中，然后再加载岗位数据
        /// </summary>
        private void ReFillDataToPost()
        {
            allPosts = new List<SMT.Saas.Tools.OrganizationWS.T_HR_POST>();
            //把岗位视图集合转换为岗位实体集合
            foreach (var ent in allPostsView)
            {
                SMT.Saas.Tools.OrganizationWS.T_HR_POST pt = new SMT.Saas.Tools.OrganizationWS.T_HR_POST();
                pt.POSTID = ent.POSTID;
                pt.FATHERPOSTID = ent.FATHERPOSTID;
                pt.CHECKSTATE = ent.CHECKSTATE;
                pt.EDITSTATE = ent.EDITSTATE;

                pt.T_HR_POSTDICTIONARY = new SMT.Saas.Tools.OrganizationWS.T_HR_POSTDICTIONARY();
                pt.T_HR_POSTDICTIONARY.POSTDICTIONARYID = Guid.NewGuid().ToString();
                pt.T_HR_POSTDICTIONARY.POSTNAME = ent.POSTNAME;

                pt.T_HR_DEPARTMENT = new SMT.Saas.Tools.OrganizationWS.T_HR_DEPARTMENT();
                pt.T_HR_DEPARTMENT = allDepartments.Where(s => s.DEPARTMENTID == ent.DEPARTMENTID).FirstOrDefault();

                allPosts.Add(pt);
            }
            //把岗位实体集合存入缓存和独立存储
            AddToResourceDictionary<List<SMT.Saas.Tools.OrganizationWS.T_HR_POST>>(RESPOSTINFOKEY, allPosts);
            if (OnGetOrgCompleted != null)
            {
                OnGetOrgCompleted(this, null);
            }
            //第三阶段优化
            //SaveOrganizationByLocal(SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.SysUserID);
        }

        /// <summary>
        /// 从本地数据库读取登陆人所能查看到的公司信息，而后加载到缓存中
        /// </summary>
        /// <param name="strUserID"></param>
        public void GetCompanysByLocal(string strUserID)
        {
            if (Application.Current.Resources["SYS_CompanyInfo"] != null && Application.Current.Resources["SYS_DepartmentInfo"] != null && Application.Current.Resources["SYS_PostInfo"] != null)
            {
                return;
            }

            List<SMT.Saas.Tools.OrganizationWS.V_COMPANY> companyinfo = new List<SMT.Saas.Tools.OrganizationWS.V_COMPANY>();

            List<V_CompanyInfo> vLocalCompanys = V_CompanyInfoVM.GetAllV_CompanyInfo(strUserID);

            foreach (var item in vLocalCompanys)
            {
                if (string.IsNullOrWhiteSpace(item.COMPANYID))
                {
                    continue;
                }

                SMT.Saas.Tools.OrganizationWS.V_COMPANY info = item.CloneObject<SMT.Saas.Tools.OrganizationWS.V_COMPANY>(new SMT.Saas.Tools.OrganizationWS.V_COMPANY());

                companyinfo.Add(info);
            }

            allCompanysView = companyinfo;
            LoadAllOrgInfo();
        }

        /// <summary>
        /// 从本地数据库读取登陆人所能查看到的部门信息，而后加载到缓存中
        /// </summary>
        /// <param name="strUserID"></param>
        public void GetDepartmentsByLocal(string strUserID)
        {
            List<SMT.Saas.Tools.OrganizationWS.V_DEPARTMENT> departmentinfos = new List<SMT.Saas.Tools.OrganizationWS.V_DEPARTMENT>();

            List<V_DepartmentInfo> vLocalCompanys = V_DepartmentInfoVM.GetAllV_DepartmentInfo(strUserID);

            foreach (var item in vLocalCompanys)
            {
                if (string.IsNullOrWhiteSpace(item.DEPARTMENTID))
                {
                    continue;
                }

                SMT.Saas.Tools.OrganizationWS.V_DEPARTMENT info = item.CloneObject<SMT.Saas.Tools.OrganizationWS.V_DEPARTMENT>(new SMT.Saas.Tools.OrganizationWS.V_DEPARTMENT());

                departmentinfos.Add(info);
            }

            allDepartmentsView = departmentinfos;
        }

        /// <summary>
        /// 从本地数据库读取登陆人所能查看到的岗位信息，而后加载到缓存中
        /// </summary>
        /// <param name="strUserID"></param>
        public void GetPostsByLocal(string strUserID)
        {
            List<SMT.Saas.Tools.OrganizationWS.V_POST> postinfos = new List<SMT.Saas.Tools.OrganizationWS.V_POST>();

            List<V_PostInfo> vLocalPosts = V_PostInfoVM.GetAllV_PostInfo(strUserID);

            foreach (var item in vLocalPosts)
            {
                if (string.IsNullOrWhiteSpace(item.POSTID))
                {
                    continue;
                }

                SMT.Saas.Tools.OrganizationWS.V_POST info = item.CloneObject<SMT.Saas.Tools.OrganizationWS.V_POST>(new SMT.Saas.Tools.OrganizationWS.V_POST());

                postinfos.Add(info);
            }

            allPostsView = postinfos;
        }

        /// <summary>
        /// 保存组织架构到本地数据库
        /// </summary>
        /// <param name="strUserID"></param>
        private void SaveOrganizationByLocal(string strUserID)
        {
            List<V_CompanyInfo> vLocalCompanys = new List<V_CompanyInfo>();
            List<V_DepartmentInfo> vLocalDepartments = new List<V_DepartmentInfo>();
            List<V_PostInfo> vLocalPosts = new List<V_PostInfo>();

            foreach (var item in allCompanysView)
            {
                if (string.IsNullOrWhiteSpace(item.COMPANYID))
                {
                    continue;
                }

                V_CompanyInfo info = item.CloneObject<V_CompanyInfo>(new V_CompanyInfo());
                info.UserModuleID = System.Guid.NewGuid().ToString();
                info.UserID = strUserID;
                vLocalCompanys.Add(info);

                if (allDepartmentsView != null)
                {
                    if (allDepartmentsView.Count() > 0)
                    {
                        foreach (var p in allDepartmentsView)
                        {
                            V_DepartmentInfo departmentInfo = item.CloneObject<V_DepartmentInfo>(new V_DepartmentInfo());
                            departmentInfo.UserModuleID = info.UserModuleID;
                            departmentInfo.UserID = strUserID;

                            vLocalDepartments.Add(departmentInfo);
                        }
                    }
                }

                if (allPostsView != null)
                {
                    if (allPostsView.Count() > 0)
                    {
                        foreach (var d in allPostsView)
                        {
                            V_PostInfo postInfo = item.CloneObject<V_PostInfo>(new V_PostInfo());
                            postInfo.UserModuleID = info.UserModuleID;
                            postInfo.UserID = strUserID;

                            vLocalPosts.Add(postInfo);
                        }
                    }
                }
            }

            V_CompanyInfoVM.SaveV_CompanyInfo(strUserID, vLocalCompanys);
            V_DepartmentInfoVM.SaveV_DepartmentInfo(strUserID, vLocalDepartments);
            V_PostInfoVM.SaveV_PostInfo(strUserID, vLocalPosts);
        }

        /// <summary>
        /// 保存记录到内存中
        /// </summary>
        /// <typeparam name="T">待存储记录</typeparam>
        /// <param name="key">字典键</param>
        /// <param name="value">字典值</param>
        private static void AddToResourceDictionary<T>(string key, T value)
        {
            try
            {
                if (Application.Current.Resources[key] == null)
                {
                    if (value != null)
                    {
                        if (Application.Current.Resources.Contains(key))
                        {
                            Application.Current.Resources.Remove(key);
                        }

                        Application.Current.Resources.Add(key, value);
                    }
                }
            }
            catch (Exception ex)
            {
                string strmsg = "将资源添加到内存时发生错误，原因：" + ex.ToString();
                SMT.SAAS.Main.CurrentContext.AppContext.SystemMessage(strmsg);
            }
        }
       
        #endregion 组织架构加载
    }
}
