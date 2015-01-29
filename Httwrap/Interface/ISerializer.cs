namespace Httwrap.Interface
{
    public interface ISerializer
    {
        T Deserialize<T>(string content);
        string Serialize<T>(T data);
    }
}