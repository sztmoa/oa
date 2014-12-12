using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMT.FBAnalysis.BLL;
using System.ServiceModel;
using SMT_FB_EFModel;
using SMT.FBAnalysis.CustomModel;
using System.IO;
using System.Text;
using SMT.SaaS.BLLCommonServices.EngineConfigWS;

namespace SMT.FBAnalysis.Service
{
    public partial class DailyManagementServices
    {
        #region   个人活动经费下拨审核通过 所有人发起待办   
        /// <summary>
        /// 提供给个人活动经费下拨审核通过后，给每个人发送待办
        /// </summary>
        /// <param name="strMasterID">下拨ID</param>
        [OperationContract(IsOneWay=true)]
        public void PersonmoneyAssignSendTask(string strMasterID)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("【个人活动经费下拨审核通过 所有人发起待办】【MasterID:" + strMasterID + "】");
            try
            {
                PersonmoneyAssignDetailBLL bll = new PersonmoneyAssignDetailBLL();
                ChargeApplyMasterBLL chargeApplyBLL = new ChargeApplyMasterBLL();
                EngineWcfGlobalFunctionClient engineClient = new EngineWcfGlobalFunctionClient();
                var vResult = bll.GetPersonmoneyAssignDetailByMasterID(strMasterID);
                if (vResult != null && vResult.Count() > 0)
                {
                  
                    //需要发送待办的下拨子表数据
                    List<T_FB_PERSONMONEYASSIGNDETAIL> listNeedSend = new List<T_FB_PERSONMONEYASSIGNDETAIL>();
                    //业务数据成功保存后 发待办的数据
                    Dictionary<string,T_FB_PERSONMONEYASSIGNDETAIL> savedListNeedSend = new Dictionary<string,T_FB_PERSONMONEYASSIGNDETAIL>();
                   List<string> listString = new List<string>();
                  
                    foreach(var v in vResult)
                    {
                        listString.Add(v.OWNERID);
                        sb.AppendLine("【活动经费个人】【" + v.OWNERID + "】【" + v.OWNERNAME + "】");
                    }
                    string[] listhaveTask = engineClient.GetPersonFromDoTaskByTitle(listString.ToArray(), "您的个人活动经费已下拨，费用报销单还未提交，请及时处理!");

                    #region 循环保存业务数据

                    foreach (var ent in vResult)
                    {
                        //只给未存在下拨待办的人，发送待办
                        if (listhaveTask.Where(c => c.Contains(ent.OWNERID)).FirstOrDefault() == null)
                        {
                            //sb.AppendLine("【需要发送待办的个人】【" + ent.OWNERID + "】【" + ent.OWNERNAME + "】");
                            T_FB_CHARGEAPPLYMASTER chargeApplyMaster = new T_FB_CHARGEAPPLYMASTER();
                            chargeApplyMaster.CREATECOMPANYID = ent.OWNERCOMPANYID;
                            chargeApplyMaster.CREATECOMPANYNAME = ent.OWNERCOMPANYNAME;
                            chargeApplyMaster.CREATEDEPARTMENTID = ent.OWNERDEPARTMENTID;
                            chargeApplyMaster.CREATEDEPARTMENTNAME = ent.OWNERDEPARTMENTNAME;
                            chargeApplyMaster.CREATEPOSTID = ent.OWNERPOSTID;
                            chargeApplyMaster.CREATEPOSTNAME = ent.OWNERPOSTNAME;
                            chargeApplyMaster.CREATEUSERID = ent.OWNERID;
                            chargeApplyMaster.CREATEUSERNAME = ent.OWNERNAME;
                            chargeApplyMaster.OWNERCOMPANYID = ent.OWNERCOMPANYID;
                            chargeApplyMaster.OWNERCOMPANYNAME = ent.OWNERCOMPANYNAME;
                            chargeApplyMaster.OWNERDEPARTMENTID = ent.OWNERDEPARTMENTID;
                            chargeApplyMaster.OWNERDEPARTMENTNAME = ent.OWNERDEPARTMENTNAME;
                            chargeApplyMaster.OWNERID = ent.OWNERID;
                            chargeApplyMaster.OWNERNAME = ent.OWNERNAME;
                            chargeApplyMaster.OWNERPOSTID = ent.OWNERPOSTID;
                            chargeApplyMaster.OWNERPOSTNAME = ent.OWNERPOSTNAME;
                            chargeApplyMaster.CHARGEAPPLYMASTERID = string.Empty;
                            chargeApplyMaster.CHARGEAPPLYMASTERID = Guid.NewGuid().ToString();
                            chargeApplyMaster.CHARGEAPPLYMASTERCODE = " ";
                            chargeApplyMaster.BUDGETARYMONTH = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM") + "-1");
                            chargeApplyMaster.PAYTYPE = 1;
                            chargeApplyMaster.TOTALMONEY = 0;
                            chargeApplyMaster.REPAYMENT = 0;
                            chargeApplyMaster.EDITSTATES = 0;
                            chargeApplyMaster.CHECKSTATES = 0;
                            chargeApplyMaster.PAYTARGET = 1;
                            chargeApplyMaster.UPDATEUSERID = ent.OWNERID;
                            chargeApplyMaster.UPDATEUSERNAME = ent.OWNERNAME;
                            chargeApplyMaster.CREATEDATE = DateTime.Now;
                            chargeApplyMaster.UPDATEDATE = DateTime.Now;
                            bool saveResult = chargeApplyBLL.Add(chargeApplyMaster);
                            if (saveResult)
                            {
                                savedListNeedSend.Add(chargeApplyMaster.CHARGEAPPLYMASTERID, ent);
                            }
                            else
                            {
                                sb.AppendLine("【保存费用报销主数据失败】");
                            }
                        }
                    }
                    #endregion
                    #region  发送待办
                  
                    List<T_WF_DOTASK> tasks = new List<T_WF_DOTASK>();
                      
                    foreach (var v in savedListNeedSend)
                    {
                         T_WF_DOTASK task = new T_WF_DOTASK();
                        task.DOTASKID = Guid.NewGuid().ToString();
                        task.COMPANYID = v.Value.OWNERCOMPANYID;
                        task.ORDERID = v.Key;
                        task.ORDERUSERID = v.Value.OWNERID;
                        task.ORDERUSERNAME = v.Value.OWNERNAME;
                        task.ORDERSTATUS = 0;
                        task.MESSAGEBODY = "您的个人活动经费已下拨，费用报销单还未提交，请及时处理!";
                        string strUrl = "<?xml version=\"1.0\" encoding=\"utf-8\"?><System>" +
                                            "<AssemblyName>SMT.FBAnalysis.UI</AssemblyName>" +
                                            "<PublicClass>SMT.FBAnalysis.UI.Common.Utility</PublicClass>" +
                                            "<ProcessName>CreateFormFromEngine</ProcessName>" +
                                            "<PageParameter>SMT.FBAnalysis.UI.Form.ChargeApplyForm</PageParameter>" +
                                            "<ApplicationOrder>{0}</ApplicationOrder>" +
                                         "<FormTypes>Edit</FormTypes></System>";
                        task.APPLICATIONURL = string.Format(strUrl, task.ORDERID);
                        task.RECEIVEUSERID = v.Value.OWNERID;
                        task.BEFOREPROCESSDATE = DateTime.Now.AddDays(3);
                        task.DOTASKTYPE = 4;
                        task.DOTASKSTATUS = 0;
                        task.MAILSTATUS = 0;
                        task.RTXSTATUS = 0;
                        task.SYSTEMCODE = "FB";
                        task.MODELCODE = "T_FB_CHARGEAPPLYMASTER";
                        task.CREATEDATETIME = DateTime.Now;
                        task.REMARK = "未提交单据";
                        tasks.Add(task);
                        sb.AppendLine("【发送待办】【待办接收者:" + v.Value.OWNERNAME + "】【报销单号:" + task.ORDERID + "】");
                    }
                    if (tasks.Count > 0)
                    {
                        var sendResult = engineClient.AddDoTaskEntity(tasks.ToArray());
                        sb.AppendLine("【调用Engine[AddDoTaskEntity]】【结果:" + sendResult + "】");
                    }
                    else
                    {
                        sb.AppendLine("【不需要发送待办】");
                    }
                    #endregion
                }

            }
            catch (Exception ex)
            {
                sb.AppendLine("【异常】【" + ex.Message + "】");
            }
            SMT.Foundation.Log.Tracer.Debug(sb.ToString());

        }
        #endregion
      
