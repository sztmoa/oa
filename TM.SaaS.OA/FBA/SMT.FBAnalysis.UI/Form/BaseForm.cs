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

namespace SMT.FBAnalysis.UI
{
    public partial class BaseForm : UserControl
    {
        private MainPage currentMain;

        //public BaseForm()
        //{
        //    currentMain = Application.Current.RootVisual as MainPage;
        //}

        public delegate void refreshGridView();

        public event refreshGridView ReloadDataEvent;

        public void ReloadData()
        {
            if (ReloadDataEvent != null)
            {
                ReloadDataEvent();
            }
        }
    }
}
