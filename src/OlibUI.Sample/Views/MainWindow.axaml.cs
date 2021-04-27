using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using OlibUI.Sample.ViewModels;
using OlibUI.Windows;
using System.Linq;

namespace OlibUI.Sample.Views
{
    public class MainWindow : OlibWindow
    {
        public MainWindow()
        {
            AvaloniaXamlLoader.Load(this);
            Closing += (sender, args) => ((MainWindowViewModel)DataContext)?.Closing();

            Activated += MainWindow_Activated;

            InteractingWithWindow += MainWindow_InteractingWithWindow;
        }

        private void MainWindow_InteractingWithWindow(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (((MainWindowViewModel)DataContext).EnableMovablePopup)
                foreach (Popup p in this.GetLogicalDescendants().OfType<Popup>())
                    if (p.IsOpen) p.Host.ConfigurePosition(p.PlacementTarget, p.PlacementMode, new Point(p.HorizontalOffset, p.VerticalOffset), p.PlacementAnchor, p.PlacementGravity);
        }

        private void MainWindow_Activated(object sender, System.EventArgs e)
        {
            Program.sw.Stop();
            Title = Program.sw.Elapsed.ToString();
        }
    }
}