        #region T_FB_CHARGEAPPLYDETAIL 服务

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="strChargeApplyDetailId">主键索引</param>
        /// <returns></returns>
        [OperationContract]
        public T_FB_CHARGEAPPLYDETAIL GetChargeApplyDetailByID(string strChargeApplyDetailId)
        {
            T_FB_CHARGEAPPLYDETAIL entRd = new T_FB_CHARGEAPPLYDETAIL();
            using (ChargeApplyDetailBLL bllChargeApplyDetail = new ChargeApplyDetailBLL())
            {
                entRd = bllChargeApplyDetail.GetChargeApplyDetailByID(strChargeApplyDetailId);
                return entRd;
            }
        }

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="strChargeApplyDetailName">名称</param>
        /// <param name="strCompanyID">公司序号</param>        
        /// <param name="strSortKey">排序字段</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页显示记录数</param>
        /// <param name="pageCount">总页码</param>
        /// <returns>返回信息</returns>
        [OperationContract]
        public List<T_FB_CHARGEAPPLYDETAIL> GetChargeApplyDetailListByMultSearch(string strFilter, List<object> objArgs,
            string strSortKey, int pageIndex, int pageSize, ref int pageCount)
        {
            IList<T_FB_CHARGEAPPLYDETAIL> entList = new List<T_FB_CHARGEAPPLYDETAIL>();
            using (ChargeApplyDetailBLL bllChargeApplyDetail = new ChargeApplyDetailBLL())
            {
                entList = bllChargeApplyDetail.GetChargeApplyDetailRdListByMultSearch(strFilter, objArgs, strSortKey, pageIndex, pageSize, ref pageCount).ToList();
                return entList.Count() > 0 ? entList.ToList() : null;
            }
        }

