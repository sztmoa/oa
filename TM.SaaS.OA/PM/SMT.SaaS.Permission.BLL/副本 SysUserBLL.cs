/*
 * 文件名：SysUserBLL.cs
 * 作  用：
 * 创建人：
 * 创建时间：2010-2-26 14:19:12
 * 修改人：向寒咏
 * 修改说明：增加缓存
 * 修改时间：2010-7-7 14:19:12
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMT.SaaS.Permission.DAL;
using SMT_System_EFModel;
using System.Data.Objects.DataClasses;
using System.Collections;
using System.Linq.Dynamic;
using System.Data.Objects;
namespace SMT.SaaS.Permission.BLL
{
    #region 系统用户
    public class SysUserBLL2 : BaseBll<T_SYS_USER>, ILookupEntity
    {
        private List<V_Permission> plist;
        public List<V_Permission> Plist
        {
            get
            {
                List<V_Permission> lsdic;
                if (CacheManager.GetCache("V_Permission") != null)
                {
                    lsdic = (List<V_Permission>)CacheManager.GetCache("V_Permission");
                }
                else
                {

                    var ents = from u in base.GetObjects()
                               join ur in dal.GetObjects<T_SYS_USERROLE>() on u.SYSUSERID equals ur.T_SYS_USER.SYSUSERID
                               join r in dal.GetObjects<T_SYS_ROLE>() on ur.T_SYS_ROLE.ROLEID equals r.ROLEID
                               join rem in dal.GetObjects<T_SYS_ROLEENTITYMENU>() on r.ROLEID equals rem.T_SYS_ROLE.ROLEID
                               join rmp in dal.GetObjects<T_SYS_ROLEMENUPERMISSION>() on rem.ROLEENTITYMENUID equals rmp.T_SYS_ROLEENTITYMENU.ROLEENTITYMENUID
                               join p in dal.GetObjects<T_SYS_PERMISSION>() on rmp.T_SYS_PERMISSION.PERMISSIONID equals p.PERMISSIONID
                               join em in dal.GetObjects<T_SYS_ENTITYMENU>() on rem.T_SYS_ENTITYMENU.ENTITYMENUID equals em.ENTITYMENUID
                               select new V_Permission
                               {
                                   RoleMenuPermission = rmp,
                                   Permission = rmp.T_SYS_PERMISSION,
                                   EntityMenu = em,
                                   SysUser=u
                               };

                    lsdic = ents.ToList();
                    CacheManager.AddCache("V_Permission", lsdic);
                }
                return lsdic.Count() > 0 ? lsdic : null;
            }

            set { plist = value; }
        }

        /// <summary>
        /// 根据用户名称得到用户信息
        /// </summary>
        /// <param name="userName">用户名称</param>
        /// <returns>用户信息</returns>
        public T_SYS_USER GetUserInfo(string userName)
        {
            var ents = from a in Plist.AsQueryable()
                       where a.SysUser.USERNAME == userName
                       select a.SysUser;
            return ents.Count() > 0 ? ents.FirstOrDefault() : null;
        }

        /// <summary>
        /// 根据用户ID得到用户信息
        /// </summary>
        /// <param name="userName">用户ID</param>
        /// <returns>用户信息</returns>
        public T_SYS_USER GetUserByID(string userID)
        {
            var ents = from a in Plist.AsQueryable()
                       where a.SysUser.SYSUSERID == userID
                       select a.SysUser;

            return ents.Count() > 0 ? ents.FirstOrDefault() : null;
        }

        /// <summary>
        /// 根据员工ID得到用户信息
        /// </summary>
        /// <param name="employeeID">员工ID</param>
        /// <returns>用户信息</returns>
        public T_SYS_USER GetUserByEmployeeID(string employeeID)
        {
            var ents = from a in Plist.AsQueryable()
                       where a.SysUser.EMPLOYEEID == employeeID
                       select a.SysUser;

            return ents.Count() > 0 ? ents.FirstOrDefault() : null;
        }

        public bool AddSysUserInfo(T_SYS_USER UserObj)
        {
            try
            {
                int i = dal.Add(UserObj);
                if (i == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// 修改系统用户信息
        /// </summary>
        /// <param name="UserObj"></param>
        /// <returns></returns>
        public bool UpdateSysUserInfo(T_SYS_USER UserObj)
        {

            try
            {
                var entity = from ent in dal.GetTable()
                             where ent.SYSUSERID == UserObj.SYSUSERID
                             select ent;

                if (entity.Count() > 0)
                {
                    var entitys = entity.FirstOrDefault();

                    //entitys.T_SYS_USERROLE = UserObj.T_SYS_USERROLE;
                    //entitys.USERNAME = UserObj.USERNAME;
                    //entitys.EMPLOYEENAME = UserObj.EMPLOYEENAME;
                    //entitys.EMPLOYEEID = UserObj.EMPLOYEEID;
                    //entitys.EMPLOYEECODE = UserObj.EMPLOYEECODE;
                    //entitys.UPDATEDATE = UserObj.UPDATEDATE;
                    //entitys.UPDATEUSER = UserObj.UPDATEUSER;
                    //entitys.STATE = UserObj.STATE;
                    //entitys.REMARK = UserObj.REMARK;
                    Utility.CloneEntity<T_SYS_USER>(UserObj, entitys);

                    if (dal.Update(entitys) == 1)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
                throw (ex);
            }
        }

        /// <summary>
        /// 查询是否存在相同的用户名
        /// </summary>
        /// <param name="StrName"></param>
        /// <returns></returns>
        public bool GetSysUserInfoByAdd(string StrUserName)
        {
            bool IsExist = false;
            var q = from ent in dal.GetTable()
                    where ent.USERNAME == StrUserName
                    select ent;
            if (q.Count() > 0)
            {

                IsExist = true;
            }
            return IsExist;
        }



        /// <summary>
        /// 获取所有的系统用户信息
        /// </summary>
        /// <returns>返回系统用户列表</returns>
        public List<T_SYS_USER> GetAllSysUserInfos()
        {
            try
            {
                var ents = from ent in Plist.AsQueryable()
                           where ent.SysUser.STATE == "1"
                           orderby ent.SysUser.CREATEDATE descending
                           select ent.SysUser;
                return ents.Count() > 0 ? ents.ToList() : null;

            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        /// 服务器端分页
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sort"></param>
        /// <param name="filterString"></param>
        /// <param name="paras"></param>
        /// <param name="pageCount"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public IQueryable<T_SYS_USER> GetAllSysUserInfosWithPaging(int pageIndex, int pageSize, string sort, string filterString, IList<object> paras, ref int pageCount, string userID)
        {
            try
            {
                var ents = from ent in Plist.AsQueryable()


                           select ent.SysUser;
                List<object> queryParas = new List<object>();
                queryParas.AddRange(paras);
                SetOrganizationFilter(ref filterString, ref queryParas, userID, "T_SYS_USER");
                if (queryParas.Count > 0)
                {
                    if (!string.IsNullOrEmpty(filterString))
                    {
                        ents = ents.ToList().AsQueryable().Where(filterString, queryParas.ToArray());
                    }
                }
                ents = ents.OrderBy(sort);
                ents = Utility.Pager<T_SYS_USER>(ents, pageIndex, pageSize, ref pageCount);
                return ents;

            }
            catch (Exception ex)
            {
                return null;
                throw (ex);
            }

        }
        //批量删除系统用户信息
        public string BatchDeleteSysUserInfos(string[] ArrSysUserIDs)
        {
            string StrReturn = "";
            try
            {
                foreach (var a in ArrSysUserIDs)
                {
                    var tmpUREnts = (from ent in dal.GetObjects<T_SYS_USERROLE>()
                                     where ent.T_SYS_USER.SYSUSERID == a
                                     select ent);
                    if (tmpUREnts.Count() > 0)
                    {
                        //TODO:多语言与自定义异常
                        //throw new Exception("此角色已关联用户，请先删除用户角色关联！");
                        StrReturn = "有角色和用户关联，不能删除";
                        break;
                    }
                    if (string.IsNullOrEmpty(StrReturn))
                    {
                        var entitys = from ent in GetObjects()
                                      where ent.SYSUSERID == a
                                      select ent;

                        if (entitys.Count() > 0)
                        {
                            foreach (var obj in entitys)
                            {
                                Utility.RefreshEntity(obj);                                
                                dal.DeleteFromContext(obj);
                                //dal.Delete(obj);
                            }

                        }
                    }
                }
                dal.SaveContextChanges();
                return StrReturn;

            }
            catch (Exception ex)
            {
                return "error";
                throw (ex);
            }
        }

        /// <summary>
        /// 批量更新用户状态
        /// </summary>
        /// <param name="ArrSysUserIDs"></param>
        /// <returns></returns>
        public string BatchUpdateSysUserInfos(string[] ArrSysUserIDs, string StrUserID, string UserName)
        {
            string StrReturn = "";
            try
            {

                if (ArrSysUserIDs.Length > 0)
                {

                    var entitys = from ent in Plist.AsQueryable()
                                  where ArrSysUserIDs.Contains(ent.SysUser.SYSUSERID)
                                  select ent.SysUser;

                    if (entitys.Count() > 0)
                    {
                        foreach (var obj in entitys)
                        {
                            //dal.Delete(obj);
                            obj.STATE = "0";
                            obj.UPDATEDATE = System.DateTime.Now;
                            obj.UPDATEUSER = StrUserID;
                            obj.USERNAME = UserName;

                            dal.Update(obj);
                        }

                    }

                }

                return StrReturn;

            }
            catch (Exception ex)
            {
                return "error";
                throw (ex);
            }
        }

        /// <summary>
        /// 根据用户名称得到用户所拥有的权限
        /// </summary>
        /// <param name="userName">用户名称</param>
        /// <returns>用户所拥有的权限列表</returns>
        public IQueryable<V_Permission> GetUserPermissionByUser(string userID)
        {
            //var ents = from p in DataContext.T_SYS_PERMISSION.Include("T_SYS_PERM_MENU")
            //           join rp in DataContext.T_SYS_ROLE_PERM on p.PERMISSIONID equals rp.T_SYS_PERMISSION.PERMISSIONID
            //           join ur in DataContext.T_SYS_USER_ROLE on rp.T_SYS_ROLE.ROLEID equals ur.T_SYS_ROLE.ROLEID
            //           where ur.T_SYS_USER.USERNAME == userName
            //           select rp.T_SYS_PERMISSION;
            //return ents;

            var ents = from p in Plist.AsQueryable()
                       where p.SysUser.SYSUSERID == userID
                       select p;
            //T_SYS_ROLEMENUPERMISSION p = new T_SYS_ROLEMENUPERMISSION();
            //p.T_SYS_PERMISSION.PERMISSIONID

            List<V_Permission> rl = ents.ToList();
            //return null;
            return ents;
        }

        /// <summary>
        /// 根据用户名称得到用户所拥有的权限
        /// </summary>
        /// <param name="userName">用户名称</param>
        /// <returns>用户所拥有的权限列表</returns>
        public IQueryable<V_Permission> GetPermissionByRoleID(string RoleID)
        {

            var ents = from p in Plist.AsQueryable()
                       where p.Role.ROLEID == RoleID
                       select p;
            //T_SYS_ROLEMENUPERMISSION p = new T_SYS_ROLEMENUPERMISSION();
            //p.T_SYS_PERMISSION.PERMISSIONID

            List<V_Permission> rl = ents.ToList();
            //return null;
            return ents;
        }

        /// <summary>
        /// 获取用户菜单的权限范围
        /// </summary>
        /// <param name="menuCode">菜单编码</param>
        /// <param name="userID">用户ID</param>
        /// <returns>权限</returns>
        public IQueryable<V_Permission> GetUserMenuPerms(string menuCode, string userID)
        {
            var emenu = from item in Plist.AsQueryable()
                        where item.EntityMenu.MENUCODE == menuCode
                        select item;
            if (emenu.Count() == 0)
            {
                return null;
            }
            var ents = from p in Plist.AsQueryable()
                       where p.SysUser.SYSUSERID == userID && p.EntityMenu.MENUCODE == menuCode
                       select p;

            List<V_Permission> rl = ents.ToList();
            return ents;
        }
        /// <summary>
        /// LookupEntity接口函数，取出Lookup窗口所需数据
        /// </summary>
        /// <param name="args">查询参数集</param>
        /// <returns>对像集合</returns>
        public EntityObject[] GetLookupData(Dictionary<string, string> args)
        {
            IQueryable<T_SYS_USER> objs = from a in Plist.AsQueryable()
                                          select a.SysUser;

            return objs.Count() > 0 ? objs.ToArray() : null;
        }
        public EntityObject[] GetLookupData(Dictionary<string, string> args, int pageIndex, int pageSize, string sort, string filterString, IList<object> paras, ref int pageCount)
        {
            IQueryable<T_SYS_USER> objs = from a in Plist.AsQueryable()
                                          select a.SysUser;

            return objs.Count() > 0 ? objs.ToArray() : null;
        }
        /// <summary>
        /// 返回用户信息
        /// </summary>
        /// <returns>用户信息列表</returns>
        public IQueryable<T_SYS_USER> GetUserPermissionAll()
        {
            var ents = from a in Plist.AsQueryable()
                       select a.SysUser;
            return ents;
        }




    }
    #endregion

    
}
