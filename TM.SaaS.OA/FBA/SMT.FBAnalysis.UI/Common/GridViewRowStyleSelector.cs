//Added by AKH 
//2011年6月3日
//作为RadGridview的StyleSelector.
using System.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using System.Windows.Controls;

namespace SMT.FBAnalysis.UI.Common
{
    public class RowStyleSelector : StyleSelector
    {
        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (((GridViewRow)container).GridViewDataControl.Items.IndexOf(item) == 0)
            {
                Style style = new Style(typeof(GridViewRow));
                Setter setter = new Setter(GridViewRow.DetailsVisibilityProperty, Visibility.Visible);
                style.Setters.Add(setter);
                return style;
            }

            return new Style(typeof(GridViewRow));
        }
    }
}
