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
using System.Windows.Interactivity;

namespace SMT.FBAnalysis.UI.Command
{
    /// <summary>
    /// 扩展行为
    /// </summary>
    public class ExtendedInvokeCommandAction : TriggerAction<FrameworkElement>
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(ExtendedInvokeCommandAction), new PropertyMetadata(null, CommandChangedCallback));
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(ExtendedInvokeCommandAction), new PropertyMetadata(null, CommandParameterChangedCallback));

        private static void CommandParameterChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var invokeCommand = d as ExtendedInvokeCommandAction;
            if (invokeCommand != null)
            {
                invokeCommand.SetValue(CommandParameterProperty, e.NewValue);
            }
        }
        private static void CommandChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var invokeCommand = d as ExtendedInvokeCommandAction;
            if (invokeCommand != null)
                invokeCommand.SetValue(CommandProperty, e.NewValue);
        }

        protected override void Invoke(object parameter)
        {
            if (this.Command == null) return;
            if (this.Command.CanExecute(parameter))
            {
                var commandParameter = new ExtendedCommandParameter(parameter as EventArgs, this.AssociatedObject,
                     GetValue(CommandParameterProperty));
                this.Command.Execute(commandParameter);
            }
        }
        #region public properties
        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }
        public ICommand Command
        {
            get { return GetValue(CommandProperty) as ICommand; }
            set { SetValue(CommandParameterProperty, value); }
        }
        #endregion
    }
}
