using System.Runtime.Serialization;

namespace Httwrap.Tests
{
    public class FilterRequest
    {
        public string Category { get; set; }
        public int NumberOfItems { get; set; }
    }

    public class AttributeRequest
    {
        [DataMember(Name = "cat")]
        public string Category { get; set; }


        public int NumberOfItems { get; set; }
    }

    public class IgnoreAttributeRequest
    {
        public string Category { get; set; }

        [IgnoreDataMember]
        public int NumberOfItems { get; set; }
    }
}