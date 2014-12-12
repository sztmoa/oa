using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SMT.FBAnalysis.UI.Command
{
    /// <summary>
    /// 扩展命令参数
    /// </summary>
    public class ExtendedCommandParameter
    {
        public ExtendedCommandParameter(EventArgs eventArgs, FrameworkElement sender, object parameter)
        {
            EventArgs = eventArgs;
            Sender = sender;
            Parameter = parameter;
        }
        public EventArgs EventArgs { get; private set; }
        public FrameworkElement Sender { get; private set; }
        public object Parameter { get; private set; }

    }
}
