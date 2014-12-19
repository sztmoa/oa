
using SMT.SAAS.Platform.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMT.SAAS.Platform.DAL
{
	/// <summary>
	/// 提供基于Oracle数据库的系统错误信息访问实现
	/// </summary>
	public class SystemErrorDAL 
	{

        public List<SystemError> GetErrorListByAppKey(string appKey)
        {
            throw new NotImplementedException();
        }

        public bool Add(SystemError entity)
        {
            throw new NotImplementedException();
        }

        public bool Update(SystemError entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(SystemError entity)
        {
            throw new NotImplementedException();
        }

        public SystemError GetEntityByKey(string key)
        {
            throw new NotImplementedException();
        }

        public bool IsExists(string key)
        {
            throw new NotImplementedException();
        }

        public List<SystemError> GetListByPager(int index, int size, ref int count, string sort)
        {
            throw new NotImplementedException();
        }
    }
}

