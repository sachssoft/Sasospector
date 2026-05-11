namespace Sachssoft.Sasospector
{
    public sealed class InspectorValueKind
    {
        //
        public static readonly InspectorValueKind Auto = new(nameof(Auto));

        //
        public static readonly InspectorValueKind Text = new(nameof(Text));
        public static readonly InspectorValueKind Spinner = new(nameof(Spinner));
        public static readonly InspectorValueKind Range = new(nameof(Range));
        public static readonly InspectorValueKind Dropdown = new(nameof(Dropdown));
        public static readonly InspectorValueKind List = new(nameof(List));
        public static readonly InspectorValueKind Checkbox = new(nameof(Checkbox));
        public static readonly InspectorValueKind Switch = new(nameof(Switch));

        public InspectorValueKind(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
