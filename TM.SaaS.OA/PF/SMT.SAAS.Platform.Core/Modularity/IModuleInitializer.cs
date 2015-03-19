
   

namespace SMT.SAAS.Platform.Core.Modularity
{
    /// <summary>
    /// 声明一个用于将模块初始化到应用程序的服务。
    /// </summary>
    public interface IModuleInitializer
    {
        /// <summary>
        /// 初始化指定模块
        /// </summary>
        /// <param name="moduleInfo">
        /// 要初始化的模块。
        /// </param>
        void Initialize(ModuleInfo moduleInfo);
    }
}
