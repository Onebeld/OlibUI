using Avalonia.Markup.Xaml;
using OlibUI.Windows;

namespace OlibUI.Sample.Views.Windows
{
    public class WindowWithControls : OlibWindow
    {
        public WindowWithControls()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
