using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Votabl2.Models
{
    public class RawVote
    {
        [DataMember]
        public string EventShareId { get; set; }

        [DataMember]
        public int Id { get; set; }
    }
}
