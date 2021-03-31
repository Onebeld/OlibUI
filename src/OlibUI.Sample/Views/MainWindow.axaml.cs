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

            Activated += MainWindow_Activated;
        }

        private void MainWindow_Activated(object sender, System.EventArgs e)
        {
            Program.sw.Stop();
            Title = Program.sw.Elapsed.ToString();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
