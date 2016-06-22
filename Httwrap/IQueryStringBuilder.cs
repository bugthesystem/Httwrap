namespace Httwrap
{
    internal interface IQueryStringBuilder
    {
        string BuildFrom<T>(T payload, string separator = ",");
    }
}