        #endregion

        #region T_FB_CHARGEAPPLYMASTER 服务

        #region 获取报销申请信息

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="strChargeApplyMasterId">主键索引</param>
        /// <returns></returns>
        [OperationContract]
        public T_FB_CHARGEAPPLYMASTER GetChargeApplyMasterByID(string strChargeApplyMasterId)
        {
            T_FB_CHARGEAPPLYMASTER entRd = new T_FB_CHARGEAPPLYMASTER();
            using (ChargeApplyMasterBLL bllChargeApplyMaster = new ChargeApplyMasterBLL())
            {
                entRd = bllChargeApplyMaster.GetChargeApplyMasterByID(strChargeApplyMasterId);
                return entRd;
            }
        }

        /// <summary>
        /// 根据报销主表ID获取T_FB_CHARGEAPPLYDETAIL信息   add by zl
        /// </summary>
        /// <param name="objChargeApplyMasterId"></param>
        /// <returns></returns>
        [OperationContract]
        public List<T_FB_CHARGEAPPLYDETAIL> GetChargeApplyDetailByMasterID(List<object> objChargeApplyMasterId)
        {
            List<T_FB_CHARGEAPPLYDETAIL> entRdlist = new List<T_FB_CHARGEAPPLYDETAIL>();
            using (ChargeApplyDetailBLL bllChargeApplyDetail = new ChargeApplyDetailBLL())
            {
                entRdlist = bllChargeApplyDetail.GetChargeApplyDetailByMasterID(objChargeApplyMasterId);
                return entRdlist;
            }
        }

