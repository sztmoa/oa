using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if !SILVERLIGHT
using System.Runtime.Serialization;
#endif
// 内容摘要: 枚举，描述系统中WEBPART、快捷方式、系统配置类型
namespace SMT.SAAS.Platform.Model
{
	/// <summary>
	/// 枚举，描述系统中WEBPART、快捷方式、系统配置类型
	/// </summary>
#if !SILVERLIGHT
    [DataContract]
#endif
    public enum SysType : int
	{
		/// <summary>
		/// 系统默认级别
		/// </summary>
		System,
		/// <summary>
		/// 管理员级别
		/// </summary>
		Admin,
		/// <summary>
		/// 用户级别
		/// </summary>
		User,
	}
}
