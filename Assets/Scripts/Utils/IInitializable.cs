namespace Utils
{
    public interface IInitializable<out TResult>
    {
        TResult Initialize();
    }
}