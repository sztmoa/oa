using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices;
using System.Collections;

namespace SmtPortalSetUp
{
   public static class IISHelper
    {
       public static string Host;
       public static string UserName;
       public static string Password;

       public static void ssssss(string host, string port
           , string webSitePath
           , string webSiteDesc
           , string webSiteComment
           , string appPool
           , string friendlyName
           , string virtualPath)
       {
           //在.Net中我们可以使用内置的类DirectoryEntry来承载IIS服务器中的任何网站，虚拟路径或应用程序池对象，例如：

           //DirectoryEntry ent = new DirectoryEntry("IIS://localhost/w3svc/1/root");
           //就创建了一个IIS路径为IIS://localhost/w3svc/1/root的虚拟路径对象。

           //为了在IIS中创建一个网站，我们首先需要确定输入的网站路径在IIS中是否存在，这里主要是根据网站在IIS中的ServerBindings属性来区分：
           DirectoryEntry ent;
           DirectoryEntry rootEntry;
           try
           {
               ent = EnsureNewWebSiteAvailable(host + ":" + port + ":" + webSiteDesc);
               if (ent != null)
               {
                   //这里如果用户输入的网站在IIS中已经存在，那么直接获取网站的root对象，也就是网站的默认应用程序
                   rootEntry = ent.Children.Find("root", "IIsWebVirtualDir");
               }
               else
               {
                   //如果网站在IIS不存在，那么我们需要首先在IIS中创建该网站，并且为该网站创建一个root应用程序
                   string entPath = string.Format("IIS://{0}/w3svc", Host);
                   DirectoryEntry root = GetDirectoryEntry(entPath);
                   string newSiteNum = GetNewWebSiteID();
                   DirectoryEntry newSiteEntry = root.Children.Add(newSiteNum, "IIsWebServer");
                   newSiteEntry.CommitChanges();
                   newSiteEntry.Properties["ServerBindings"].Value = host + ":" + port + ":" + webSiteDesc;
                   newSiteEntry.Properties["ServerComment"].Value = webSiteComment;
                   newSiteEntry.CommitChanges();
                   rootEntry = newSiteEntry.Children.Add("root", "IIsWebVirtualDir");
                   rootEntry.CommitChanges();
                   rootEntry.Properties["Path"].Value = webSitePath;
                   rootEntry.Properties["AppPoolId"].Value = appPool;
                   rootEntry.Properties["AccessRead"][0] = true; // 勾选读取
                   rootEntry.Properties["AuthFlags"][0] = 1 + 4;
                   //勾选匿名访问和windows身份验证
                   /** 标志
                   标志名 AuthBasic
                   描述 指定基本身份验证作为可能的 
                   Windows 验证方案之一，返回给客户端作为有效验证方案。
                   配置数据库位掩码标识符 MD_AUTH_BASIC
                   十进制值 2
                   十六进制值 0x00000002
　　　　
                   标志名 AuthAnonymous
                   描述 指定匿名身份验证作为可能的 
                   Windows 验证方案之一，返回给客户端作为有效验证方案。
                   配置数据库位掩码标识符 MD_AUTH_ANONYMOUS
                   十进制值 1
                   十六进制值 0x00000001
　　　　
                   标志名 AuthNTLM
                   描述 指定集成 Windows 
                   身份验证（也称作质询/响应或 NTLM 验证）作为可能的 Windows 验证方案之一，返回给客户端作为有效验证方案。
                   配置数据库位掩码标识符 MD_AUTH_NT
                   十进制值 4
                   十六进制值 0x00000001
 
                   标志名 AuthMD5
                   描述 指定摘要式身份验证和高级摘要式身份验证作为可能的 Windows 
                   验证方案之一，返回给客户端作为有效验证方案。
                   配置数据库位掩码标识符 MD_AUTH_MD5
                   十进制值 16
                   十六进制值 0x00000010
　　　　
                   标志名 AuthPassport
                   描述 true 的值表示启用了 Microsoft .NET Passport 身份验证。 详细信息，请参阅 .NET Passport 验证。
                   配置数据库位掩码标识符 MD_AUTH_PASSPORT
                   十进制值 64
                   十六进制值 0x00000040
                   */
                   rootEntry.Properties["DontLog"][0] = true;
                   rootEntry.Properties["AuthAnonymous"][0] = true;
                   rootEntry.Properties["AnonymousUserName"][0] =
                   //XmlSettings.GetWebXmlSettingString("IISAnonymousUserName");

                   /*这里AnonymousUserPass属性如果不去设置，IIS会自动控制匿名访问账户的密码。之前我尝试将匿名访问用户的密码传给网站，之后发现创建出来的网站尽管勾选的匿名访问并且设置了匿名用户密码，浏览的时候还是提示要输入密码，很是纠结*/
                   rootEntry.Invoke("AppCreate", true);
                   rootEntry.CommitChanges();
               }
               DirectoryEntry de = rootEntry.Children.Add(friendlyName, rootEntry.SchemaClassName);
               de.CommitChanges();
               de.Properties["Path"].Value = virtualPath;
               de.Properties["AccessRead"][0] = true; // 勾选读取
               de.Invoke("AppCreate", true);
               de.Properties["EnableDefaultDoc"][0] = true;
               de.Properties["AccessScript"][0] = true; // 脚本资源访问
               de.Properties["DontLog"][0] = true; // 勾选记录访问
               de.Properties["ContentIndexed"][0] = true; // 勾选索引资源
               de.Properties["AppFriendlyName"][0] = friendlyName; //应用程序名
               de.Properties["AuthFlags"][0] = 5;
               /*这里在创建虚拟路径时不需要再次设置匿名访问，因为网站下的虚拟路径会默认接受网站的访问限制设置*/
               de.CommitChanges();
           }
           catch (Exception e)
           {
               throw e;
           }
       }
 
public static string GetNewWebSiteID() 
{ 
　　ArrayList list = new ArrayList(); 
　　string tempStr; 
　　string entPath = string.Format("IIS://{0}/w3svc",Host); 
　　DirectoryEntry ent = GetDirectoryEntry(entPath); 
　　foreach (DirectoryEntry child in ent.Children) 
　　{ 
　　　　if (child.SchemaClassName == "IIsWebServer")
　　　　{
　　　　　　tempStr = child.Name.ToString(); 
　　　　　　list.Add(Convert.ToInt32(tempStr)); 
　　　　}
　　}
　　list.Sort();
　　var newId = Convert.ToInt32(list[list.Count - 1]) + 1;
　　return newId.ToString();
} 
 
public static DirectoryEntry GetDirectoryEntry(string entPath) 
{ 
　　DirectoryEntry ent;
　　if (string.IsNullOrEmpty(UserName))
　　{ 
　　　　ent = new DirectoryEntry(entPath);
　　} 
　　else
　　{ 
　　　　ent = new DirectoryEntry(entPath, Host + "\\" + UserName, Password, AuthenticationTypes.Secure);
　　} 
　　return ent;
} 
 
public static DirectoryEntry EnsureNewWebSiteAvailable(string bindStr) 
{ 
　　string entPath = string.Format("IIS://{0}/w3svc",Host); 
　　DirectoryEntry ent = GetDirectoryEntry(entPath); 
　　foreach (DirectoryEntry child in ent.Children) 
　　{ 
　　　　if (child.SchemaClassName == "IIsWebServer") 
　　　　{ 
　　　　　　if (child.Properties["ServerBindings"].Value != null)
　　　　　　{
　　　　　　　　if (child.Properties["ServerBindings"].Value.ToString() == bindStr) 
　　　　　　　　{ return child; } 
　　　　　　}
　　　　} 
　　} 
　　return null;
}  


    }
}
