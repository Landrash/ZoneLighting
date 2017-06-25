using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;

namespace WorkerRole1
{
	public class WorkerRole : RoleEntryPoint
	{
		private IDisposable App = null;

		private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
		private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

		public override void Run()
		{
			//Trace.TraceInformation("WorkerRole1 is running");

			//try
			//{
			//	this.RunAsync(this.cancellationTokenSource.Token).Wait();
			//}
			//finally
			//{
			//	this.runCompleteEvent.Set();
			//}

			//this.runCompleteEvent.Set();

			//Thread.Sleep(Timeout.Infinite);

			while (true)
			{
				Trace.TraceInformation("Working");
				Thread.Sleep(10000);
			}
		}

		public override bool OnStart()
		{
			ServicePointManager.DefaultConnectionLimit = 12;

			// New code:
			var endpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["Endpoint1"];
			var baseUri = $"http://{endpoint.IPEndpoint}";

			Trace.TraceInformation($"Starting OWIN at {baseUri}",
				"Information");

			App = WebApp.Start<Startup>(new StartOptions(url: baseUri));
			return base.OnStart();
		}

		public override void OnStop()
		{
			App?.Dispose();
			base.OnStop();
		}

		//private async Task RunAsync(CancellationToken cancellationToken)
		//{
		//	// TODO: Replace the following with your own logic.
		//	while (!cancellationToken.IsCancellationRequested)
		//	{
		//		Trace.TraceInformation("Working");
		//		await Task.Delay(10000, cancellationToken);
		//	}
		//}
	}
}
