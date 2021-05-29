using System.Runtime.Serialization;
using Avalonia.Media;

namespace OlibUI.Structures
{
    [DataContract]
    public class Theme : ViewModelBase
    {
        private string _name = string.Empty;

        private Color _backgroundColor;
        private Color _hoverBackgroundColor;
        private Color _foregroundColor;
        private Color _foregroundOpacityColor;
        private Color _pressedForegroundColor;
        private Color _accentColor;
        private Color _borderBackgroundColor;
        private Color _borderColor;
        private Color _windowBorderColor;
        private Color _notActiveWindowBorderColor;
        private Color _hoverScrollBoxColor;
        private Color _scrollBoxColor;
        private Color _errorColor;
        private Color _windowBorderBackgroundColor;
        
        [DataMember]
        public string Name
        {
            get => _name;
            set => RaiseAndSetIfChanged(ref _name, value);
        }
        
        [DataMember]
        public Color BackgroundColor
        {
            get => _backgroundColor;
            set => RaiseAndSetIfChanged(ref _backgroundColor, value);
        }
        [DataMember]
        public Color HoverBackgroundColor
        {
            get => _hoverBackgroundColor;
            set => RaiseAndSetIfChanged(ref _hoverBackgroundColor, value);
        }
        [DataMember]
        public Color ForegroundColor
        {
            get => _foregroundColor;
            set => RaiseAndSetIfChanged(ref _foregroundColor, value);
        }
        [DataMember]
        public Color ForegroundOpacityColor
        {
            get => _foregroundOpacityColor;
            set => RaiseAndSetIfChanged(ref _foregroundOpacityColor, value);
        }
        [DataMember]
        public Color PressedForegroundColor
        {
            get => _pressedForegroundColor;
            set => RaiseAndSetIfChanged(ref _pressedForegroundColor, value);
        }
        [DataMember]
        public Color AccentColor
        {
            get => _accentColor;
            set => RaiseAndSetIfChanged(ref _accentColor, value);
        }
        [DataMember]
        public Color BorderBackgroundColor
        {
            get => _borderBackgroundColor;
            set => RaiseAndSetIfChanged(ref _borderBackgroundColor, value);
        }
        [DataMember]
        public Color BorderColor
        {
            get => _borderColor;
            set => RaiseAndSetIfChanged(ref _borderColor, value);
        }
        [DataMember]
        public Color WindowBorderColor
        {
            get => _windowBorderColor;
            set => RaiseAndSetIfChanged(ref _windowBorderColor, value);
        }
        [DataMember]
        public Color NotActiveWindowBorderColor
        {
            get => _notActiveWindowBorderColor;
            set => RaiseAndSetIfChanged(ref _notActiveWindowBorderColor, value);
        }
        [DataMember]
        public Color HoverScrollBoxColor
        {
            get => _hoverScrollBoxColor;
            set => RaiseAndSetIfChanged(ref _hoverScrollBoxColor, value);
        }
        [DataMember]
        public Color ScrollBoxColor
        {
            get => _scrollBoxColor;
            set => RaiseAndSetIfChanged(ref _scrollBoxColor, value);
        }
        [DataMember]
        public Color ErrorColor
        {
            get => _errorColor;
            set => RaiseAndSetIfChanged(ref _errorColor, value);
        }
        [DataMember]
        public Color WindowBorderBackgroundColor
        {
            get => _windowBorderBackgroundColor;
            set => RaiseAndSetIfChanged(ref _windowBorderBackgroundColor, value);
        }
    }
}
