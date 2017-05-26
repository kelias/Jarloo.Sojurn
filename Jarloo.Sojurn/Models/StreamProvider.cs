using System.Runtime.Serialization;

namespace Jarloo.Sojurn.StreamProviders
{
    [DataContract]
    public class StreamProvider
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string SpaceSeperator { get; set; }  
    }
}