        /// <summary>
        /// 根据报销主表ID获取T_FB_CHARGEAPPLYREPAYDETAIL信息   add by zl
        /// </summary>
        /// <param name="strChargeApplyMasterId"></param>
        /// <returns></returns>
        [OperationContract]
        public List<T_FB_CHARGEAPPLYREPAYDETAIL> GetChargeApplyRepayDetailByMasterID(string strChargeApplyMasterId)
        {
            List<T_FB_CHARGEAPPLYREPAYDETAIL> entRdlist = new List<T_FB_CHARGEAPPLYREPAYDETAIL>();
            using (ChargeApplyRepayDetailBLL bllChargeRepayApplyDetail = new ChargeApplyRepayDetailBLL())
            {
                entRdlist = bllChargeRepayApplyDetail.GetChargeApplyRepayDetailByMasterID(strChargeApplyMasterId);
                return entRdlist;
            }
        }

        #endregion

        #region 查询个人报销申请

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <param name="strSortKey"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <param name="checkState"></param>
        /// <returns></returns>
        [OperationContract]
        public List<T_FB_CHARGEAPPLYMASTER> GetChargeApplyMasterListByMultSearch(string userID, string strDateStart, string strDateEnd,
            string checkState, string strFilter, List<object> objArgs, string strSortKey, int pageIndex, int pageSize, ref int pageCount)
        {
            IQueryable<T_FB_CHARGEAPPLYMASTER> entIq;
            IList<T_FB_CHARGEAPPLYMASTER> entList = new List<T_FB_CHARGEAPPLYMASTER>();
            using (ChargeApplyMasterBLL bllChargeApplyMaster = new ChargeApplyMasterBLL())
            {
                entIq = bllChargeApplyMaster.GetChargeApplyMasterRdListByMultSearch(userID, strDateStart, strDateEnd,
                        checkState, strFilter, objArgs, strSortKey, pageIndex, pageSize, ref pageCount);
                if (entIq == null)
                {
                    return null;
                }
                //add zl 2012.1.4
                //foreach (T_FB_CHARGEAPPLYMASTER obj in entIq)
                //{
                //    if (obj.PAYTYPE == 2)
                //    {
                //        if (obj.REPAYMENT != null)
                //        {
                //            obj.TOTALMONEY = obj.TOTALMONEY - obj.REPAYMENT.Value;
                //        }
                //    }
                //    entList.Add(obj);
                //}
                //add end
                entList = entIq.ToList();
                return entList.Count() > 0 ? entList.ToList() : null;
            }
        }
        #endregion

        /// <summary>
        /// 写入费用报销主从表数据  add by zl
        /// </summary>
        /// <param name="chargeMaster"></param>
        /// <param name="chargeDetail"></param>
        /// <returns></returns>
        [OperationContract]
        public bool AddChargeApplyMasterAndDetail(T_FB_CHARGEAPPLYMASTER chargeMaster, List<T_FB_CHARGEAPPLYDETAIL> chargeDetail,
            List<T_FB_CHARGEAPPLYREPAYDETAIL> chargeRepDetail)
        {
            using (ChargeApplyMasterBLL chargeBLL = new ChargeApplyMasterBLL())
            {
                bool re;
                re = chargeBLL.AddChargeApplyMasterAndDetail(chargeMaster, chargeDetail, chargeRepDetail);
                //string aa = "";
                //re = chargeBLL.AddChargeApplyMasterAndDetailMobile(chargeMaster, chargeDetail, chargeRepDetail,ref aa);
                return re;
            }
        }

        [OperationContract]
        public bool AddChargeApplyMasterAndDetailMobile(T_FB_CHARGEAPPLYMASTER chargeMaster, List<T_FB_CHARGEAPPLYDETAIL> chargeDetail,
            List<T_FB_CHARGEAPPLYREPAYDETAIL> chargeRepDetail,ref string strMsg)
        {
            using (ChargeApplyMasterBLL chargeBLL = new ChargeApplyMasterBLL())
            {
                bool re;
                re = chargeBLL.AddChargeApplyMasterAndDetailMobile(chargeMaster, chargeDetail, chargeRepDetail,ref strMsg);
                //re = chargeBLL.AddChargeApplyMasterAndDetail(chargeMaster, chargeDetail, chargeRepDetail);
                return re;
            }
        }

