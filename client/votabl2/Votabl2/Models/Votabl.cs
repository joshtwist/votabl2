using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Votabl2.Models
{
    [DataContract(Name = "votabls")]
    public class Votabl
    {
        public Votabl()
        {
            ImageUrl = string.Empty;
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int EventId { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "imageUrl")]
        public string ImageUrl { get; set; }
    }
}
