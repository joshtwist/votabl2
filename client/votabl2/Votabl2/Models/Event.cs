using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Votabl2.Models
{
    [DataContract(Name="events")]
    public class Event
    {
        public Event()
        {
            ImageUrl = string.Empty;
            EventShareId = string.Empty;
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
        
        [DataMember(Name = "imageUrl")]
        public string ImageUrl { get; set; }

        [DataMember(Name = "eventShareId")]
        public string EventShareId { get; set; }
    }
}
