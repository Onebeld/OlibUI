using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Media.Imaging;
using OlibUI.Windows;
using System;
using System.IO;
using System.Reactive.Disposables;

namespace OlibUI.Controls.Chrome
{
    [PseudoClasses(":minimized", ":normal", ":maximized", ":fullscreen", ":isactive")]
    public class TitleBar : TemplatedControl
    {
        private CompositeDisposable _disposables;
        private CaptionButtons _captionButtons;
        private Image _image;
        private TextBlock _title;
        private Avalonia.Controls.Shapes.Path _textIcon;
        private ContentPresenter _titleBarMenu;
        private Border _dragWindow;
        private ContextMenu _contextMenu;

        private MenuItem _closeMenuItem;
        private MenuItem _collapseMenuItem;
        private MenuItem _expandMenuItem;
        private MenuItem _reestablishMenuItem;
        private Separator _separator;


        private void UpdateSize(OlibWindow window)
        {
            if (window != null)
            {
                if (window.WindowState != WindowState.FullScreen)
                {
                    Height = 30;

                    if (_captionButtons != null)
                    {
                        _captionButtons.Height = Height;
                    }
                }

                IsVisible = window.PlatformImpl.NeedsManagedDecorations;
            }
        }

        private void AttachIcon(OlibWindow window)
        {
            if (_image == null) return;

            if (window.Icon != null)
            {
                WindowIcon wIcon = window.Icon;
                MemoryStream stream = new MemoryStream();
                wIcon.Save(stream);
                stream.Position = 0;
                try
                {
                    _image.Source = new Bitmap(stream);
                }
                catch
                {
                    _image.Source = null;
                }
            }
            else
                _image.Source = null;
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            _captionButtons?.Detach();

            _captionButtons = e.NameScope.Get<CaptionButtons>("PART_CaptionButtons");
            _image = e.NameScope.Get<Image>("PART_Icon");
            _title = e.NameScope.Get<TextBlock>("PART_Title");
            _textIcon = e.NameScope.Get<Avalonia.Controls.Shapes.Path>("PART_TextIcon");
            _titleBarMenu = e.NameScope.Get<ContentPresenter>("PART_TitleBarMenu");
            _dragWindow = e.NameScope.Get<Border>("PART_DragWindow");
            _contextMenu = e.NameScope.Get<ContextMenu>("PART_ContextMenu");

            _closeMenuItem = e.NameScope.Get<MenuItem>("PART_CloseMenuItem");
            _collapseMenuItem = e.NameScope.Get<MenuItem>("PART_CollapseMenuItem");
            _expandMenuItem = e.NameScope.Get<MenuItem>("PART_ExpandMenuItem");
            _reestablishMenuItem = e.NameScope.Get<MenuItem>("PART_ReestablishMenuItem");
            _separator = e.NameScope.Get<Separator>("PART_Separator");

            if (VisualRoot is OlibWindow window)
            {
                _captionButtons.HostWindow = window;

                _closeMenuItem.Click += (_, e1) => window.Close();
                _reestablishMenuItem.Click += (_, e1) => window.WindowState = WindowState.Normal;
                _expandMenuItem.Click += (_, e1) => window.WindowState = WindowState.Maximized;
                _collapseMenuItem.Click += (_, e1) => window.WindowState = WindowState.Minimized;

                _dragWindow.PointerPressed += (_, e1) =>
                {
                    if (window.WindowState != WindowState.FullScreen)
                    {
                        _contextMenu.Close();
                        window.PlatformImpl?.BeginMoveDrag(e1);
                    }
                };
                _dragWindow.DoubleTapped += (_, e1) =>
                {
                    if (window.WindowButtons == WindowButtons.CloseAndExpand || window.WindowButtons == WindowButtons.All)
                        window.WindowState = window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
                };
                window.PointerReleased += (_, e1) => _contextMenu.Close();

                UpdateSize(window);
            }

            Attach();
        }

