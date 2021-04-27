using System.Linq;
using System.Text;
using Avalonia.LogicalTree;
using Avalonia.Media;
using Avalonia.VisualTree;
using OlibUI.Structures;

namespace OlibUI
{
    public static class Extensions
    {
        public static IBrush ToBursh(this Color color) => new SolidColorBrush(color);

        public static string ToAxaml(this Theme theme)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<Style xmlns=\"https://github.com/avaloniaui\"");
            sb.AppendLine("       xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\">");
            sb.AppendLine("    <Style.Resources>");
            sb.AppendLine($"        <Color x:Key=\"BackgroundColor\">{theme.BackgroundColor}</Color>");
            sb.AppendLine($"        <Color x:Key=\"HoverBackgroundColor\">{theme.HoverBackgroundColor}</Color>");
            sb.AppendLine($"        <Color x:Key=\"ForegroundColor\">{theme.ForegroundColor}</Color>");
            sb.AppendLine($"        <Color x:Key=\"ForegroundOpacityColor\">{theme.ForegroundOpacityColor}</Color>");
            sb.AppendLine($"        <Color x:Key=\"PressedForegroundColor\">{theme.PressedForegroundColor}</Color>");
            sb.AppendLine($"        <Color x:Key=\"AccentColor\">{theme.AccentColor}</Color>");
            sb.AppendLine($"        <Color x:Key=\"BorderBackgroundColor\">{theme.BorderBackgroundColor}</Color>");
            sb.AppendLine($"        <Color x:Key=\"BorderColor\">{theme.BorderColor}</Color>");
            sb.AppendLine($"        <Color x:Key=\"WindowBorderColor\">{theme.WindowBorderColor}</Color>");
            sb.AppendLine($"        <Color x:Key=\"NotActiveWindowBorderColor\">{theme.NotActiveWindowBorderColor}</Color>");
            sb.AppendLine($"        <Color x:Key=\"HoverScrollBoxColor\">{theme.HoverScrollBoxColor}</Color>");
            sb.AppendLine($"        <Color x:Key=\"ScrollBoxColor\">{theme.ScrollBoxColor}</Color>");
            sb.AppendLine($"        <Color x:Key=\"ErrorColor\">{theme.ErrorColor}</Color>");
            sb.AppendLine($"        <Color x:Key=\"WindowBorderBackgroundColor\">{theme.WindowBorderBackgroundColor}</Color>");
            sb.AppendLine("    </Style.Resources>");
            sb.AppendLine("</Style>");

            return sb.ToString();
        }

        public static T GetParentTOfLogical<T>(this ILogical logical) where T : class => logical.GetSelfAndLogicalAncestors().OfType<T>().FirstOrDefault<T>();

        public static T GetParentTOfVisual<T>(this IVisual visual) where T : class => visual.GetSelfAndVisualAncestors().OfType<T>().FirstOrDefault<T>();
    }
}