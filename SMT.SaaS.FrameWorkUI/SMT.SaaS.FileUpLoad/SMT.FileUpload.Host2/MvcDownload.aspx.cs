using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using SMT.FileUpLoad.Service;

namespace SMT.FileUpload.Host
{
    public partial class MvcDownload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SMT.Foundation.Log.Tracer.Debug("开始下载文件");
            try
            {
                //\SMT\OA\考勤管理\2010121702324784503941.JPG
                //string fileName = "123.jpg";//客户端保存的文件名
                bool IsThum = false;//是否下载缩略图、不经过解密处理开关
                if (Request["filename"] == null) return;
                if (Request["flag"] != null)
                {
                    IsThum = true;
                }
                String fileurl = HttpUtility.UrlDecode(Request["filename"]);
                SMT.Foundation.Log.Tracer.Debug("下载文件地址：" + fileurl);
                string[] arrFilePath = fileurl.Split('\\');
                // string filePath = string.Format(SavePath, fileName.Split('|')[0], fileName.Split('|')[1],  fileName.Split('|')[2]+"\\"+fileName.Split('|')[3]); //context.Server.MapPath("Bubble.jpg");
                string filePath = "";
                if (arrFilePath.Length == 6)
                {
                    filePath = string.Format(FileConfig.GetCompanyItem(arrFilePath[1]).SavePath, arrFilePath[1], arrFilePath[2], arrFilePath[3] + "\\" + arrFilePath[4]);
                }
                else
                {
                    filePath = string.Format(FileConfig.GetCompanyItem(arrFilePath[1]).SavePath, arrFilePath[1], arrFilePath[2], arrFilePath[3] + "\\" + arrFilePath[4]) + "\\" + arrFilePath[arrFilePath.Length - 2];
                }
                string NewfilePath = "";
                if (!IsThum)
                {
                    NewfilePath = string.Format(FileConfig.GetCompanyItem(arrFilePath[1]).SavePath, arrFilePath[1], arrFilePath[2], arrFilePath[3] + "\\" + arrFilePath[5]);
                }
                else
                {
                    //缩略图没有进行加密操作所以还是显示原图
                    NewfilePath = string.Format(FileConfig.GetCompanyItem(arrFilePath[1]).SavePath, arrFilePath[1], arrFilePath[2], arrFilePath[3] + "\\" + arrFilePath[4]) + "\\" + arrFilePath[arrFilePath.Length - 2];
                }
                SMT.Foundation.Log.Tracer.Debug("下载真实文件地址NewfilePath：" + NewfilePath);
                string saveFileName = arrFilePath[arrFilePath.Length - 1];//获取文件名
                string StrServicesFileName = arrFilePath[arrFilePath.Length - 2].Substring(0, 8);//取前面的作为解密字符串
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);
                if (fileInfo.Exists == true)
                {

                    string Key = StrServicesFileName;
                    if (!IsThum)
                    {
                        CryptoHelp.DecryptFile(filePath, NewfilePath, StrServicesFileName);
                    }

                    Response.Charset = "utf-8";
                    Response.ContentEncoding = System.Text.Encoding.UTF8;
                    //ctx.Response.ContentEncoding

                    Response.AppendHeader("Content-Type", "text/html;charset=utf-8");
                    Response.AppendHeader("Content-Disposition", string.Format("attachment; filename={0}", HttpUtility.UrlEncode(saveFileName, System.Text.Encoding.UTF8)));
                    using (FileStream fs = new FileStream(NewfilePath, FileMode.Open, FileAccess.Read))
                    {
                        SMT.Foundation.Log.Tracer.Debug("下载文件路径NewfilePath：" + NewfilePath);
                        SMT.Foundation.Log.Tracer.Debug("下载文件大小：" + fs.Length);
                        byte[] fileData = new byte[fs.Length];
                        int numBytesToRead = (int)fs.Length;
                        int numBytesRead = 0;
                        while (numBytesToRead > 0)
                        {
                            // Read may return anything from 0 to numBytesToRead.
                            int n = fs.Read(fileData, numBytesRead, numBytesToRead);
                            // Break when the end of the file is reached.
                            if (n == 0)
                                break;
                            numBytesRead += n;
                            numBytesToRead -= n;
                        }
                        fs.Close();
                        SMT.Foundation.Log.Tracer.Debug("下载文件地址fileData：" + fileData.Length);
                        Response.BinaryWrite(fileData);
                    }
                    ////System.IO.FileStream iStream = System.IO.File.OpenRead(NewfilePath);
                    //long dataToRead;
                    //int length;
                    //byte[] buffer = new Byte[300000];
                    //System.IO.FileStream iStream = new System.IO.FileStream(NewfilePath, System.IO.FileMode.Open,
                    //System.IO.FileAccess.Read, System.IO.FileShare.Read);

                    ////   Total   bytes   to   read:   
                    //dataToRead = iStream.Length;

                    //Response.ContentType = "application/octet-stream";

                    //Response.Charset = "UTF-8";
                    //Response.AddHeader("Content-Disposition", "attachment;   filename=" + System.Web.HttpUtility.UrlEncode(saveFileName, System.Text.Encoding.UTF8));
                    //Response.AddHeader("Content-Length", dataToRead.ToString());
                    ////   Read   the   bytes.   
                    //while (dataToRead > 0)
                    //{
                    //    //   Verify   that   the   client   is   connected.   
                    //    if (Response.IsClientConnected)
                    //    {
                    //        //   Read   the   data   in   buffer.   
                    //        length = iStream.Read(buffer, 0, 300000);

                    //        //   Write   the   data   to   the   current   output   stream.   
                    //        Response.OutputStream.Write(buffer, 0, length);

                    //        //   Flush   the   data   to   the   HTML   output.   
                    //        Response.Flush();

                    //        buffer = new Byte[300000];
                    //        dataToRead = dataToRead - length;
                    //    }
                    //    else
                    //    {
                    //        //prevent   infinite   loop   if   user   disconnects   
                    //        dataToRead = -1;
                    //    }

                    //}
                    //if (iStream != null)
                    //{
                    //    //Close   the   file.   
                    //    iStream.Close();
                    //    iStream.Dispose();

                    //}
                    //删除文件
                    if (!IsThum)
                    {
                        File.Delete(NewfilePath);
                    }
                }
                else
                {
                    System.Web.HttpContext.Current.Response.Write("<script>alert('该文件不存在！');</script>");
                }
                //context.Response.ContentType = "text/plain";
                //context.Response.Write("Hello World");
            }
            catch (Exception ex)
            {
                //   Trap   the   error,   if   any.   
                Response.Write("下载出现异常Error   :   " + ex.Message);
                Response.Clear();
                Response.End();
            }
            finally
            {                
                Response.End();
            }
        }





        private string key = "12345678";
        /// <summary>
        /// 将文件解密
        /// </summary>
        /// <param name="FileName"></param>
        public void Decrypt(string FileName, FileStream InputFileStream)
        {
            string Key = this.key;
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Key = ASCIIEncoding.Default.GetBytes(Key);
            des.IV = ASCIIEncoding.Default.GetBytes(Key);
            //FileStream InputFileStream = File.OpenRead(FileName);
            byte[] FileStreamArray = new byte[InputFileStream.Length];
            InputFileStream.Read(FileStreamArray, 0, FileStreamArray.Length);
            InputFileStream.Close();
            MemoryStream ms = new MemoryStream();
            CryptoStream FileCryptoStream = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            FileCryptoStream.Write(FileStreamArray, 0, FileStreamArray.Length);
            FileCryptoStream.FlushFinalBlock();

            InputFileStream = File.OpenWrite(FileName);
            foreach (byte b in ms.ToArray())
            {
                InputFileStream.WriteByte(b);
            }
            ms.Close();
            //InputFileStream.Close();
            FileCryptoStream.Close();
            //InputFileStream.Dispose();

        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}