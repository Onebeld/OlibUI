using Avalonia.Controls.Mixins;
using Avalonia.Interactivity;
using Avalonia.Input;

namespace OlibUI.Controls
{
    public class NavigationViewItem : NavigationViewItemBase
    {
        static NavigationViewItem()
        {
            SelectableMixin.Attach<NavigationViewItem>(IsSelectedProperty);
            FocusableProperty.OverrideDefaultValue<NavigationViewItem>(true);
        }

        protected override void OnClosed(object sender, RoutedEventArgs e)
        {
            base.OnClosed(sender, e);

            this.GetParentTOfLogical<NavigationView>().SelectSingleItem(this);
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);

            e.Handled = true;

            if (!IsSelected) this.GetParentTOfLogical<NavigationView>().SelectSingleItem(this);
        }
    }
}
