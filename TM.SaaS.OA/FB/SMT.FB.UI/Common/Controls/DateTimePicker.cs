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

namespace SMT.FB.UI.Common.Controls
{
    public class DateTimePicker : UserControl
    {

        public static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DateTimePicker dp = d as DateTimePicker;
            dp.InitDateTime((DateTime?)e.NewValue);
        }
        public static readonly DependencyProperty ValueProperty = 
            DependencyProperty.Register("Value", typeof(DateTime?), typeof(DateTimePicker), new PropertyMetadata(DateTime.Now, ValueChanged));

        private DatePicker InnerDatePicker;
        private TimePicker InnerTimePicker;
        public DateTimePicker() : base()
        {
            StackPanel pnl = new StackPanel();
            InnerDatePicker  = new DatePicker();
            InnerTimePicker  = new TimePicker();
            InnerTimePicker.Width = 50;
            StackPanel spSpliter = new StackPanel();
            spSpliter.Width = 5;
            pnl.Orientation = Orientation.Horizontal;

            pnl.Children.Add(InnerDatePicker);
            pnl.Children.Add(spSpliter);
            pnl.Children.Add(InnerTimePicker);

            this.Content = pnl;
            SetBinding();
        }

        public DateTime? Value
        {
            get
            {
                return (DateTime)base.GetValue(ValueProperty);
            }
            set
            {
                base.SetValue(ValueProperty, value);
            }
        }

        
        private void SetBinding()
        {
            InnerDatePicker.SelectedDateChanged += new EventHandler<SelectionChangedEventArgs>(InnerDatePicker_SelectedDateChanged);
            InnerTimePicker.ValueChanged += new RoutedPropertyChangedEventHandler<DateTime?>(InnerTimePicker_ValueChanged);
        }

        void InnerTimePicker_ValueChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            SetDateTime();
        }
        void InnerDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            SetDateTime();
        }

        private void SetDateTime()
        {
            DateTime? dateS = InnerDatePicker.SelectedDate;
            DateTime? timeS = InnerTimePicker.Value;

            if (dateS == null && timeS == null)
            {
                Value = null;
            }
            else
            {
                DateTime currentValue = DateTime.Now.Date;

                if (dateS != null)
                {
                    currentValue = new DateTime(dateS.Value.Ticks);
                }
                if (timeS != null)
                {
                    currentValue.AddHours(timeS.Value.Hour);
                    currentValue.AddMinutes(timeS.Value.Minute);
                }
                Value = currentValue;
            }
        }

        private void InitDateTime(DateTime? initValue)
        {
            this.InnerDatePicker.SelectedDate = initValue.Value;
            this.InnerTimePicker.Value = initValue;
        }






    }
}
