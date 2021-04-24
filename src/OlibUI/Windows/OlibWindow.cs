using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Styling;
using OlibUI.Controls.Chrome;
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
        public static readonly StyledProperty<bool> CompactModeProperty =
            AvaloniaProperty.Register<OlibWindow, bool>(nameof(CompactMode));
        public static readonly RoutedEvent<RoutedEventArgs> InteractingWithWindowEvent =
            RoutedEvent.Register<OlibWindow, RoutedEventArgs>(nameof(InteractingWithWindow), RoutingStrategies.Bubble);

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

        public bool CompactMode
        {
            get => GetValue(CompactModeProperty);
            set => SetValue(CompactModeProperty, value);
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

        public event EventHandler<RoutedEventArgs> InteractingWithWindow
        {
            add { AddHandler(InteractingWithWindowEvent, value); }
            remove { RemoveHandler(InteractingWithWindowEvent, value); }
        }

        public OlibWindow() { }

        Type IStyleable.StyleKey => typeof(OlibWindow);

        protected virtual void OnInteractingWithWindow()
        {
            RoutedEventArgs e = new RoutedEventArgs(InteractingWithWindowEvent);
            RaiseEvent(e);

            e.Handled = true;;
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            SizeToContent content = SizeToContent;

            ExtendClientAreaToDecorationsHint = true;
            ExtendClientAreaChromeHints = Avalonia.Platform.ExtendClientAreaChromeHints.PreferSystemChrome | Avalonia.Platform.ExtendClientAreaChromeHints.OSXThickTitleBar;
            ExtendClientAreaTitleBarHeightHint = -1;

            SizeToContent = content;

            this.GetObservable(CompactModeProperty).Subscribe(x =>
            {
                e.NameScope.Get<OlibTitleBar>("PART_TitleBar").Height = x ? 23 : 30;
                if (WindowState != WindowState.FullScreen)
                    e.NameScope.Get<ContentPresenter>("PART_ContentWindow").Margin = x ? Thickness.Parse("0 23 0 0") : Thickness.Parse("0 30 0 0");
                else e.NameScope.Get<ContentPresenter>("PART_ContentWindow").Margin = Thickness.Parse("0");
            });
            this.GetObservable(WindowStateProperty).Subscribe(x =>
            {
                if (x == WindowState.FullScreen)
                    e.NameScope.Get<ContentPresenter>("PART_ContentWindow").Margin = Thickness.Parse("0");
                else
                    e.NameScope.Get<ContentPresenter>("PART_ContentWindow").Margin = CompactMode ? Thickness.Parse("0 23 0 0") : Thickness.Parse("0 30 0 0");

                OnInteractingWithWindow();
            });

            PositionChanged += (_, e1) => OnInteractingWithWindow();

            try
            {
                KeyDown += (s, ep) =>
                {
                    if (ep.KeyModifiers == KeyModifiers.Control && ep.Key == Key.Q) Close();
                };

                if (BottomPanel == null)
                    e.NameScope.Get<Border>("BottomBorder").IsVisible = false;
            }
            catch { }
        }
    }
}
