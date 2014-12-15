using System;
using System.Collections.Generic;
using System.Text;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            ActivityMonitor monitor = new ActivityMonitor();
            monitor.ActivedTimeOut = 2000;// 2 秒钟超时
            monitor.TimerInterval = 1000;//1秒钟检查一次
            monitor.OnSleep += delegate { Console.WriteLine("无操作，等待中......"); };
            monitor.OnActive += delegate { Console.WriteLine("已捕捉到输入"); };

            monitor.Start();
            Console.ReadLine();
        }
    }
}
