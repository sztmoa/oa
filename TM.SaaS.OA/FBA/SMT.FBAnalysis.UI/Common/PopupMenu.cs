using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Reflection;
using System.Threading;

namespace SMT.FBAnalysis.UI.Common
{
    public enum TriggerTypes { LeftClick, RightClick, Hover }

    public class PopupMenu //: ListBox
    {
        DispatcherTimer _timerOpen;
        DispatcherTimer _timerClose;
        public static List<PopupMenu> OpenMenuList = new List<PopupMenu>();
        public event EventHandler<MouseEventArgs> Opening;
        public event EventHandler<MouseEventArgs> Showing;
        public event EventHandler<MouseEventArgs> Shown;
        public event EventHandler<MouseEventArgs> Closing;
        public int HoverShowDelay { get; set; }

        public Grid RootGrid { get; set; }
        public Popup MenuPopup { get; set; }
        public ListBox ListBox { get; set; }
        public Effect ListBoxEffect { get; set; }
        public Effect ItemsEffect { get; set; }

        IEnumerable<UIElement> ClickedElements { get; set; }

        FrameworkElement HoveredTriggerElement { get; set; }
        private List<FrameworkElement> _hoverTriggerElements = new List<FrameworkElement>();
        public double OffsetX { get; set; }
        public double OffsetY { get; set; }
        public bool ShowOnRight { get; set; }

        public new ItemCollection Items
        {
            get { return ListBox.Items; }
            set
            {
                ListBox.Items.Clear();
                while (value.GetEnumerator().MoveNext())
                    ListBox.Items.Add(value.GetEnumerator().Current);
            }
        }

        #region Constructors

        public PopupMenu()
            : this(0, 0)
        { }

        public PopupMenu(double offsetX, double offsetY)
            : this(new ListBox { Background = new SolidColorBrush(Color.FromArgb(255, 240, 240, 240)) },
                 null, offsetX, offsetY)
        { }

        public PopupMenu(ListBox listBox)
            : this(listBox, null, 0, 0)
        { }

        public static implicit operator PopupMenu(ListBox listBox)
        {
            return new PopupMenu(listBox);
        }

        public PopupMenu(ListBox listBox, double offsetX, double offsetY)
            : this(listBox, null, offsetX, offsetY)
        { }

        #endregion

        public PopupMenu(ListBox listBox, Effect itemsEffect, double offsetX, double offsetY)
        {
            OffsetX = offsetX;
            OffsetY = offsetY;

            HoverShowDelay = 150;

            MenuPopup = new Popup();

            ListBox = listBox;
            if (ListBox.Effect == null)
                ListBox.Effect = new DropShadowEffect { Color = Colors.Black, BlurRadius = 4, Opacity = 0.5, ShadowDepth = 2 };

            ItemsEffect = itemsEffect;

            MenuPopup.Child = GeneratePopupContent();

            // Enable stretching on each listbox item
            Style style = new Style(typeof(ListBoxItem));
            style.Setters.Add(new Setter(ListBoxItem.HorizontalContentAlignmentProperty, HorizontalAlignment.Stretch));
            //style.Setters.Add(new Setter(ListBoxItem.HorizontalAlignmentProperty, HorizontalAlignment.Stretch));
            ListBox.ItemContainerStyle = style;
        }

        private Grid GeneratePopupContent()
        {
            Grid outerGrid = new Grid
            {
                Width = Application.Current.Host.Content.ActualWidth,
                Height = Application.Current.Host.Content.ActualHeight,
                Background = new SolidColorBrush(Colors.Transparent)
            };

            Canvas outerCanvas = new Canvas
            {
                Width = outerGrid.Width,
                Height = outerGrid.Height
            };

            RootGrid = new Grid
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
            };

            outerGrid.Children.Add(outerCanvas);
            outerCanvas.Children.Add(RootGrid);

            if (ListBox.Parent != null)
            {
                if (ListBox.Parent is Panel)
                    (ListBox.Parent as Panel).Children.Remove(ListBox);
                else
                    throw new Exception("The template listbox must be placed in a container that inherits from the type Panel");
            }

            RootGrid.Children.Add(ListBox);

            // Close the menu when a click is made outside popup itself
            outerGrid.MouseLeftButtonDown += delegate
            {
                this.Close();
            };

