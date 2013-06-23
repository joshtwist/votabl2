using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Votabl2.Common;

namespace Votabl2.Models
{
    [DataContract(Name = "votabls")]
    public class Votabl : ViewModel
    {
        public Votabl()
        {
            ImageUrl = string.Empty;
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string EventShareId{ get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string ImageUrl { get; set; }

        private int _count;

        public int Count
        {
            get { return _count; }
            set
            {
                SetValue(ref _count, value, "Count");
            }
        }
    }
}
