using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMT.FileUpLoad.Service
{
    public class LogonImpersonate : IDisposable  
    {  
        static public string DefaultDomain  
        {  
            get  
            {  
                return ".";  
            }  
        }  
  
        const int LOGON32_LOGON_INTERACTIVE = 2;  
        const int LOGON32_PROVIDER_DEFAULT = 0;  
  
        [System.Runtime.InteropServices.DllImport("Kernel32.dll")]  
        extern static int FormatMessage(int flag, ref   IntPtr source, int msgid, int langid, ref   string buf, int size, ref   IntPtr args);  
  
        [System.Runtime.InteropServices.DllImport("Kernel32.dll")]  
        extern static bool CloseHandle(IntPtr handle);  
  
        [System.Runtime.InteropServices.DllImport("Advapi32.dll", SetLastError = true)]  
        extern static bool LogonUser(  
        string lpszUsername,  
        string lpszDomain,  
        string lpszPassword,  
        int dwLogonType,  
        int dwLogonProvider,  
        ref   IntPtr phToken  
        );  
  
        IntPtr token;  
        System.Security.Principal.WindowsImpersonationContext context;  
  
        public LogonImpersonate(string username, string password)  
        {  
            if (username.IndexOf("//") == -1)  
            {  
                Init(username, password, DefaultDomain);  
            }  
            else  
            {  
                string[] pair = username.Split(new char[] { '/' }, 2);  
                Init(pair[1], password, pair[0]);  
            }  
        }  
        public LogonImpersonate(string username, string password, string domain)  
        {  
            Init(username, password, domain);  
        }  
        void Init(string username, string password, string domain)  
        {  
            if (LogonUser(username, domain, password, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, ref   token))  
            {  
                bool error = true;  
                try  
                {  
                    context = System.Security.Principal.WindowsIdentity.Impersonate(token);  
                    error = false;  
                }  
                finally  
                {  
                    if (error)  
                        CloseHandle(token);  
                }  
            }  
            else  
            {  
                int err = System.Runtime.InteropServices.Marshal.GetLastWin32Error();  
  
                IntPtr tempptr = IntPtr.Zero;  
                string msg = null;  
  
                FormatMessage(0x1300, ref   tempptr, err, 0, ref   msg, 255, ref   tempptr);  
  
                throw (new Exception(msg));  
            }  
        }  
        ~LogonImpersonate()  
        {  
            Dispose();  
        }  
        public void Dispose()  
        {  
            if (context != null)  
            {  
                try  
                {  
                    context.Undo();  
                }  
                finally  
                {  
                    CloseHandle(token);  
                    context = null;  
                }  
            }  
        }  
    }  

}
