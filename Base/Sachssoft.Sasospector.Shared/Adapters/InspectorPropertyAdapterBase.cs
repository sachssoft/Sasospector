using System;

namespace Sachssoft.Sasospector.Adapters
{
    public abstract class InspectorPropertyAdapterBase<T>
    {
        protected InspectorPropertyInfo PropertyInfo { get; private set; } = null!;

        public void Bind(InspectorPropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo
                ?? throw new ArgumentNullException(nameof(propertyInfo));
        }

        public bool CanHandle()
        {
            EnsureBound();
            return OnCanHandle();
        }

        public T GetValue()
        {
            EnsureBound();
            return OnGetValue();
        }

        public void SetValue(T value)
        {
            EnsureBound();
            OnSetValue(value);
        }

        protected abstract bool OnCanHandle();

        protected abstract T OnGetValue();

        protected abstract void OnSetValue(T value);

        protected void EnsureBound()
        {
            if (PropertyInfo == null)
                throw new InvalidOperationException(
                    "The adapter is not bound to a property.");
        }
    }
}
