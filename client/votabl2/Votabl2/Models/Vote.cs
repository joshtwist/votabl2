using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Votabl2.Models
{
    [DataContract(Name = "votes")]
    public class Vote
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember(Name = "votablId")]
        public int VotablId { get; set; }
    }
}
