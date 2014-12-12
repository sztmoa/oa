using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SMT_FB_EFModel;
using OrganizationWS = SMT.SaaS.BLLCommonServices.OrganizationWS;
using SMT.SaaS.BLLCommonServices;
using EngineWS = SMT.SaaS.BLLCommonServices.EngineConfigWS;
using PersonnelWS = SMT.SaaS.BLLCommonServices.PersonnelWS;
using System.Reflection;

namespace SMT.FB.BLL
{
    public class OrganizationBLL : BudgetAccountBLL
    {
       

        private OrganizationWS.OrganizationServiceClient organizationService;
        public OrganizationBLL()
        {
            organizationService = new OrganizationWS.OrganizationServiceClient();
        }

        public List<FBEntity> GetCompany(QueryExpression qe)
        {
            List<VirtualCompany> listC = InnerGetCompany(qe);
            List<FBEntity> listResult = listC.ToFBEntityList();
            listResult.ForEach(company =>
            {
                VirtualCompany vcCompany = company.Entity as VirtualCompany;
                List<FBEntity> listDepartMent = vcCompany.DepartmentCollection.ToFBEntityList();
                RelationManyEntity rme = new RelationManyEntity();
                rme.EntityType = typeof(VirtualDepartment).Name;
                rme.FBEntities = listDepartMent;
                company.CollectionEntity.Add(rme);

                listDepartMent.ForEach(department =>
                {
                    VirtualDepartment virtualDepartment = department.Entity as VirtualDepartment;
                    List<FBEntity> listPost = virtualDepartment.PostCollection.ToFBEntityList();
                    RelationManyEntity rmePost = new RelationManyEntity();
                    rmePost.EntityType = typeof(VirtualPost).Name;
                    rmePost.FBEntities = listPost;
                    department.CollectionEntity.Add(rmePost);
                });
            });
            return listResult;
        }
        public List<VirtualCompany> InnerGetCompany(QueryExpression qe)
        {
            try
            {

                string action = ((int)Utility.Permissions.Browse).ToString();
                
                List<OrganizationWS.T_HR_COMPANY> comList = organizationService.GetCompanyByEntityPerm(qe.VisitUserID, action, qe.VisitModuleCode).ToList();
                List<OrganizationWS.V_DEPARTMENT> deptList = organizationService.GetDepartmentView(qe.VisitUserID, action, qe.VisitModuleCode).ToList();
                List<OrganizationWS.V_POST> vpostList = organizationService.GetPostView(qe.VisitUserID, action, qe.VisitModuleCode).ToList();

                List<OrganizationWS.T_HR_POST> postList = new List<OrganizationWS.T_HR_POST>();
                foreach (var ent in vpostList)
                {
                    OrganizationWS.T_HR_POST pt = new OrganizationWS.T_HR_POST();
                    pt.POSTID = ent.POSTID;
                    pt.FATHERPOSTID = ent.FATHERPOSTID;
                    pt.CHECKSTATE = ent.CHECKSTATE;
                    pt.EDITSTATE = ent.EDITSTATE;

                    pt.T_HR_POSTDICTIONARY = new OrganizationWS.T_HR_POSTDICTIONARY();
                    pt.T_HR_POSTDICTIONARY.POSTDICTIONARYID = Guid.NewGuid().ToString();
                    pt.T_HR_POSTDICTIONARY.POSTNAME = ent.POSTNAME;

                    pt.T_HR_DEPARTMENT = new OrganizationWS.T_HR_DEPARTMENT();
                    pt.T_HR_DEPARTMENT.DEPARTMENTID = deptList.Where(s => s.DEPARTMENTID == ent.DEPARTMENTID).FirstOrDefault().DEPARTMENTID;
              

                    postList.Add(pt);
                }

                List<VirtualCompany> listCompany = new List<VirtualCompany>();
                
                comList.ForEach(comHR =>
                {
                    List<VirtualDepartment> listDepartment = new List<VirtualDepartment>();

                    VirtualCompany vc = new VirtualCompany();
                    vc.ID = comHR.COMPANYID;
                    vc.Name = comHR.CNAME;
                    vc.DepartmentCollection = listDepartment;
                    listCompany.Add(vc);

                    List<OrganizationWS.V_DEPARTMENT> deptListPart = deptList.FindAll(item =>
                    {
                        if (item.COMPANYID == null)
                        {
                            return false;
                        }
                        return item.COMPANYID == comHR.COMPANYID;
                        
                    });

                    deptListPart.ForEach(deptHR =>
                    {
                        List<VirtualPost> listPost = new List<VirtualPost>();

                        VirtualDepartment vd = new VirtualDepartment();
                        vd.ID = deptHR.DEPARTMENTID;
                        vd.Name = deptHR.DEPARTMENTNAME;
                        vd.VirtualCompany = vc;
                        vd.PostCollection = listPost;
                        listDepartment.Add(vd);

                        List<OrganizationWS.T_HR_POST> postListPart = postList.FindAll(item =>
                        {
                            if (item.T_HR_DEPARTMENT.DEPARTMENTID == null)
                            {
                                return false;
                            }
                            return item.T_HR_DEPARTMENT.DEPARTMENTID == deptHR.DEPARTMENTID;
                        });
                        postListPart.ForEach(postHR =>
                        {
                            VirtualPost vp = new VirtualPost();
                            vp.ID = postHR.POSTID;
                            vp.Name = postHR.T_HR_POSTDICTIONARY.POSTNAME;
                            vp.VirtualCompany = vc;
                            vp.VirtualDepartment = vd;
                            listPost.Add(vp);
                        });
                    });


                });
                return listCompany;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex);
                throw new Exception ("调用HR服务异常", ex);
            }
        }

        internal List<VirtualCompany> GetVirtualCompany(QueryExpression qe)
        {
            List<VirtualCompany> listCompany = new List<VirtualCompany>();
            string action = ((int)Utility.Permissions.Browse).ToString();
            string ModuleCode = qe.VisitModuleCode;
            if (qe.VisitModuleCode == "T_FB_SUBJECTCOMPANY_COMPANY")
            {
                ModuleCode = "T_FB_SUBJECTCOMPANY";
            }
            List<string> CompanyIds = new List<string>();
            //在公司科目维护中去掉以下2个公司
            CompanyIds.Add("427eb67d-35b4-47a9-9609-baf5355d2ed5");//深圳市神州通投资集团有限公司
            CompanyIds.Add("7cd6c0a4-9735-476a-9184-103b962d3383");//初始化公司
            List<OrganizationWS.T_HR_COMPANY> comList = organizationService.GetCompanyByEntityPerm(qe.VisitUserID, action, ModuleCode).ToList();

            comList.ForEach(comHR =>
            {
                VirtualCompany vc = new VirtualCompany();
                vc.ID = comHR.COMPANYID;
                vc.Name = comHR.CNAME;
                if (ModuleCode == "T_FB_SUBJECTCOMPANY" || ModuleCode == "T_FB_SUBJECTCOMPANYSET")
                {
                    if (!(CompanyIds.IndexOf(vc.ID) > -1))
                    {
                        listCompany.Add(vc);
                    }
                }
                else
                {
                    listCompany.Add(vc);
                }
                
            });
            return listCompany;
        }
        internal List<VirtualDepartment> GetVirtualDepartment(QueryExpression qe)
        {
            List<VirtualDepartment> listDepartment = new List<VirtualDepartment>();
            string action = ((int)Utility.Permissions.Browse).ToString();
            List<OrganizationWS.V_DEPARTMENT> deptList = organizationService.GetDepartmentView(qe.VisitUserID, action, qe.VisitModuleCode).ToList();

            deptList.ForEach(deptHR =>
            {
                VirtualDepartment vd = new VirtualDepartment();
                vd.ID = deptHR.DEPARTMENTID;
                vd.Name = deptHR.DEPARTMENTNAME;
                listDepartment.Add(vd);
            });
            return listDepartment;
        }
        internal List<VirtualPost> GetVirtualPost(QueryExpression qe)
        {
            List<VirtualPost> listPost = new List<VirtualPost>();
            string action = ((int)Utility.Permissions.Browse).ToString();
            List<OrganizationWS.T_HR_POST> postList = organizationService.GetPostByEntityPerm(qe.VisitUserID, action, qe.VisitModuleCode).ToList();

            postList.ForEach(postHR =>
            {
                VirtualPost vp = new VirtualPost();
                vp.ID = postHR.POSTID;
                vp.Name = postHR.T_HR_POSTDICTIONARY.POSTNAME;
                listPost.Add(vp);
            });
            return listPost;
        }
        internal List<VirtualUser> GetVirtualUser(List<string> userIDs)
        {
            List<VirtualUser> listUser = new List<VirtualUser>();
            if (userIDs.Count == 0)
            {
                return listUser;
            }
            try
            {
                PersonnelWS.PersonnelServiceClient personService = new PersonnelWS.PersonnelServiceClient();

                List<PersonnelWS.T_HR_EMPLOYEE> employeeList = personService.GetEmployeeByIDs(userIDs.ToArray()).ToList();

                employeeList.ForEach(employeeHR =>
                {
                    VirtualUser vu = new VirtualUser();
                    vu.ID = employeeHR.EMPLOYEEID;
                    vu.Name = employeeHR.EMPLOYEECNAME + "(" + employeeHR.EMPLOYEECODE + ")";
                    listUser.Add(vu);
                });
            }
            catch ( Exception ex)
            {
                System.Diagnostics.Debug.Write(ex);
            }
            return listUser;

        }

