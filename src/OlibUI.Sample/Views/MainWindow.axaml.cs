using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using OlibUI.Sample.ViewModels;
using OlibUI.Windows;

namespace OlibUI.Sample.Views
{
    public class MainWindow : OlibWindow
    {
        public MainWindow()
        {
            AvaloniaXamlLoader.Load(this);
            Closing += (sender, args) => ((MainWindowViewModel) DataContext)?.Closing();

            Activated += MainWindow_Activated;

            InteractingWithWindow += MainWindow_InteractingWithWindow;
        }

        private void MainWindow_InteractingWithWindow(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Popup popup = this.FindControl<Popup>("PopupNot");

            if (popup.IsOpen)
            {
                popup.Host.ConfigurePosition(popup.PlacementTarget, PlacementMode.AnchorAndGravity, new Point(popup.HorizontalOffset, popup.VerticalOffset), Avalonia.Controls.Primitives.PopupPositioning.PopupAnchor.TopLeft, Avalonia.Controls.Primitives.PopupPositioning.PopupGravity.Top);
            }
        }

        private void MainWindow_Activated(object sender, System.EventArgs e)
        {
            Program.sw.Stop();
            Title = Program.sw.Elapsed.ToString();
        }
    }
}
