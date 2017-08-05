using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebRemote.Extensions;
using WebRemote.IoC;

using WebRemote.Models;
using ZoneLighting;
using ZoneLighting.ZoneProgramNS;

namespace WebRemote
{
	public class ZLMController : ApiController
	{
		#region Internals

		public IZLMRPC ZLMRPC  => Container.ZLMRPC;
		public IZLM ZLM => Container.ZLM;
		private ZLMJsonModel ZLMJsonModel => ((ZLM)ZLM).ToZLMJsonModel();

		#endregion

		#region Actions

		#region Admin

		//public ZLMJsonModel AddFadeCandyZone(string name, int numberOfLights)
		//{
		//	ZLMRPC.AddFadeCandyZone(name, numberOfLights);
		//	return ZLMJsonModel;
		//}

		[Route("")]
		public ZLMJsonModel GetZLM()
		{
			return ZLMJsonModel;
		}

		public ZLMJsonModel CreateZLM()
		{
			ZLMRPC.CreateZLM();
			return ZLMJsonModel;
		}

		public void DisposeZLM()
		{
			ZLMRPC.DisposeZLM();
		}

		public ZLMJsonModel Save()
		{
			ZLMRPC.Save();
			return ZLMJsonModel;
		}

		public string GetStatus()
		{
			return ZLMRPC.GetStatus();
		}

		#endregion

		#region Program Set API

		public ZLMJsonModel CreateProgramSet(JArray param)
		{
			var programSetName = param[0].ToObject<string>();
			var programName = param[1].ToObject<string>();
			var zoneNames = param[2].ToObject<IEnumerable<string>>();
			// ReSharper disable once SimplifyConditionalTernaryExpression
			var sync = param.Count > 3 ? param[3].ToObject<bool>() : true;
			var inputBag = param.Count > 4 ? param[4].ToObject<InputBag>() : null;
			dynamic startingParameters = param.Count > 5 ? param[5] : null;

			ZLMRPC.CreateProgramSet(programSetName, programName, zoneNames, sync, inputBag, startingParameters);
			return ZLMJsonModel;
		}

		public ZLMJsonModel DisposeProgramSets()
		{
			ZLMRPC.DisposeProgramSets();
			return ZLMJsonModel;
		}

		[HttpPost]
		public ZLMJsonModel SetInputs(JArray param)
		{
			var programSetOrZoneName = param[0].ToObject<string>();
			var inputBag = param[1].ToObject<InputBag>();
			ZLMRPC.SetInputs(programSetOrZoneName, inputBag);

			return ZLMJsonModel;
		}

		[HttpPost]
		public ZLMJsonModel RecreateProgramSet(JArray param)
		{
			var programSetName = param[0].ToObject<string>();
			var programName = param[1].ToObject<string>();
			var zoneNames = param[2].ToObject<List<string>>();

			ZLMRPC.RecreateProgramSet(programSetName, programName, zoneNames, null);
			return ZLMJsonModel;
		}


		[HttpPost]
		public ZLMJsonModel RecreateProgramSetWithoutZone(JArray param)
		{
			var programSetName = param[0].ToObject<string>();
			var zoneName = param[1].ToObject<string>();

			ZLMRPC.RecreateProgramSetWithoutZone(programSetName, zoneName);
			return ZLMJsonModel;
		}

		#endregion

		#region Zone API

		public ZLMJsonModel StopZone(string zoneName)
		{
			ZLMRPC.StopZone(zoneName, true);
			return ZLMJsonModel;
		}

		public List<ZoneJsonModel> GetZones()
		{
			return ZLMJsonModel.Zones.ToBetterList();
		}

		public string GetZoneSummary()
		{
			return ZLMRPC.GetZoneSummary();
		}

		[HttpPost]
		public ZLMJsonModel SetZoneInputs(JArray param)
		{
			var zoneName = param[0].ToObject<string>();
			var inputBag = param[1].ToObject<InputBag>();

			ZLMRPC.SetZoneInputs(zoneName, inputBag);
			return ZLMJsonModel;
		}

		#endregion

		#endregion
	}
}
