using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OlibUI.Instruments;
using System.Threading.Tasks;
using Avalonia.Input;

namespace OlibUI.Windows
{
    public class WindowColorPicker : OlibWindow
    {
        public WindowColorPicker() => AvaloniaXamlLoader.Load(this);

        /// <summary>
        /// Allows you to assign a color in a separate window
        /// </summary>
        /// <param name="parent">Snap to window (could be null)</param>
        /// <param name="defaultColor">Color before choosing a new one</param>
        /// <returns>Result after pressing the button</returns>
        public static Task<string> SelectColor(Window parent, string defaultColor = null)
        {
            WindowColorPicker windowColorPicker = new WindowColorPicker { Icon = parent?.Icon };

            ColorPicker picker = windowColorPicker.FindControl<ColorPicker>("ColorPicker");

            bool cancel = true;

            string res = defaultColor != null ? defaultColor : "#FFFFFFFF";

            picker.Color = ColorHelpers.FromHexColor(res);
            picker.ChangeColor += (s,e) =>
            {
                res = ColorHelpers.ToHexColor(picker.Color);
            };

            windowColorPicker.FindControl<Button>("Cancel").Click += (s, e) =>
            {
                windowColorPicker.Close();
            };
            windowColorPicker.FindControl<Button>("OK").Click += (s, e) =>
            {
                cancel = false;
                windowColorPicker.Close();
            };

            windowColorPicker.KeyDown += (s, e) =>
            {
                if (e.Key == Key.Enter)
                {
                    cancel = false;
                    windowColorPicker.Close();
                }
            };

            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
            windowColorPicker.Closed += (s,e) => { if (!cancel) tcs.TrySetResult(res); else tcs.TrySetCanceled(); };
            if (parent != null) windowColorPicker.ShowDialog(parent);
            else windowColorPicker.Show();
            return tcs.Task;
        }
    }
}
