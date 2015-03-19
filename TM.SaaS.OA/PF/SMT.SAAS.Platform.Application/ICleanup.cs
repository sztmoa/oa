
// 内容摘要: 
namespace SMT.SAAS.Platform
{
	public interface ICleanup 
	{
		/// <summary>
		/// 清理实例资源，例如保存实例、移除资源等
        /// 此接口不等同于IDisposable接口。仅为显式释放提供支持。
		/// </summary>
		void Cleanup();

	}
}

