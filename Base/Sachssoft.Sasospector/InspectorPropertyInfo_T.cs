using Sachssoft.Sasospector.Schemas;
using System;

namespace Sachssoft.Sasospector
{
    public class InspectorPropertyInfo<TOwner, T> : IInspectorPropertyInfo
        where TOwner : class
    {

        public event EventHandler<InspectorPropertyChangingEventArgs>? Changing;
        public event EventHandler<InspectorPropertyChangedEventArgs>? Changed;

        public InspectorPropertyInfo(
            InspectorSchema<TOwner> schema,
            string name,
            Func<TOwner, T?> getter,
            Action<TOwner, T?>? setter,
            InspectorPropertyInfoMetadata meta)
        {
            Schema = schema ?? throw new ArgumentNullException(nameof(schema));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Getter = getter ?? throw new ArgumentNullException(nameof(getter));
            Setter = setter;
            Metadata = meta ?? throw new ArgumentNullException(nameof(meta));

            //Metadata.Setup(this);
        }

        public string Name { get; }

        public Type Type => typeof(T);

        public InspectorSchema<TOwner> Schema { get; }

        IInspectorSchema IInspectorPropertyInfo.Schema => Schema;

        public InspectorPropertyInfoMetadata Metadata { get; }

        public Func<TOwner, T?> Getter { get; }

        public Action<TOwner, T?>? Setter { get; }

        public bool IsReadOnly => Setter == null;

        public T? GetValue()
        {
            return OnGetting();
        }

        public void SetValue(T? value)
        {
            if (IsReadOnly)
                return;

            var changingEventArgs = new InspectorPropertyChangingEventArgs(this);
            OnChanging(changingEventArgs);
            if (changingEventArgs.Cancel)
                return;

            OnSetting(value);

            OnChanged(new InspectorPropertyChangedEventArgs(this));
        }

        object? IInspectorPropertyInfo.GetValue()
        {
            return GetValue();
        }

        void IInspectorPropertyInfo.SetValue(object? value)
        {
            if (IsReadOnly)
                return;

            if (value is null)
            {
                SetValue(default);
                return;
            }

            if (value is T typed)
            {
                SetValue(typed);
                return;
            }

            throw new InvalidOperationException(
                $"Invalid value type '{value.GetType()}' for property '{Name}'. Expected '{typeof(T)}'.");
        }

        protected virtual T? OnGetting()
        {
            return Getter(Schema.Owner);
        }

        protected virtual void OnSetting(T? value)
        {
            Setter?.Invoke(Schema.Owner, value);
        }

        protected virtual void OnChanging(InspectorPropertyChangingEventArgs e)
            => Changing?.Invoke(Schema, e);

        protected virtual void OnChanged(InspectorPropertyChangedEventArgs e)
            => Changed?.Invoke(Schema, e);

        public override string ToString()
        {
            return $"{Name} ({GetValue()})";
        }
    }
}