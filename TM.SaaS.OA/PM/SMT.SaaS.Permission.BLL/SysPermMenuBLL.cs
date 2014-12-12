using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SMT.SaaS.Permission.DAL;
using SMT_System_EFModel;

namespace SMT.SaaS.Permission.BLL
{
    //public class SysPermMenuBLL : BaseBll<T_SYS_PERM_MENU>
    //{
    //    /// <summary>
    //    /// 根据系统类型获取权限菜单信息
    //    /// </summary>
    //    /// <param name="sysType">系统类型,为空时获取所有类型的系统权限菜单</param>
    //    /// <returns>权限菜单信息列表</returns>
    //    public IQueryable<T_SYS_PERM_MENU> GetSysPermMenuByType(string sysType)
    //    {

    //        var ents = from a in DataContext.T_SYS_PERM_MENU.Include("T_SYS_MENU").Include("T_SYS_PERMISSION")
    //                        where string.IsNullOrEmpty(sysType) || a.T_SYS_PERMISSION.SYSTEMTYPE == sysType 
    //                        select a;
    //        return ents;

    //    }
    //    /// <summary>
    //    /// 根据权限菜单ID获取权限菜单信息
    //    /// </summary>
    //    /// <param name="sysType">权限菜单ID</param>
    //    /// <returns>权限菜单信息</returns>
    //    public T_SYS_PERM_MENU GetSysPermMenuByID(string permMenuID)
    //    {
    //        var ents = from ent in DataContext.T_SYS_PERM_MENU.Include("T_SYS_MENU").Include("T_SYS_PERMISSION")
    //                   where ent.ID == permMenuID
    //                   select ent;
    //        return ents.Count() > 0 ? ents.FirstOrDefault() : null;

    //    }
    //    /// <summary>
    //    /// 修改系统权限菜单
    //    /// </summary>
    //    /// <param name="entity">被修改的权限菜单的实体</param>
    //    public void SysPermMenuUpdate(T_SYS_PERM_MENU sourceEntity)
    //    {
    //        try
    //        {
    //            var ents = from ent in dal.GetTable()
    //                       where ent.ID == sourceEntity.ID
    //                        select ent;

    //            if (ents.Count() > 0)
    //            {
    //                var ent = ents.FirstOrDefault();

    //                ent.CREATEDATE = sourceEntity.CREATEDATE;
    //                ent.CREATEUSER = sourceEntity.CREATEUSER;
    //                ent.UPDATEDATE = sourceEntity.UPDATEDATE;
    //                ent.UPDATEUSER = sourceEntity.UPDATEUSER;
    //                ent.T_SYS_PERMISSIONReference.EntityKey =
    //                    new System.Data.EntityKey("SMT_System_EFModelContext.T_SYS_PERMISSION", "PERMISSIONID", sourceEntity.T_SYS_PERMISSION.PERMISSIONID);
    //                ent.T_SYS_MENUReference.EntityKey = 
    //                    new System.Data.EntityKey("SMT_System_EFModelContext.T_SYS_MENU","MENUID",sourceEntity.T_SYS_MENU.MENUID);
                        
    //                dal.Update(ent);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw (ex);
    //        }
    //    }
    //    /// <summary>
    //    /// 添加系统权限菜单
    //    /// </summary>
    //    /// <param name="sourceEntity">被添加的权限菜单的实体</param>
    //    public void SysPermMenuAdd(T_SYS_PERM_MENU sourceEntity)
    //    {
    //        try
    //        {                
    //            T_SYS_PERM_MENU ent = new T_SYS_PERM_MENU();
    //            ent.ID = sourceEntity.ID;
    //            ent.CREATEDATE = sourceEntity.CREATEDATE;
    //            ent.CREATEUSER = sourceEntity.CREATEUSER;
    //            ent.UPDATEDATE = sourceEntity.UPDATEDATE;
    //            ent.UPDATEUSER = sourceEntity.UPDATEUSER;
    //            ent.T_SYS_PERMISSIONReference.EntityKey =
    //                new System.Data.EntityKey("SMT_System_EFModelContext.T_SYS_PERMISSION", "PERMISSIONID", sourceEntity.T_SYS_PERMISSION.PERMISSIONID);
    //            ent.T_SYS_MENUReference.EntityKey =
    //                new System.Data.EntityKey("SMT_System_EFModelContext.T_SYS_MENU", "MENUID", sourceEntity.T_SYS_MENU.MENUID);
    //            dal.Add(ent);
    //        }
    //        catch (Exception ex)
    //        {
    //            throw (ex);
    //        }
    //    }

    //    /// <summary>
    //    /// 删除系统权限菜单
    //    /// </summary>
    //    /// <param name="menuID">权限菜单ID</param>
    //    /// <returns>是否删除成功</returns>
    //    public bool SysPermMenuDelete(string id)
    //    {
    //        try
    //        {
    //            var entitys = (from ent in dal.GetTable()
    //                           where ent.ID == id
    //                           select ent);
    //            if (entitys.Count() > 0)
    //            {
    //                var entity = entitys.FirstOrDefault();
    //                dal.Delete(entity);
    //                return true;
    //            }
    //            return false;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw (ex);
    //        }
    //    }
    //}
}
