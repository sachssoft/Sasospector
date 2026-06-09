using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Sachssoft.Sasospector.Schemas
{
    public class InspectorSchemaSynchronizedEventArgs : EventArgs
    {
        public InspectorSchemaSynchronizedEventArgs(string propertyName)
        {
            PropertyName = propertyName;
        }

        public string PropertyName { get; }
    }
}