            outerGrid.MouseMove += (sender, e) =>
            {
                if (HoveredTriggerElement != null)
                {
                    List<FrameworkElement> triggerElementList;
                    triggerElementList = new List<FrameworkElement> { HoveredTriggerElement, RootGrid };
                    // Textblocks tend to have a variable width depending on the text length
                    // In this case the parent element is also used for hit testing to avoid limiting the hover region to the text only region.
                    if (HoveredTriggerElement is TextBlock)
                        if (double.IsNaN(HoveredTriggerElement.Width) && double.IsInfinity(HoveredTriggerElement.MaxWidth))
                            triggerElementList.Add(HoveredTriggerElement.Parent as FrameworkElement);

                    //if (_timerClose == null)
                    //{
                    //    _timerClose = new DispatcherTimer();
                    //    _timerClose.Tick += delegate
                    //    {
                    //_timerClose.Stop();
                    //        HoveredTriggerElement = null;
                    //        this.Close();
                    //};
                    //_timerClose.Interval = TimeSpan.FromMilliseconds(300);
                    //}

                    //if (!_timerClose.IsEnabled && !HitTestAny(e, triggerElementList.ToArray()))
                    //{
                    //    _timerClose.Start();
                    //}

                    if (!HitTestAny(e, triggerElementList.ToArray()))
                    {
                        HoveredTriggerElement = null;
                        this.Close();
                    }
                }

            };

            outerGrid.MouseRightButtonDown += (sender, e) =>
            {
                if (!HitTest(e, RootGrid))
                    this.Close();
                e.Handled = true;
            };

            return outerGrid;
        }

        #region Static Methods

        //private static bool HitTest(Point point, FrameworkElement element)
        //{
        //    List<UIElement> hits = System.Windows.Media.VisualTreeHelper.FindElementsInHostCoordinates(point, element) as List<UIElement>;
        //    return (hits.Contains(element));
        //}

        public static bool HitTestAny(MouseEventArgs e, params FrameworkElement[] elements)
        {
            foreach (var element in elements)
                if (HitTest(e, element))
                    return true;
            return false;
        }

        public static bool HitTest(MouseEventArgs e, FrameworkElement element)
        {
            //HtmlPage.Document.SetProperty("Title", e.GetSafePosition(null).X);
            return element.GetBoundsRelativeTo(Application.Current.RootVisual).HasValue
                && IsPointWithinBounds(e.GetSafePosition(null), element.GetBoundsRelativeTo(Application.Current.RootVisual).Value);
        }

        public static bool HitTest(Point point, FrameworkElement element)
        {
            return IsPointWithinBounds(point, element.GetBoundsRelativeTo(Application.Current.RootVisual).Value);
        }

        public static bool IsPointWithinBounds(Point pt, Rect b)
        {
            return (pt.X > b.Left && pt.X < b.Right && pt.Y > b.Top && pt.Y < b.Bottom);
        }

        public static Point GetAbsoluteElementPos(FrameworkElement element)
        {
            return element.TransformToVisual(null).Transform(new Point());
        }

        public static Point GetAbsoluteMousePos(MouseEventArgs e)
        {
            // This will not work for MouseLeave events
            return Application.Current.RootVisual.TransformToVisual(null).Transform(e.GetPosition(Application.Current.RootVisual));
        }

        //public static Point GetAbsoluteMousePos(MouseEventArgs e, FrameworkElement element)
        //{
        //    return element.TransformToVisual(null).Transform(e.GetPosition(element));
        //}

        public static FrameworkElement GetRootLayoutElement(FrameworkElement element)
        {
            while ((element != null && element.Parent != null && element.Parent is FrameworkElement))
                element = (FrameworkElement)element.Parent;
            return element;
        }

        public static Rectangle CreateSeperator(string tag)
        {
            Rectangle rectangle = new Rectangle
            {
                Height = 2,
                Margin = new Thickness(-3, 0, 0, 0),
                Fill = MakeColorGradient(Color.FromArgb(4, 0, 0, 0), Color.FromArgb(100, 255, 255, 255), 90)
            };
            DockPanel.SetDock(rectangle, Dock.Top);

            if (tag != null) rectangle.Tag = tag;
            return rectangle;
        }

        public static LinearGradientBrush MakeColorGradient(Color startColor, Color endColor, double angle)
        {
            GradientStopCollection gradientStopCollection = new GradientStopCollection();
            gradientStopCollection.Add(new GradientStop { Color = startColor, Offset = 0 });
            gradientStopCollection.Add(new GradientStop { Color = endColor, Offset = 1 });
            LinearGradientBrush brush = new LinearGradientBrush(gradientStopCollection, angle);
            return brush;
        }

        #endregion

        #region AddItem

