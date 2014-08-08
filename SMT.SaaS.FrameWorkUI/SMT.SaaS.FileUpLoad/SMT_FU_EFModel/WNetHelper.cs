using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;  


namespace SMT.FileUpLoad.Service
{
  
    /// <summary>
    /// 
    /// </summary>
    public class WNetHelper  
    {  
        [DllImport("mpr.dll", EntryPoint = "WNetAddConnection2")]  
        private static extern uint WNetAddConnection2(NetResource lpNetResource, string lpPassword, string lpUsername, uint dwFlags);  
  
        [DllImport("Mpr.dll", EntryPoint = "WNetCancelConnection2")]  
        private static extern uint WNetCancelConnection2(string lpName, uint dwFlags, bool fForce);  
  
        [StructLayout(LayoutKind.Sequential)]  
        public class NetResource  
        {  
            public int dwScope;  
  
            public int dwType;  
  
            public int dwDisplayType;  
  
            public int dwUsage;  
  
            public string lpLocalName;  
  
            public string lpRemoteName;  
  
            public string lpComment;  
  
            public string lpProvider;  
        }  
  
        /// <summary>   
        /// 为网络共享做本地映射   
       /// </summary>   
        /// <param name="username">访问用户名（windows系统需要加计算机名，如：comp-1/user-1）</param>   
        /// <param name="password">访问用户密码</param>   
        /// <param name="remoteName">网络共享路径（如：//192.168.0.9/share）</param>   
        /// <param name="localName">本地映射盘符</param>   
        /// <returns></returns>   
        public static uint WNetAddConnection(string username, string password, string remoteName, string localName)  
        {  
            NetResource netResource = new NetResource();  
  
            netResource.dwScope = 2;  
            netResource.dwType = 1;  
            netResource.dwDisplayType = 3;  
            netResource.dwUsage = 1;  
            netResource.lpLocalName = localName;  
            netResource.lpRemoteName = remoteName.TrimEnd('\\');  
            uint result = WNetAddConnection2(netResource, password, username, 0);  
  
            return result;  
        }  
  
        public static uint WNetCancelConnection(string name, uint flags, bool force)  
        {  
            uint nret = WNetCancelConnection2(name, flags, force);  
  
            return nret;  
        }  
    }  

}
