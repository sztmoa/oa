using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SMT.AttendanceLogs.UI
{
   public class BaseForm : Form
    {
        delegate void DelShow(TextBox labMsg, String Msg); //代理
        //将对控件的操作写到一个函数中 
        public void ShowMessage(TextBox labMsg,String para)
        {
            if (!labMsg.InvokeRequired) //不需要唤醒，就是创建控件的线程
            //如果是创建控件的线程，直接正常操作 
            {
                labMsg.Text =para+ labMsg.Text;
            }
            else //非创建线程，用代理进行操作
            {
                DelShow ds = new DelShow(ShowMessage);
                //唤醒主线程，可以传递参数，也可以为null，即不传参数
                Invoke(ds, new object[] {labMsg, para });
            }
        }
    }
}
