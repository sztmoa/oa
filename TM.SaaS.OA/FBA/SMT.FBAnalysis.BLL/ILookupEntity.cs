using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.Objects.DataClasses;
using System.Collections;

namespace SMT.FBAnalysis.BLL
{
    public interface ILookupEntity
    {
        EntityObject[] GetLookupData(string modelCode, string userID, int pageIndex, int pageSize, string sort, string filterString, IList<object> paras, ref int pageCount);
    }
}
