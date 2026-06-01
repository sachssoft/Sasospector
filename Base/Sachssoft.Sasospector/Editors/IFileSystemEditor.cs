using System.Collections.Generic;

namespace Sachssoft.Sasospector.Editors
{
    public interface IFileSystemEditor : IPropertyEditor
    {
        public FileSystemMode Mode { get; set; }

        public IEnumerable<string>? Filters { get; set; } // z.B. *.txt, *.png
    }
}
