using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PubbliSelfHostedAPI.Model;
using System.Web.Http.Cors;
using System.Xml.Linq;
using System.Globalization;
using System.Configuration;

/*
 * http://www.asp.net/web-api/overview/security/enabling-cross-origin-requests-in-web-api
 */

namespace PubbliSelfHostedAPI.Controller
{

    //setting route prefix for the api
    [RoutePrefix("api/kpis")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PubbliController : ApiController
    {

        static string url = ConfigurationManager.AppSettings["urlServices"].ToString();
        static string un = ConfigurationManager.AppSettings["un"].ToString();
        static string pw = ConfigurationManager.AppSettings["pw"].ToString();
        static string domain = ConfigurationManager.AppSettings["domain"].ToString();


        [Route("getall")]
        [HttpGet]
        public KpiResult GetAll(string userid)
        {

            KpiResult res = new KpiResult();


#if !DEBUG
            KPIMovimentiGiorno service = new KPIMovimentiGiorno();
            service.Url = url;
            service.Credentials = new NetworkCredential(un, pw, domain);
            var result = service.GetKPI(userid);
#else
            var result = File.ReadAllText("data.xml");
#endif



            #region Parse and reduce XML

            string monthName = DateTime.Now.GetMonthName();

            XDocument xdoc = XDocument.Parse(result);
            var cAnno = (from doc in ReduceXml(xdoc, "Clienti")
                         where doc.Element("KpiCommissionato").Value == "Totale Annuo"
                         select new
                         {
                             AnnoPrecedente = doc.Element("AnnoPrecedente").Value,
                             AnnoCorrente = doc.Element("AnnoCorrente").Value,
                             PariData = doc.Element("AnnoPrecedentePD").Value

                         }).FirstOrDefault();

            var cMese = (from doc in ReduceXml(xdoc, "Clienti")
                         where doc.Element("KpiCommissionato").Value.ToLower() == monthName
                         select new
                         {
                             AnnoPrecedente = doc.Element("AnnoPrecedente").Value,
                             AnnoCorrente = doc.Element("AnnoCorrente").Value,
                             PariData = doc.Element("AnnoPrecedentePD").Value

                         }).FirstOrDefault();

            var aAnno = (from doc in ReduceXml(xdoc, "Agenzie")
                         where doc.Element("KpiCommissionato").Value == "Totale Annuo"
                         select new
                         {
                             AnnoPrecedente = doc.Element("AnnoPrecedente").Value,
                             AnnoCorrente = doc.Element("AnnoCorrente").Value,
                             PariData = doc.Element("AnnoPrecedentePD").Value

                         }).FirstOrDefault();

            var aMese = (from doc in ReduceXml(xdoc, "Agenzie")
                         where doc.Element("KpiCommissionato").Value.ToLower() == monthName
                         select new
                         {
                             AnnoPrecedente = doc.Element("AnnoPrecedente").Value,
                             AnnoCorrente = doc.Element("AnnoCorrente").Value,
                             PariData = doc.Element("AnnoPrecedentePD").Value

                         }).FirstOrDefault();

            #endregion

            res.TotaleAgenzia = xdoc.Descendants("TotaleMovimentiByClienti").First().Value.ToUnformatString();
            res.TotaleCliente = xdoc.Descendants("TotaleMovimentiByAgenzie").First().Value.ToUnformatString();
            //Anno
            res.AnnoPrecedenteCliente = cAnno.AnnoPrecedente.ToUnformatString();
            res.AnnoCorrenteCliente = cAnno.AnnoCorrente.ToUnformatString();
            res.PariDataCliente = cAnno.PariData.ToUnformatString();

            res.AnnoPrecedenteAgenzia = aAnno.AnnoPrecedente.ToUnformatString();
            res.AnnoCorrenteAgenzia = aAnno.AnnoCorrente.ToUnformatString();
            res.PariDataAgenzia = aAnno.PariData.ToUnformatString();

            //Mese
            res.AnnoPrecedenteClienteMese = cMese.AnnoPrecedente.ToUnformatString();
            res.AnnoCorrenteClienteMese = cMese.AnnoCorrente.ToUnformatString();
            res.PariDataClienteMese = cMese.PariData.ToUnformatString();

            res.AnnoPrecedenteAgenziaMese = aMese.AnnoPrecedente.ToUnformatString();
            res.AnnoCorrenteAgenziaMese = aMese.AnnoCorrente.ToUnformatString();
            res.PariDataAgenziaMese = aMese.PariData.ToUnformatString();


            ////deserialize string
            //XmlSerializer ser = new XmlSerializer(typeof(PubbliSelfHostedAPI.Service.KPI));
            //PubbliSelfHostedAPI.Service.KPI ret;
            //using (TextReader reader = new StringReader(result))
            //{
            //    ret = (PubbliSelfHostedAPI.Service.KPI)ser.Deserialize(reader);
            //}


            //return ret;


            //doc.LoadXml(result);
            //string jsonText = JsonConvert.SerializeXmlNode(doc);

            //return jsonText;

            return res;
        }


        private IEnumerable<XElement> ReduceXml(XDocument xdoc, string type)
        {
            var mreduce = (from doc in xdoc
                                    .Descendants(type)
                                    .Descendants("Commissionato")
                           select doc);
            return mreduce;
        }
    }

    static class Extensions
    {
        public static string GetMonthName(this DateTime dtIn)
        {
            string monthName = dtIn.ToString("MMMM", CultureInfo.GetCultureInfo("IT")).ToLower();
            return monthName;
        }
        public static string ToUnformatString(this string strIn)
        {
            string strOut = strIn.Replace(".", "").Replace(",", ".");
            return strOut;
        }
    }
}
