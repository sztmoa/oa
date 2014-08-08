using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using SMT.FileUpLoad.Service;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace SMT.FileUpLoad.Host
{
    /// <summary>
    /// Download 的摘要说明
    /// </summary>
    public class Download : IHttpHandler
    {
        private long ChunkSize = 102400;//100K 每次读取文件，只读取100Ｋ，这样可以缓解服务器的压力

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            SMT.Foundation.Log.Tracer.Debug("开始下载文件");
            try
            {
                //\SMT\OA\考勤管理\2010121702324784503941.JPG
                //string fileName = "123.jpg";//客户端保存的文件名
                if (context.Request.QueryString["filename"] == null) return;
                String fileurl = HttpUtility.UrlDecode(context.Request.QueryString["filename"]);
                SMT.Foundation.Log.Tracer.Debug("下载文件地址："+ fileurl);
                // string filePath = string.Format(SavePath, fileName.Split('|')[0], fileName.Split('|')[1],  fileName.Split('|')[2]+"\\"+fileName.Split('|')[3]); //context.Server.MapPath("Bubble.jpg");
                string filePath = string.Format(FileConfig.GetCompanyItem(fileurl.Split('\\')[1]).SavePath, fileurl.Split('\\')[1], fileurl.Split('\\')[2], fileurl.Split('\\')[3] + "\\" + fileurl.Split('\\')[4]);
                string NewfilePath = string.Format(FileConfig.GetCompanyItem(fileurl.Split('\\')[1]).SavePath, fileurl.Split('\\')[1], fileurl.Split('\\')[2], fileurl.Split('\\')[3] + "\\" + fileurl.Split('\\')[5]);
                string saveFileName = fileurl.Split('\\')[5];//保存文件名
                string StrServicesFileName = fileurl.Split('\\')[4].Substring(0, 8);//取前面的作为解密字符串
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);
                if (fileInfo.Exists == true)
                {
                    //byte[] buffer = new byte[ChunkSize];
                    //context.Response.Clear();
                    //string Key = this.key;
                    string Key = StrServicesFileName;

                    //DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                    //des.Key = ASCIIEncoding.Default.GetBytes(Key);
                    //des.IV = ASCIIEncoding.Default.GetBytes(Key);

                    ////Create a file stream to read the encrypted file back.
                    //FileStream fsread = new FileStream(filePath, FileMode.Open,FileAccess.Read);
                    ////Create a DES decryptor from the DES instance.
                    //ICryptoTransform desdecrypt = des.CreateDecryptor();


                    //byte[] FileStreamArray = new byte[fsread.Length];
                    //fsread.Read(FileStreamArray, 0, FileStreamArray.Length);

                    //MemoryStream ms = new MemoryStream();
                    //CryptoStream FileCryptoStream = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read);
                    //FileCryptoStream.Write(FileStreamArray, 0, FileStreamArray.Length);
                    //FileCryptoStream.FlushFinalBlock();
                    //FileStream filOutStream = new FileStream(NewfilePath, FileMode.OpenOrCreate, FileAccess.Write);
                    ////InputFileStream = File.OpenWrite(FileName);
                    //foreach (byte b in ms.ToArray())
                    //{
                    //    filOutStream.WriteByte(b);
                    //}
                    //fsread.Close();
                    //ms.Close();
                    //filOutStream.Close();
                    //filOutStream.Dispose();
                    CryptoHelp.DecryptFile(filePath, NewfilePath, StrServicesFileName);
                    //System.IO.FileStream iStream = System.IO.File.OpenRead(NewfilePath);
                    long dataToRead;
                    int length;
                    byte[] buffer = new Byte[300000];
                    System.IO.FileStream iStream = new System.IO.FileStream(NewfilePath, System.IO.FileMode.Open,
                    System.IO.FileAccess.Read, System.IO.FileShare.Read);

                    //   Total   bytes   to   read:   
                    dataToRead = iStream.Length;

                    context.Response.ContentType = "application/octet-stream";
                    context.Response.Charset = "UTF-8";
                    context.Response.AddHeader("Content-Disposition", "attachment;   filename=" + System.Web.HttpUtility.UrlEncode(saveFileName, System.Text.Encoding.UTF8));

                    //   Read   the   bytes.   
                    while (dataToRead > 0)
                    {
                        //   Verify   that   the   client   is   connected.   
                        if (context.Response.IsClientConnected)
                        {
                            //   Read   the   data   in   buffer.   
                            length = iStream.Read(buffer, 0, 300000);

                            //   Write   the   data   to   the   current   output   stream.   
                            context.Response.OutputStream.Write(buffer, 0, length);

                            //   Flush   the   data   to   the   HTML   output.   
                            context.Response.Flush();

                            buffer = new Byte[300000];
                            dataToRead = dataToRead - length;
                        }
                        else
                        {
                            //prevent   infinite   loop   if   user   disconnects   
                            dataToRead = -1;
                        }

                    }
                    if (iStream != null)
                    {
                        //Close   the   file.   
                        iStream.Close();
                    }
                    context.Response.Clear();

                    context.Response.End();
                    //long dataLengthToRead = iStream.Length;//获得下载文件的总大小

                    //context.Response.ContentType = "application/octet-stream";
                    ////通知浏览器下载文件而不是打开
                    //context.Response.AddHeader("Content-Disposition", "attachment;  filename=" + HttpUtility.UrlEncode(saveFileName, System.Text.Encoding.UTF8));
                    //while (dataLengthToRead > 0 && context.Response.IsClientConnected)
                    //{
                    //    int lengthRead = iStream.Read(buffer, 0, Convert.ToInt32(ChunkSize));//读取的大小
                    //    context.Response.OutputStream.Write(buffer, 0, lengthRead);
                    //    //context.Response.Flush();
                    //    dataLengthToRead = dataLengthToRead - lengthRead;
                    //}
                    //context.Response.Flush();
                    //iStream.Dispose();
                    //iStream.Close();
                    ////context.Response.Close();
                    //context.Response.Clear();
                    ////File.Delete(NewfilePath);
                    //context.Response.End();

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
                SMT.Foundation.Log.Tracer.Debug("下载文件出错"+ex.ToString());
            }
        }

       

        //public void ProcessRequest(HttpContext context)
        //{
        //    //\SMT\OA\考勤管理\2010121702324784503941.JPG
        //    //string fileName = "123.jpg";//客户端保存的文件名
        //    if (context.Request.QueryString["filename"] == null) return;
        //    String fileurl = HttpUtility.UrlDecode(context.Request.QueryString["filename"]);
        //    // string filePath = string.Format(SavePath, fileName.Split('|')[0], fileName.Split('|')[1],  fileName.Split('|')[2]+"\\"+fileName.Split('|')[3]); //context.Server.MapPath("Bubble.jpg");
        //    string filePath = string.Format(FileConfig.GetCompanyItem(fileurl.Split('\\')[1]).SavePath, fileurl.Split('\\')[1], fileurl.Split('\\')[2], fileurl.Split('\\')[3] + "\\" + fileurl.Split('\\')[4]);

        //    string saveFileName = fileurl.Split('\\')[5];//保存文件名
        //    string NewfilePath = string.Format(FileConfig.GetCompanyItem(fileurl.Split('\\')[1]).SavePath, fileurl.Split('\\')[1], fileurl.Split('\\')[2], fileurl.Split('\\')[3] + "\\" + fileurl.Split('\\')[5]);
        //    System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);


        //    if (fileInfo.Exists == true)
        //    {
        //        byte[] buffer = new byte[ChunkSize];
        //        context.Response.Clear();
        //        System.IO.FileStream iStream = System.IO.File.OpenRead(filePath);
        //        //FileStream iStream = new FileStream(NewfilePath, FileMode.Open, FileAccess.Read);

        //        //Decrypt(filePath, iStream);
        //        long dataLengthToRead = iStream.Length;//获得下载文件的总大小
        //        context.Response.ContentType = "application/octet-stream";
        //        //通知浏览器下载文件而不是打开
        //        string Key = this.key;
        //        DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
        //        //A 64 bit key and IV is required for this provider.
        //        //Set secret key For DES algorithm.
        //        DES.Key = ASCIIEncoding.ASCII.GetBytes(Key);
        //        //Set initialization vector.
        //        DES.IV = ASCIIEncoding.ASCII.GetBytes(Key);

        //        //Create a file stream to read the encrypted file back.
        //        FileStream fsread = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        //        //Create a DES decryptor from the DES instance.
        //        ICryptoTransform desdecrypt = DES.CreateDecryptor();
        //        //Create crypto stream set to read and do a 
        //        //DES decryption transform on incoming bytes.
        //        CryptoStream cryptostreamDecr = new CryptoStream(fsread, desdecrypt, CryptoStreamMode.Read);
        //        //Print the contents of the decrypted file.
        //        StreamWriter fsDecrypted = new StreamWriter(NewfilePath);
        //        fsDecrypted.Write(new StreamReader(cryptostreamDecr).ReadToEnd());
        //        fsDecrypted.Flush();
        //        fsDecrypted.Close();
        //        fsDecrypted.Dispose();


        //        System.IO.FileStream iStream2 = System.IO.File.OpenRead(NewfilePath);
        //        long dataLengthToRead2 = iStream2.Length;//获得下载文件的总大小
        //        context.Response.ContentType = "application/octet-stream";
        //        //通知浏览器下载文件而不是打开
        //        context.Response.AddHeader("Content-Disposition", "attachment;  filename=" + HttpUtility.UrlEncode(saveFileName, System.Text.Encoding.UTF8));
        //        while (dataLengthToRead2 > 0 && context.Response.IsClientConnected)
        //        {
        //            int lengthRead = iStream2.Read(buffer, 0, Convert.ToInt32(ChunkSize));//读取的大小
        //            context.Response.OutputStream.Write(buffer, 0, lengthRead);
        //            context.Response.Flush();
        //            dataLengthToRead = dataLengthToRead2 - lengthRead;
        //        }
        //        iStream.Dispose();
        //        iStream.Close();
        //        context.Response.Close();
        //        context.Response.End();
        //        File.Delete(NewfilePath);
        //        //string Key = this.key;
        //        //DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        //        //des.Key = ASCIIEncoding.Default.GetBytes(Key);
        //        //des.IV = ASCIIEncoding.Default.GetBytes(Key);
        //        //FileStream InputFileStream = File.OpenRead(FileName);
        //        //byte[] FileStreamArray = new byte[iStream.Length];
        //        //iStream.Read(FileStreamArray, 0, FileStreamArray.Length);
        //        //iStream.Close();
        //        //MemoryStream ms = new MemoryStream();
        //        //CryptoStream FileCryptoStream = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
        //        //FileCryptoStream.Write(FileStreamArray, 0, FileStreamArray.Length);
        //        //FileCryptoStream.FlushFinalBlock();


        //        //filOutStream.SetLength(0);
        //        //byte[] bin = new byte[100];
        //        //long rdlen = 0;
        //        //long totlen = iStream.Length;
        //        //int len;
        //        ////DES des = new DESCryptoServiceProvider();
        //        ////CryptoStream encStream = new CryptoStream(filOutStream, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
        //        //while (rdlen < totlen)
        //        //{
        //        //    len = iStream.Read(buffer, 0, 102400);
        //        //    filOutStream.Write(buffer, 0, len);
        //        //    rdlen += len;
        //        //}

        //        //iStream = File.OpenWrite(filePath);
        //        //foreach (byte b in ms.ToArray())
        //        //{
        //        //    iStream.WriteByte(b);
        //        //}
        //        //ms.Close();
        //        //InputFileStream.Close();
        //        //FileCryptoStream.Close();
        //        //dataLengthToRead = iStream.Length;
        //        //context.Response.AddHeader("Content-Disposition", "attachment;  filename=" + HttpUtility.UrlEncode(saveFileName, System.Text.Encoding.UTF8));
        //        //while (dataLengthToRead > 0 && context.Response.IsClientConnected)
        //        //{
        //        //    //int lengthRead = 0;
        //        //    //if (FileStreamArray.Length > 102400)
        //        //    //{
        //        //    //    lengthRead = 102400;
        //        //    //}
        //        //    //else
        //        //    //{
        //        //    //    lengthRead = FileStreamArray.Length;
        //        //    //}
        //        //    int lengthRead = fsDecrypted.Read(buffer, 0, Convert.ToInt32(ChunkSize));//读取的大小
        //        //    context.Response.OutputStream.Write(buffer, 0, FileStreamArray.Length);
        //        //    context.Response.Flush();
        //        //    dataLengthToRead = dataLengthToRead - lengthRead;
        //        //}
        //        //ms.Close();
        //        //InputFileStream.Close();
        //        //FileCryptoStream.Close();
        //        //filOutStream.Close();
        //        //filOutStream.Dispose();
        //        //iStream.Dispose();
        //        //iStream.Close();
        //        //context.Response.Close();
        //        //context.Response.End();


        //    }
        //    else
        //    {
        //        System.Web.HttpContext.Current.Response.Write("<script>alert('该文件不存在！');</script>");
        //    }
        //    //context.Response.ContentType = "text/plain";
        //    //context.Response.Write("Hello World");
        //}


        //public void ProcessRequest(HttpContext context)
        //{
        //    //\SMT\OA\考勤管理\2010121702324784503941.JPG
        //    //string fileName = "123.jpg";//客户端保存的文件名
        //    if (context.Request.QueryString["filename"]==null) return;
        //    String fileurl = HttpUtility.UrlDecode(context.Request.QueryString["filename"]);
        //   // string filePath = string.Format(SavePath, fileName.Split('|')[0], fileName.Split('|')[1],  fileName.Split('|')[2]+"\\"+fileName.Split('|')[3]); //context.Server.MapPath("Bubble.jpg");
        //    string filePath = string.Format(FileConfig.GetCompanyItem(fileurl.Split('\\')[1]).SavePath, fileurl.Split('\\')[1], fileurl.Split('\\')[2], fileurl.Split('\\')[3] + "\\" + fileurl.Split('\\')[4]);

        //    string saveFileName = fileurl.Split('\\')[5];//保存文件名
        //    string NewfilePath = string.Format(FileConfig.GetCompanyItem(fileurl.Split('\\')[1]).SavePath, fileurl.Split('\\')[1], fileurl.Split('\\')[2], fileurl.Split('\\')[3] + "\\" + fileurl.Split('\\')[5]);
        //    System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);
        //    if (fileInfo.Exists == true)
        //    {
        //        byte[] buffer = new byte[ChunkSize];
        //        context.Response.Clear();
        //        System.IO.FileStream iStream = System.IO.File.OpenRead(filePath);
        //        //FileStream filOutStream = new FileStream(NewfilePath, FileMode.OpenOrCreate, FileAccess.Write);

        //        //Decrypt(filePath, iStream);
        //        long dataLengthToRead = iStream.Length;//获得下载文件的总大小
        //        context.Response.ContentType = "application/octet-stream";
        //        //通知浏览器下载文件而不是打开
        //        string Key = this.key;
        //        DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
        //        //A 64 bit key and IV is required for this provider.
        //        //Set secret key For DES algorithm.
        //        DES.Key = ASCIIEncoding.ASCII.GetBytes(Key);
        //        //Set initialization vector.
        //        DES.IV = ASCIIEncoding.ASCII.GetBytes(Key);

        //        //Create a file stream to read the encrypted file back.
        //        FileStream fsread = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        //        //Create a DES decryptor from the DES instance.
        //        ICryptoTransform desdecrypt = DES.CreateDecryptor();
        //        //Create crypto stream set to read and do a 
        //        //DES decryption transform on incoming bytes.
        //        CryptoStream cryptostreamDecr = new CryptoStream(fsread,
        //           desdecrypt,
        //           CryptoStreamMode.Read);
        //        //Print the contents of the decrypted file.
        //        StreamWriter fsDecrypted = new StreamWriter(NewfilePath);
        //        fsDecrypted.Write(new StreamReader(cryptostreamDecr).ReadToEnd());
        //        fsDecrypted.Flush();

        //        //string Key = this.key;
        //        //DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        //        //des.Key = ASCIIEncoding.Default.GetBytes(Key);
        //        //des.IV = ASCIIEncoding.Default.GetBytes(Key);
        //        //FileStream InputFileStream = File.OpenRead(FileName);
        //        byte[] FileStreamArray = new byte[fsread.Length];
        //        //iStream.Read(FileStreamArray, 0, FileStreamArray.Length);
        //        //iStream.Close();
        //        //MemoryStream ms = new MemoryStream();
        //        //CryptoStream FileCryptoStream = new CryptoStream(filOutStream, des.CreateDecryptor(), CryptoStreamMode.Write);
        //        //FileCryptoStream.Write(FileStreamArray, 0, FileStreamArray.Length);
        //        //FileCryptoStream.FlushFinalBlock();


        //        //filOutStream.SetLength(0);
        //        //byte[] bin = new byte[100];
        //        //long rdlen = 0;
        //        //long totlen = iStream.Length;
        //        //int len;
        //        ////DES des = new DESCryptoServiceProvider();
        //        ////CryptoStream encStream = new CryptoStream(filOutStream, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
        //        //while (rdlen < totlen)
        //        //{
        //        //    len = iStream.Read(buffer, 0, 102400);
        //        //    filOutStream.Write(buffer, 0, len);
        //        //    rdlen += len;
        //        //}

        //        //iStream = File.OpenWrite(filePath);
        //        //foreach (byte b in ms.ToArray())
        //        //{
        //        //    iStream.WriteByte(b);
        //        //}
        //        //ms.Close();
        //        //InputFileStream.Close();
        //        //FileCryptoStream.Close();

        //        context.Response.AddHeader("Content-Disposition", "attachment;  filename=" + HttpUtility.UrlEncode(saveFileName, System.Text.Encoding.UTF8));
        //        while (dataLengthToRead > 0 && context.Response.IsClientConnected)
        //        {
        //            int lengthRead = 0;
        //            if (FileStreamArray.Length > 102400)
        //            {
        //                lengthRead = 102400;
        //            }
        //            else
        //            {
        //                lengthRead = FileStreamArray.Length;
        //            }
        //            //int lengthRead = fsDecrypted.Read(buffer, 0, Convert.ToInt32(ChunkSize));//读取的大小
        //            context.Response.OutputStream.Write(buffer, 0, FileStreamArray.Length);
        //            context.Response.Flush();
        //            dataLengthToRead = dataLengthToRead - lengthRead;
        //        }
        //        //ms.Close();
        //        //InputFileStream.Close();
        //        //FileCryptoStream.Close();
        //        iStream.Dispose();
        //        iStream.Close();
        //        context.Response.Close();
        //        context.Response.End();
        //    }
        //    else
        //    {
        //        System.Web.HttpContext.Current.Response.Write("<script>alert('该文件不存在！');</script>");
        //    }
        //    //context.Response.ContentType = "text/plain";
        //    //context.Response.Write("Hello World");
        //}

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