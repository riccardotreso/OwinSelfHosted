using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;

/*
 reference at:
 * http://www.asp.net/web-api/overview/hosting-aspnet-web-api/use-owin-to-self-host-web-api
 * https://www.nuget.org/packages/Microsoft.AspNet.WebApi.OwinSelfHost/
 
 */

namespace PubbliSelfHostedAPI
{
    class Program
    {
        static ManualResetEvent _quitEvent = new ManualResetEvent(false);
        static IDisposable _app = null;

        static void Main(string[] args)
        {

            Console.CancelKeyPress += Console_CancelKeyPress;

            string baseAddress = "http://localhost:9000/";

            // Start OWIN host 
            _app = WebApp.Start<Startup>(url: baseAddress);
            
            Console.WriteLine("OWIN PubbliSelfHostedAPI listent on " + baseAddress);
            Console.WriteLine("CTRL+C to stop the service");

            _quitEvent.WaitOne();

        }

        static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            if (_app != null)
                _app.Dispose();

            _quitEvent.Set();
            e.Cancel = true;
        }
    }
}
