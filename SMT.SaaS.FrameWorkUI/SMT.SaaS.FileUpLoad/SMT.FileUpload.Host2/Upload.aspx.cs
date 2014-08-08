using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SMT.FileUpLoad.Service;
using System.Configuration;
using System.IO;

namespace SMT.FileUpload.Host
{
    public partial class Upload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //UploadService bb = new UploadService();
            //bb.CreateCompanyDirectory("703dfb3c-d3dc-4b1d-9bf0-3507ba01b716", "集团本部", "SMT");  
            SMT.Foundation.Log.Tracer.Debug("开始上传文件时间:"+System.DateTime.Now.ToString());
            try
            {
                string NewPath = "";
                HttpPostedFile fileData = Request.Files["Filedata"];
                SMT.Foundation.Log.Tracer.Debug("开始上传文件的大小：" + fileData.ContentLength.ToString());
                SMT.Foundation.Log.Tracer.Debug("开始上传文件：" + fileData.FileName);
                string filePath = "";
                string companyCode = "";//公司代码
                string systemCode = "";//系统代码
                string modelCode = "";//模块代码
                string encryptName = "";//加密字符串
                string strFileName = "";
                strFileName = fileData.FileName;
                UserFile model = null;

                string DownloadUrl = string.Concat(ConfigurationManager.AppSettings["DownLoadUrl"]);
                //获取用户身份，由用户名,密码组成
                string StrAuthUser = string.Concat(ConfigurationManager.AppSettings["FileUploadUser"]);
                //if (Request["path"] != null)
                //{
                //    filePath = Request["path"].ToString();
                //    SMT.Foundation.Log.Tracer.Debug("上传文件完成,路径：" + filePath);
                //}
                if (Request["companyCode"] != null)//公司代码
                {
                    companyCode = Request["companyCode"].ToString();
                    model = FileConfig.GetCompanyItem(companyCode);
                    SMT.Foundation.Log.Tracer.Debug("aspx页面上传文件companyCode：" + companyCode);
                }
                if (Request["systemCode"] != null)//系统代码
                {
                    systemCode = Request["systemCode"].ToString();
                    SMT.Foundation.Log.Tracer.Debug("aspx页面上传文件systemcode：" + systemCode);
                }
                if (Request["modelCode"] != null)//模块代码
                {
                    modelCode = Request["modelCode"].ToString();
                    SMT.Foundation.Log.Tracer.Debug("aspx页面上传文件modlecode：" + modelCode);
                }
                if (Request["encryptName"] != null)//加密文件名
                {
                    encryptName = Request["encryptName"].ToString();
                    SMT.Foundation.Log.Tracer.Debug("aspx页面上传文件encryptName：" + encryptName);
                }
                if (fileData != null)
                {
                    //string path = ctx.Server.MapPath(ctx.Request["file"].ToString());
                    string strPath = string.Format(model.SavePath, companyCode, systemCode, modelCode);
                    if (!System.IO.Directory.Exists(strPath))
                    {
                        if (strPath.IndexOf("\\\\") == 0)
                        {
                            string FirstName = "";
                            string UserName = "";
                            string StrPwd = "";
                            string ComputerName = "";
                            if (StrAuthUser.IndexOf(",") > 0)
                            {
                                FirstName = StrAuthUser.Split(',')[0];
                                ComputerName = FirstName.Split('/')[0];
                                UserName = FirstName.Split('/')[1];
                                StrPwd = StrAuthUser.Split(',')[1];
                            }
                            LogonImpersonate imper = new LogonImpersonate(UserName, StrPwd);
                            string path = @"Z:\" + companyCode + @"\" + systemCode + @"\" + modelCode;
                            string OldPath = model.SavePath.Split('{')[0].TrimEnd('\\');
                            if (CreateDirectory(path, UserName, StrPwd, OldPath))
                            {
                                SMT.Foundation.Log.Tracer.Debug("aspx页面创建了驱动器映射");
                            }
                            else
                            {
                                SMT.Foundation.Log.Tracer.Debug("aspx页面有创建映射问题");
                            }

                        }
                        else
                        {
                            System.IO.Directory.CreateDirectory(strPath);
                        }
                    }
                    NewPath = Path.Combine(strPath, encryptName); //还未上传完成的路径
                    string OldFilePath = Path.Combine(strPath, strFileName); //原来上传的文件
                    string PassWordKey = "";//加密字符串
                    if (encryptName.Length > 8)
                    {
                        PassWordKey = encryptName.Substring(0, 8);
                    }
                    else
                    {
                        PassWordKey = "12345678";
                    }
                    int fileLength = fileData.ContentLength;
                    byte[] data = new Byte[fileLength];
                    Stream fileStream = fileData.InputStream;
                    fileStream.Read(data, 0, fileLength);

                    SMT.Foundation.Log.Tracer.Debug("aspx页面上传文件开始,路径NewPath：" + NewPath);
                    using (FileStream fs = File.Create(NewPath))
                    {
                        fs.Write(data, 0, data.Length);
                        fs.Close();
                        fs.Dispose();
                    }
                    //创建上传过来的文件
                    SMT.Foundation.Log.Tracer.Debug("aspx页面上传文件开始,路径OldFilePath：" + OldFilePath);
                    using (FileStream fsOld = File.Create(OldFilePath))
                    {
                        fsOld.Write(data, 0, data.Length);
                        fsOld.Close();
                        fsOld.Dispose();                        
                    }

                    string strTempName = Path.Combine(strPath, encryptName);//已经上传完成的路径
                    SMT.Foundation.Log.Tracer.Debug("aspx页面上传文件开始,路径strTempName：" + strTempName);
                    File.Move(NewPath, strTempName);
                    NewPath = strTempName;//最近返回已完成的路径                 
                    string imageType = ".JPEG,.JPG,.GIF,.BMP";
                    //判断上传的模块的类型，如果是新闻则生成缩略图
                    #region 生成缩略图

                    string FileType = "";
                    if (imageType.IndexOf(FileType) > 0 || imageType.ToLower().IndexOf(FileType) > 0)
                    {
                        string thumbPath = string.Format(model.SavePath, companyCode, systemCode, modelCode) + "\\thumb\\";
                        SMT.Foundation.Log.Tracer.Debug("aspx页面上传图片文件,路径thumbPath：" + thumbPath);
                        if (!System.IO.Directory.Exists(thumbPath))
                        {
                            SMT.Foundation.Log.Tracer.Debug("aspx页面上传图片文件,不存在路径thumbPath：" + thumbPath);
                            if (strPath.IndexOf("\\\\") == 0)
                            {
                                string FirstName = "";
                                string UserName = "";
                                string StrPwd = "";
                                string ComputerName = "";
                                if (StrAuthUser.IndexOf(",") > 0)
                                {
                                    FirstName = StrAuthUser.Split(',')[0];
                                    ComputerName = FirstName.Split('/')[0];
                                    UserName = FirstName.Split('/')[1];
                                    StrPwd = StrAuthUser.Split(',')[1];
                                }
                                LogonImpersonate imper = new LogonImpersonate(UserName, StrPwd);
                                string path = @"Z:\" + companyCode + @"\" + systemCode + @"\" + modelCode;
                                string OldPath = model.SavePath.Split('{')[0].TrimEnd('\\');
                                if (CreateDirectory(path, UserName, StrPwd, OldPath))
                                {
                                    SMT.Foundation.Log.Tracer.Debug("创建了驱动器映射");
                                }
                                else
                                {
                                    SMT.Foundation.Log.Tracer.Debug("aspx页面图片上传有问题");
                                }

                            }
                            else
                            {
                                SMT.Foundation.Log.Tracer.Debug("aspx页面上传图片文件,开始创建路径thumbPath：" + thumbPath);
                                System.IO.Directory.CreateDirectory(thumbPath);
                            }
                        }
                        string thumbFile = thumbPath + encryptName;
                        string strType = "JPG";
                        strType = FileType;
                        MakeImageThumb.MakeThumbnail(OldFilePath, thumbFile, 250, 200, "DB", strType);
                        //保存到数据库的路径
                        string strSaveThumb = string.Format("\\{0}\\{1}\\{2}", companyCode, systemCode, modelCode + "\\thumb\\" + encryptName);
                        //entity.THUMBNAILURL = string.Format("\\{0}\\{1}\\{2}", model.CompanyCode, model.SystemCode, model.ModelCode + "\\" + strMd5Name);
                        //entity.THUMBNAILURL = strSaveThumb;
                    }
                    #endregion
                    CryptoHelp.EncryptFileForMVC(OldFilePath, NewPath, PassWordKey);
                    //fileStream.Close();
                    //fileStream.Dispose();
                    SMT.Foundation.Log.Tracer.Debug("aspx页面上传文件完成,路径：" + NewPath + " tempName:" + strTempName);
                }
            }
            catch (Exception ex)
            {
                SMT.Foundation.Log.Tracer.Debug("上传文件upload.aspx页面出现错误：" + ex.ToString());
            }
        }


        /// <summary>
        /// 创建映射驱动器路径，默认创建的映射驱动器为Z:
        /// </summary>
        /// <param name="path">路径名</param>
        /// <param name="UserName">用户名</param>
        /// <param name="Pwd">用户密码</param>
        /// <param name="WNetPath">虚拟路径</param>
        /// <returns></returns>
        private bool CreateDirectory(string path, string UserName, string Pwd, string WNetPath)
        {
            uint state = 0;
            bool IsReturn = false;
            try
            {
                if (!Directory.Exists("Z:"))
                {
                    //state = WNetHelper.WNetAddConnection("user1", "123456", @"\\172.16.1.57\fileupload", "Z:");  
                    state = WNetHelper.WNetAddConnection(UserName, Pwd, WNetPath, "Z:");
                }
                if (state.Equals(0))
                {
                    Directory.CreateDirectory(path);
                    IsReturn = true;
                }
                else
                {
                    SMT.Foundation.Log.Tracer.Debug("添加网络驱动器错误，错误号：" + state.ToString());
                    throw new Exception("添加网络驱动器错误，错误号：" + state.ToString());
                }
            }
            catch (Exception ex)
            {
                SMT.Foundation.Log.Tracer.Debug("添加网络驱动器错误，错误号：" + ex.ToString());

            }
            return IsReturn;
        }

    }
}