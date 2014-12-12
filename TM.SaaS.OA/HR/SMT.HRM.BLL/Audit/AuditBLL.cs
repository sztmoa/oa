using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using SMT.Saas.CommonBLL.NHibernate;
using SMT.SaaS.Services.SmtFlowWF;
using SMT.HRM.CustomModel;
using SMT_HRM_EFModel;
using SMT.SaaS.Services;
using SMT.SaaS.Common;
using System.Configuration;
using SMT.SaaS.Services.Model;
using System.Reflection;

namespace SMT.HRM.BLL.Audit
{
    /// <summary>
    /// 业务注册
    /// </summary>
    public class WPBLLRegistration : BLLRegistration
    {
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="context">注册上下文</param>
        public override void RegisterBLL(BLLRegistrationContext context)
        {
            base.RegisterBLL(context);
            context.MapBLL(typeof(AuditEntity), typeof(AuditBLL));
            //context.MapBLL(typeof(T_HR_EMPLOYEEOVERTIMERECORD), typeof(EmployeeVacationOT_AuditBLL));//综合结转

            BaseBLL.Saved += new EventHandler<BLLEventArgs>(BaseBLL_Saved);
        }

        void BaseBLL_Saved(object sender, BLLEventArgs e)
        {
            #region
            //EntityBase entity = e.Entity as EntityBase;
            //ModelEntity model = Audit.AuditBLL.ModelEntityList.Where(s => s.ModelCode == e.Entity.GetType().Name).FirstOrDefault();
            //if (model != null)
            //{
            //    if (entity.Action == EntityAction.Add)
            //    {
            //        //新增未提交的代办
            //        DoTaskBLL.GetInstance().SendTask<EntityBase>(entity);
            //    }
            //    else if (entity.Action == EntityAction.Delete)
            //    {
            //        DoTaskBLL.GetInstance().DeleteTask<EntityBase>(entity);
            //    }
            //}
            #endregion
        }
    }
    public class AuditBLL : BaseBLLObject
    {

        public override IQueryable Query(Type elementType, Dictionary<string, string> parameters = null)
        {
            throw new NotImplementedException();
        }

        public override object Query(System.Linq.Expressions.Expression expression)
        {
            throw new NotImplementedException();
        }

        public override SaaS.Common.SaveResult Save(SaaS.Common.IEntityBase data)
        {
            AuditEntity auditEntity = data as AuditEntity;
            SubmitData submitData = auditEntity.Data as SubmitData;
            var dataResult = Submit(submitData);
            return new SaaS.Common.SaveResult(new AuditEntity() { Data = dataResult }) { State = SaaS.Common.SaveResultState.Success };

        }

        public DataResult Submit(SubmitData submitData)
        {
            FlowWFServices service = new FlowWFServices();
            if (submitData.SubmitFlag == SubmitFlag.New)
            {
                CommonBLL bll = new CommonBLL("");
                IEntityBase obj = bll.GetEntity(submitData.ModelCode.ToUpper(), submitData.FormID);
                string xml = GetFullXml(obj, submitData.ModelCode.ToUpper(), submitData).ToString();
                XElement element = XElement.Parse(xml);
                element = bll.GetXml(obj, element);
                submitData.XML = element.ToString();
            }
            return service.SubmitFlow(submitData);
        }
        /// <summary>
        /// 获取填充xml
        /// </summary>
        /// <param name="obj">要填充的实体</param>
        /// <param name="modelCode">实体名称</param>
        /// <returns>返回xml</returns>
        public XElement GetFullXml(IEntityBase obj, string modelCode, SubmitData submitData)
        {
            XElement element = null;
            Metadata meta = new Metadata();
            string modelType = string.Empty;
            if (!string.IsNullOrEmpty(modelCode))
            {
                modelType = modelCode.ToUpper();
            }
            else
            {
                modelType = obj.GetType().Name;
            }
            List<AutoDictionary> listAutoDic = new List<AutoDictionary>();
            string strMainKey = string.Empty;
            string strMainValue = string.Empty;
            CommonBLL bll = new CommonBLL("");
            #region 处理元数据

            #region "   T_HR_EMPLOYEEOVERTIMERECORD  "

            if (modelType.ToUpper() == Constants.T_HR_EMPLOYEEOVERTIMERECORD)
            {
                strMainKey = "OVERTIMERECORDID";
                strMainValue = string.Empty;
                Type objtype = obj.GetType();
                PropertyInfo[] propinfos = objtype.GetProperties();
                foreach (PropertyInfo propinfo in propinfos)
                {
                    string keyValue = propinfo.GetValue(obj, null) != null ? propinfo.GetValue(obj, null).ToString() : string.Empty;
                    if (propinfo.Name == strMainKey)
                    {
                        strMainValue = keyValue;
                    }
                }

                if (obj is T_HR_EMPLOYEEOVERTIMERECORD)
                {
                    T_HR_EMPLOYEEOVERTIMERECORD entity = obj as T_HR_EMPLOYEEOVERTIMERECORD;
                    if (submitData.SubmitFlag != SubmitFlag.New && submitData.ApprovalResult == ApprovalResult.NoPass)
                    {
                        var overtimeDetail = bll.Query<T_HR_EMPLOYEEOVERTIMERECORD>().Where(w => w.OVERTIMERECORDID == strMainValue).ToList();
                        //Dictionary<object, object> detail = new Dictionary<object, object>();
                        //detail.Add(overtimeDetail, null);//normItemConfigList 是2级从表列表

                        Dictionary<object, object> detail = new Dictionary<object, object>();
                        detail.Add(entity.T_HR_EMPLOYEEOVERTIMEDETAILRD, null);

                        listAutoDic.Add(new AutoDictionary
                        {
                            TableName = modelType,
                            KeyValue = "CREATEUSERID",
                            DataValue = submitData.ApprovalUser.UserID,
                            DataText = submitData.ApprovalUser.UserName,
                            Name = "CREATEUSERID"
                        });
                        listAutoDic.Add(new AutoDictionary
                        {
                            TableName = modelType,
                            KeyValue = "CREATEPOSTID",
                            DataValue = submitData.ApprovalUser.PostID,
                            DataText = submitData.ApprovalUser.PostName,
                            Name = "CREATEPOSTID"
                        });
                        listAutoDic.Add(new AutoDictionary
                        {
                            TableName = modelType,
                            KeyValue = "CREATEDEPARTMENTID",
                            DataValue = submitData.ApprovalUser.DepartmentID,
                            DataText = submitData.ApprovalUser.DepartmentName,
                            Name = "CREATEDEPARTMENTID"
                        });
                        listAutoDic.Add(new AutoDictionary
                        {
                            TableName = modelType,
                            KeyValue = "CREATECOMPANYID",
                            DataValue = submitData.ApprovalUser.CompanyID,
                            DataText = submitData.ApprovalUser.CompanyName,
                            Name = "CREATECOMPANYID"
                        });
                    }
                }
            }
            #endregion




            #endregion
            //auditInfo.ObjXml = metaData.TableToXml(yearNormDraft, null, auditInfo.SystemCode, auditInfo.ModelCode, listAutoDic);//  将Detail设置成了null
            string xml = meta.TableToXml(obj, null, "HR", modelType, listAutoDic);
            element = XElement.Parse(xml);
            //SMT.Portal.Common.MetaData metaData = new MetaData();
            return element;
        }


