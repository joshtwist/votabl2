using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Votabl2.Common;

namespace Votabl2.Models
{
    [DataContract(Name="events")]
    public class Event : ViewModel
    {
        public Event()
        {
            ImageUrl = string.Empty;
            EventShareId = string.Empty;
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string ImageUrl { get; set; }

        [DataMember]
        public string EventShareId { get; set; }


    }
}