        /// <summary>
        /// 更新费用报销主表和明细表数据  add by zl
        /// </summary>
        /// <param name="chargeMaster"></param>
        /// <param name="chargeDetail"></param>
        /// <returns></returns>
        [OperationContract]
        public void UptChargeApplyMasterAndDetail(string strActionType, T_FB_CHARGEAPPLYMASTER chargeMaster,
            List<T_FB_CHARGEAPPLYDETAIL> chargeDetail, List<T_FB_CHARGEAPPLYREPAYDETAIL> chargeRepDetail, ref string strMsg)
        {
            using (ChargeApplyMasterBLL chargeBLL = new ChargeApplyMasterBLL())
            {
                chargeBLL.UptChargeApplyMasterAndDetail(strActionType, chargeMaster, chargeDetail, chargeRepDetail, ref strMsg);
            }
        }

        /// <summary>
        /// 更新手机版费用报销
        /// </summary>
        /// <param name="strActionType"></param>
        /// <param name="chargeMaster"></param>
        /// <param name="chargeDetail"></param>
        /// <param name="chargeRepDetail"></param>
        /// <param name="strMsg"></param>
        [OperationContract]
        public void UptChargeApplyMasterAndDetailToMobile(string strActionType, T_FB_CHARGEAPPLYMASTER chargeMaster,
            List<T_FB_CHARGEAPPLYDETAIL> chargeDetail, List<T_FB_CHARGEAPPLYREPAYDETAIL> chargeRepDetail, ref string strMsg)
        {
            using (ChargeApplyMasterBLL chargeBLL = new ChargeApplyMasterBLL())
            {
                chargeBLL.UptChargeApplyMasterAndDetailForMobile(strActionType, chargeMaster, chargeDetail, chargeRepDetail, ref strMsg);
            }
        }


