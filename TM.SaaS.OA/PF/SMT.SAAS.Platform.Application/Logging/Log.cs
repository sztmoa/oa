using System;

// 内容摘要: 元数据，描述记录描述系统信息的内容。
namespace SMT.SAAS.Platform.Logging
{
	/// <summary>
	/// 元数据，描述记录描述系统信息的内容。
	/// </summary>
	public class Log
	{
		/// <summary>
		/// 消息类型.<see cerf="Category"/>
		/// </summary>
		public virtual Category Category
		{
			get;
			set;
		}

		/// <summary>
		/// 消息的优先级。<see cerf="Priority"/>
		/// </summary>
		public virtual Priority Priority
		{
			get;
			set;
		}

		/// <summary>
		/// 描述消息的内容
		/// </summary>
		public virtual string Message
		{
			get;
			set;
		}

		/// <summary>
		/// 消息编号
		/// </summary>
		public virtual string LogCode
		{
			get;
			set;
		}

		/// <summary>
		/// 消息包含的系统异常信息。
		/// </summary>
		public virtual Exception Exception
		{
			get;
			set;
		}

		/// <summary>
		/// 消息所属系统编号
		/// </summary>
		public virtual string AppCode
		{
			get;
			set;
		}

		/// <summary>
		/// 消息所属的模块名称
		/// </summary>
		public virtual string ModuleCode
		{
			get;
			set;
		}

		/// <summary>
		/// 消息产生的时间
		/// </summary>
		public virtual DateTime CreateTime
		{
			get;
			set;
		}

	}
}

