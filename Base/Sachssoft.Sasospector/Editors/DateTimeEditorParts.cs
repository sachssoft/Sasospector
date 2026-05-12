using System;

namespace Sachssoft.Sasospector.Editors
{
    [Flags]
    public enum DateTimeEditorParts
    {
        None = 0,
        Year = 1 << 0,
        Month = 1 << 1,
        Day = 1 << 2,
        Hour = 1 << 3,
        Minute = 1 << 4,
        Second = 1 << 5,
        Millisecond = 1 << 6,
        Nanosecond = 1 << 7,

        Date = Year | Month | Day,
        Time = Hour | Minute | Second | Millisecond,
        DateTime = Date | Time
    }
}
