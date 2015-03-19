using System;

namespace SMT.SAAS.Platform.Core.Modularity
{
    /// <summary>
    /// 定义用于模块加载的接口。
    /// </summary>
    public interface IModuleTypeLoader
    {
        /// <summary>
        /// 若当前加载器需要检索 <paramref name="moduleInfo"/>，那么要对 <see cref="ModuleInfo.Ref"/> 进行验证。
        /// </summary>
        /// <param name="moduleInfo">
        /// 模块需要具有加载类型。
        /// </param>
        /// <returns>
        /// 若当前加载器能够找带模块返回<see langword="true"/>，否则返回 <see langword="false"/>
        /// </returns>
        bool CanLoadModuleType(ModuleInfo moduleInfo);      

        /// <summary>
        /// 检索 <paramref name="moduleInfo"/>。
        /// </summary>
        /// <param name="moduleInfo">
        /// 模块需要有加载的类型。
        /// </param>
        void LoadModuleType(ModuleInfo moduleInfo);
   
        /// <summary>
        /// 模块在后台下载的进程发送更改时触发。
        /// </summary>
        event EventHandler<ModuleDownloadProgressChangedEventArgs> ModuleDownloadProgressChanged;

        /// <summary>
        /// 模块加载成功或失败时触发。
        /// </summary>
        /// <remarks>
        /// </remarks>
        event EventHandler<LoadModuleCompletedEventArgs> LoadModuleCompleted;
    }
}
