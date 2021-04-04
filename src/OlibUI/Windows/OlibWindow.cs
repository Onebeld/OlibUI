using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Styling;
using System;

namespace OlibUI.Windows
{
    [Flags]
    public enum WindowButtons
    {
        Close = 0,
        CloseAndCollapse = 1,
        CloseAndExpand = 2,
        All = 3
    }

    public class OlibWindow : Window, IStyleable
    {
        public static readonly StyledProperty<WindowButtons> WindowButtonsProperty =
            AvaloniaProperty.Register<OlibWindow, WindowButtons>(nameof(WindowButtons));
        public static readonly StyledProperty<IControl> BottomPanelProperty =
            AvaloniaProperty.Register<OlibWindow, IControl>(nameof(BottomPanel));
        public static readonly StyledProperty<IControl> TitleBarMenuProperty =
            AvaloniaProperty.Register<OlibWindow, IControl>(nameof(TitleBarMenu));
        public static readonly StyledProperty<bool> InLoadModeProperty =
            AvaloniaProperty.Register<OlibWindow, bool>(nameof(InLoadMode));
        public static readonly StyledProperty<bool> IsIndeterminateProperty =
            AvaloniaProperty.Register<OlibWindow, bool>(nameof(IsIndeterminate));
        public static readonly StyledProperty<Geometry> TextIconProperty =
            AvaloniaProperty.Register<OlibWindow, Geometry>(nameof(TextIcon));
        public static readonly StyledProperty<int> ProgressLoadProperty = 
            AvaloniaProperty.Register<OlibWindow, int>(nameof(ProgressLoad));
        public static readonly StyledProperty<string> ProgressTextProperty = 
            AvaloniaProperty.Register<OlibWindow, string>(nameof(ProgressText));
        public static readonly StyledProperty<bool> FullScreenButtonProperty =
            AvaloniaProperty.Register<OlibWindow, bool>(nameof(FullScreenButton));

        /// <summary>
        /// Shows or hides the Expand and Collapse buttons
        /// </summary>
        public WindowButtons WindowButtons
        {
            get => GetValue(WindowButtonsProperty);
            set => SetValue(WindowButtonsProperty, value);
        }

        /// <summary>
        /// Shows full screen button
        /// </summary>
        public bool FullScreenButton
        {
            get => GetValue(FullScreenButtonProperty);
            set => SetValue(FullScreenButtonProperty, value);
        }

        /// <summary>
        /// Panel if you need to add something
        /// </summary>
        public IControl BottomPanel
        {
            get => GetValue(BottomPanelProperty);
            set => SetValue(BottomPanelProperty, value);
        }

        public IControl TitleBarMenu
        {
            get => GetValue(TitleBarMenuProperty);
            set => SetValue(TitleBarMenuProperty, value);
        }

        /// <summary>
        /// Activates load mode. 
        /// </summary>
        public bool InLoadMode
        {
            get => GetValue(InLoadModeProperty);
            set => SetValue(InLoadModeProperty, value);
        }

        public bool IsIndeterminate
        {
            get => GetValue(IsIndeterminateProperty);
            set => SetValue(IsIndeterminateProperty, value);
        }

        /// <summary>
        /// Required if the logo is drawn in vector graphics
        /// </summary>
        public Geometry TextIcon
        {
            get => GetValue(TextIconProperty);
            set => SetValue(TextIconProperty, value);
        }

        public int ProgressLoad
        {
            get => GetValue(ProgressLoadProperty);
            set => SetValue(ProgressLoadProperty, value);
        }

        public string ProgressText
        {
            get => GetValue(ProgressTextProperty);
            set => SetValue(ProgressTextProperty, value);
        }

        static OlibWindow() { }

        Type IStyleable.StyleKey => typeof(OlibWindow);

        T GetControl<T>(TemplateAppliedEventArgs e, string name) where T : class => e.NameScope.Get<T>(name);

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            SizeToContent content = SizeToContent;

            ExtendClientAreaToDecorationsHint = true;
            ExtendClientAreaChromeHints = Avalonia.Platform.ExtendClientAreaChromeHints.PreferSystemChrome | Avalonia.Platform.ExtendClientAreaChromeHints.OSXThickTitleBar;
            ExtendClientAreaTitleBarHeightHint = -1;

            SizeToContent = content;

            try
            {
                KeyDown += (s, ep) =>
                {
                    if (ep.KeyModifiers == KeyModifiers.Control && ep.Key == Key.Q) Close();
                };

                if (BottomPanel == null)
                    GetControl<Border>(e, "BottomBorder").IsVisible = false;
            }
            catch { }
        }
    }
}