        public List<VirtualCompany> InitData()
        {            
            string userID = "";
            
            List<OrganizationWS.T_HR_COMPANY> comList = organizationService.GetCompanyActived(userID).ToList();
            List<OrganizationWS.T_HR_DEPARTMENT> deptList = organizationService.GetDepartmentActived(userID).ToList();
            List<OrganizationWS.T_HR_POST> postList = organizationService.GetPostActived(userID).ToList();

            List<VirtualCompany> listCompany = new List<VirtualCompany>();
            comList.ForEach(comHR =>
                {
                    List<VirtualDepartment> listDepartment = new List<VirtualDepartment>();

                    VirtualCompany vc = new VirtualCompany();
                    vc.ID = comHR.COMPANYID;
                    vc.Name = comHR.CNAME;
                    vc.DepartmentCollection = listDepartment;
                    listCompany.Add(vc);

                    List<OrganizationWS.T_HR_DEPARTMENT> deptListPart = deptList.FindAll(item =>
                        {
                            return item.T_HR_COMPANY.COMPANYID == comHR.COMPANYID;
                        });
                    
                    deptListPart.ForEach(deptHR =>
                        {
                            List<VirtualPost> listPost = new List<VirtualPost>();

                            VirtualDepartment vd = new VirtualDepartment();
                            vd.ID = deptHR.DEPARTMENTID;
                            vd.Name = deptHR.T_HR_DEPARTMENTDICTIONARY.DEPARTMENTNAME;
                            vd.VirtualCompany = vc;
                            vd.PostCollection = listPost;
                            listDepartment.Add(vd);

                            List<OrganizationWS.T_HR_POST> postListPart = postList.FindAll(item =>
                                {
                                    return item.T_HR_DEPARTMENT.DEPARTMENTID == deptHR.DEPARTMENTID;
                                });
                            postListPart.ForEach(postHR =>
                                {
                                    VirtualPost vp = new VirtualPost();
                                    vp.ID = postHR.POSTID;
                                    vp.Name = postHR.T_HR_POSTDICTIONARY.POSTNAME;
                                    vp.VirtualCompany = vc;
                                    vp.VirtualDepartment = vd;
                                    listPost.Add(vp);
                                });
                        });

                   
                });
            return listCompany;
        }

        //处理岗位信息一栏
        public List<T_FB_PERSONMONEYASSIGNDETAIL> UpdatePostInfo(List<T_FB_PERSONMONEYASSIGNDETAIL> listperson)
        {
            PersonnelWS.PersonnelServiceClient pe=new PersonnelWS.PersonnelServiceClient ();
            PersonnelWS.V_EMPLOYEEPOSTFORFB[] vlistpostinfo = new PersonnelWS.V_EMPLOYEEPOSTFORFB[listperson.Count];
            int i = 0;
            listperson.ForEach(item =>
                {
                    PersonnelWS.V_EMPLOYEEPOSTFORFB vpostinfo = new PersonnelWS.V_EMPLOYEEPOSTFORFB();
                    vpostinfo.PERSONBUDGETAPPLYDETAILID = item.PERSONBUDGETAPPLYDETAILID;
                    vpostinfo.OWNERID = item.OWNERID;
                    vpostinfo.OWNERPOSTID = item.OWNERPOSTID;
                    vlistpostinfo[i]=vpostinfo;
                    i++;
                });
            try
            {
                vlistpostinfo = pe.GetEmployeeListForFB(vlistpostinfo) as PersonnelWS.V_EMPLOYEEPOSTFORFB[];
                               
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write("调用HR服务异常 GetEmployeeListForFB "+ex);  
            }

            listperson.ForEach(item =>
            {
                var person = vlistpostinfo.Where(p => p.PERSONBUDGETAPPLYDETAILID == item.PERSONBUDGETAPPLYDETAILID ).FirstOrDefault();
                if (person != null)
                {
                    item.SUGGESTBUDGETMONEY = person.SUM;
                    switch (person.EMPLOYEESTATE)
                    {
                        case "0"://试用期
                            item.POSTINFO = "试用期，请注意";
                            break;
                        case "1"://在职
                            item.POSTINFO = string.Empty;
                            break;
                        case "2"://已离职
                            item.POSTINFO = "已离职，请删除";
                            break;
                        case "3"://离职中
                            item.POSTINFO = "离职中，请注意";
                            break;
                        case "4"://未入职
                            item.POSTINFO = "未入职，请删除";
                            break;
                        case "10"://异动中
                            item.POSTINFO = "异动中，请异动后再处理";
                            break;
                        case "11"://异动过
                            item.POSTINFO = "异动过，已转换成新岗位";

                            item.OWNERPOSTID = person.NEWPOSTID;
                            item.OWNERPOSTNAME  = person.NEWPOSTNAME;
                            item.OWNERDEPARTMENTID = person.NEWDEPARTMENTID;
                            item.OWNERDEPARTMENTNAME  = person.NEWDEPARTMENTNAME;
                            item.OWNERCOMPANYID = person.NEWCOMPANYID;
                            item.OWNERCOMPANYNAME = person.NEWCOMPANYNAME;
                            break;
                        case "12"://岗位异常
                            item.POSTINFO = "岗位异常，请删除后再选择";
                            break;
                        default:
                            item.POSTINFO = string.Empty;
                            break;
                    }

                    switch (person.ISAGENCY)
                    {
                        case "0"://主岗位
                          // item.POSTINFO += string.Empty;
                            break;
                        case "1"://赚职
                            if (item.POSTINFO != string.Empty && person.EMPLOYEESTATE!="1")
                                item.POSTINFO = item.POSTINFO.Insert(0, "兼职，");
                            else
                                item.POSTINFO = "兼职";
                            break;
                        default:
                           // item.POSTINFO += string.Empty;
                            break;
                    }
                }
            });
           return listperson;
        }

