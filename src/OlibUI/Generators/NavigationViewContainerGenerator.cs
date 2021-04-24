using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Generators;
using OlibUI.Instruments.NavigationView;

namespace OlibUI.Generators
{
    public class NavigationViewContainerGenerator : TreeItemContainerGenerator<NavigationViewItem>
    {
        public NavigationViewContainerGenerator(NavigationView owner,
                                                AvaloniaProperty contentProperty,
                                                AvaloniaProperty itemsProperty,
                                                AvaloniaProperty headerProperty,
                                                AvaloniaProperty titleProperty,
                                                AvaloniaProperty expandedProperty) : base(owner, contentProperty, null, itemsProperty,expandedProperty)
        {
            Content = contentProperty;
            Items = itemsProperty;
            Header = headerProperty;
            Title = titleProperty;
        }

        AvaloniaProperty Content { get; set; }
        AvaloniaProperty Header { get; set; }
        AvaloniaProperty Items { get; set; }
        AvaloniaProperty Title { get; set; }

        protected override IControl CreateContainer(object item)
        {
            NavigationViewItem navviewitem = (NavigationViewItem)base.CreateContainer(item);

            navviewitem.Bind(NavigationViewItem.HeaderProperty, navviewitem.GetBindingObservable(Header));
            navviewitem.Bind(NavigationViewItem.ContentProperty, navviewitem.GetBindingObservable(Content));
            navviewitem.Bind(NavigationViewItem.ItemsProperty, navviewitem.GetBindingObservable(Items));
            navviewitem.Bind(NavigationViewItem.TitleProperty, navviewitem.GetBindingObservable(Title));

            return navviewitem;
        }
    }
}
