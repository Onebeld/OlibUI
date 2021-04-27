using Avalonia.Controls;
using Avalonia.Input;
using System.Collections.Generic;

namespace OlibUI.Dragging
{
    public class ControlObject : IDataObject
    {
        public Control Source { get; private set; }

        public ControlObject(Control data)
        {
            Source = data;
        }

        public IEnumerable<string> GetDataFormats() => new List<string>{ nameof(Control) };

        public bool Contains(string dataFormat)
        {
            switch (dataFormat)
            {
                case nameof(Control):
                    return true;
                default:
                    return false;
            }
        }

        public string GetText() => nameof(Control);

        public IEnumerable<string> GetFileNames() => null;

        public object Get(string dataFormat) => Source;
    }
}
