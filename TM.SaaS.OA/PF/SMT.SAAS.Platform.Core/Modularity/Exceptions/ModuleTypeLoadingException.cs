using System;
using System.Globalization;
   
namespace SMT.SAAS.Platform.Core.Modularity
{
    /// <summary>
    /// <see cref="IModuleManager"/>接口的实现加载模块失败时需要抛出的异常。
    /// </summary>
    public partial class ModuleTypeLoadingException : ModularityException
    {
        /// <summary>
        /// 初始化<see cref="ModuleTypeLoadingException"/>的实例
        /// </summary>
        public ModuleTypeLoadingException()
            : base()
        {
        }

        /// <summary>
        /// 根据指定的错误消息，初始化<see cref="ModuleTypeLoadingException"/>的实例
        /// </summary>
        /// <param name="message">
        /// 错误的描述信息。
        /// </param>
        public ModuleTypeLoadingException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 根据指定的错误消息和产生当前异常的内部异常，初始化<see cref="ModuleTypeLoadingException"/>的实例
        /// </summary>
        /// <param name="message">
        /// 解释、描述错误原因的消息。
        /// </param>
        /// 
        /// </param>
        public ModuleTypeLoadingException(string message, Exception exception)
            : base(message, exception)
        {
        }

        /// <summary>
        /// 根据给定的模块信息和错误信息，初始化<see cref="ModuleTypeLoadingException"/>的实例
        /// </summary>
        /// <param name="moduleName">
        /// 模块名称。
        /// </param>
        /// <param name="message">
        /// 用于说明产生错误的具体原因。
        /// </param>
        public ModuleTypeLoadingException(string moduleName, string message)
            : this(moduleName, message, null)
        {
        }


        /// <summary>
        /// 根据给定的模块信息和错误信息、内部异常，初始化<see cref="ModuleTypeLoadingException"/>的实例。
        /// </summary>
        /// <param name="moduleName">
        /// 模块名称。
        /// </param>
        /// <param name="message">
        /// 用于说明产生错误的具体原因。
        /// </param>
        /// <param name="innerException">
        /// 产生当前异常的内部异常，
        /// 如果没有特定的内部异常则为 <see langword="null"/>。
        /// </param>
        public ModuleTypeLoadingException(string moduleName, string message, Exception innerException)
            : base(moduleName, String.Format(CultureInfo.CurrentCulture, Resources.FailedToRetrieveModule, moduleName, message), innerException)
        {
        }
    }
}