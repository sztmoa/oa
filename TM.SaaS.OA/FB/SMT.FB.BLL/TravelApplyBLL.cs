using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMT_FB_EFModel;
using System.Data.Objects.DataClasses;
using System.Data;
using System.Data.Objects;
namespace SMT.FB.BLL
{
    public class TravelApplyBLL : BaseBll<T_FB_TRAVELEXPAPPLYMASTER>
    {

        public ObjectContext lbc;
        public override bool Add(T_FB_TRAVELEXPAPPLYMASTER entity)
        {

            lbc = this.dal.GetDataContext();
            AttachObject(entity);
            lbc.SaveChanges();
            return true;

        }
        public override bool Update(T_FB_TRAVELEXPAPPLYMASTER entity)
        {
            lbc = this.dal.GetDataContext();
            AttachObject(entity);
            lbc.SaveChanges();
            return true;
            // return base.Update(entity);
        }
        public void AttachObject(EntityObject entity)
        {
            
            List<EntityObject> list = new List<EntityObject>();
            int maxLevel = 2;
            int curLevel = 0;
            AttachObjectRelation(entity, ref list, maxLevel, curLevel);

            EntityKey key = entity.EntityKey;
            if (key != null)
            {
                GetEntity(key);
                lbc.ApplyPropertyChanges(entity.GetType().Name, entity);
            }
            else
            {
                lbc.AddObject(entity.GetType().Name, entity);
            }

            //for (int i = 0;i < list.Count; i++)
            //{
            //    EntityObject curEntity = list[i];
            //    try
            //    {
            //        EntityKey key = curEntity.EntityKey;
            //        if (key != null)
            //        {
            //            GetEntity(key);
            //            lbc.ApplyPropertyChanges(curEntity.GetType().Name, entity);
            //        }
            //        else
            //        {
            //            lbc.AddObject(curEntity.GetType().Name, entity);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
                  
            //    }

            //}
        }

        private void AttachObjectRelation(EntityObject entity, ref List<EntityObject> list, int MaxLevel, int curLevel)
        {
            list.Add(entity);
            curLevel++;
            var rs = (entity as IEntityWithRelationships).RelationshipManager.GetAllRelatedEnds();
            foreach (IRelatedEnd re in rs)
            {
                foreach (var a in re)
                {
                    EntityObject subEo = a as EntityObject;

                    if (subEo == null)
                    {
                        throw new Exception(re.SourceRoleName + " 不能为空");
                    }
                    if (re.GetType().BaseType == typeof(EntityReference))
                    {
                        if (list.Contains(subEo))
                        {
                            continue;
                        }

                        if (subEo.EntityKey != null)
                        {
                            object refObject = GetEntity(subEo.EntityKey);
                            re.Remove(subEo);
                            (re as EntityReference).EntityKey = subEo.EntityKey;
                        }
                    }
                    else
                    {
                        if (curLevel < MaxLevel)
                        {
                            AttachObjectRelation(subEo, ref list, curLevel, MaxLevel);
                        }
                    }

                }
            }
        }

    
    }
}
