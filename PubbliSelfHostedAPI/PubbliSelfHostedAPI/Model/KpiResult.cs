using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PubbliSelfHostedAPI.Model
{
    [DataContract]
    public class KpiResult
    {
        [DataMember]
        public string TotaleCliente { get; set; }
        [DataMember]
        public string TotaleAgenzia { get; set; }

        //Totale Annuo
        [DataMember]
        public string AnnoPrecedenteAgenzia { get; set; }
        [DataMember]
        public string AnnoCorrenteAgenzia { get; set; }
        [DataMember]
        public string PariDataAgenzia { get; set; }
        [DataMember]
        public string AnnoPrecedenteCliente { get; set; }
        [DataMember]
        public string AnnoCorrenteCliente { get; set; }
        [DataMember]
        public string PariDataCliente { get; set; }

        //mese
        [DataMember]
        public string AnnoPrecedenteAgenziaMese { get; set; }
        [DataMember]
        public string AnnoCorrenteAgenziaMese { get; set; }
        [DataMember]
        public string PariDataAgenziaMese { get; set; }
        [DataMember]
        public string AnnoPrecedenteClienteMese { get; set; }
        [DataMember]
        public string AnnoCorrenteClienteMese { get; set; }
        [DataMember]
        public string PariDataClienteMese { get; set; }
        
    }
}
