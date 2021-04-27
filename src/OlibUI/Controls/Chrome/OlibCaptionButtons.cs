using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using OlibUI.Windows;
using System;
using System.Reactive.Disposables;

namespace OlibUI.Controls.Chrome
{
    [PseudoClasses(":minimized", ":normal", ":maximized", ":fullscreen", ":isactive")]
    public class OlibCaptionButtons : TemplatedControl
    {
        public OlibWindow HostWindow;
        private CompositeDisposable _disposables;

        private Button _maximizeButton;
        private Button _minimizeButton;
        private Button _fullscreenButton;

        public void Detach()
        {
            if (_disposables != null)
            {
                _disposables.Dispose();
                _disposables = null;
                HostWindow = null;
            }
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            e.NameScope.Get<Button>("PART_CloseButton").Click += (_, e1) => HostWindow?.Close();

            _maximizeButton = e.NameScope.Get<Button>("PART_MaximizeButton");
            _minimizeButton = e.NameScope.Get<Button>("PART_MinimizeButton");
            _fullscreenButton = e.NameScope.Get<Button>("PART_FullScreenButton");

            _maximizeButton.Click += (_, e1) =>
            {
                if (HostWindow != null) HostWindow.WindowState = HostWindow.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            };

            _minimizeButton.Click += (_, e1) =>
            {
                if (HostWindow != null) HostWindow.WindowState = WindowState.Minimized;
            };

            _fullscreenButton.Click += (_, e1) =>
            {
                if (HostWindow != null) HostWindow.WindowState = HostWindow.WindowState == WindowState.FullScreen ? WindowState.Normal : WindowState.FullScreen;
            };

            if (_disposables == null)
            {
                _disposables = new CompositeDisposable
                {
                    HostWindow.GetObservable(OlibWindow.WindowStateProperty).Subscribe(x =>
                    {
                        PseudoClasses.Set(":minimized", x == WindowState.Minimized);
                        PseudoClasses.Set(":normal", x == WindowState.Normal);
                        PseudoClasses.Set(":maximized", x == WindowState.Maximized);
                        PseudoClasses.Set(":fullscreen", x == WindowState.FullScreen);
                    }),
                    HostWindow.GetObservable(OlibWindow.IsActiveProperty).Subscribe(x =>
                    {
                        PseudoClasses.Set(":isactive", !x);
                    }),
                    HostWindow.GetObservable(OlibWindow.WindowButtonsProperty).Subscribe(x =>
                    {
                        if (x != WindowButtons.All)
                        {
                            if (x != WindowButtons.CloseAndCollapse) _minimizeButton.IsVisible = false; 
                            if (x != WindowButtons.CloseAndExpand) _maximizeButton.IsVisible = false; 
                        }
                    }),
                    HostWindow.GetObservable(OlibWindow.FullScreenButtonProperty).Subscribe(x =>
                    {
                        if (_fullscreenButton != null) _fullscreenButton.IsVisible = x;
                    })
                };
            }
        }
    }
}
