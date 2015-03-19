using System.Windows;

// 内容摘要: 主容器
      
namespace SMT.SAAS.Platform
{
    /// <summary>
    /// 主容器，应用程序仅有一个对象。
    /// </summary>
    public interface IHost
    {
        void SetRootVisual(UIElement Content);

        void LoginOff();
    }
}
