using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFMysql
{
    class Program
    {
        static void Main(string[] args)
        {
            
            using (TMAwardEntities context = new TMAwardEntities())
            {

                var dt = from ent in context.TmpTicket
                         select ent;

            }

        }
    }
}