        public T_FB_PERSONMONEYASSIGNMASTER CreatePersonMoneyAssignInfo(string ASSIGNCOMPANYID, string OWNERID)
        {
                T_FB_PERSONMONEYASSIGNMASTER master = GetPersonMoneyAssign(ASSIGNCOMPANYID, OWNERID);

                if (master == null)
                {
                    return null;
                }


                FBEntity fbEntity = new FBEntity();
                fbEntity.Entity = master;
                fbEntity.FBEntityState = FBEntityState.Added;

                SaveFBEntityDefault(fbEntity);

                EngineWS.EngineWcfGlobalFunctionClient Client = new EngineWS.EngineWcfGlobalFunctionClient();
                EngineWS.CustomUserMsg userMsg = new EngineWS.CustomUserMsg();
                userMsg.FormID = master.PERSONMONEYASSIGNMASTERID;
                userMsg.UserID = master.OWNERID;
                EngineWS.CustomUserMsg[] List = new EngineWS.CustomUserMsg[1];
                List[0] = userMsg;
                string submitName = master.OWNERNAME;
                Client.ApplicationMsgTrigger(List, "FB", "T_FB_PERSONMONEYASSIGNMASTER", ObjListToXml(master, "FB", submitName), EngineWS.MsgType.Task);

                return master;
            
        }

        public T_FB_PERSONMONEYASSIGNMASTER GetPersonMoneyAssign(string ASSIGNCOMPANYID, string OWNERID)
        {
            PersonnelWS.PersonnelServiceClient pe = new PersonnelWS.PersonnelServiceClient();

            T_FB_PERSONMONEYASSIGNMASTER master = new T_FB_PERSONMONEYASSIGNMASTER();
            master.PERSONMONEYASSIGNMASTERID = Guid.NewGuid().ToString();

            PersonnelWS.V_EMPLOYEEPOST employee = pe.GetEmployeeDetailByID(OWNERID);

            OrganizationWS.OrganizationServiceClient orgClient = new OrganizationWS.OrganizationServiceClient();
            OrganizationWS.T_HR_COMPANY entCompany = orgClient.GetCompanyById(ASSIGNCOMPANYID);


            master.OWNERID = OWNERID;
            master.OWNERNAME = employee.T_HR_EMPLOYEE.EMPLOYEECNAME;
            master.OWNERCOMPANYID = ASSIGNCOMPANYID;
            master.OWNERCOMPANYNAME = employee.EMPLOYEEPOSTS[0].T_HR_POST.T_HR_DEPARTMENT.T_HR_COMPANY.CNAME;
            master.OWNERDEPARTMENTID = employee.EMPLOYEEPOSTS[0].T_HR_POST.T_HR_DEPARTMENT.T_HR_DEPARTMENTDICTIONARY.DEPARTMENTDICTIONARYID;
            master.OWNERDEPARTMENTNAME = employee.EMPLOYEEPOSTS[0].T_HR_POST.T_HR_DEPARTMENT.T_HR_DEPARTMENTDICTIONARY.DEPARTMENTDICTIONARYID;
            master.OWNERPOSTID = employee.EMPLOYEEPOSTS[0].T_HR_POST.POSTID;
            master.OWNERPOSTNAME = employee.EMPLOYEEPOSTS[0].T_HR_POST.T_HR_POSTDICTIONARY.POSTNAME;

            master.ASSIGNCOMPANYID = ASSIGNCOMPANYID;
            master.ASSIGNCOMPANYNAME = ASSIGNCOMPANYID;
            if (entCompany != null)
            {
                master.ASSIGNCOMPANYNAME = entCompany.CNAME;
            }

            master.BUDGETARYMONTH = DateTime.Now;
            master.EDITSTATES = 1;
            master.CHECKSTATES = 0;


            master.CREATECOMPANYID = "001";
            master.CREATECOMPANYNAME = "系统生成";
            master.CREATEDEPARTMENTID = "001";
            master.CREATEDEPARTMENTNAME = "系统生成";
            master.CREATEPOSTID = "001";
            master.CREATEPOSTNAME = "系统生成";
            master.CREATEUSERID = "001";
            master.CREATEUSERNAME = "系统生成";

            master.UPDATEUSERID = "001";
            master.UPDATEUSERNAME = "系统生成";
            master.CREATEDATE = DateTime.Now;
            master.UPDATEDATE = DateTime.Now;

            PersonnelWS.V_EMPLOYEEFUNDS[] vlistpostinfo = null;

            try
            {
                vlistpostinfo = pe.GetEmployeeFunds(ASSIGNCOMPANYID);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write("调用HR服务异常 GetEmployeeFunds " + ex);
            }

            if (vlistpostinfo != null && vlistpostinfo.Count() > 0)
            {
                QueryExpression qe = QueryExpression.Equal("SUBJECTID", "d5134466-c207-44f2-8a36-cf7b96d5851f");
                qe.QueryType = typeof(T_FB_SUBJECT).Name;
                T_FB_SUBJECT entSubject = GetEntity<T_FB_SUBJECT>(qe);
                if (entSubject == null)
                {
                    return null;
                }

                foreach (var item in vlistpostinfo)
                {
                    T_FB_PERSONMONEYASSIGNDETAIL detail = new T_FB_PERSONMONEYASSIGNDETAIL();
                    detail.PERSONBUDGETAPPLYDETAILID = Guid.NewGuid().ToString();
                    detail.T_FB_PERSONMONEYASSIGNMASTER = master;
                    detail.T_FB_SUBJECT = entSubject;
                    //detail.BUDGETMONEY = item.REALSUM;
                    detail.BUDGETMONEY = item.NEEDSUM;
                    detail.SUGGESTBUDGETMONEY = item.NEEDSUM;
                    detail.POSTINFO = item.ATTENDREMARK;

                    detail.OWNERID = item.EMPLOYEEID;
                    detail.OWNERNAME = item.EMPLOYECNAME;
                    detail.OWNERPOSTID = item.POSTID;
                    detail.OWNERPOSTNAME = item.POSTNAME;
                    detail.OWNERDEPARTMENTID = item.DEPARTMENTID;
                    detail.OWNERDEPARTMENTNAME = item.DEPARTMENTNAME;
                    detail.OWNERCOMPANYID = item.COMPANYID;
                    detail.OWNERCOMPANYNAME = item.COMPANYNAME;

                    detail.CREATEUSERID = "001";
                    detail.CREATEUSERNAME = "系统生成";
                    detail.CREATEDATE = DateTime.Now;

                    detail.UPDATEUSERID = "001";
                    detail.UPDATEUSERNAME = "系统生成";
                    detail.UPDATEDATE = DateTime.Now;

                    master.BUDGETMONEY += item.REALSUM;
                }
            }

            return master;

        }
        
        /// <summary>
        /// 引擎需要的XML形式的实体字符串转化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectdata"></param>
        /// <param name="SystemCode"></param>
        /// <returns></returns>
        public static string ObjListToXml<T>(T objectdata, string SystemCode, string currentUserName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sb.AppendLine("<System>");
            Type objtype = objectdata.GetType();
            sb.AppendLine("<Name>" + SystemCode + "</Name>");
            sb.AppendLine("<Object Name=\"Approval\" Description=\"\">");
            PropertyInfo[] propinfos = objtype.GetProperties();
            foreach (PropertyInfo propinfo in propinfos)
            {
                if (propinfo.Name.ToUpper() != "CHECKSTATE")
                {
                    sb.AppendLine("<Attribute Name=\"" + propinfo.Name + "\" Description=\"" + "" + "\" DataType=\"" + "" + "\" DataValue=\"" + propinfo.GetValue(objectdata, null) + "\"/>");
                }
            }
            sb.AppendLine("<Attribute Name=\"" + "CURRENTEMPLOYEENAME" + "\" Description=\"" + "提交者" + "\" DataType=\"" + "" + "\" DataValue=\"" + currentUserName + "\"/>");
            sb.AppendLine("</Object>");
            sb.AppendLine("</System>");
            return sb.ToString();
        }

        #region 静态变量
        public static LockManager LockHelper = new LockManager();
        #endregion

        #region Save FBEntityList

