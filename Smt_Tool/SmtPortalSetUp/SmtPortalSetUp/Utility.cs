using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using System.Xml;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;
using System.Text.RegularExpressions;

namespace SmtPortalSetUp
{
    public static class Utility
    {
        public static Form_Second from;
        public static int progreesValue = 0;
        public static int MaxProgreesValue = 0;

        /// <summary>
        /// 拷贝文件并修改内容再保存文件
        /// </summary>
        /// <param name="srcDir">源文件目录</param>
        /// <param name="tgtDir">目标目录</param>
        /// <param name="repalceContext">替换的键值key=原内容，value=替换的内容</param>
        public static void CopyDirectory(string srcDir, string tgtDir, Dictionary<string, string> repalceContext)
        {
            DirectoryInfo source = new DirectoryInfo(srcDir);
            DirectoryInfo target = new DirectoryInfo(tgtDir);

            if (target.FullName.StartsWith(source.FullName, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new Exception("父目录不能拷贝到子目录！");
            }

            if (!source.Exists)
            {
                return;
            }

            if (!target.Exists)
            {
                target.Create();
            }

            FileInfo[] files = source.GetFiles();

            #region "拷贝文件"
            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    //string fileType=System.IO.Path.GetExtension(files[i].FullName);
                    if (files[i].Name.IndexOf(@".txt") > -1
                        || files[i].Name.IndexOf(@".log") > -1
                        || files[i].Name.IndexOf(@".bak") > -1
                        || files[i].Name.IndexOf("备份") > -1
                        || files[i].Name.IndexOf("复制") > -1
                        || files[i].Name.ToLower().IndexOf("tmp") > -1)
                    {
                        MaxProgreesValue = MaxProgreesValue - 1;
                        from.SetProgressMaxValue(MaxProgreesValue);
                        from.ShowMessage("复制文件被跳过：" + files[i].Name);
                        continue;
                    }

                    if (files[i].Name.IndexOf(@".config") > -1
                        || files[i].Name.IndexOf(@".html") > -1)
                    {
                        FileStream fsr = files[i].OpenRead();
                        ///使用OpenRead()方法打开my.ini文件 
                        byte[] datar = new byte[(int)fsr.Length];
                        ///创建datar数组，保存从my.ini文件读取的内容 
                        fsr.Read(datar, 0, (int)fsr.Length);
                        ///读取my.ini文件的内容，保存到datar数组 
                        string valuer = System.Text.Encoding.UTF8.GetString(datar);
                        fsr.Close();
                        foreach (var q in repalceContext)
                        {
                            valuer = valuer.Replace(q.Key, q.Value);
                        }
                        #region "直接替换，不再单独处理webconfig"
                        //if (files[i].Name.IndexOf(@".config") > -1)
                        //{
                        //    try
                        //    {
                        //        XmlDocument xmlDoc = new XmlDocument();
                        //        xmlDoc.Load(files[i].FullName);
                        //        string xpath = @"/configuration/appSettings/add[@key='ConnectionString']";
                        //        XmlNode addkey = xmlDoc.SelectSingleNode(xpath);
                        //        if (addkey != null)
                        //        {
                        //            if (!string.IsNullOrEmpty(connectString))
                        //            {
                        //                addkey.Attributes["value"].InnerText = connectString;
                        //            }
                        //        }
                        //        xpath = @"/configuration/connectionStrings/add[@key='ConnectiongString']";
                        //        addkey = xmlDoc.SelectSingleNode(xpath);
                        //        if (addkey != null)
                        //        {
                        //            if (!string.IsNullOrEmpty(connectString))
                        //            {
                        //                addkey.Attributes["connectionString"].InnerText = connectString;
                        //            }
                        //        }

                        //        XmlNode root = xmlDoc.SelectSingleNode(@"/configuration/connectionStrings");
                        //        if (root != null)
                        //        {
                        //            if (root.ChildNodes != null)
                        //            {
                        //                string connectStringHead = string.Empty;
                        //                string connectStringFoot = string.Empty;
                        //                foreach (XmlNode q in root.ChildNodes)
                        //                {
                        //                    if (q.Attributes!=null)
                        //                    {
                        //                        //if (q.Attributes["name"].Value.IndexOf("EFModelContext") > -1)
                        //                        //{
                        //                            string oldstr = q.Attributes["connectionString"].Value;
                        //                            int startIndex = oldstr.IndexOf(@"'");
                        //                            int endIndex = oldstr.LastIndexOf(@"'");
                        //                            if (startIndex > -1 && endIndex > -1)
                        //                            {
                        //                                connectStringHead = oldstr.Substring(0, startIndex + 1);
                        //                                connectStringFoot = oldstr.Substring(endIndex, oldstr.Length - endIndex);
                        //                                if (!string.IsNullOrEmpty(connectString))
                        //                                {
                        //                                    q.Attributes["connectionString"].Value = connectStringHead + connectString + connectStringFoot;
                        //                                }
                        //                            }
                        //                            else
                        //                            {
                        //                                if (!string.IsNullOrEmpty(connectString))
                        //                                {
                        //                                    q.Attributes["connectionString"].Value = connectString;
                        //                                }
                        //                            }
                        //                        //}
                        //                    }
                        //                    else
                        //                    {
                        //                        from.ShowMessage(@"/configuration/connectionStrings/连接字符串未修改，文件：" + files[i].FullName);                                            
                        //                    }
                        //                }
                        //            }
                        //        }
                        //        foreach (var q in repalceContext)
                        //        {
                        //            xmlDoc.InnerXml = xmlDoc.InnerXml.Replace(q.Key, q.Value);
                        //        }
                        //        //valuer = xmlDoc.InnerText;
                        //        xmlDoc.Save(target.FullName + @"\" + files[i].Name);
                        //        from.ShowMessage("拷贝修改完成，文件：" + files[i].FullName);
                        //        progreesValue++;
                        //        from.ShowProgress(progreesValue);
                        //        continue;
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        from.ShowMessage("修改文件异常，文件：" + files[i].FullName+ex.ToString());
                        //    }
                        //}
                        #endregion

                        FileStream fsWrite = File.Create(target.FullName + @"\" + files[i].Name);
                        ///设置被写入的内容 
                        byte[] dataw = System.Text.Encoding.UTF8.GetBytes(valuer);
                        ///转换为byte数组，保存为dataw 
                        fsWrite.Write(dataw, 0, dataw.Length);
                        ///将dataw数组中的内容写入到my.ini文件 
                        fsWrite.Close();
                        //from.ShowMessage("拷贝完成，文件：" + files[i].FullName);
                    }
                    else if (files[i].Name == "SMT.SAAS.Loading.xap"
                        || files[i].Name == "LightWatchDemo.First.LightWatcApp.xap")
                    {
                        ZipInputStream ZipReadstream = new ZipInputStream(File.OpenRead(files[i].FullName));
                        ZipEntry theEntry;
                        int startIndex = 0;
                        int theEntryLength = 0;
                        FileStream fstream = File.Create(target.FullName + @"\" + files[i].Name);
                        fstream.Close();
                        using (ZipOutputStream ZipEditStream = new ZipOutputStream(File.OpenWrite(target.FullName + @"\" + files[i].Name)))
                        {
                            while ((theEntry = ZipReadstream.GetNextEntry()) != null)
                            {
                                string fileName = theEntry.Name;
                                theEntryLength = (int)theEntry.Size;
                                if (fileName == "ServiceReferences.ClientConfig")
                                {
                                    byte[] filebytes = new byte[theEntry.Size];
                                    ZipReadstream.Read(filebytes, 0, (int)theEntry.Size);

                                    string OldString = System.Text.Encoding.UTF8.GetString(filebytes);
                                    foreach (var q in repalceContext)
                                    {
                                        OldString = OldString.Replace(q.Key, q.Value);
                                    }

                                    //Zipstream.Close();
                                    ///设置被写入的内容 

                                    byte[] bufferBody = System.Text.Encoding.UTF8.GetBytes(OldString);


                                    ZipEntry ZipEntry = new ZipEntry(fileName);
                                    ZipEntry.DateTime = DateTime.Now;
                                    ZipEntry.Size = bufferBody.Length;

                                    ZipEditStream.PutNextEntry(ZipEntry);
                                    ZipEditStream.Write(bufferBody, 0, bufferBody.Length);

                                }
                                else
                                {
                                    startIndex = (int)(startIndex + theEntry.Size);

                                    ZipEntry ZipEntry = new ZipEntry(fileName);
                                    ZipEntry.DateTime = DateTime.Now;

                                    byte[] bufferBody = new byte[theEntry.Size];
                                    ZipReadstream.Read(bufferBody, 0, (int)theEntry.Size);

                                    ZipEntry.Size = bufferBody.Length;

                                    ZipEditStream.PutNextEntry(ZipEntry);
                                    ZipEditStream.Write(bufferBody, 0, bufferBody.Length);

                                }
                            }
                            ZipEditStream.SetLevel(5);
                            ZipEditStream.Finish();
                            ZipEditStream.Close();
                            //from.ShowMessage("拷贝完成，文件：" + files[i].FullName);
                        }
                    }
                    else
                    {
                        FileStream fsRead = files[i].OpenRead();
                        ///使用OpenRead()方法打开my.ini文件 
                        byte[] datar = new byte[(int)fsRead.Length];
                        ///创建datar数组，保存从my.ini文件读取的内容 
                        fsRead.Read(datar, 0, (int)fsRead.Length);
                        fsRead.Close();
                        FileStream fsWrite = File.Create(target.FullName + @"\" + files[i].Name);
                        fsWrite.Write(datar, 0, datar.Length);
                        fsWrite.Close();
                        //File.Copy(files[i].FullName, target.FullName + @"\" + files[i].Name, true);
                        //from.ShowMessage("拷贝完成，文件：" + files[i].FullName);
                    }
                    progreesValue++;
                    from.ShowProgress(progreesValue);
                }
                catch (Exception ex)
                {
                    from.ShowMessage("1拷贝文件异常：" + ex.ToString());
                    continue;
                }
            }
            #endregion

            #region "拷贝子文件夹"
            DirectoryInfo[] dirs = source.GetDirectories();

            for (int j = 0; j < dirs.Length; j++)
            {
                try
                {

                    if (dirs[j].Name.ToLower().IndexOf("errorlog") > -1
                        || dirs[j].Name.ToLower().IndexOf("back") > -1
                        || dirs[j].Name.ToLower().IndexOf("bak") > -1
                        || dirs[j].Name.ToLower().IndexOf("log") > -1
                        || dirs[j].Name.ToLower().IndexOf("upload") > -1
                        || dirs[j].Name.IndexOf("备份") > -1
                        || dirs[j].Name.IndexOf("复制") > -1
                        || dirs[j].Name.IndexOf("副本") > -1
                        || dirs[j].Name.ToLower().IndexOf("download") > -1
                        || dirs[j].Name.ToLower().IndexOf("log") > -1
                        || dirs[j].Name.ToLower().IndexOf("log") > -1
                        || dirs[j].Name.ToLower().IndexOf("businesscustomer") > -1
                        || dirs[j].Name.ToLower().IndexOf("debug") > -1
                        || dirs[j].Name.ToLower().IndexOf("old") > -1
                        || dirs[j].Name.ToLower().IndexOf("文件上传") > -1
                        || dirs[j].Name.ToLower().IndexOf("temp") > -1
                        || IsGUID(dirs[j].Name))
                    {
                        string[] filesAll = System.IO.Directory.GetFiles(dirs[j].FullName, "*", System.IO.SearchOption.AllDirectories);
                        MaxProgreesValue = MaxProgreesValue - filesAll.Length;
                        from.SetProgressMaxValue(MaxProgreesValue);
                        from.ShowMessage("文件夹被跳过：" + dirs[j].Name);
                        continue;
                    }
                    CopyDirectory(dirs[j].FullName, target.FullName + @"\" + dirs[j].Name, repalceContext);
                }
                catch (Exception ex)
                {
                    from.ShowMessage("2拷贝文件夹异常：" + ex.ToString());
                    continue;
                }
            }
            #endregion
        }
        public static bool IsGUID(string str)
        {
            Match m = Regex.Match(str, @"^[0-9a-f]{8}(-[0-9a-f]{4}){3}-[0-9a-f]{12}$", RegexOptions.IgnoreCase);
            if (m.Success)
            {
                //可以转换 
                //Guid guid = new Guid(str);
                return true;
            }
            else
            {
                //不可转换 
                return false;
            }
        }

        
        /// <summary>
        /// 递归(删除文件夹下的所有文件)
        /// </summary>
        /// <param name="dirRoot">目录地址</param>
        public static void DeleteFile(string dirRoot)
        {
            //string deleteFileName = "_desktop.ini";//要删除的文件名称
            try
            {
                string[] rootDirs = Directory.GetDirectories(dirRoot); //当前目录的子目录：
                string[] rootFiles = Directory.GetFiles(dirRoot);        //当前目录下的文件：

                foreach (string s2 in rootFiles)
                {
                   File.Delete(s2);                      //删除文件                    
                }
                foreach (string s1 in rootDirs)
                {
                    DeleteFile(s1);
                }
            }
            catch (Exception ex)
            {
                from.ShowMessage(ex.Message.ToString());
            }
        }


    }
}
