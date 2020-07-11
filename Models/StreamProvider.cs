using System.Runtime.Serialization;

namespace Jarloo.Sojurn.Models
{
    [DataContract]
    public class StreamProvider
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Url { get; set; }

        [DataMember]
        public string SpaceSeperator { get; set; }  
    }
}