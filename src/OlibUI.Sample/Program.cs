using Avalonia;
using Avalonia.Controls;
using Avalonia.Rendering;
using OlibUI.Sample.Structures;
using OlibUI.Sample.Views;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace OlibUI.Sample
{
    class Program
    {
        private static readonly object Sync = new object();

        public static Settings Settings;

        public static MainWindow MainWindow { get; set; }

        public static Stopwatch sw;

        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Settings = File.Exists(AppDomain.CurrentDomain.BaseDirectory + "settings.json")
                ? FileSettings.LoadSettings()
                : new Settings();
            sw = Stopwatch.StartNew();
            BuildAvaloniaApp().Start(AppMain, args);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string pathToLog = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
            if (!Directory.Exists(pathToLog)) Directory.CreateDirectory(pathToLog);

            if (e.ExceptionObject is Exception ex)
            {
                string filename = $"{AppDomain.CurrentDomain.FriendlyName}_{DateTime.Now:dd.MM.yyy}.log";

                lock (Sync) File.AppendAllText(Path.Combine(pathToLog, filename),
                        $"[{DateTime.Now:dd.MM.yyy HH:mm:ss.fff}] | Fatal | [{ex.TargetSite.DeclaringType}.{ex.TargetSite.Name}()] {ex}\r\n",
                        Encoding.UTF8);

                Process.Start(Path.Combine(pathToLog, filename));
            }
        }

        private static AppBuilder BuildAvaloniaApp()
        {
            var result = AppBuilder.Configure<App>();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                result
                    .UseWin32()
                    .UseSkia();

                if (DwmIsCompositionEnabled(out bool dwmEnabled) == 0 && dwmEnabled)
                {
                    var wp = result.WindowingSubsystemInitializer;
                    result.UseWindowingSubsystem(() =>
                    {
                        wp();
                        AvaloniaLocator.CurrentMutable.Bind<IRenderTimer>().ToConstant(new WindowsDWMRenderTimer());
                    });
                }
            }
            else
            {
                result.UsePlatformDetect();
            }
            return result
                .LogToTrace()
                .With(new Win32PlatformOptions { AllowEglInitialization = true, UseDeferredRendering = true, OverlayPopups = false, UseWgl = false })
                .With(new MacOSPlatformOptions { ShowInDock = true })
                .With(new AvaloniaNativePlatformOptions { UseGpu = true, UseDeferredRendering = true, OverlayPopups = false })
                .With(new X11PlatformOptions { UseGpu = true, UseEGL = true, OverlayPopups = false });
        }

        private static void AppMain(Application app, string[] args)
        {
            MainWindow = new MainWindow();

            app.Run(MainWindow);
        }

        [DllImport("Dwmapi.dll")]
        private static extern int DwmIsCompositionEnabled(out bool enabled);
    }

    public class WindowsDWMRenderTimer : IRenderTimer
    {
        public event Action<TimeSpan> Tick;
        private Thread _renderTick;

        public WindowsDWMRenderTimer()
        {
            _renderTick = new Thread(() =>
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                while (true)
                {
                    DwmFlush();
                    Tick?.Invoke(sw.Elapsed);
                }
            })
            {
                IsBackground = true,
                Priority = ThreadPriority.Highest
            };
            _renderTick.Start();
        }

        [DllImport("Dwmapi.dll")]
        private static extern int DwmFlush();
    }
}
