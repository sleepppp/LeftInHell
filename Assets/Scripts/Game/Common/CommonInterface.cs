
namespace Project
{
    public interface IClone<T> where T : class
    {
        T Clone();
    }
}