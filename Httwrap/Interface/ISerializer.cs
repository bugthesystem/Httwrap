namespace Httwrap.Interface
{
    public interface ISerializer
    {
        T DeserializeObject<T>(string json);
        string SerializeObject<T>(T value);
    }
}