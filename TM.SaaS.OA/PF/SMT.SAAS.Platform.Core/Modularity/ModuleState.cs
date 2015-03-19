

namespace SMT.SAAS.Platform.Core.Modularity
{
    /// <summary>
    /// 定义<see cref="ModuleInfo"/>状态，可以在加载模块或初始化的时候访问。
    /// </summary>
    public enum ModuleState
    {
        /// <summary>
        /// <see cref="ModuleInfo"/>初始状态。<see cref="ModuleInfo"/>已定义，但没有加载，初始化，检索。
        /// </summary>
        NotStarted,

        /// <summary>
        /// 包含当前模块的程序集正在使用<see cref="IModuleTypeLoader"/>进行加载。
        /// <see cref="IModuleTypeLoader"/>. 
        /// </summary>
        LoadingTypes,

        /// <summary>
        /// 包含此模块的程序集已经存在。
        /// 意思就是<see cref="IModule"/> 可以进行实例化或初始化。
        /// </summary>
        ReadyForInitialization,

        /// <summary>
        /// 模块正在使用<see cref="IModuleInitializer"/>初始化。
        /// </summary>
        Initializing,

        /// <summary>
        /// 模块已经初始化，可以使用。
        /// </summary>
        Initialized
    }
}
