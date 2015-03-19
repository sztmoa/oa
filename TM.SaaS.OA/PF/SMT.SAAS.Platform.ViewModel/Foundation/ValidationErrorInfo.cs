
// 内容摘要: 属性异步验证错误信息
      
namespace SMT.SAAS.Platform.ViewModel.Foundation
{
    /// <summary>
    /// 属性异步验证错误信息。
    /// </summary>
    public class ValidationErrorInfo
    {
        /// <summary>
        /// 错误编号
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }

        public override string ToString()
        {
            return ErrorMessage;
        }
    }
}
