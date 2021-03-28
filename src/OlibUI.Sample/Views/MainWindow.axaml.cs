using Avalonia.Markup.Xaml;
using OlibUI.Sample.ViewModels;
using OlibUI.Windows;

namespace OlibUI.Sample.Views
{
    public class MainWindow : OlibWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Closing += (sender, args) => ((MainWindowViewModel) DataContext)?.Closing();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