        /// <summary>
        /// 删除费用报销主明细表数据   add by zl
        /// </summary>
        /// <param name="chargeMasterID"></param>
        /// <returns></returns>
        [OperationContract]
        public bool DelChargeApplyMasterAndDetail(List<string> chargeMasterID)
        {
            using (ChargeApplyMasterBLL chargeBLL = new ChargeApplyMasterBLL())
            {
                foreach (string obj in chargeMasterID)
                {
                    if (!chargeBLL.DelChargeApplyMasterAndDetail(obj))
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// 审核流程时更新费用报销主表CHECKSTATES字段值 add by zl
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="chargeDetail"></param>
        /// <returns></returns>
        [OperationContract]
        public bool UptChargeApplyCheckState(T_FB_CHARGEAPPLYMASTER entity, List<T_FB_CHARGEAPPLYDETAIL> chargeDetail,
            List<T_FB_CHARGEAPPLYREPAYDETAIL> chargeRepDetail)
        {
            using (ChargeApplyMasterBLL chargeBLL = new ChargeApplyMasterBLL())
            {
                string strMsg = string.Empty;
                chargeBLL.UptChargeApplyCheckState(entity, chargeDetail, chargeRepDetail, ref strMsg);

                if (!string.IsNullOrWhiteSpace(strMsg))
                {
                    return false;
                }

                return true;
            }

        }

        /// <summary>
        /// 获取单据编号 add by zl
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [OperationContract]
        public string GetChargeOrderCode(T_FB_CHARGEAPPLYMASTER entity)
        {
            return new OrderCodeBLL().GetAutoOrderCode(entity);
        }

        #endregion
        #region 导入、导出报表
        /// <summary>
        /// 导出报表
        /// </summary>
        /// <param name="strOwnerID">strOwnerID</param>
        /// <param name="strDateStart">strDateStart</param>
        /// <param name="strDateEnd">strDateEnd</param>
        /// <param name="strCheckState">strCheckState</param>
        /// <param name="strFilter">strFilter</param>
        /// <param name="objArgs">objArgs</param>
        /// <param name="strSortKey">strSortKey</param>
        /// <returns>导出报表</returns>
        [OperationContract]
        public List<V_ChargeApplyReport> ExportChargeApplyMasterReports(string strOwnerID, string strDateStart, string strDateEnd, string strCheckState, string strFilter, List<object> objArgs, string strSortKey)
        {
            using (ChargeApplyMasterBLL chargeBLL = new ChargeApplyMasterBLL())
            {
                return chargeBLL.ChargeApplyMasterRdListByMultSearchReports(strOwnerID, strDateStart, strDateEnd, strCheckState, strFilter, objArgs, strSortKey);
            }
        }
        [OperationContract]
        public List<V_ChargeApplyReport> ImportAttendMonthlyBalanceFromCSV(UploadFileModel UploadFile, string strCreateUserID, ref string strMsg)
        {
            string strPath = string.Empty;
            SaveFile(UploadFile, out strPath);
            string strPhysicalPath = HttpContext.Current.Server.MapPath(strPath);

            using (ChargeApplyMasterBLL chargeBLL = new ChargeApplyMasterBLL())
            {
                return chargeBLL.ImportChargeApplyByImportExcel(strCreateUserID, strPhysicalPath, ref strMsg);
            }
        }
        #endregion

        #region 更新费用报销状态
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="strChargeApplyDetailId">主键索引</param>
        /// <returns></returns>
        [OperationContract]
        public int UptChargeApplyIsPayed(List<V_ChargeApplyReport> vChargeApplys)
        {
            using (ChargeApplyMasterBLL chargeBLL = new ChargeApplyMasterBLL())
            {
                return chargeBLL.UptChargeApplyIsPayed(vChargeApplys);
            }
        }
        #endregion
        #region 文件上传服务
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UploadFile">上传载体</param>
        /// <param name="strFilePath">上传文件存储的相对路径</param>
        [OperationContract]
        public void SaveFile(UploadFileModel UploadFile, out string strFilePath)
        {
            // Store File to File System
            string strNewFileName = string.Empty;
            string strVirtualPath = System.Configuration.ConfigurationManager.AppSettings["FileUploadLocation"].ToString();
            if (!string.IsNullOrWhiteSpace(UploadFile.FileName))
            {
                strNewFileName = DateTime.Now.ToString("yyMMddhhmmss") + DateTime.Now.Millisecond.ToString() + UploadFile.FileName.Substring(UploadFile.FileName.LastIndexOf("."));
            }

            string strPath = HttpContext.Current.Server.MapPath(strVirtualPath) + strNewFileName;
            if (Directory.Exists(HttpContext.Current.Server.MapPath(strVirtualPath)) == false)
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(strVirtualPath));
            }
            FileStream FileStream = new FileStream(strPath, FileMode.Create);
            FileStream.Write(UploadFile.File, 0, UploadFile.File.Length);

            FileStream.Close();
            FileStream.Dispose();

            strFilePath = strVirtualPath + strNewFileName;
        }
        #endregion
        #region T_FB_EXTENSIONALORDER 服务
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="strExtensionalOrderId">主键索引</param>
        /// <returns></returns>
        [OperationContract]
        public T_FB_EXTENSIONALORDER GetExtensionalOrderByID(string strExtensionalOrderId)
        {
            T_FB_EXTENSIONALORDER entRd = new T_FB_EXTENSIONALORDER();
            ExtensionalOrderBLL bllExtensionalOrder = new ExtensionalOrderBLL();
            entRd = bllExtensionalOrder.GetExtensionalOrderByID(strExtensionalOrderId);
            return entRd;
        }

        #endregion

        #region 获取费用报销版本
        /// <summary>
        /// 获取费用报销版本
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        public bool GetVersionIsHuNan()
        {
            using (ChargeApplyMasterBLL chargeBLL = new ChargeApplyMasterBLL())
            {                
                bool isReturn = chargeBLL.GetVersionIsHuNan();
                return isReturn;
            }
        }
        #endregion

    }
}