        public bool SaveEntityBLLSaveList(List<FBEntity> fbEntityList)
        {
            if (fbEntityList.Count == 0)
            {
                return true;
            }
            string returnType = fbEntityList[0].Entity.GetType().Name;
            Type type = this.GetType();
            MethodInfo method = type.GetMethod("SaveList" + returnType);
            if (method != null)
            {
                try
                {

                    object result = method.Invoke(this, new object[] { fbEntityList });
                    return (bool)result;
                }
                catch (Exception ex)
                {

                    throw ex.InnerException;
                }
            }
            else
            {
                return SaveFbEntityList(fbEntityList);
            }

        }

        public bool SaveFbEntityList(List<FBEntity> fbEntityList)
        {
            return SaveEntityBLLSaveList(fbEntityList);
        }

        public bool SaveListT_FB_SUMSETTINGSMASTER(List<FBEntity> fbEntityList)
        {
            fbEntityList.ForEach(item =>
            {
                T_FB_SUMSETTINGSMASTER Master = item.Entity as T_FB_SUMSETTINGSMASTER;
                if (Master.EDITSTATES == 0)
                {
                    QueryExpression qeID = QueryExpression.Equal("T_FB_SUMSETTINGSMASTER.SUMSETTINGSMASTERID", Master.SUMSETTINGSMASTERID);

                    qeID.QueryType = "T_FB_SUMSETTINGSDETAIL";
                    var result = GetFBEntities(qeID);
                    if (result != null)
                    {
                        List<FBEntity> fbEntity = result;
                        fbEntity.ForEach(p =>
                        {
                            //QueryExpression qeCompany = QueryExpression.Equal("SUMSETTINGSMASTERID", Master.SUMSETTINGSMASTERID);
                            //qeCompany.QueryType = "T_FB_COMPANYBUDGETSUMMASTER";
                            //var v = GetFBEntity(qeCompany);
                            //QueryExpression qeDept = QueryExpression.Equal("SUMSETTINGSMASTERID", Master.SUMSETTINGSMASTERID);
                            //qeDept.QueryType = "T_FB_DEPTBUDGETSUMMASTER";
                            //var q = GetFBEntity(qeCompany);

                            //if (v != null||q!=null)
                            //{
                            //    throw new FBBLLException("以下公司已经有汇总使用，不能删除！");
                            //}

                            p.FBEntityState = FBEntityState.Modified;
                            T_FB_SUMSETTINGSDETAIL detail = p.Entity as T_FB_SUMSETTINGSDETAIL;
                            detail.EDITSTATES = 0;
                            SaveEntityBLLSaveList(fbEntity);
                        });
                    }
                }
            });

            return SaveEntityBLLSaveList(fbEntityList);
        }

        /// <summary>
        /// 保存公司科目维护
        ///   级联的去除不可用的部门科目和岗位科目
        /// </summary>
        /// <param name="fbEntityList"></param>
        /// <returns></returns>
        public bool SaveListT_FB_SUBJECTCOMPANY(List<FBEntity> fbEntityList)
        {
            QueryExpression qeSCom = new QueryExpression();
            QueryExpression qeTop = qeSCom;
            string StrCompanyID = "";//公司ID
            bool IsExistPlus = false;
            // 找出没有设置年度预算而后又允许年度预算的
            List<T_FB_SUBJECTCOMPANY> inActivedlist = fbEntityList.CreateList(item =>
            {
                T_FB_SUBJECTCOMPANY entity = item.Entity as T_FB_SUBJECTCOMPANY;
                if (string.IsNullOrEmpty(StrCompanyID))
                {
                    StrCompanyID = entity.OWNERCOMPANYID;
                    QueryExpression qe = QueryExpression.Equal("SUBJECTCOMPANYID", entity.SUBJECTCOMPANYID);
                    var baData = this.InnerGetEntities<T_FB_SUBJECTCOMPANY>(qe);
                    if (baData.Count() > 0)
                    {
                        T_FB_SUBJECTCOMPANY OldSub = new T_FB_SUBJECTCOMPANY();
                        OldSub = baData.FirstOrDefault();
                        if (OldSub.ISYEARBUDGET == 0)
                        {
                            if (entity.ISYEARBUDGET == 1)
                            {
                                QueryExpression qeAccount = QueryExpression.Equal("OWNERCOMPANYID", entity.OWNERCOMPANYID);
                                QueryExpression qeAccount1 = QueryExpression.Equal("T_FB_SUBJECT.SUBJECTID", entity.T_FB_SUBJECT != null ? entity.T_FB_SUBJECT.SUBJECTID : entity.T_FB_SUBJECTReference.EntityKey.EntityKeyValues[0].Value.ToString());
                                QueryExpression qeAccount2 = QueryExpression.Equal("ACCOUNTOBJECTTYPE", "1");
                                QueryExpression qeAccount3 = new QueryExpression();
                                qeAccount3.PropertyName = "USABLEMONEY";
                                qeAccount3.PropertyValue = "0";
                                qeAccount3.Operation = QueryExpression.Operations.LessThanOrEqual;
                                qeAccount3.Operation = QueryExpression.Operations.LessThan;//是否有问题
                                qeAccount.RelatedType = QueryExpression.RelationType.And;
                                qeAccount1.RelatedType = QueryExpression.RelationType.And;
                                qeAccount2.RelatedType = QueryExpression.RelationType.And;
                                qeAccount3.RelatedType = QueryExpression.RelationType.And;

                                qeAccount.RelatedExpression = qeAccount1;
                                qeAccount2.RelatedExpression = qeAccount1;
                                qeAccount3.RelatedExpression = qeAccount2;
                                qeAccount3.QueryType = typeof(T_FB_BUDGETACCOUNT).Name;


                                //var baDataAccount = this.InnerGetEntities<T_FB_BUDGETACCOUNT>(qeAccount);
                                //if(baDataAccount.Count() >0)
                                //{
                                //    //IsExistPlus= true;
                                //}
                            }
                        }
                    }
                }
                //return entity.ACTIVED != 1 ? entity : null;

                return entity;
            });
            if (IsExistPlus)
            {
                return IsExistPlus;
            }

            //var baData = this.InnerGetEntities<T_FB_SUBJECTCOMPANY>(qeDept);
            // 查出公司科目相关的部门科目及岗位科目
            inActivedlist.ForEach(item =>
            {
                qeTop.RelatedExpression = QueryExpression.Equal("T_FB_SUBJECTCOMPANY.SUBJECTCOMPANYID", item.SUBJECTCOMPANYID);
                qeTop.RelatedType = QueryExpression.RelationType.Or;
                qeTop = qeTop.RelatedExpression;
            });
            // 将部门科目及岗位科目置为不可用
            if (qeSCom.RelatedExpression != null)
            {
                qeSCom = qeSCom.RelatedExpression;
                qeSCom.Include = new string[] { "T_FB_SUBJECTPOST" };
                List<T_FB_SUBJECTDEPTMENT> inActiveDataList = GetEntities<T_FB_SUBJECTDEPTMENT>(qeSCom.RelatedExpression);
                inActiveDataList.ForEach(item =>
                {
                    item.ACTIVED = 0;
                    item.T_FB_SUBJECTPOST.ToList().ForEach(itemPost =>
                    {
                        itemPost.ACTIVED = 0;
                    });
                });
            }

            if (fbEntityList.Count > 0)
            {
                //记录公司部门科目设置修改流水
                SaveListT_FB_WFSUBJECTSETTING(fbEntityList, "1");
            }
            return SaveEntityBLLSaveList(fbEntityList);
        }


