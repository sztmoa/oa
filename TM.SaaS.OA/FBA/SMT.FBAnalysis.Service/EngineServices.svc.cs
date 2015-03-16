using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Smt.Global.IContract;
using System.Xml.Linq;
using System.IO;
using EngineWS = SMT.SaaS.BLLCommonServices.EngineConfigWS;
using SMT.SaaS.BLLCommonServices.PermissionWS;
using SMT.Foundation.Log;
//using SMT.SaaS.OA.BLL;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TM_SaaS_OA_EFModel;
using SMT.FBAnalysis.BLL;

namespace SMT.FBAnalysis.Service
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“EngineServices”。
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class EngineServices : IApplicationService
    {
        #region IApplicationService 成员

        /// <summary>
        /// 引擎调用入口
        /// ModelNames  是个枚举类型  需要在里面添加相应的表单名
        /// AddSenddoc  是个函数
        /// </summary>
        /// <param name="strXml"></param>
        /// <returns></returns>
        public string CallWaitAppService(string strXml)
        {
            try
            {
                //生成社包单的业务逻辑
                Tracer.Debug(strXml);

                string strReturn = string.Empty;
                Byte[] b = System.Text.UTF8Encoding.UTF8.GetBytes(strXml);
                XElement xele = XElement.Load(System.Xml.XmlReader.Create(new MemoryStream(b)));
                var eGFunc = from c in xele.Descendants("Para")
                             select c;
                // string funcName = eGFunc.FirstOrDefault().Attribute("TableName").Value;
                string paraValue = string.Empty;
                string formID = string.Empty;
                string systemCode = string.Empty;
                string modelCode = string.Empty;

                foreach (var item in eGFunc.Attributes("TableName"))
                {
                    if (item.Value == ModelNames.T_FB_BORROWAPPLYMASTER.ToString()) //个人还款
                    {
                        paraValue = RepayApplyAdd(eGFunc);
                        formID = "BORROWAPPLYMASTERID";
                        modelCode = ModelNames.T_FB_BORROWAPPLYMASTER.ToString();
                        break;
                    }
                }
                //strReturn = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" +
                //                  "<System>" +
                //                  "  <NewFormID>" + paraValue + "</NewFormID>" +
                //                  "<IsNewFlow>0</IsNewFlow>" +
                //                  "<Attribute  Name=\"" + formID + "\" DataValue=\"" + paraValue + "\"></Attribute>" +
                //                      "<Attribute  Name=\"SYSTEMCODE\" DataValue=\"FB\"></Attribute>" +
                //                       "<Attribute  Name=\"MODELCODE\" DataValue=\"" + modelCode + "\"></Attribute>" +
                //                    "</System>";
                //Tracer.Debug(strReturn);
                return strReturn;
            }
            catch (Exception e)
            {
                string abc = "<FB>Message=[" + e.Message + "]" + "<FB>Source=[" + e.Source + "]<FB>StackTrace=[" + e.StackTrace + "]<FB>TargetSite=[" + e.TargetSite + "]";
                Tracer.Debug(abc);
                return abc;

            }
        }

        #endregion

        #region 根据传回的XML，添加还款申请信息
        /// <summary>
        /// 根据传回的XML，添加还款申请信息
        /// </summary>
        /// <param name="eGFunc"></param>
        /// <returns></returns>
        private static string RepayApplyAdd(IEnumerable<XElement> eGFunc)
        {
            try
            {
                if (eGFunc.Count() == 0)
                {
                    return "";
                }
                string strEmployeeID = string.Empty;
                string strOwnerID = string.Empty;
                string strOwnerName = string.Empty;
                string strOwnerPostID = string.Empty;
                string strOwnerPostName = string.Empty;
                string strOwnerDepartmentID = string.Empty;
                string strOwnerDepartmentName = string.Empty;
                string strOwnerCompanyID = string.Empty;
                string strOwnerCompanyName = string.Empty;
                string strCheckState = string.Empty;
                string strCreateCompanyID = string.Empty;
                string strCreateCompanyName = string.Empty;
                string strCreateDepartmentID = string.Empty;
                string strCreateDepartmentName = string.Empty;
                string strCreatePostID = string.Empty;
                string strCreatePostName = string.Empty;
                string strCreateID = string.Empty;
                string strCreateName = string.Empty;
                string strBorrowMasterID = string.Empty;
                decimal dBorrowTotal = 0;

                foreach (var q in eGFunc)
                {
                    string strName = q.Attribute("Name").Value;
                    switch (strName)
                    {
                        case "CREATEUSERID":
                            strEmployeeID = q.Attribute("Value").Value;
                            break;
                        case "OWNERID":
                            strOwnerID = q.Attribute("Value").Value;
                            break;
                        case "OWNERNAME":
                            strOwnerName = q.Attribute("Value").Value;
                            break;
                        case "OWNERPOSTID":
                            strOwnerPostID = q.Attribute("Value").Value;
                            break;
                        case "OWNERPOSTNAME":
                            strOwnerPostName = q.Attribute("Value").Value;
                            break;
                        case "OWNERDEPARTMENTID":
                            strOwnerDepartmentID = q.Attribute("Value").Value;
                            break;
                        case "OWNERDEPARTMENTNAME":
                            strOwnerDepartmentName = q.Attribute("Value").Value;
                            break;
                        case "OWNERCOMPANYID":
                            strOwnerCompanyID = q.Attribute("Value").Value;
                            break;
                        case "OWNERCOMPANYNAME":
                            strOwnerCompanyName = q.Attribute("Value").Value;
                            break;
                        case "CHECKSTATE":
                            strCheckState = q.Attribute("Value").Value;
                            break;
                        case "CREATEID":
                            strCreateID = q.Attribute("Value").Value;
                            break;
                        case "CREATENAME":
                            strCreateName = q.Attribute("Value").Value;
                            break;
                        case "CREATEPOSTID":
                            strCreatePostID = q.Attribute("Value").Value;
                            break;
                        case "CREATEPOSTNAME":
                            strCreatePostName = q.Attribute("Value").Value;
                            break;
                        case "CREATEDEPARTMENTID":
                            strCreateDepartmentID = q.Attribute("Value").Value;
                            break;
                        case "CREATEDEPARTMENTNAME":
                            strCreateDepartmentName = q.Attribute("Value").Value;
                            break;
                        case "CREATECOMPANYID":
                            strCreateCompanyID = q.Attribute("Value").Value;
                            break;
                        case "CREATECOMPANYNAME":
                            strCreateCompanyName = q.Attribute("Value").Value;
                            break;
                        case "BORROWAPPLYMASTERID":
                            strBorrowMasterID = q.Attribute("Value").Value;
                            break;
                    }
                }
                DailyManagementServices doc = new DailyManagementServices();
                string employeeid = strEmployeeID.Replace("{", "").Replace("}", "");

                T_FB_BORROWAPPLYMASTER borMaster = doc.GetBorrowApplyMasterByID(strBorrowMasterID);
                T_FB_REPAYAPPLYMASTER entity = new T_FB_REPAYAPPLYMASTER();
                entity.REPAYAPPLYMASTERID = Guid.NewGuid().ToString();
                entity.CHECKSTATES = 0;
                entity.CREATECOMPANYID = borMaster.CREATECOMPANYID;
                entity.CREATECOMPANYNAME = borMaster.CREATECOMPANYNAME;
                entity.CREATEDATE = DateTime.Now;
                entity.CREATEDEPARTMENTID = borMaster.CREATEDEPARTMENTID;
                entity.CREATEDEPARTMENTNAME = borMaster.CREATEDEPARTMENTNAME;
                entity.CREATEPOSTID = borMaster.CREATEPOSTID;
                entity.CREATEPOSTNAME = borMaster.CREATEPOSTNAME;
                entity.CREATEUSERID = borMaster.CREATEUSERID;
                entity.CREATEUSERNAME = borMaster.CREATEUSERNAME;
                entity.EDITSTATES = 0;
                entity.OWNERCOMPANYID = borMaster.OWNERCOMPANYID;
                entity.OWNERCOMPANYNAME = borMaster.OWNERCOMPANYNAME;
                entity.OWNERDEPARTMENTID = borMaster.OWNERDEPARTMENTID;
                entity.OWNERDEPARTMENTNAME = borMaster.OWNERDEPARTMENTNAME;
                entity.OWNERID = borMaster.OWNERID;
                entity.OWNERNAME = borMaster.OWNERNAME;
                entity.OWNERPOSTID = borMaster.OWNERPOSTID;
                entity.OWNERPOSTNAME = borMaster.OWNERPOSTNAME;
                entity.PROJECTEDREPAYDATE = DateTime.Now;
                entity.REMARK = "";
                entity.UPDATEDATE = DateTime.Now;
                entity.UPDATEUSERID = borMaster.UPDATEUSERID;
                entity.UPDATEUSERNAME = borMaster.UPDATEUSERNAME;
                entity.REPAYTYPE = borMaster.REPAYTYPE;
                entity.TOTALMONEY = 0;
                entity.T_FB_BORROWAPPLYMASTER = borMaster;

                //从表操作
                List<object> masterCode = new List<object>();
                masterCode.Add(entity.T_FB_BORROWAPPLYMASTER.BORROWAPPLYMASTERID);
                List<T_FB_BORROWAPPLYDETAIL> BorDetail = doc.GetBorrowApplyDetailByMasterID(masterCode);
                List<T_FB_REPAYAPPLYDETAIL> RepDetail = new List<T_FB_REPAYAPPLYDETAIL>();//还款从表

                dBorrowTotal = 0;
                foreach (var detail in BorDetail)
                {
                    T_FB_REPAYAPPLYDETAIL RepayDetailInfo = new T_FB_REPAYAPPLYDETAIL();
                    RepayDetailInfo.REPAYAPPLYDETAILID = Guid.NewGuid().ToString();
                    RepayDetailInfo.REPAYAPPLYDETAILID = System.Guid.NewGuid().ToString();
                    RepayDetailInfo.BORROWMONEY = detail.BORROWMONEY;
                    RepayDetailInfo.REMARK = detail.REMARK;
                    RepayDetailInfo.T_FB_SUBJECT = detail.T_FB_SUBJECT;
                    RepayDetailInfo.T_FB_BORROWAPPLYDETAIL = detail;
                    RepayDetailInfo.UPDATEDATE = DateTime.Now;
                    RepayDetailInfo.CREATEDATE = DateTime.Now;
                    RepayDetailInfo.T_FB_REPAYAPPLYMASTER = entity;
                    RepayDetailInfo.CREATEUSERID = detail.CREATEUSERID;
                    RepayDetailInfo.UPDATEUSERID = detail.UPDATEUSERID;
                    RepayDetailInfo.CHARGETYPE = 1;
                    RepayDetailInfo.REPAYMONEY = 0;
                    RepayDetailInfo.CREATEDATE = DateTime.Now;
                    dBorrowTotal += detail.BORROWMONEY;
                    RepDetail.Add(RepayDetailInfo);
                }
                entity.BRORROWEDMONEY = dBorrowTotal;

                string strRepayCode = string.Empty, strMsg = string.Empty;

                doc.AddRepayApplyMasterAndDetail(entity, RepDetail);

                SMT.SaaS.BLLCommonServices.EngineConfigWS.EngineWcfGlobalFunctionClient Client = new EngineWS.EngineWcfGlobalFunctionClient();
                EngineWS.CustomUserMsg[] user = new EngineWS.CustomUserMsg[1];
                user[0] = new EngineWS.CustomUserMsg() { UserID = entity.OWNERID, FormID = entity.REPAYAPPLYMASTERID };
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("BORROWAPPLYMASTERID", borMaster.BORROWAPPLYMASTERID);
                Client.ApplicationMsgTrigger(user, "FB", "T_FB_REPAYAPPLYMASTER", Utility.ObjListToXml<T_FB_REPAYAPPLYMASTER>(entity, "FB", null), EngineWS.MsgType.Task);

                return entity.REPAYAPPLYMASTERID;
            }
            catch (Exception e)
            {
                string abc = "<FB>Message=[" + e.Message + "]" + "<FB>Source=[" + e.Source + "]<FB>StackTrace=[" + e.StackTrace + "]<FB>TargetSite=[" + e.TargetSite + "]";
                Tracer.Debug(abc);
                return abc;
            }
        }

        #endregion

        public void CallApplicationService(string strXml)
        {
            throw new NotImplementedException();
        }
    }
}
