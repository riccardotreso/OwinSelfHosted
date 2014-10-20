using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Owin;
using System.Web.Http;
using System.Net.Http.Formatting;

/*
 * FOR CORS Install-Package Microsoft.AspNet.WebApi.Cors
 */

namespace PubbliSelfHostedAPI
{
    public class Startup
    {

        //il metodo deve chiamarsi in questo modo
        public void Configuration(IAppBuilder appBuilder) {

            HttpConfiguration config = new HttpConfiguration();
            //setting formatters
            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());

            //abilitazione cross domain
            config.EnableCors();
           
            //attribute routing Web api 2.0
            config.MapHttpAttributeRoutes();


            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            appBuilder.UseWebApi(config);

        }
    }
}
