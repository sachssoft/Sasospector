using System.Collections.Generic;

namespace Sachssoft.Sasospector.Constraints
{
    public interface IInstanceSelectionConstraint : IInspectorConstraint
    {
        bool AllowNull { get; set; }

        int DefaultItemIndex { get; set; }

        // Wenn true, werden nur Instanzen unterschiedlicher Typen berücksichtigt (keine doppelten Typen in der Liste).
        bool DistinctTypesOnly { get; set; }


        // Fügt fehlende Instanzen automatisch hinzu.
        // Hat keine Wirkung, wenn DistinctTypesOnly aktiviert ist.
        bool AutoAddMissingInstances { get; set; }

        IReadOnlyList<object?>? Instances { get; }
    }
}
