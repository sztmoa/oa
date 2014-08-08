using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMT.SAAS.MP.DAL
{
    /// <summary>
    /// IDAL层基础接口
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    public interface IDAL<T>
    {
        /// <summary>
        /// 根据给定的实体判断实体是否存在
        /// </summary>
        /// <param name="entity">实体参数</param>
        /// <returns>是否存在。True：存在，False：不存在</returns>
        bool ExistsEntity(T entity);
        /// <summary>
        /// 根据给定的实体添加到数据库
        /// </summary>
        /// <param name="entity">实体参数</param>
        /// <returns>是否新增成功。True：成功，False：不成功</returns>
        bool AddEntity(T entity);
        /// <summary>
        ///更新给定的实体到数据库
        /// </summary>
        /// <param name="entity">实体参数</param>
        /// <returns>是否更新成功。True：成功，False：不成功</returns>
        bool UpdateEntity(T entity);
        ///从数据库删除给定的实体数据
        /// </summary>
        /// <param name="entity">实体参数</param>
        /// <returns>是否删除成功。True：成功，False：不成功</returns>
        bool DeleteEntity(T entity);
        /// <summary>
        /// 根据给定的关键值获取单个实体对象
        /// </summary>
        /// <param name="key">主键.关键字</param>
        /// <returns>类型<see cref="T"/>.指定的数据库实体</returns>
        T GetEntity(string key);
        /// <summary>
        /// 返回所有数据库实体
        /// </summary>
        /// <returns>类型<see cref="List<T>"/>.指定的数据库实体集合</returns>
        List<T> GetEntityList();
        /// <summary>
        /// 查询分页数据.根据给定的分页参数返回结果集.
        /// PageSize:单页的数据条数
        /// PageIndex:要获取的页编码
        /// strWhere：条件
        /// </summary>
        /// <param name="PageSize">单页的数据条数</param>
        /// <param name="PageIndex">要获取的页编码</param>
        /// <param name="strWhere">条件</param>
        /// <returns>指定的数据集合</returns>
        List<T> GetEntityListByPage(int PageSize, int PageIndex, string strWhere);
       
    }
}
