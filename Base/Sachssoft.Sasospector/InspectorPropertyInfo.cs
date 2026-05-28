using Sachssoft.Sasospector.Schemas;
using System;

namespace Sachssoft.Sasospector
{
    public class InspectorPropertyInfo : IInspectorPropertyInfo
    {
        private readonly Type _type;

        public event EventHandler<InspectorPropertyChangingEventArgs>? ValueChanging;
        public event EventHandler<InspectorPropertyChangedEventArgs>? ValueChanged;

        public InspectorPropertyInfo(
            InspectorSchema schema,
            string name,
            Type type,
            GetterDelegate getter,
            SetterDelegate? setter,
            InspectorPropertyInfoMetadata meta)
        {
            Schema = schema ?? throw new ArgumentNullException(nameof(schema));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            _type = type ?? throw new ArgumentNullException(nameof(type));
            Getter = getter ?? throw new ArgumentNullException(nameof(getter));
            Setter = setter;
            Metadata = meta ?? throw new ArgumentNullException(nameof(meta));

            //Metadata.Setup(this);
        }

        public string Name { get; }

        public Type Type => _type;

        public InspectorSchema Schema { get; }

        IInspectorSchema IInspectorPropertyInfo.Schema => Schema;

        public InspectorPropertyInfoMetadata Metadata { get; }

        public GetterDelegate Getter { get; }

        public SetterDelegate? Setter { get; }

        public bool IsReadOnly => Setter == null;

        public object? GetValue(object source)
        {
            source = source ?? throw new ArgumentNullException(nameof(source));

            var value = OnGetting(source, _type);

            if (value != null && !_type.IsInstanceOfType(value))
            {
                throw new InvalidOperationException(
                    $"Getter returned '{value.GetType()}', expected '{_type}' for '{Name}'.");
            }

            return value;
        }

        public void SetValue(object source, object? value)
        {
            source = source ?? throw new ArgumentNullException(nameof(source));

            if (IsReadOnly)
                return;

            if (value != null && !_type.IsInstanceOfType(value))
            {
                throw new InvalidOperationException(
                    $"Invalid value type '{value.GetType()}' for '{Name}'. Expected '{_type}'.");
            }

            var changingEventArgs = new InspectorPropertyChangingEventArgs(this);
            OnChanging(changingEventArgs);
            if (changingEventArgs.Cancel)
                return;

            OnSetting(source, _type, value);

            OnChanged(new InspectorPropertyChangedEventArgs(this));
        }

        protected virtual object? OnGetting(object source, Type type)
        {
            return Getter.Invoke(source, type);
        }

        protected virtual void OnSetting(object source, Type type, object? value)
        {
            Setter?.Invoke(source, type, value);
        }

        protected virtual void OnChanging(InspectorPropertyChangingEventArgs e)
            => ValueChanging?.Invoke(this, e);

        protected virtual void OnChanged(InspectorPropertyChangedEventArgs e)
            => ValueChanged?.Invoke(this, e);

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}