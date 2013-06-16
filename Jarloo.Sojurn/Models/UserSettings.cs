using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Jarloo.Sojurn.Models
{
    [DataContract]
    public class UserSettings
    {
        [DataMember]
        public List<Show> Shows { get; set; }

        public UserSettings()
        {
            Shows = new List<Show>();
        }

        
    }
}