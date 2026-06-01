using Sachssoft.Sasospector.Purposes;
using Sachssoft.Sasospector.Editors;
using System;
using System.IO;

namespace Sachssoft.Sasospector.Purposes
{
    public sealed class FileSystemPurpose : IInspectorPropertyPurpose
    {
        public FileSystemMode Mode { get; init; }

        public bool IsMatch(Type propertyType)
        {
            return propertyType == typeof(string)
                || propertyType == typeof(FileInfo)
                || propertyType == typeof(DirectoryInfo);
        }
        
        public bool IsEquivalentTo(IInspectorPropertyPurpose? other)
        {
            if (other is not FileSystemPurpose otherPurpose)
                return false;

            return Mode == otherPurpose.Mode;
        }
    }
}
