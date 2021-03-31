using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.VisualTree;
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

        /// <summary>
        /// Shows or hides the Expand and Collapse buttons
        /// </summary>
        public WindowButtons WindowButtons
        {
            get => GetValue(WindowButtonsProperty);
            set => SetValue(WindowButtonsProperty, value);
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

        private MenuItem ExpandMenuItem;
        private MenuItem ReestablishMenuItem;
        private MenuItem CollapseMenuItem;
        private Separator ContextMenuSeparator;
        private TextBlock TitleTextBlock;
        private Grid MarginWindow;
        private Path PathIcon;

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            SizeToContent content = SizeToContent;

            ExtendClientAreaToDecorationsHint = true;
            ExtendClientAreaChromeHints = Avalonia.Platform.ExtendClientAreaChromeHints.PreferSystemChrome;
            ExtendClientAreaTitleBarHeightHint = -1;

            SizeToContent = content;

            ReestablishMenuItem = GetControl<MenuItem>(e, "ReestablishMenuItem");
            ExpandMenuItem = GetControl<MenuItem>(e, "ExpandMenuItem");
            CollapseMenuItem = GetControl<MenuItem>(e, "CollapseMenuItem");
            ContextMenuSeparator = GetControl<Separator>(e, "ContextMenuSeparator");
            TitleTextBlock = GetControl<TextBlock>(e, "TitleTextBlock");
            MarginWindow = GetControl<Grid>(e, "MarginWindow");
            PathIcon = GetControl<Path>(e, "PathIcon");

            WindowStateProperty.Changed.Subscribe((x) =>
            {
                if (x.NewValue.Value == WindowState.Maximized)
                {
                    ReestablishMenuItem.IsEnabled = true;
                    ExpandMenuItem.IsEnabled = false;
                    MarginWindow.Margin = Thickness.Parse("7");
                }
                else
                {
                    ReestablishMenuItem.IsEnabled = false;
                    ExpandMenuItem.IsEnabled = true;
                    MarginWindow.Margin = Thickness.Parse("0");
                }
            });

            ReestablishMenuItem.IsEnabled = false;

            try
            {
                Control titleBar = GetControl<Control>(e, "TitleBar");

                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    titleBar.DoubleTapped += (_, e1) =>
                    {
                        if (WindowButtons == WindowButtons.CloseAndExpand || WindowButtons == WindowButtons.All)
                        {
                            WindowState = ((Window)this.GetVisualRoot()).WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
                        }
                    };
                }

                KeyDown += (s, ep) =>
                {
                    if (ep.KeyModifiers == KeyModifiers.Control && ep.Key == Key.Q) Close();
                };

                titleBar.PointerPressed += (s, ep) =>
                {
                    GetControl<ContextMenu>(e, "GlobalContextMenu").Close();
                    PlatformImpl?.BeginMoveDrag(ep);
                };

                PointerReleased += (s, ep) =>
                {
                    GetControl<ContextMenu>(e, "GlobalContextMenu").Close();
                };

                if (BottomPanel == null)
                {
                    GetControl<Border>(e, "BottomBorder").IsVisible = false;
                }

                if (TextIcon == null)
                {
                    PathIcon.IsVisible = false;
                }
                else TitleTextBlock.IsVisible = false;

                Button minimizeButton = GetControl<Button>(e, "MinimizeButton");
                minimizeButton.Click += (s, ep) =>
                {
                    WindowState = WindowState.Minimized;
                };

                Button maximizeButton = GetControl<Button>(e, "MaximizeButton");
                maximizeButton.Click += (s, ep) =>
                {
                    WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
                };

                if (WindowButtons != WindowButtons.All)
                {
                    if (WindowButtons != WindowButtons.CloseAndCollapse) minimizeButton.IsVisible = false;
                    if (WindowButtons != WindowButtons.CloseAndExpand) maximizeButton.IsVisible = false;
                }

                GetControl<Button>(e, "CloseButton").Click += (s, ep) => Close();

                ReestablishMenuItem.Click += (s, ep) => WindowState = WindowState.Normal;
                ExpandMenuItem.Click += (s, ep) => WindowState = WindowState.Maximized;
                CollapseMenuItem.Click += (s, ep) => WindowState = WindowState.Minimized;

                GetControl<MenuItem>(e, "CloseMenuItem").Click += (s, ep) => Close();

                if (WindowButtons == WindowButtons.CloseAndCollapse)
                {
                    ExpandMenuItem.IsVisible = false;
                    ReestablishMenuItem.IsVisible = false;
                }
                else if (WindowButtons == WindowButtons.CloseAndExpand)
                {
                    CollapseMenuItem.IsVisible = false;
                }
                else if (WindowButtons == WindowButtons.Close)
                {
                    ExpandMenuItem.IsVisible = false;
                    ReestablishMenuItem.IsVisible = false;
                    CollapseMenuItem.IsVisible = false;
                    ContextMenuSeparator.IsVisible = false;
                }
            }
            catch { }
        }
    }
}
