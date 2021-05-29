using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using OlibUI.Structures;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OlibUI.Windows
{
    public class MessageBox : OlibWindow
    {
        public enum MessageBoxIcon
        {
            Information,
            Error,
            Warning,
            Question,
            None
        }

        public MessageBox() => AvaloniaXamlLoader.Load(this);

        /// <summary>
        /// Shows a message box
        /// </summary>
        /// <param name="parent">Snap to window (could be null)</param>
        /// <param name="title">Name of the message box</param>
        /// <param name="text">Content message box</param>
        /// <param name="buttons">Creating buttons for a window</param>
        /// <param name="icon">Message box icon</param>
        /// <param name="textException">Adding additional information</param>
        /// <returns>Result after pressing the button</returns>
        public static Task<string> Show(Window parent, string title, string text, IList<MessageBoxButton> buttons, MessageBoxIcon icon = MessageBoxIcon.None, string textException = null, double maxWidth = 450, double maxHeight = 200)
        {
            MessageBox msgbox = new MessageBox
            {
                Title = title,
                Icon = parent?.Icon,
                CompactMode = ((OlibWindow)parent)?.CompactMode ?? false,
                EnableBlur = ((OlibWindow)parent)?.EnableBlur ?? true,
                MaxHeight = maxHeight,
                MaxWidth = maxWidth
            };
            msgbox.FindControl<TextBlock>("Text").Text = text;
            StackPanel buttonPanel = msgbox.FindControl<StackPanel>("Buttons");
            Path iconControl = msgbox.FindControl<Path>("Icon");
            TextBox errorText = msgbox.FindControl<TextBox>("ErrorText");

            errorText.MaxHeight = maxHeight - 130;
            errorText.MaxWidth = maxWidth - 5;

            string res = "OK";

            void AddButton(MessageBoxButton r)
            {
                Button btn = new Button { Content = r.Text };
                btn.Click += (_, __) =>
                {
                    res = r.Result;
                    msgbox.Close();
                };
                buttonPanel.Children.Add(btn);
                if (r.Def) res = r.Result;
            }

            void ChangeIcon(string icn) => iconControl.Data = (Geometry)Application.Current.FindResource($"{icn}Icon");

            foreach (MessageBoxButton button in buttons)
            {
                if (button.IsKeyDown)
                    msgbox.KeyDown += (s, e) =>
                    {
                        if (e.Key == Key.Enter)
                        {
                            res = button.Result;
                            msgbox.Close();
                        }
                    };

                AddButton(button);
            }

            switch (icon)
            {
                case MessageBoxIcon.Error:
                    ChangeIcon("Error");
                    break;
                case MessageBoxIcon.Information:
                    ChangeIcon("Information");
                    break;
                case MessageBoxIcon.Question:
                    ChangeIcon("Question");
                    break;
                case MessageBoxIcon.Warning:
                    ChangeIcon("Warning");
                    break;

                default:
                    break;
            }

            if (!string.IsNullOrEmpty(textException)) errorText.Text = textException;
            else errorText.IsVisible = false;

            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
            msgbox.Closed += delegate { _ = tcs.TrySetResult(res); };
            if (parent != null) _ = msgbox.ShowDialog(parent);
            else msgbox.Show();
            return tcs.Task;
        }
    }
}