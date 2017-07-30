using System.Collections.Generic;
using WebRemote.Models;
using ZoneLighting.ZoneProgramNS;

namespace WebRemote
{
	public interface IZLMRPC
	{
		void DisposeZLM();
		void Notify(string colorString, int? time, int? cycles, double? brightness);
		void Save();
		void SetZoneColor(string zoneName, string color, float brightness = 1);
		void StopZone(string zoneName, bool force);
		void CreateZLM();
		void DisposeProgramSets();
		List<ZoneJsonModel> GetZones();
		
		//void RestartProgramSet(string programSetName, string programName, List<string> zoneNames, InputBag inputBag);
		void StopProgramSet(string programSetName);
		void SetZoneInputs(string zoneName, InputBag inputBag);
		string GetZoneSummary();
		void SetProgramSetInputs(string programSetName, InputBag inputBag);
		void StartProgramSet(string programSetName);

		ProgramSetJsonModel CreateProgramSet(string programSetName, string programName, IEnumerable<string> zoneNames, bool sync = true, InputBag inputBag = null, dynamic startingParameters = null);

		//List<Zone> GetZones();
		//List<Zone> GetAvailableZones();

		void DisposeProgramSet(string programSetName);
		string GetStatus();
		void RecreateProgramSetWithoutZone(string programSetName, string zoneName, bool force = false);

		/// <summary>
		/// Sets the color of a single light on a zone.
		/// </summary>
		/// <param name="zoneName">Name of the zone.</param>
		/// <param name="color">The color.</param>
		/// <param name="index">The index of the light.</param>
		/// <param name="brightness">The brightness.</param>
		void SetLightColor(string zoneName, string color, int index, float brightness = 1);

		void SetAllZonesColor(string color, float brightness = 1);
		void SetInputs(string programSetOrZoneName, InputBag inputBag);
		void RecreateProgramSet(string programSetName, string programName, List<string> zoneNames, InputBag inputBag);
	}
}