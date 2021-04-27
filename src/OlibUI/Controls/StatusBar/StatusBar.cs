using Avalonia.Controls;
using Avalonia.Controls.Generators;
using Avalonia.Controls.Templates;
using Avalonia.Input;

namespace OlibUI.Controls                                  
{
    /// <summary>
    /// An <see cref="ItemsControl"/> in which individual items can be selected.
    /// </summary>
    public class StatusBar : ItemsControl
    {
        private static readonly ITemplate<IPanel> DefaultPanel = 
            new FuncTemplate<IPanel>(() => new DockPanel { [DockPanel.DockProperty] = Dock.Bottom });

        static StatusBar()
        {
            ItemsPanelProperty.OverrideDefaultValue(typeof(StatusBar), DefaultPanel);
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
        }

        protected override IItemContainerGenerator CreateItemContainerGenerator() => new ItemContainerGenerator<StatusBarItem>(this, StatusBarItem.ContentProperty, StatusBarItem.ContentTemplateProperty);
    }
}
