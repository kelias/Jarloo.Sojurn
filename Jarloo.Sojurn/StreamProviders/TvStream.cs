using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Jarloo.Sojurn.StreamProviders
{
    public class TvStream
    {
        [DataContract]
        public enum SeparationCharacter
        {
            Underscore,
            Hyphen,
            HtmlSpaceCode
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Url { get; set; }

        [DataMember]
        public SeparationCharacter SepCharacter { get; set; }

        public TvStream(string name, string url, SeparationCharacter separationCharacter)
        {
            Name = name;
            Url = url;
            SepCharacter = separationCharacter;
        }

    }
}
