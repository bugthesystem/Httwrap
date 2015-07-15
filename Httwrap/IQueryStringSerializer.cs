namespace Httwrap
{
    internal interface IQueryStringSerializer
    {
        string Serialize<T>(T payload, string separator = ",");
    }
}