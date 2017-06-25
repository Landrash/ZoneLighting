using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Owin;
using WebRemote.IoC;

namespace WorkerRole1
{
	class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			// Configure Web API for self-host. 
			HttpConfiguration config = new HttpConfiguration();
			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "{controller}/{action}",
				defaults: new { controller = "ZLM", action = "Index" }

			);

			config.Routes.IgnoreRoute("Glimpse", "{resource}.axd/{*pathInfo}");

			app.UseWebApi(config);

			Container.CreateZLM();
			Container.CreateZLMRPC();
		}
	}
}
