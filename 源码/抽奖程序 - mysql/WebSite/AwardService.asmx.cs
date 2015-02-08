using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using EFMysql;

namespace Asd.Award
{
    /// <summary>
    /// AwardService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    [System.Web.Script.Services.GenerateScriptType(typeof(AwardEntity))]
    public class AwardService : System.Web.Services.WebService
    {
        [WebMethod]
        public string[] GetTicket()
        {
            using (TMAwardEntities context = new TMAwardEntities())
            {

                var dt = from ent in context.TmpTicket
                         select ent;



                var ar = new string[dt.Count()];
                var index = 0;
                foreach (var item in dt)
                {
                    ar[index] = item.TicketNO;
                    index++;
                }
                return ar;
            }
        }

        /// <summary>
        /// 为了获取奖号数量
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public int[] GetTicketCount()
        {
            using (TMAwardEntities context = new TMAwardEntities())
            {
                var dt = from ent in  context.TmpTicket
                         select ent;              
                var entity = dt.FirstOrDefault();
                int iStart = int.Parse(entity.TicketStart);
                int iEnd = int.Parse(entity.TicketEnd);

                var index = 0;
                var ar = new int[int.Parse(entity.TicketCount)];
                for (int i = iStart; i < iEnd;i++ )
                {
                    ar[index] = i;
                    index++;
                }
                return ar;
            }
        }


        [WebMethod]
        public void AddAward(string ticketNO, string level, string remark)
        {
            using (TMAwardEntities context = new TMAwardEntities())
            {
                var item =new TmpAward();
                item.TicketNO = ticketNO;
                item.Level = level;
                item.Remark = remark;
                item.UpdateTime = DateTime.Now;
                context.AddToTmpAward(item);
                context.SaveChanges();
            }
        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod()]
        public void AddAwardMany(AwardEntity[] awards)
        {
            using (TMAwardEntities context = new TMAwardEntities())
            {
                foreach (var q in awards)
                {
                    var item = new TmpAward();
                    item.TicketNO = q.TicketNO;
                    item.Level = q.Level;
                    item.Remark = q.Remark;
                    item.UpdateTime = DateTime.Now;
                    context.AddToTmpAward(item);
                }
                context.SaveChanges();
            }
        }

        [WebMethod]
        public string[] GetAward()
        {
            using (TMAwardEntities context = new TMAwardEntities())
            {

                var dt = from ent in context.TmpTicket
                         select ent;


                var ar = new string[dt.Count()];
                var index = 0;
                foreach (var item in dt)
                {
                    ar[index] = item.TicketNO;
                    index++;
                }
                return ar;
            }
        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod()]
        public AwardEntity[] GetAwardObj()
        {
            using (TMAwardEntities context = new TMAwardEntities())
            {

                var dt = from ent in context.TmpAward
                         select ent;



                var result = new List<AwardEntity>();

               foreach(var q in dt)
                {
                    var item = new AwardEntity();
                    item.TicketNO = q.TicketNO;
                    item.Level =q.Level;
                    item.Remark = q.Remark;
                    result.Add(item);
                }
                return result.ToArray();
            }
        }

        [WebMethod]
        public void DiscardAward(string ticketNO)
        {
            using (TMAwardEntities context = new TMAwardEntities())
            {

                var dt = (from ent in context.TmpAward
                         where ent.TicketNO==ticketNO
                         select ent).FirstOrDefault();
                if (dt != null)
                {
                    context.DeleteObject(dt);
                    context.SaveChanges();
                }
            }
        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod()]
        public AwardEntity[] GenerateAward(string belongTo)
        {
            //var machine = DataContext.BuildAwardMachine();
            //machine.BelongTo = belongTo;
            //var ongoing = machine.GenerateAwardByPlan();
            //DataContext.Update(machine.Award);

            //var result = DataContext.BuildAwardEntity(ongoing);
            //return result;
            AwardEntity[] result=new AwardEntity[0];
            return result;
        }

        [WebMethod]
        public void UpdateQuery(int ticketCount, string ticketNO)
        {
            using (TMAwardEntities context = new TMAwardEntities())
            {

                var dt = (from ent in context.TmpTicket
                         where ent.TicketNO==ticketNO
                         select ent).FirstOrDefault();
                if (dt == null)
                {
                    dt = new TmpTicket();
                    dt.TicketCount = ticketCount.ToString();
                    dt.TicketNO = ticketNO;
                    context.Attach(dt);
                    context.SaveChanges();
                }
                else
                {
                    dt.TicketCount = ticketCount.ToString();
                    context.SaveChanges();
                }
            }
        }

    }


}