        /// <summary>
        ///   部门科目记录修改流水
        /// </summary>
        /// <param name="fbEntityList"></param>
        /// <returns></returns>
        public bool SaveListT_FB_SUBJECTDEPTMENT(List<FBEntity> fbEntityList)
        {
            if (fbEntityList.Count > 0)
            {
                //记录公司部门科目设置修改流水
                SaveListT_FB_WFSUBJECTSETTING(fbEntityList, "2");

                //修改部门启用时，同时更新岗位启用。
                fbEntityList.ForEach(item =>
                {
                    T_FB_SUBJECTDEPTMENT entity = item.Entity as T_FB_SUBJECTDEPTMENT;
                    if (entity != null && entity.ACTIVED == 0)
                    {
                        List<FBEntity> EntityListPost = new List<FBEntity>();
                        QueryExpression qe = QueryExpression.Equal("T_FB_SUBJECTDEPTMENT.SUBJECTDEPTMENTID", entity.SUBJECTDEPTMENTID);

                        List<T_FB_SUBJECTPOST> PostList = GetEntities<T_FB_SUBJECTPOST>(qe);
                        PostList.ForEach(p =>
                        {
                            FBEntity a = new FBEntity();
                            a.FBEntityState = FBEntityState.Modified;

                            p.ACTIVED = 0;//1 : 可用 ; 0 : 不可用

                            a.Entity = p;
                            a.EntityKey = null;
                            EntityListPost.Add(a);
                        });
                        SaveEntityBLLSaveList(EntityListPost);
                    }
                });
            }
            return SaveEntityBLLSaveList(fbEntityList);
        }

        /// <summary>
        ///   部门科目记录修改流水
        /// </summary>
        /// <param name="fbEntityList"></param>
        /// <returns></returns>
        public bool SaveListT_FB_SUBJECTPOST(List<FBEntity> fbEntityList)
        {
            if (fbEntityList.Count > 0)
            {
                //记录修改流水
                SaveListT_FB_WFSUBJECTSETTING(fbEntityList, "3");

                // 写死活动经费科目 可用；
                //string MoneyAssign = SystemBLL.etityT_FB_SYSTEMSETTINGS.MONEYASSIGNSUBJECTID;
                //fbEntityList.ForEach(item =>
                //{
                //    T_FB_SUBJECTPOST entity = item.Entity as T_FB_SUBJECTPOST;
                //    string strSubjectID = entity.T_FB_SUBJECTReference.EntityKey.EntityKeyValues[0].Value.ToString();
                //    if (strSubjectID == MoneyAssign)
                //    {
                //        FBEntity a = new FBEntity();
                //        a.FBEntityState = FBEntityState.Modified;

                //        entity.ACTIVED = 1;//1 : 可用 ; 0 : 不可用

                //        a.Entity = entity;
                //        a.EntityKey = null;
                //        item = a;

                //        return;
                //    }
                //});
            }
            return SaveEntityBLLSaveList(fbEntityList);
        }


        /// <summary>        
        ///   保存科目设置流水
        /// </summary>
        /// <param name="fbEntityList"></param>
        /// <returns></returns>
        public bool SaveListT_FB_WFSUBJECTSETTING(List<FBEntity> fbEntityList, string strfig)
        {
            List<FBEntity> inActivedlist = fbEntityList.CreateList(item =>
            {
                T_FB_WFSUBJECTSETTING fbEntity = new T_FB_WFSUBJECTSETTING();

                if (strfig == "1")
                {
                    T_FB_SUBJECTCOMPANY SubjectEntity = item.Entity as T_FB_SUBJECTCOMPANY;

                    fbEntity.WFSUBJECTSETTINGID = Guid.NewGuid().ToString();
                    fbEntity.SUBJECTID = SubjectEntity.T_FB_SUBJECTReference.EntityKey.EntityKeyValues[0].Value.ToString();
                    fbEntity.ACTIVED = SubjectEntity.ACTIVED;
                    fbEntity.ISMONTHADJUST = SubjectEntity.ISMONTHADJUST;
                    fbEntity.ISMONTHLIMIT = SubjectEntity.ISMONTHLIMIT;
                    fbEntity.ISPERSON = SubjectEntity.ISPERSON;
                    fbEntity.ISYEARBUDGET = SubjectEntity.ISYEARBUDGET;
                    fbEntity.CONTROLTYPE = SubjectEntity.CONTROLTYPE;
                    fbEntity.OWNERCOMPANYID = SubjectEntity.OWNERCOMPANYID;
                    fbEntity.OWNERCOMPANYNAME = SubjectEntity.OWNERCOMPANYNAME;
                    fbEntity.OWNERDEPARTMENTID = SubjectEntity.OWNERDEPARTMENTID;
                    fbEntity.OWNERDEPARTMENTNAME = SubjectEntity.OWNERDEPARTMENTNAME;
                    fbEntity.OWNERPOSTID = SubjectEntity.OWNERPOSTID;
                    fbEntity.OWNERPOSTNAME = SubjectEntity.OWNERPOSTNAME;
                    fbEntity.CREATEUSERID = SubjectEntity.CREATEUSERID;
                    fbEntity.UPDATEUSERID = SubjectEntity.UPDATEUSERID;
                    fbEntity.UPDATEDATE = DateTime.Now;
                    fbEntity.CREATEDATE = DateTime.Now;
                    fbEntity.ORDERTYPE = strfig;//1 公司 2部门 3岗位
                }
                else if (strfig == "2")
                {
                    T_FB_SUBJECTDEPTMENT SubjectEntity = item.Entity as T_FB_SUBJECTDEPTMENT;
                    if (SubjectEntity == null)
                    {
                        T_FB_SUBJECTPOST SubjectEntity1 = item.Entity as T_FB_SUBJECTPOST;

                        fbEntity.WFSUBJECTSETTINGID = Guid.NewGuid().ToString();
                        fbEntity.SUBJECTID = SubjectEntity1.T_FB_SUBJECTReference.EntityKey.EntityKeyValues[0].Value.ToString();
                        fbEntity.ACTIVED = SubjectEntity1.ACTIVED;
                        fbEntity.LIMITBUDGEMONEY = SubjectEntity1.LIMITBUDGEMONEY;
                        fbEntity.OWNERCOMPANYID = SubjectEntity1.OWNERCOMPANYID;
                        fbEntity.OWNERCOMPANYNAME = SubjectEntity1.OWNERCOMPANYNAME;
                        fbEntity.OWNERDEPARTMENTID = SubjectEntity1.OWNERDEPARTMENTID;
                        fbEntity.OWNERDEPARTMENTNAME = SubjectEntity1.OWNERDEPARTMENTNAME;
                        fbEntity.OWNERPOSTID = SubjectEntity1.OWNERPOSTID;
                        fbEntity.OWNERPOSTNAME = SubjectEntity1.OWNERPOSTNAME;
                        fbEntity.CREATEUSERID = SubjectEntity1.CREATEUSERID;
                        fbEntity.UPDATEUSERID = SubjectEntity1.UPDATEUSERID;
                        fbEntity.UPDATEDATE = DateTime.Now;
                        fbEntity.CREATEDATE = DateTime.Now;
                        fbEntity.ORDERTYPE = "3";//1 公司 2部门 3岗位
                    }
                    else
                    {
                        fbEntity.WFSUBJECTSETTINGID = Guid.NewGuid().ToString();
                        fbEntity.SUBJECTID = SubjectEntity.T_FB_SUBJECTReference.EntityKey.EntityKeyValues[0].Value.ToString();
                        fbEntity.ACTIVED = SubjectEntity.ACTIVED;
                        fbEntity.LIMITBUDGEMONEY = SubjectEntity.LIMITBUDGEMONEY;
                        fbEntity.OWNERCOMPANYID = SubjectEntity.OWNERCOMPANYID;
                        fbEntity.OWNERCOMPANYNAME = SubjectEntity.OWNERCOMPANYNAME;
                        fbEntity.OWNERDEPARTMENTID = SubjectEntity.OWNERDEPARTMENTID;
                        fbEntity.OWNERDEPARTMENTNAME = SubjectEntity.OWNERDEPARTMENTNAME;
                        fbEntity.OWNERPOSTID = SubjectEntity.OWNERPOSTID;
                        fbEntity.OWNERPOSTNAME = SubjectEntity.OWNERPOSTNAME;
                        fbEntity.CREATEUSERID = SubjectEntity.CREATEUSERID;
                        fbEntity.UPDATEUSERID = SubjectEntity.UPDATEUSERID;
                        fbEntity.UPDATEDATE = DateTime.Now;
                        fbEntity.CREATEDATE = DateTime.Now;
                        fbEntity.ORDERTYPE = strfig;//1 公司 2部门 3岗位
                    }
                }
                else if (strfig == "3")
                {
                    T_FB_SUBJECTPOST SubjectEntity = item.Entity as T_FB_SUBJECTPOST;

                    fbEntity.WFSUBJECTSETTINGID = Guid.NewGuid().ToString();
                    fbEntity.SUBJECTID = SubjectEntity.T_FB_SUBJECTReference.EntityKey.EntityKeyValues[0].Value.ToString();
                    fbEntity.ACTIVED = SubjectEntity.ACTIVED;
                    fbEntity.LIMITBUDGEMONEY = SubjectEntity.LIMITBUDGEMONEY;
                    fbEntity.OWNERCOMPANYID = SubjectEntity.OWNERCOMPANYID;
                    fbEntity.OWNERCOMPANYNAME = SubjectEntity.OWNERCOMPANYNAME;
                    fbEntity.OWNERDEPARTMENTID = SubjectEntity.OWNERDEPARTMENTID;
                    fbEntity.OWNERDEPARTMENTNAME = SubjectEntity.OWNERDEPARTMENTNAME;
                    fbEntity.OWNERPOSTID = SubjectEntity.OWNERPOSTID;
                    fbEntity.OWNERPOSTNAME = SubjectEntity.OWNERPOSTNAME;
                    fbEntity.CREATEUSERID = SubjectEntity.CREATEUSERID;
                    fbEntity.UPDATEUSERID = SubjectEntity.UPDATEUSERID;
                    fbEntity.UPDATEDATE = DateTime.Now;
                    fbEntity.CREATEDATE = DateTime.Now;
                    fbEntity.ORDERTYPE = strfig;//1 公司 2部门 3岗位
                }
                FBEntity a = new FBEntity();
                a.Entity = fbEntity;
                a.FBEntityState = FBEntityState.Added;
                a.EntityKey = null;
                return a;
            });
            return SaveEntityBLLSaveList(inActivedlist);
        }
        #endregion

