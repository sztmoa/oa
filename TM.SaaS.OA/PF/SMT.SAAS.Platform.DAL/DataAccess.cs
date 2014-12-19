using System.Configuration;
using SMT.SAAS.Platform.Model;
using System.Reflection;
using System.Diagnostics;
using System;
using SMT.SAAS.Platform.DAL;
namespace SMT.SAAS.Platform.DALFactory
{
    /// <summary>
    /// 获取数据访问成实例。<br/>
    /// 创建过程中会自动根据配置的工厂类型FactoryType创建对应的数据访问层。
    /// </summary>
    public sealed class DataAccess  
    {
        private static readonly string typePath = ConfigurationManager.AppSettings["FactoryType"];
        private DataAccess() { }

        public static ModuleInfoDAL CreateModuleInfo()
        {
            return new ModuleInfoDAL();
        }

        public static ModuleUpdateLogDAL CreateModuleUpdateLog()
        {
            return new ModuleUpdateLogDAL();
        }

        public static PublicPartDAL CreatePublicPart()
        {
            return new PublicPartDAL() ;
        }

        public static ResourceDAL CreateResource()
        {
            return new ResourceDAL();
        }

        public static ShortCutDAL CreateShortCut()
        {
            return new ShortCutDAL();
        }

        public static SystemConfigDAL CreateSystemConfig()
        {
            return new SystemConfigDAL() ;
        }

        public static SystemErrorDAL CreateSystemError()
        {
            return new SystemErrorDAL();
        }

        public static WebPartDAL CreateWebPart()
        {
            return new WebPartDAL();
        }
        
        private static object CreateObject(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                    throw new ArgumentNullException("name");

                string className = typePath + "." + name;
                return Assembly.Load(typePath).CreateInstance(className);
            }
            catch (System.Exception ex)
            {
                throw new Exception("创建工厂对象产生异常：" + name, ex);
            }
        }

    }
}
