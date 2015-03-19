using System.Collections.Generic;
 
namespace SMT.SAAS.Platform.Core.Modularity
{
    public partial class ModuleManager
    {
        /// <summary>
        /// 返回注册的<see cref="IModuleTypeLoader"/> 实例，用于加载初始化模块。
        /// </summary>
        /// <value>
        /// 模块加载对象。<see cref="IModuleTypeLoader"/> 
        /// </value>
        public virtual IEnumerable<IModuleTypeLoader> ModuleTypeLoaders
        {
            get
            {
                if (this.typeLoaders == null)
                {
                    this.typeLoaders = new List<IModuleTypeLoader>()
                                          {
                                              new XapModuleTypeLoader()
                                          };
                }

                return this.typeLoaders;
            }

            set
            {
                this.typeLoaders = value;
            }
        }
    }
}
