namespace Sachssoft.Sasospector.Adapters
{
    public sealed class StringPropertyAdapter
        : InspectorPropertyAdapterBase<string?>
    {
        protected override bool OnCanHandle()
        {
            return PropertyInfo.Type == typeof(string);
        }

        protected override string? OnGetValue()
        {
            return (string?)PropertyInfo.GetValue();
        }

        protected override void OnSetValue(string? value)
        {
            PropertyInfo.SetValue(value);
        }
    }
}