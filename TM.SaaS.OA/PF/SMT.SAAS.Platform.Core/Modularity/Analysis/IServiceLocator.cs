using System;
using System.Collections.Generic;
    
namespace SMT.SAAS.Platform.Core
{
    /// <summary>
    /// 定义获取对象实例的一组方法
    /// </summary>
    public interface IServiceLocator
    {
        IEnumerable<TService> GetAllInstances<TService>();

        IEnumerable<object> GetAllInstances(Type serviceType);

        TService GetInstance<TService>();

        TService GetInstance<TService>(string key);

        object GetInstance(Type serviceType);

        object GetInstance(Type serviceType, string key);
    }
}
