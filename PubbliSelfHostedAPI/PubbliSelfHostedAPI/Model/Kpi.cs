using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PubbliSelfHostedAPI.Model
{
    [DataContract]
    public class Kpi
    {
        [DataMember]
        public string AnnoPrecedente { get; set; }
        [DataMember]
        public string PariData { get; set; }
        [DataMember]
        public string AnnoCorrente { get; set; }
    }
}
