using System.Net.Http;

namespace Httwrap
{
    internal interface IRequestContent
    {
        HttpContent GetContent();
    }
}