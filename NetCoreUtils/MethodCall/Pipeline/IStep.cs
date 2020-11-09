namespace NetCoreUtils.MethodCall.Pipeline
{
    public interface IStep<T>
    {
        bool Execute(T msg);
    }
}