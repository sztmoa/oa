using System;
using System.Collections.Generic;

namespace SMT.SAAS.Platform.Core
{
    /// <summary>
    /// 为<see cref="List{T}"/>定义的扩展方法。
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// 根据给定的过滤筛选条件，从集合中移除不匹配的元素。
        /// </summary>
        /// <typeparam name="T">
        /// 列表元素类型。<see cref="Type"/> 。
        /// </typeparam>
        /// <param name="list">
        /// <see langword="this"/>.
        /// </param>
        /// <param name="filter">
        /// 定义了移除元素条件的委托。
        /// </param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public static void RemoveAll<T>(this List<T> list, Func<T, bool> filter)
        {
            if (list == null) throw new ArgumentNullException("list");
            if (filter == null) throw new ArgumentNullException("filter");
            for (int i = 0; i < list.Count; i++)
            {
                if (filter(list[i]))
                {
                    list.Remove(list[i]);
                }
            }
        }
    }
}