        public PopupMenuItem AddItem(FrameworkElement item)
        {
            return InsertItem(-1, null, item, null, null, null);
        }

        public PopupMenuItem AddItem(string title, MouseButtonEventHandler leftClickHandler)
        {
            return InsertItem(-1, null, null, title, null, null, leftClickHandler);
        }

        public PopupMenuItem AddItem(FrameworkElement item, MouseButtonEventHandler leftClickHandler)
        {
            return InsertItem(-1, item, leftClickHandler);
        }

        public PopupMenuItem AddItem(string iconUrl, FrameworkElement item, MouseButtonEventHandler leftClickHandler)
        {
            return InsertItem(-1, iconUrl, item, null, null, leftClickHandler);
        }

        public PopupMenuItem AddItem(string iconUrl, string title, string tag, MouseButtonEventHandler leftClickHandler)
        {
            return InsertItem(-1, iconUrl, new TextBlock() { Text = title, Tag = tag }, null, null, leftClickHandler);
        }

        public PopupMenuItem AddItem(string leftIconUrl, string title, string rightIconUrl, string tag, MouseButtonEventHandler leftClickHandler)
        {
            return InsertItem(-1, leftIconUrl, new TextBlock() { Text = title, Tag = tag }, rightIconUrl, null, leftClickHandler);
        }

        public PopupMenuItem AddItem(string leftIconUrl, string title, string rightIconUrl, string tag, string name, MouseButtonEventHandler leftClickHandler)
        {
            return InsertItem(-1, leftIconUrl, new TextBlock() { Text = title, Tag = tag }, rightIconUrl, name, leftClickHandler);
        }

        #endregion

        #region InsertItem

        public PopupMenuItem InsertItem(int index, FrameworkElement item)
        {
            return InsertItem(index, item, null);
        }

        public PopupMenuItem InsertItem(int index, string text)
        {
            return InsertItem(index, new TextBlock() { Text = text }, null);
        }

        public PopupMenuItem InsertItem(int index, FrameworkElement item, MouseButtonEventHandler leftClickHandler)
        {
            return InsertItem(index, null, item, null, null, leftClickHandler);
        }

        public PopupMenuItem InsertItem(int index, string leftIconUrl, FrameworkElement item, string tag, MouseButtonEventHandler leftClickHandler)
        {
            return InsertItem(index, leftIconUrl, item, null, null, leftClickHandler);
        }

        public PopupMenuItem InsertItem(int index, string leftIconUrl, string title, string rightIconUrl, string tag, MouseButtonEventHandler leftClickHandler)
        {
            return InsertItem(index, leftIconUrl, new TextBlock() { Text = title, Tag = tag }, rightIconUrl, null, leftClickHandler);
        }

        public PopupMenuItem InsertItem(int index, string leftIconUrl, string title, string rightIconUrl, string tag, string name, MouseButtonEventHandler leftClickHandler)
        {
            return InsertItem(index, leftIconUrl, new TextBlock() { Text = title, Tag = tag }, rightIconUrl, name, leftClickHandler);
        }

        #endregion

        public PopupMenuItem InsertItem(int index, string leftIconUrl, FrameworkElement item, string rightIconUrl, string name, MouseButtonEventHandler leftClickHandler)
        {
            foreach (FrameworkElement child in item.GetVisualChildrenAndSelf())
                if (child.Effect == null && !(child is Panel))
                    child.Effect = ItemsEffect;

            //item.HorizontalAlignment = HorizontalAlignment.Stretch;

            if (item.Parent != null)
                (item.Parent as Panel).Children.Remove(item);

            if (leftClickHandler != null)
            {
                item.MouseLeftButtonUp += leftClickHandler;
                item.MouseRightButtonUp += leftClickHandler;
            }

            PopupMenuItem popupMenuItem = item is PopupMenuItem ? item as PopupMenuItem : new PopupMenuItem(leftIconUrl, item);

            if (rightIconUrl != null)
                popupMenuItem.RightSidedImagePath = rightIconUrl;

            if (name != null)
                popupMenuItem.Name = name;

            ListBox.Items.Insert(index == -1 ? ListBox.Items.Count : index, popupMenuItem);

            return popupMenuItem;
        }

        public void RemoveAt(int index)
        {
            ListBox.Items.RemoveAt(index);
        }

        public void Remove(ListBoxItem item)
        {
            ListBox.Items.Remove(item);
        }