        private static List<ModelEntity> modelEntityList = null;
        /// <summary>
        /// 引擎使用实体集合
        /// </summary>
        public static List<ModelEntity> ModelEntityList
        {
            get
            {
                if (modelEntityList == null)
                {
                    InitModelEntityList();
                }
                return modelEntityList;
            }
        }
        /// <summary>
        /// 初始化引擎所需的数据
        /// </summary>
        private static void InitModelEntityList()
        {
            modelEntityList = new List<ModelEntity>();
            XElement element = null;
            //BOList路径
            string bolistPath = ConfigurationManager.AppSettings["TemplatePath"] + "WPBO\\BOList.xml";
            //加载BOList
            element = XElement.Load(bolistPath);
            //获取Object节点集合
            List<XElement> elmList = element.Element("ObjectList").Elements("Object").ToList();
            foreach (var item in elmList)
            {
                ModelEntity model = new ModelEntity();
                string code = item.Attribute("Name").Value.ToUpper();
                model.ModelCode = code;
                model.ModelName = item.Attribute("Description").Value;
                model.EntityName = code;
                //根据Object中的实体名称设置该实体对应的xml的路径
                string xmlPath = ConfigurationManager.AppSettings["TemplatePath"] + "WPBO\\" + code + ".xml";
                //加载对应xml
                XElement elm = XElement.Load(xmlPath);
                //获取MsgOpen节点
                elm = elm.Element("MsgOpen");
                //获取MsgOpen的子节点集合
                List<XNode> nodeList = elm.Nodes().ToList();
                //定义字符串
                string xml = "";
                //遍历MsgOpen的子节点集合，将每个子节点转换成字符串后连接在一起
                foreach (var xnode in nodeList)
                {
                    xml += xnode.ToString();
                }
                model.MessageSetting = xml;
                modelEntityList.Add(model);
            }
        }

        public static int UpdateCheckState(string EntityType, string EntityKey, string EntityId, int CheckState, string xmlDoc, ref string msg)
        {
            CommonBLL bll = new CommonBLL("");
            var entityInfo = bll.GetEntity(EntityType.ToUpper(), EntityId);
            if (entityInfo == null)
            {
                return 0;
            }
            var newCheckState = CheckState;
            var oldCheckState = entityInfo.GetType().GetProperty("CHECKSTATES").GetValue(entityInfo, null);
            int oState = 0;
            if (oldCheckState != null)
            {
                oState = int.Parse(oldCheckState.ToString());
            }
            if ((oState == 0 && newCheckState == 1) || oState == 3 && newCheckState == 1)
            {
                entityInfo.Action = EntityAction.AuditSubmit;
                entityInfo.GetType().GetProperty("CHECKSTATES").SetValue(entityInfo, newCheckState.ToString(), null);
            }
            else if (newCheckState == 2)
            {
                entityInfo.Action = EntityAction.AuditPass;
                entityInfo.GetType().GetProperty("CHECKSTATES").SetValue(entityInfo, newCheckState.ToString(), null);
            }
            else if (newCheckState == 3)
            {
                entityInfo.Action = EntityAction.AuditFail;
                entityInfo.GetType().GetProperty("CHECKSTATES").SetValue(entityInfo, newCheckState.ToString(), null);
            }
            else if (newCheckState == 5)
            {
                //entityInfo.Action = EntityAction.AuditRollBack;
            }
            else
            {
            }
            ErrorLog.Log("处理状态：" + newCheckState.ToString());
            var saverResult = bll.Audit(entityInfo);
            if (saverResult.State == SaveResultState.Success)
            {
                return 1;
            }
            else
            {
                msg = saverResult.Message;
                return 0;
            }
        }
    }
    /// <summary>
    /// 提供给引擎使用的实体
    /// </summary>
    public class ModelEntity
    {
        /// <summary>
        /// 实体编码
        /// </summary>
        public string ModelCode { get; set; }
        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModelName { get; set; }
        /// <summary>
        /// 实体名称
        /// </summary>
        public string EntityName { get; set; }
        /// <summary>
        /// XML的MessageOpen节点
        /// </summary>
        public string MessageSetting { get; set; }
    }
}
