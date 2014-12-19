
using SMT.SAAS.Platform.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace SMT.SAAS.Platform.DAL
{
	/// <summary>
	/// 提供基于Oracle数据库的系统更新日志信息访问实现
	/// </summary>
    public class ModuleUpdateLogDAL 
	{
        public List<ModuleUpdateLog> GetUpdateLogByModule(string moduleKey)
        {
            throw new NotImplementedException();
        }

        public bool Add(ModuleUpdateLog entity)
        {
            throw new NotImplementedException();
        }

        public bool Update(ModuleUpdateLog entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(ModuleUpdateLog entity)
        {
            throw new NotImplementedException();
        }

        public ModuleUpdateLog GetEntityByKey(string key)
        {
            throw new NotImplementedException();
        }

        public bool IsExists(string key)
        {
            throw new NotImplementedException();
        }

        public List<ModuleUpdateLog> GetListByPager(int index, int size, ref int count, string sort)
        {
            throw new NotImplementedException();
        }
    }
}

