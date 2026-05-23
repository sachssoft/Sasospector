using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sachssoft.Sasospector.Schemas
{
    public interface IInspectorSchemaSource
    {
        IInspectorSchema Resolve(object? model);
    }
}
