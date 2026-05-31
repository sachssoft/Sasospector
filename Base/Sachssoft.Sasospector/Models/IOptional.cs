namespace Sachssoft.Sasospector.Models
{
    public interface IOptional<T>
    {
        bool HasValue { get; set; }
        T? Value { get; set; }
    }
}