        private void Attach()
        {
            if (VisualRoot is OlibWindow window)
            {
                _disposables = new CompositeDisposable
                {
                    window.GetObservable(OlibWindow.WindowDecorationMarginProperty)
                        .Subscribe(x => UpdateSize(window)),
                    window.GetObservable(OlibWindow.ExtendClientAreaTitleBarHeightHintProperty)
                        .Subscribe(x => UpdateSize(window)),
                    window.GetObservable(OlibWindow.OffScreenMarginProperty)
                        .Subscribe(x => UpdateSize(window)),
                    window.GetObservable(OlibWindow.ExtendClientAreaChromeHintsProperty)
                        .Subscribe(x => UpdateSize(window)),
                    window.GetObservable(OlibWindow.WindowStateProperty)
                        .Subscribe(x =>
                        {
                            PseudoClasses.Set(":minimized", x == WindowState.Minimized);
                            PseudoClasses.Set(":normal", x == WindowState.Normal);
                            PseudoClasses.Set(":maximized", x == WindowState.Maximized);
                            PseudoClasses.Set(":fullscreen", x == WindowState.FullScreen);

                            if (x == WindowState.Maximized)
                            {
                                _reestablishMenuItem.IsEnabled = true;
                                _expandMenuItem.IsEnabled = false;
                            }
                            else
                            {
                                _reestablishMenuItem.IsEnabled = false;
                                _expandMenuItem.IsEnabled = true;
                            }

                            if (x == WindowState.FullScreen)
                            {
                                _reestablishMenuItem.IsEnabled = false;
                                _expandMenuItem.IsEnabled = false;
                                _collapseMenuItem.IsEnabled = false;
                            }
                        }),
                    window.GetObservable(OlibWindow.IsActiveProperty)
                        .Subscribe(x =>
                        {
                            PseudoClasses.Set(":isactive", !x);
                        }),
                    window.GetObservable(OlibWindow.IsExtendedIntoWindowDecorationsProperty)
                        .Subscribe(x => UpdateSize(window)),
                    window.GetObservable(OlibWindow.IconProperty)
                        .Subscribe(x => AttachIcon(window)),
                    window.GetObservable(OlibWindow.TitleProperty)
                        .Subscribe(x =>
                        {
                            _title.Text = x;
                        }),
                    window.GetObservable(OlibWindow.TextIconProperty)
                        .Subscribe(x =>
                        {
                            _textIcon.Data = x;
                            _textIcon.IsVisible = _textIcon.Data == null ? false : true;
                            _title.IsVisible = _textIcon.Data == null ? true : false;
                        }),
                    window.GetObservable(OlibWindow.TitleBarMenuProperty)
                        .Subscribe(x =>
                        {
                            _titleBarMenu.Content = x;
                        }),
                    window.GetObservable(OlibWindow.WindowButtonsProperty)
                        .Subscribe(x =>
                        {
                            if (x == WindowButtons.All)
                            {
                                _expandMenuItem.IsVisible = true;
                                _reestablishMenuItem.IsVisible = true;
                                _collapseMenuItem.IsVisible = true;
                                _separator.IsVisible = true;
                            }
                            else if (x == WindowButtons.CloseAndCollapse)
                            {
                                _expandMenuItem.IsVisible = false;
                                _reestablishMenuItem.IsVisible = false;
                                _collapseMenuItem.IsVisible = true;
                                _separator.IsVisible = true;
                            }
                            else if (x == WindowButtons.CloseAndExpand)
                            {
                                _expandMenuItem.IsVisible = true;
                                _reestablishMenuItem.IsVisible = true;
                                _collapseMenuItem.IsVisible = false;
                                _separator.IsVisible = true;
                            }
                            else if (x == WindowButtons.Close)
                            {
                                _expandMenuItem.IsVisible = false;
                                _reestablishMenuItem.IsVisible = false;
                                _collapseMenuItem.IsVisible = false;
                                _separator.IsVisible = false;
                            }
                        })
                };
            }
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);

            _disposables?.Dispose();

            _captionButtons?.Detach();
            _captionButtons = null;
        }
    }
}
