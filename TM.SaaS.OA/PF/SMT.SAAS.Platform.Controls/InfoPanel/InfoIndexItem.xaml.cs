using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SMT.SAAS.Platform.Controls.InfoPanel
{
    public partial class InfoIndexItem : UserControl
    {
        private int id;
        public RoutedEventHandler Click;
        public InfoIndexItem()
        {
            this.InitializeComponent();
            this.LayoutRoot.MouseLeftButtonDown += new MouseButtonEventHandler(LayoutRoot_MouseLeftButtonDown);
        }
        public void DeSelectItem()
        {
            VisualStateManager.GoToState(this, "Deselect", false);
        }

        private void LayoutRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.Click != null)
            {
                this.Click.Invoke(this, e);
            }
        }

        public void SelectItem()
        {
            VisualStateManager.GoToState(this, "Select", false);
        }

        // Properties
        public int Id
        {
            get
            {
                return this.id;
            }

            set
            {
                this.id = value;
            }
        }
    }
}
