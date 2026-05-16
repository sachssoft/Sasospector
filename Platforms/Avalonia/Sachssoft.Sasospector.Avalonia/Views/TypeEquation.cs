namespace Sachssoft.Sasospector.Views
{
    public enum TypeEquation
    {
        Exact,         // 1:1 gleich (==)
        Assignable,    // IsAssignableFrom (vererbbar / Interface)
        Strict,        // wie Exact, aber ohne Nullable/Boxing-Logik
        Compatible     // erweitert (Enum, Nullable, primitive Konvertierungen)
    }
}
