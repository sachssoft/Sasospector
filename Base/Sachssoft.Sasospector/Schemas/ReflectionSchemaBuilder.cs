using System;

namespace Sachssoft.Sasospector.Schemas
{
    public class ReflectionSchemaBuilder
    {
        private readonly object _owner;

        public ReflectionSchemaBuilder(object owner)
        {
            _owner = owner ?? throw new ArgumentNullException(nameof(owner));
        }

        public object Owner => _owner;
    }
}
