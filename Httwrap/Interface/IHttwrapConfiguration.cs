namespace Httwrap.Interface
{
    public interface IHttwrapConfiguration
    {
        string BasePath { get; }
        ISerializer Serializer { get; }
    }
}