using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Interactivity;
using Avalonia.Media;
using System;

namespace OlibUI.Instruments.NavigationView
{
    [PseudoClasses(":opened", ":closed", ":selected")]
    public partial class NavigationViewItemBase : TreeViewItem, IHeadered
    {
        public static readonly DirectProperty<NavigationViewItemBase, object> ContentProperty =
            AvaloniaProperty.RegisterDirect<NavigationViewItemBase, object>(
                nameof(Content),
                o => o.Content,
                (o, v) => o.Content = v);
        public readonly static DirectProperty<NavigationViewItemBase, Geometry> IconProperty =
            AvaloniaProperty.RegisterDirect<NavigationViewItemBase, Geometry>(
                nameof(Icon),
                o => o.Icon,
                (o, v) => o.Icon = v);
        public static readonly DirectProperty<NavigationViewItemBase, object> TitleProperty =
            AvaloniaProperty.RegisterDirect<NavigationViewItemBase, object>(
                nameof(Title),
                o => o.Title,
                (o, v) => o.Title = v);
        public static readonly StyledProperty<bool> IsOpenProperty =
            AvaloniaProperty.Register<NavigationView, bool>(nameof(IsOpen), true);
        public static readonly RoutedEvent<RoutedEventArgs> OpenedEvent =
            RoutedEvent.Register<NavigationViewItemBase, RoutedEventArgs>(nameof(Opened), RoutingStrategies.Bubble);
        public static readonly RoutedEvent<RoutedEventArgs> ClosedEvent =
            RoutedEvent.Register<NavigationViewItemBase, RoutedEventArgs>(nameof(Closed), RoutingStrategies.Bubble);

        private object _title = "Title";
        private Geometry _icon;
        private object _content = "Content";

        public object Content
        {
            get => _content;
            set => SetAndRaise(ContentProperty, ref _content, value);
        }        

        public Geometry Icon
        {
            get => _icon;
            set => SetAndRaise(IconProperty, ref _icon, value);
        }

        public object Title
        {
            get => _title;
            set => SetAndRaise(TitleProperty, ref _title, value);
        }

        public bool IsOpen
        {
            get => GetValue(IsOpenProperty);
            set => SetValue(IsOpenProperty, value);
        }

        public event EventHandler<RoutedEventArgs> Opened
        {
            add => AddHandler(OpenedEvent, value);
            remove => RemoveHandler(OpenedEvent, value);
        }

        public event EventHandler<RoutedEventArgs> Closed
        {
            add => AddHandler(ClosedEvent, value);
            remove => RemoveHandler(ClosedEvent, value);
        }

        static NavigationViewItemBase()
        {
            IsExpandedProperty.Changed.AddClassHandler<NavigationViewItemBase>((x, e) =>
            {
                if (x.IsExpanded)
                {
                    RoutedEventArgs o_e = new RoutedEventArgs(OpenedEvent);
                    x.RaiseEvent(o_e);
                }
                else
                {
                    RoutedEventArgs c_e = new RoutedEventArgs(ClosedEvent);
                    x.RaiseEvent(c_e);
                }
            });

            OpenedEvent.AddClassHandler<NavigationViewItemBase>((x, e) => x.OnOpened(x, e));
            ClosedEvent.AddClassHandler<NavigationViewItemBase>((x, e) => x.OnClosed(x, e));

            IsSelectedProperty.Changed.AddClassHandler<NavigationViewItemBase>((x, e) =>
            {
                if (x.IsSelected) x.OnSelected(x, e);
                else x.OnDeselected(x, e);
            });
        }

        protected virtual void OnOpened(object sender, RoutedEventArgs e)
        {
            PseudoClasses.Remove(":closed");
            PseudoClasses.Add(":opened");
        }

        protected virtual void OnClosed(object sender, RoutedEventArgs e)
        {
            PseudoClasses.Remove(":opened");
            PseudoClasses.Add(":closed");
        }

        private static void OnIsOpenChanged(AvaloniaPropertyChangedEventArgs<bool> e)
        {
            NavigationViewItem sender = (NavigationViewItem)e.Sender;

            if (sender != null && e.NewValue.HasValue)
            {
                if (sender.IsSelected && sender.Parent is NavigationViewItem nw && nw.Parent is NavigationView nwp)
                {
                    nwp.SelectSingleItem(nw);
                    nw.IsExpanded = false;
                }
            }
        }

        protected virtual void OnDeselected(object sender, AvaloniaPropertyChangedEventArgs e)
        {

        }

        protected virtual void OnSelected(object sender, AvaloniaPropertyChangedEventArgs e)
        {

        }
    }
}