        public void AddTrigger(TriggerTypes triggerType, params UIElement[] triggerElements)
        {
            foreach (FrameworkElement triggerElement in triggerElements)
            {
                switch (triggerType)
                {

                    case TriggerTypes.RightClick:
                        triggerElement.MouseRightButtonDown += (sender, e) =>
                        {
                            e.Handled = true;
                        };

                        triggerElement.MouseRightButtonUp += (sender, e) =>
                        {
                            Point mousePos = GetAbsoluteMousePos(e);
                            this.Open(mousePos, 0, sender, e);
                        };
                        break;

                    case TriggerTypes.Hover:
                        triggerElement.MouseEnter += (sender, e) =>
                        {
                            if (_timerClose != null)
                                _timerClose.Stop();

                            HoveredTriggerElement = sender as FrameworkElement;

                            Point elemPos = GetAbsoluteElementPos(HoveredTriggerElement);
                            if (ShowOnRight)
                                elemPos.X += HoveredTriggerElement.ActualWidth - 1;
                            else
                                elemPos.Y += HoveredTriggerElement.ActualHeight;

                            this.Open(elemPos, 200, sender, e);

                        };
                        _hoverTriggerElements.Add(triggerElement);
                        break;

                    case TriggerTypes.LeftClick:
                        triggerElement.MouseLeftButtonDown += (sender, e) =>
                        {
                            if (MenuPopup.IsOpen)
                            {
                                this.Close();
                            }
                            else
                            {
                                Point mousePos = GetAbsoluteElementPos(triggerElement);
                                mousePos.Y += triggerElement.ActualHeight;
                                this.Open(mousePos, 0, sender, e);
                            }
                        };
                        break;
                }
            }
        }

        public void Open(Point mousePos, int showDelayPeriod, object sender, MouseEventArgs e)
        {
            if (!MenuPopup.IsOpen)
            {
                if (!OpenMenuList.Contains(this))
                    OpenMenuList.Add(this);
                UIElement LayoutRoot = GetRootLayoutElement((FrameworkElement)sender);
                ClickedElements = VisualTreeHelper.FindElementsInHostCoordinates(mousePos, LayoutRoot);
                RootGrid.Margin = new Thickness(mousePos.X-200 + OffsetX, mousePos.Y + OffsetY, 0, 0);
                ListBox.SelectedIndex = -1; // Reset selected item

                // Invoking the event via a dispatcher to make sure the visual tree for our listbox is created before the event handler is called
                if (Opening != null)
                    ListBox.Dispatcher.BeginInvoke(() => Opening.Invoke(sender, e));

                RootGrid.Width = 0;

                _timerOpen = new DispatcherTimer();
                _timerOpen.Interval = TimeSpan.FromMilliseconds(showDelayPeriod);
                _timerOpen.Tick += delegate
                {
                    _timerOpen.Stop();
                    // If menu has not already been closed by hovering on the outergrid
                    if (MenuPopup.IsOpen)
                    {
                        // Remove Width = 0 constraint set originally
                        RootGrid.Width = double.NaN;
                        if (Showing != null)
                            Showing.Invoke(sender, e);
                        Animate(RootGrid, "UIElement.Opacity", 0, 1, TimeSpan.FromMilliseconds(HoverShowDelay));
                        //Animate(ListBox, "(UIElement.Projection).(PlaneProjection.RotationY)", 90, 0, TimeSpan.FromMilliseconds(200));
                        if (Shown != null)
                            Shown.Invoke(sender, e);
                    }
                };
                _timerOpen.Start();

                MenuPopup.IsOpen = true;
            }
        }

        public static void CloseAllOpenMenus()
        {
            foreach (PopupMenu menu in OpenMenuList.ToArray())
                menu.Close();
        }

        public void Close()
        {
            if (Closing != null)
                Closing.Invoke(this, new EventArgs() as MouseEventArgs);
            if (_timerOpen != null)
                _timerOpen.Stop();
            MenuPopup.IsOpen = false;
            OpenMenuList.Remove(this);
        }


        public Storyboard Animate(FrameworkElement element, string targetProperty, double? from, double? to, Duration duration)
        {
            DoubleAnimation da = new DoubleAnimation { From = from, To = to, Duration = duration };
            Storyboard.SetTarget(da, element);
            Storyboard.SetTargetProperty(da, new PropertyPath(targetProperty));
            Storyboard sb = new Storyboard();
            sb.Children.Add(da);
            sb.Begin();
            return sb;
        }

        public ListBoxItem GetListBoxItem(int index)
        {
            return (ListBoxItem)(ListBox.ItemContainerGenerator.ContainerFromItem(ListBox.Items[index]));
        }

