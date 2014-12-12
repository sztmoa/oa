using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using SMT.FileUpLoad.Service;

namespace SMT.FileUpLoad.Host
{
    /// <summary>
    /// Download 的摘要说明
    /// </summary>
    public class Download : IHttpHandler
    {
        private long ChunkSize = 102400;//100K 每次读取文件，只读取100Ｋ，这样可以缓解服务器的压力
       
        public void ProcessRequest(HttpContext context)
        {
            //\SMT\OA\考勤管理\2010121702324784503941.JPG
            //string fileName = "123.jpg";//客户端保存的文件名
            if (context.Request.QueryString["filename"]==null) return;
            String fileurl = HttpUtility.UrlDecode(context.Request.QueryString["filename"]);
           // string filePath = string.Format(SavePath, fileName.Split('|')[0], fileName.Split('|')[1],  fileName.Split('|')[2]+"\\"+fileName.Split('|')[3]); //context.Server.MapPath("Bubble.jpg");
            string filePath = string.Format(FileConfig.GetCompanyItem(fileurl.Split('\\')[1]).SavePath, fileurl.Split('\\')[1], fileurl.Split('\\')[2], fileurl.Split('\\')[3] + "\\" + fileurl.Split('\\')[4]);

            string saveFileName = fileurl.Split('\\')[5];//保存文件名
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);
            if (fileInfo.Exists == true)
            {
                byte[] buffer = new byte[ChunkSize];
                context.Response.Clear();
                System.IO.FileStream iStream = System.IO.File.OpenRead(filePath);
                long dataLengthToRead = iStream.Length;//获得下载文件的总大小
                context.Response.ContentType = "application/octet-stream";
                //通知浏览器下载文件而不是打开
                context.Response.AddHeader("Content-Disposition", "attachment;  filename=" + HttpUtility.UrlEncode(saveFileName, System.Text.Encoding.UTF8));
                while (dataLengthToRead > 0 && context.Response.IsClientConnected)
                {
                    int lengthRead = iStream.Read(buffer, 0, Convert.ToInt32(ChunkSize));//读取的大小
                    context.Response.OutputStream.Write(buffer, 0, lengthRead);
                    context.Response.Flush();
                    dataLengthToRead = dataLengthToRead - lengthRead;
                }
                iStream.Dispose();
                iStream.Close();
                context.Response.Close();
                context.Response.End();
            }
            else
            {
                System.Web.HttpContext.Current.Response.Write("<script>alert('该文件不存在！');</script>");
            }
            //context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
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