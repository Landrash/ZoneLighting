using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Refigure;
using Topshelf;

namespace WebRemote
{
	class Program
	{
		public class WebAppStarter
		{
			private IDisposable App { get; set; }

			public void Start()
			{
				string baseAddress = Config.Get("BaseURL");
				App = WebApp.Start<Startup>(new StartOptions(url: baseAddress));
				Console.WriteLine($"Server running at {baseAddress}.");
			}

			public void Stop()
			{
				App.Dispose();
			}

		}

		static void Main(string[] args)
		{
			HostFactory.Run(x =>
			{
				x.Service<WebAppStarter>(s =>
				{
					s.ConstructUsing(name => new WebAppStarter());
					s.WhenStarted(tc => tc.Start());
					s.WhenStopped(tc => tc.Stop());
				});
				x.RunAsLocalSystem();

				x.SetDescription(Config.Get("ServiceName"));
				x.SetDisplayName(Config.Get("ServiceName"));
				x.SetServiceName(Config.Get("ServiceName"));
			});
		}


	}
}