        public T GetItem<T>(int index) where T : class
        {
            T item = (ListBox.Items[index] as FrameworkElement).GetVisualDescendantsAndSelf()
                .Where(i => i is T)
                .Select(i => i as T).FirstOrDefault();

            if (item == default(T))
                throw new Exception(string.Format("{0} at item {1} is not of type {2}", ListBox.Items[index].GetType(), index, typeof(T).ToString()));
            else
                return item;
        }

        public T GetChildItem<T>(UIElement element)
        {
            foreach (object item in (element as UIElement).GetVisualChildren())
            {
                if (item != null && item is T)
                    return (T)item;
            }
            return default(T);
        }

        public T GetClickedElement<T>()
        {
            return GetElement<T>(ClickedElements);
        }

        private static T GetElement<T>(IEnumerable<UIElement> elements)
        {
            return GetElement<T>(elements, 0);
        }

        private static T GetElement<T>(IEnumerable<UIElement> elements, int index)
        {
            foreach (object elem in elements)
                if (elem is T && index-- <= 0)
                    return (T)elem;
            return default(T);
        }


        public T FindItemContainerByTag<T>(object tag)
        {
            return FindItemsByTag<FrameworkElement>(tag).Select(e => GetContainer<T>(e)).First();
        }

        public ListBoxItem FindItemContainerByTag(object tag)
        {
            return FindItemContainersByTag(tag).FirstOrDefault();
        }

        public List<ListBoxItem> FindItemContainersByTag(object tag)
        {
            return FindItemsByTag<FrameworkElement>(tag).Select(e => GetContainer<ListBoxItem>(e)).ToList();
        }

        public T FindItemByTag<T>(object tag) where T : class
        {
            return FindItemsByTag<T>(tag).FirstOrDefault();
        }

        public List<T> FindItemsByTag<T>(object tag) where T : class
        {
            List<T> elements = new List<T>();
            foreach (FrameworkElement item in Items.Where(i => i is FrameworkElement))
                foreach (FrameworkElement element in item.GetVisualChildrenAndSelf())
                    if (element is T && element.Tag != null && element.Tag.Equals(tag))
                        elements.Add(element as T);

            // If no element was found search all the visual tree instead(only works after the latter has been created)
            if (elements.Count == 0)
                return ListBox.GetVisualDescendantsAndSelf()
                    .Where(i => i is T && (i as FrameworkElement).Tag == tag)
                    .Select(i => i as T).ToList();

            return elements;
        }

        public List<ListBoxItem> FindItemContainersByName(string regexPattern)
        {
            return FindItemsByName(regexPattern).Select(e => GetContainer<ListBoxItem>(e)).ToList();
        }

        public List<FrameworkElement> FindItemsByName(string regexPattern)
        {
            // This method only works after the visual tree has been created 
            return Items.GetVisualDescendantsAndSelf()
                .Where(i => i is FrameworkElement && (new Regex(regexPattern).IsMatch((i as FrameworkElement).Name ?? "")))
                .Select(i => i as FrameworkElement).ToList();
        }

        public T FindItemByName<T>(string name) where T : class
        {
            List<T> elements = new List<T>();
            foreach (FrameworkElement item in Items.Where(i => i is FrameworkElement))
                foreach (FrameworkElement element in item.GetVisualChildrenAndSelf())
                    if (element is T && element.Name == name)
                        return element as T;

            // If no element was found search all the visual tree instead(only works after the latter has been created)
            return ListBox.GetVisualDescendantsAndSelf()
                .Where(i => i is T && (i as FrameworkElement).Name == name)
                .Select(i => i as T).FirstOrDefault();
        }

        public static T GetContainer<T>(FrameworkElement item)
        {
            return item.GetVisualAncestors().Where(i => i is T).Select(i => (T)(object)i).FirstOrDefault();
            //do
            //{
            //    DependencyObject o = ListBox.ItemContainerGenerator.ContainerFromItem(item);
            //    item = (o ?? item.Parent) as FrameworkElement;
            //    if (item is T)
            //        return (T)(object)item;
            //}
            //while (item != null);
            //return default(T);
        }

        public T GetItem<T>(string name)
        {
            return Items.GetVisualDescendantsAndSelf()
                .Where(i => i is T && (i as FrameworkElement).Name == name)
                .Select(i => (T)(i as object)).First();
        }

