
using SMT.SAAS.Platform.Model;
using System.Collections.Generic;
using SMT.Foundation.Core;
using TM_SaaS_OA_EFModel;
using System.Linq.Dynamic;
using System.Linq;
using System;
using System.Text;
using System.Diagnostics;
using System.Xml.Linq;
using System.Collections.ObjectModel;
using SMT.Foundation.Log;
namespace SMT.SAAS.Platform.DAL
{
    /// <summary>
    /// 提供基于Oracle数据库的子系统(应用)信息访问实现
    /// SMT.SAAS.Platform.DAL.AppDAL
    /// </summary>
    public class ModuleInfoDAL : BaseDAL
    {
        private CommonDAL<T_PF_MODULEINFO> _commonDAL;

        public ModuleInfoDAL()
        {
            _commonDAL = new CommonDAL<T_PF_MODULEINFO>();
        }

        public bool Add(ModuleInfo entity)
        {
            T_PF_MODULEINFO model = entity.CloneObject<T_PF_MODULEINFO>(new T_PF_MODULEINFO());
            model = Utility.CreateCommonProperty().CloneObject<T_PF_MODULEINFO>(model);

            return _commonDAL.Add(model);
        }

        public bool Update(ModuleInfo entity)
        {
            T_PF_MODULEINFO model = entity.CloneObject<T_PF_MODULEINFO>(new T_PF_MODULEINFO());

            model = Utility.CreateCommonProperty().CloneObject<T_PF_MODULEINFO>(model);


            return _commonDAL.Update(model);
        }

        public bool Delete(ModuleInfo entity)
        {
            throw new System.NotImplementedException();
        }

        public List<ModuleInfo> GetListByPager(int index, int size, ref int count, string sort)
        {
            throw new System.NotImplementedException();
        }

    }
}

