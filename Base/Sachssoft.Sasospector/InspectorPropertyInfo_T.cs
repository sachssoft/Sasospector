using Sachssoft.Sasospector.Schemas;
using System;

namespace Sachssoft.Sasospector
{
    public class InspectorPropertyInfo<TSource> : IInspectorPropertyInfo
        where TSource : class
    {
        private readonly Type _type;

        public event EventHandler<InspectorPropertyChangingEventArgs>? ValueChanging;
        public event EventHandler<InspectorPropertyChangedEventArgs>? ValueChanged;

        public InspectorPropertyInfo(
            InspectorSchema schema,
            string name,
            Type type,
            GetterDelegate<TSource> getter,
            SetterDelegate<TSource>? setter,
            InspectorPropertyInfoMetadata meta)
        {
            Schema = schema ?? throw new ArgumentNullException(nameof(schema));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            _type = type ?? throw new ArgumentNullException(nameof(type));
            Getter = getter ?? throw new ArgumentNullException(nameof(getter));
            Setter = setter;
            Metadata = meta ?? throw new ArgumentNullException(nameof(meta));
        }

        public string Name { get; }

        public Type Type => _type;

        public InspectorSchema Schema { get; }

        IInspectorSchema IInspectorPropertyInfo.Schema => Schema;

        public InspectorPropertyInfoMetadata Metadata { get; }

        public GetterDelegate<TSource> Getter { get; }

        public SetterDelegate<TSource>? Setter { get; }

        public bool IsReadOnly => Setter == null;

        public object? GetValue(TSource source)
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

        object? IInspectorPropertyInfo.GetValue(object source)
        {
            if (source is not TSource typedSource)
            {
                throw new InvalidOperationException(
                    $"Invalid source type '{source.GetType()}' for '{Name}'. Expected '{typeof(TSource)}'.");
            }

            return GetValue(typedSource);
        }

        public void SetValue(TSource source, object? value)
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

        void IInspectorPropertyInfo.SetValue(object source, object? value)
        {
            if (source is not TSource typedSource)
            {
                throw new InvalidOperationException(
                    $"Invalid source type '{source.GetType()}' for '{Name}'. Expected '{typeof(TSource)}'.");
            }

            SetValue(typedSource, value);
        }

        protected virtual object? OnGetting(TSource source, Type type)
        {
            return Getter.Invoke(source, type);
        }

        protected virtual void OnSetting(TSource source, Type type, object? value)
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