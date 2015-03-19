
// 内容摘要: 
// 完成日期：2011-04-08 
namespace SMT.SAAS.Platform
{
	public interface IApp:IModule
	{
		/// <summary>
		/// 应用编号
		/// </summary>
        string AppCode { get; set; }

		/// <summary>
		/// 注销。系统注销接口，在应用程序注销的时候讲会调用此接口，用于注销个性化资源。
		/// </summary>
		void LogOut();
	}
}