        public void CloseParentPopupMenu()
        {
            // Close the parent popup if any
            foreach (var triggerElement in _hoverTriggerElements)
            {
                var parentPopup = GetContainer<PopupMenu>(triggerElement);
                if (parentPopup != null)
                    parentPopup.Close();
            }
        }

        public void AddSeperator()
        {
            AddSeperator(null);
        }

        public void AddSeperator(string tag)
        {
            ListBox.Items.Add(new PopupMenuItem(null, CreateSeperator(tag)));
        }
    }

    public class PopupMenuItem : DockPanel
    {
        // From left to right:
        Image _imageLeft = new Image();
        Rectangle _verticalMargin;
        TextBlock _textBlock = new TextBlock();
        Image _imageRight = new Image();

        private bool _isEnabled = true;
        public bool IsEnabled
        {
            set
            {
                foreach (FrameworkElement element in this.GetVisualDescendants().Where(e => e is FrameworkElement))
                    if (element is Control)
                        element.SetValue(Control.IsEnabledProperty, value);
                    else if (element is FrameworkElement && !(element is Rectangle))
                        (element as FrameworkElement).Opacity = value ? 1 : 0.5;

                this.CloseOnClick = value;
                _isEnabled = value;
            }
            get
            {
                return _isEnabled;
            }
        }

        public Brush Foreground
        {
            get { return _textBlock.Foreground; }
            set { _textBlock.Foreground = value; }
        }

        public string ImagePath
        {
            set { _imageLeft.Source = new BitmapImage(new Uri(value, UriKind.RelativeOrAbsolute)); }
        }

        public string RightSidedImagePath
        {
            set { _imageRight.Source = new BitmapImage(new Uri(value, UriKind.RelativeOrAbsolute)); }
        }

        public ImageSource ImageSource
        {
            set { _imageLeft.Source = value; }
            get { return _imageLeft.Source; }
        }

        public ImageSource RightSidedImageSource
        {
            set { _imageRight.Source = value; }
            get { return _imageRight.Source; }
        }

        public bool CloseOnClick { get; set; }

        public string Title
        {
            get { return _textBlock.Text; }
            set { _textBlock.Text = value; }
        }

        public PopupMenuItem() :
            this(null)
        { }

        public PopupMenuItem(string iconUrl, params UIElement[] elements)
            : this(iconUrl, true, elements)
        { }

        public PopupMenuItem(string iconUrl, bool addIconMargin, params UIElement[] elements)
        {
            //this.Margin = new Thickness(0, -1, 0, 0);
            CloseOnClick = true;

            // Add right sided image
            _imageRight = new Image
            {
                MinWidth = 0,
                Margin = new Thickness(0, -3, 0, -3),
            };
            DockPanel.SetDock(_imageRight, Dock.Right);
            this.Children.Add(_imageRight);

            // Add left icon margin
            if (addIconMargin)
            {
                _imageLeft = new Image
                {
                    MinWidth = 0,
                    Margin = new Thickness(0, -3, 3, -3),
                };

                if (!string.IsNullOrEmpty(iconUrl))
                    ImagePath = iconUrl;

                _verticalMargin = new Rectangle
                {
                    Width = 0,
                    Margin = new Thickness(0, -3, 3, -3),
                    Fill = PopupMenu.MakeColorGradient(Color.FromArgb(4, 0, 0, 0), Color.FromArgb(100, 255, 255, 255), 0)
                };

                if (elements != null && elements.Length > 0 && elements[0] is TextBlock)
                    _textBlock = elements[0] as TextBlock;

                DockPanel.SetDock(_textBlock, Dock.Top);

                this.Children.Add(_imageLeft);
                this.Children.Add(_verticalMargin);
            }

            // Add title
            this.Children.Add(_textBlock);

            // Add custom elements if any
            if (elements != null)
                foreach (UIElement element in elements)
                    if (element != null && element != _textBlock)
                    {
                        if ((element as FrameworkElement).Parent is Panel)
                            ((element as FrameworkElement).Parent as Panel).Children.Remove(element);
                        this.Children.Add(element);
                        DockPanel.SetDock(element, Dock.Top);
                    }

            // Close parent popup on click
            this.MouseLeftButtonUp += (sender, e) =>
            {
                if ((sender is PopupMenuItem) && (sender as PopupMenuItem).CloseOnClick)
                {
                    //PopupMenu.CloseAllOpenMenus();
                }
            };
        }
    }

    public class PopupMenuSeperator : PopupMenuItem
    {
        public PopupMenuSeperator() : base(null, PopupMenu.CreateSeperator(null)) { }
    }
}
