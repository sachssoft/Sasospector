using Sachssoft.Sasospector.Editors;

namespace Sachssoft.Sasospector.Constraints
{
    public class FileSystemConstraint : InspectorConstraintBase<string>
    {

        public FileSystemMode Mode { get; init; }

    }
}
