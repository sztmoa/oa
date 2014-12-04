using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMT.Portal.Common.SmtForm.Logic
{
    public class SmtException:Exception
    {
        public SmtException(string msg)
            : base(msg)
        { }
    }
}