        #region Save SaveEntity
        public FBEntity SaveEntityBLLSave(SaveEntity saveEntity)
        {
            FBEntity fbEntity = saveEntity.FBEntity;
            FBEntity result = this.SaveEntityBLLSave(fbEntity);
            if (saveEntity.QueryExpression != null)
            {
                result = QueryEntities(saveEntity.QueryExpression).FirstOrDefault();
            }
            return result;
        }
        #endregion

        #region Save FBEntity

        public FBEntity InnerSave(FBEntity fbEntity)
        {
            //暂时不特殊处理
            //if (fbEntity.FBEntityState == FBEntityState.ReSubmit)
            //{
            //    return ReSubmit(fbEntity);
            //}
            string returnType = fbEntity.Entity.GetType().Name;
            Type type = this.GetType();
            MethodInfo method = type.GetMethod("Save" + returnType);
            if (method != null)
            {
                try
                {
                    object result = method.Invoke(this, new object[] { fbEntity });
                    return result as FBEntity;
                }
                catch (Exception ex)
                {

                    throw ex.InnerException;
                }
            }
            else
            {
                return SaveFBEntityDefault(fbEntity);
            }
        }

        public FBEntity SaveEntityBLLSave(FBEntity fbEntity)
        {
            string orderid = "";
            try
            {
                orderid = fbEntity.Entity.GetOrderID();
                if (orderid != null && LockHelper.LockOrder(orderid))
                {
                    throw new FBBLLException("单据已锁定，不能操作!");
                }
                return InnerSave(fbEntity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                LockHelper.ReleaseOrder(orderid);
            }
        }

        public FBEntity SaveFBEntityDefault(FBEntity fbEntity)
        {
            if (base.FBEntityBllSave(fbEntity))
            {
                return GetFBEntityByEntityKey(fbEntity.Entity.EntityKey);
            }
            return null;
        }

        public FBEntity SaveT_FB_SUMSETTINGSMASTER(FBEntity fbEntity)
        {
            T_FB_SUMSETTINGSMASTER entity = fbEntity.Entity as T_FB_SUMSETTINGSMASTER;
            entity.T_FB_SUMSETTINGSDETAIL.ToList().ForEach(item =>
            {
                QueryExpression qeCompanyID = QueryExpression.Equal(FieldName.OwnerCompanyID, item.OWNERCOMPANYID).And(FieldName.EditStates, "1");

                qeCompanyID.QueryType = "T_FB_SUMSETTINGSDETAIL";

                var result = GetFBEntityByExpression(qeCompanyID);
                if (result != null)
                {
                    throw new FBBLLException("以下公司已经有汇总: " + item.OWNERCOMPANYNAME);
                }
            });
            return SaveFBEntityDefault(fbEntity);
        }

        public FBEntity SaveT_FB_DEPTBUDGETSUMMASTER(FBEntity fbEntity)
        {

            T_FB_DEPTBUDGETSUMMASTER entity = fbEntity.Entity as T_FB_DEPTBUDGETSUMMASTER;

            #region 提交的单据的有效时间控制在上个月
            CheckStates currentCheckStates = (CheckStates)Convert.ToInt16(entity.CHECKSTATES);
            // if ( currentCheckStates =
            #endregion
            #region 审核中的单据
            QueryExpression qeCompanyID = QueryExpression.Equal(FieldName.OwnerCompanyID, entity.CREATECOMPANYID);
            QueryExpression qeMonth = QueryExpression.Equal("BUDGETARYMONTH", entity.BUDGETARYMONTH.ToString("yyyy-MM-dd"));
            QueryExpression qeStatesApproving = QueryExpression.Equal(FieldName.CheckStates, ((int)CheckStates.Approving).ToString());

            qeCompanyID.RelatedExpression = qeMonth;
            qeMonth.RelatedExpression = qeStatesApproving;

            var result2 = InnerGetEntities<T_FB_DEPTBUDGETSUMMASTER>(qeCompanyID);
            if (result2.Count() > 0)
            {
                string departs = " ";
                result2.ToList().ForEach(item => { departs = departs.Trim() + "、" + item.OWNERDEPARTMENTNAME; });
                throw new FBBLLException("该月度还有以下部门的月度预算处于审核中: " + departs.Substring(1));
            }
            #endregion
            return SaveFBEntityDefault(fbEntity);
        }

        public FBEntity SaveT_FB_COMPANYBUDGETSUMMASTER(FBEntity fbEntity)
        {

            T_FB_COMPANYBUDGETSUMMASTER entity = fbEntity.Entity as T_FB_COMPANYBUDGETSUMMASTER;

            QueryExpression qeCompanyID = QueryExpression.Equal(FieldName.OwnerCompanyID, entity.CREATECOMPANYID);
            QueryExpression qeYear = QueryExpression.Equal("BUDGETYEAR", entity.BUDGETYEAR.ToString());
            QueryExpression qeStatesApproving = QueryExpression.Equal(FieldName.CheckStates, ((int)CheckStates.Approving).ToString());

            qeCompanyID.RelatedExpression = qeYear;
            qeYear.RelatedExpression = qeStatesApproving;

            var result2 = InnerGetEntities<T_FB_COMPANYBUDGETAPPLYMASTER>(qeCompanyID);
            if (result2.Count() > 0)
            {
                string departs = " ";
                result2.ToList().ForEach(item => { departs = departs.Trim() + "、" + item.OWNERDEPARTMENTNAME; });
                throw new FBBLLException("该年度还有以下部门的年度预算处于审核中: " + departs.Substring(1));
            }
            return SaveFBEntityDefault(fbEntity);
        }

        public FBEntity SaveT_FB_DEPTBUDGETAPPLYMASTER(FBEntity fbEntity)
        {

            T_FB_DEPTBUDGETAPPLYMASTER entity = fbEntity.Entity as T_FB_DEPTBUDGETAPPLYMASTER;
            entity.BUDGETARYMONTH = new DateTime(entity.BUDGETARYMONTH.Year, entity.BUDGETARYMONTH.Month, 1);
            DateTime bDate = entity.BUDGETARYMONTH;


            #region 审核中
            QueryExpression qeID = QueryExpression.Equal("DEPTBUDGETAPPLYMASTERID", entity.DEPTBUDGETAPPLYMASTERID);
            qeID.Operation = QueryExpression.Operations.NotEqual;
            QueryExpression qeBUDGETARYMONTH = QueryExpression.Equal("BUDGETARYMONTH", bDate.ToString("yyyy-MM-dd"));
            qeID.RelatedExpression = qeBUDGETARYMONTH;
            QueryExpression qeDept = QueryExpression.Equal(FieldName.OwnerDepartmentID, entity.OWNERDEPARTMENTID);
            qeBUDGETARYMONTH.RelatedExpression = qeDept;
            QueryExpression qeStatesApproving = QueryExpression.Equal(FieldName.CheckStates, ((int)CheckStates.Approving).ToString());
            qeDept.RelatedExpression = qeStatesApproving;

            var result = InnerGetEntities<T_FB_DEPTBUDGETAPPLYMASTER>(qeID);
            if (result.Count() > 0)
            {
                throw new FBBLLException("该部门月度预算正在审核中");
            }
            #endregion

            #region 审核通过
            qeID = QueryExpression.Equal("DEPTBUDGETAPPLYMASTERID", entity.DEPTBUDGETAPPLYMASTERID);
            qeID.Operation = QueryExpression.Operations.NotEqual;
            qeBUDGETARYMONTH = QueryExpression.Equal("BUDGETARYMONTH", bDate.ToString("yyyy-MM-dd"));
            qeID.RelatedExpression = qeBUDGETARYMONTH;
            qeDept = QueryExpression.Equal(FieldName.OwnerDepartmentID, entity.OWNERDEPARTMENTID);
            qeBUDGETARYMONTH.RelatedExpression = qeDept;
            qeStatesApproving = QueryExpression.Equal(FieldName.CheckStates, ((int)CheckStates.Approved).ToString());
            qeDept.RelatedExpression = qeStatesApproving;
            QueryExpression qeEDITSTATES = QueryExpression.Equal("ISVALID", "0");
            qeStatesApproving.RelatedExpression = qeEDITSTATES;

            result = InnerGetEntities<T_FB_DEPTBUDGETAPPLYMASTER>(qeID);
            if (result.Count() > 0)
            {
                throw new FBBLLException("该部门月度预算已审核通过");
            }
            #endregion

            #region 审核通过
            qeID = QueryExpression.Equal("DEPTBUDGETAPPLYMASTERID", entity.DEPTBUDGETAPPLYMASTERID);
            qeID.Operation = QueryExpression.Operations.NotEqual;
            qeBUDGETARYMONTH = QueryExpression.Equal("BUDGETARYMONTH", bDate.ToString("yyyy-MM-dd"));
            qeID.RelatedExpression = qeBUDGETARYMONTH;
            qeDept = QueryExpression.Equal(FieldName.OwnerDepartmentID, entity.OWNERDEPARTMENTID);
            qeBUDGETARYMONTH.RelatedExpression = qeDept;
            //qeStatesApproving = QueryExpression.Equal(FieldName.CheckStates, ((int)CheckStates.Approved).ToString());
            //qeDept.RelatedExpression = qeStatesApproving;
            qeEDITSTATES = QueryExpression.Equal("ISVALID", "1");
            qeDept.RelatedExpression = qeEDITSTATES;

            result = InnerGetEntities<T_FB_DEPTBUDGETAPPLYMASTER>(qeID);
            if (result.Count() > 0)
            {
                throw new FBBLLException("该部门月度预算已生效");
            }
            #endregion

            #region 月度汇总
            QueryExpression qeCompany = QueryExpression.Equal(FieldName.OwnerCompanyID, entity.OWNERCOMPANYID);
            qeBUDGETARYMONTH = QueryExpression.Equal("BUDGETARYMONTH", bDate.ToString("yyyy-MM-dd"));
            QueryExpression qeStatesApproved = QueryExpression.Equal(FieldName.CheckStates, ((int)CheckStates.Approved).ToString());
            qeStatesApproving = QueryExpression.Equal(FieldName.CheckStates, ((int)CheckStates.Approving).ToString());
            qeStatesApproved.RelatedExpression = qeStatesApproving;
            qeStatesApproved.RelatedType = QueryExpression.RelationType.Or;
            qeBUDGETARYMONTH.RelatedExpression = qeStatesApproved;
            qeCompany.RelatedExpression = qeBUDGETARYMONTH;

            var result2 = InnerGetEntities<T_FB_DEPTBUDGETSUMMASTER>(qeCompany);
            if (result2.Count() > 0 && entity.CHECKSTATES != (int)CheckStates.UnApproved)
            {
                throw new FBBLLException("该月度已做过月度汇总或正在审核中");
            }
            #endregion



            return SaveFBEntityDefault(fbEntity);
        }

        public FBEntity SaveT_FB_DEPTBUDGETADDMASTER(FBEntity fbEntity)
        {

            T_FB_DEPTBUDGETADDMASTER entity = fbEntity.Entity as T_FB_DEPTBUDGETADDMASTER;
            entity.BUDGETARYMONTH = new DateTime(entity.BUDGETARYMONTH.Year, entity.BUDGETARYMONTH.Month, 1);
            if (entity.BUDGETCHARGE <= 0)
            {
                throw new Exception("费用总预算必须大于0");
            }
            //DateTime bDate = entity.BUDGETARYMONTH;

            //QueryExpression qeDept = QueryExpression.Equal(FieldName.OwnerDepartmentID, entity.OWNERDEPARTMENTID);
            //QueryExpression qe = QueryExpression.Equal("BUDGETARYMONTH", bDate.ToString("yyyy-MM-dd"));
            //QueryExpression qeStatesApproved = QueryExpression.Equal(FieldName.CheckStates, ((int)CheckStates.Approved).ToString());

            //qeDept.RelatedExpression = qe;
            //qe.RelatedExpression = qeStatesApproved;


            //var result = InnerGetEntities<T_FB_DEPTBUDGETAPPLYMASTER>(qeDept);
            //if (result.Count() == 0)
            //{
            //    throw new FBBLLException("该部门尚未做过月度预算");
            //}

            return SaveFBEntityDefault(fbEntity);
        }

        public FBEntity SaveT_FB_COMPANYBUDGETAPPLYMASTER(FBEntity fbEntity)
        {

            #region 是否存在审核中
            T_FB_COMPANYBUDGETAPPLYMASTER entity = fbEntity.Entity as T_FB_COMPANYBUDGETAPPLYMASTER;
            //entity.BUDGETYEAR = DateTime.Now.Year;
            QueryExpression qeID = QueryExpression.Equal("COMPANYBUDGETAPPLYMASTERID", entity.COMPANYBUDGETAPPLYMASTERID);
            qeID.Operation = QueryExpression.Operations.NotEqual;
            QueryExpression qeBUDGETYEAR = QueryExpression.Equal("BUDGETYEAR", entity.BUDGETYEAR.ToString());
            qeID.RelatedExpression = qeBUDGETYEAR;
            QueryExpression qeCom = QueryExpression.Equal("OWNERDEPARTMENTID", entity.OWNERDEPARTMENTID);
            qeBUDGETYEAR.RelatedExpression = qeCom;
            QueryExpression qeStatesApproving = QueryExpression.Equal(FieldName.CheckStates, ((int)CheckStates.Approving).ToString());
            qeCom.RelatedExpression = qeStatesApproving;

            var result = InnerGetEntities<T_FB_COMPANYBUDGETAPPLYMASTER>(qeID);
            if (result.Count() > 0)
            {
                throw new FBBLLException("该部门年度预算正在审核中");
            }
            #endregion

            #region 是否存在审核通过
            qeID = QueryExpression.Equal("COMPANYBUDGETAPPLYMASTERID", entity.COMPANYBUDGETAPPLYMASTERID);
            qeID.Operation = QueryExpression.Operations.NotEqual;
            qeBUDGETYEAR = QueryExpression.Equal("BUDGETYEAR", entity.BUDGETYEAR.ToString());
            qeID.RelatedExpression = qeBUDGETYEAR;
            qeCom = QueryExpression.Equal("OWNERDEPARTMENTID", entity.OWNERDEPARTMENTID);
            qeBUDGETYEAR.RelatedExpression = qeCom;
            qeStatesApproving = QueryExpression.Equal(FieldName.CheckStates, ((int)CheckStates.Approved).ToString());
            qeCom.RelatedExpression = qeStatesApproving;
            QueryExpression qeEDITSTATES = QueryExpression.Equal("ISVALID", "0");
            qeStatesApproving.RelatedExpression = qeEDITSTATES;
            result = InnerGetEntities<T_FB_COMPANYBUDGETAPPLYMASTER>(qeID);

            if (result.Count() > 0)
            {
                throw new FBBLLException("该部门年度预算已通过审核");
            }

            #endregion

            #region 是否存在生效
            qeID = QueryExpression.Equal("COMPANYBUDGETAPPLYMASTERID", entity.COMPANYBUDGETAPPLYMASTERID);
            qeID.Operation = QueryExpression.Operations.NotEqual;
            qeBUDGETYEAR = QueryExpression.Equal("BUDGETYEAR", entity.BUDGETYEAR.ToString());
            qeID.RelatedExpression = qeBUDGETYEAR;
            qeCom = QueryExpression.Equal("OWNERDEPARTMENTID", entity.OWNERDEPARTMENTID);
            qeBUDGETYEAR.RelatedExpression = qeCom;
            //qeStatesApproving = QueryExpression.Equal(FieldName.CheckStates, ((int)CheckStates.Approved).ToString());
            //qeCom.RelatedExpression = qeStatesApproving;
            qeEDITSTATES = QueryExpression.Equal("ISVALID", "1");
            qeCom.RelatedExpression = qeEDITSTATES;
            result = InnerGetEntities<T_FB_COMPANYBUDGETAPPLYMASTER>(qeID);
            if (result.Count() > 0)
            {
                throw new FBBLLException("该部门年度预算已生效");
            }

            #endregion

            #region 月度汇总
            QueryExpression qeCompany = QueryExpression.Equal(FieldName.OwnerCompanyID, entity.OWNERCOMPANYID);
            qeBUDGETYEAR = QueryExpression.Equal("BUDGETYEAR", entity.BUDGETYEAR.ToString());
            QueryExpression qeStatesApproved = QueryExpression.Equal(FieldName.CheckStates, ((int)CheckStates.Approved).ToString());
            qeStatesApproving = QueryExpression.Equal(FieldName.CheckStates, ((int)CheckStates.Approving).ToString());
            qeStatesApproved.RelatedExpression = qeStatesApproving;
            qeStatesApproved.RelatedType = QueryExpression.RelationType.Or;
            qeBUDGETYEAR.RelatedExpression = qeStatesApproved;
            qeCompany.RelatedExpression = qeBUDGETYEAR;

            var result2 = InnerGetEntities<T_FB_COMPANYBUDGETSUMMASTER>(qeCompany);
            if (result2.Count() > 0)
            {
                throw new FBBLLException("该年度已做过年度汇总或正在审核中");
            }
            #endregion

            return SaveFBEntityDefault(fbEntity);
        }

        public FBEntity SaveT_FB_COMPANYBUDGETMODMASTER(FBEntity fbEntity)
        {

            //T_FB_COMPANYBUDGETMODMASTER entity = fbEntity.Entity as T_FB_COMPANYBUDGETMODMASTER;
            //entity.BUDGETYEAR = DateTime.Now.Year;
            //QueryExpression qeCom = QueryExpression.Equal(FieldName.OwnerCompanyID, entity.OWNERCOMPANYID);
            //QueryExpression qe = QueryExpression.Equal("BUDGETYEAR", entity.BUDGETYEAR.ToString());
            //QueryExpression qeStatesApproved = QueryExpression.Equal(FieldName.CheckStates, ((int)CheckStates.Approved).ToString());
            //QueryExpression qeEDITSTATES = QueryExpression.Equal("ISVALID", "1");
            //qeCom.RelatedExpression = qe;
            //qe.RelatedExpression = qeStatesApproved;
            //qeStatesApproved.RelatedExpression = qeEDITSTATES;
            //var result = InnerGetEntities<T_FB_COMPANYBUDGETAPPLYMASTER>(qeCom);
            //if (result.Count() == 0)
            //{
            //    throw new FBBLLException("该公司尚未做过年度预算");
            //}
            return SaveFBEntityDefault(fbEntity);
        }

        public FBEntity SaveT_FB_SYSTEMSETTINGS(FBEntity fbEntity)
        {
            try
            {


                bool isUpdateCheckDate = true;
                DateTime checkDateNew = Convert.ToDateTime(fbEntity.Entity.GetValue("CHECKDATE"));

                if (fbEntity.FBEntityState == FBEntityState.Modified)
                {
                    T_FB_SYSTEMSETTINGS setting = GetEntity(fbEntity.Entity.EntityKey) as T_FB_SYSTEMSETTINGS;
                    DateTime checkDateOld = setting.CHECKDATE.Value;

                    isUpdateCheckDate = !checkDateNew.Equals(checkDateOld);
                }

                if (isUpdateCheckDate)
                {
                    bool isSuccessful = EngineX.ConfigCheckDate(checkDateNew);
                    if (!isSuccessful)
                    {
                        throw new FBBLLException("设置自动预算结算失败。");
                    }
                }
                FBEntity result = this.SaveFBEntityDefault(fbEntity);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public FBEntity SaveT_FB_EXTENSIONALORDER(FBEntity fbEntity)
        {
            ExtensionOrderBLL bll = new ExtensionOrderBLL();
            return bll.SaveT_FB_EXTENSIONALORDER(fbEntity);
        }

        public FBEntity SaveT_FB_PERSONMONEYASSIGNMASTER(FBEntity fbEntity)
        {
            T_FB_SYSTEMSETTINGS setting = SystemBLL.GetSetting(null);
            var qe = QueryExpression.Equal("SUBJECTID", setting.MONEYASSIGNSUBJECTID);
            qe.IsNoTracking = true;

            var subject = qe.Query<T_FB_SUBJECT>(this).FirstOrDefault();
            if (subject == null)
            {
                throw new Exception("未找到对应的活动经费，保存失败！");
            }

            T_FB_PERSONMONEYASSIGNMASTER master = fbEntity.Entity as T_FB_PERSONMONEYASSIGNMASTER;
            if (fbEntity.FBEntityState == FBEntityState.Added)
            {
                //T_FB_SYSTEMSETTINGS setting = SystemBLL.GetSetting(null);
                //var subject = QueryExpression.Equal("SUBJECTID", setting.MONEYASSIGNSUBJECTID).Query<T_FB_SUBJECT>().FirstOrDefault();

                master.T_FB_PERSONMONEYASSIGNDETAIL.ToList().ForEach(item =>
                {
                    item.T_FB_SUBJECT = subject;

                });
            }



            var listDetail = fbEntity.GetRelationFBEntities(typeof(T_FB_PERSONMONEYASSIGNDETAIL).Name);
            listDetail.ForEach(item =>
            {
                T_FB_PERSONMONEYASSIGNDETAIL entdetail = item.Entity as T_FB_PERSONMONEYASSIGNDETAIL;
                if (item.FBEntityState == FBEntityState.Added && subject != null)
                {
                    entdetail.T_FB_SUBJECT = subject;
                }
            });
            return this.SaveFBEntityDefault(fbEntity);

        }
        #endregion
    }
}
