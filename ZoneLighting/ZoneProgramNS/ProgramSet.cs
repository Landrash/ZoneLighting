﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Anshul.Utilities;
using Newtonsoft.Json;
using ZoneLighting.Usables;
using ZoneLighting.Usables.TestInterfaces;
using ZoneLighting.ZoneNS;
using ZoneLighting.ZoneProgramNS.Factories;

namespace ZoneLighting.ZoneProgramNS
{
	/// <summary>
	/// A logical grouping of programs that will be run on the zones passed into the constructor. All programs can either
	/// be started with or without sync. 
	/// </summary>
	[DataContract]
	public class ProgramSet : IDisposable, IBetterListType, ITestProgramSet
	{
		#region Internals

		[DataMember]
		public string Name { get; private set; }

		public SyncContext SyncContext { get; private set; }

		[DataMember]
		public List<Zone> Zones { get; private set; }

		[DataMember]
		public string ProgramName { get; private set; }

		//public ZoneProgramInputCollection Inputs
		//{
			//get
			//{
			//	var inputs = ZonePrograms.First().Inputs;

			//	foreach (var input in inputs)
			//	{
			//		if (ZonePrograms.Any(x =>
			//		{
			//			var value = x.Inputs[input.Name].Value;
			//			return value != null && ((input.Value == null) ||
			//				                                              !value.Equals(input.Value));
			//		}))
			//		{
			//			input.SetValue("Indeterminate");
			//		}
			//	}

			//	return inputs;
				
			//	//var areInputValuesSame = true;
			//	//object value;

			//	//var inputs = ZonePrograms[0].Inputs;

			//	//foreach (var input in inputs)
			//	//{
			//	//	if (ZonePrograms.Any(p => p.Inputs[input.Name] != input.Value))
			//	//	{
			//	//		return
			//	//	}
			//	//}

			//	//inputs.ForEach(program =>
			//	//{
			//	//	program.Inputs
			//	//});
			//}
		//}

		public List<ZoneProgram> ZonePrograms => Zones.Select(zone => zone.ZoneProgram).ToList();

		[DataMember]
		public bool Sync { get; private set; }

		[DataMember]
		public dynamic StartingParameters { get; private set; }

		/// <summary>
		/// For testing only
		/// </summary>
		IEnumerable<ZoneProgram> ITestProgramSet.ZoneProgramsTest => ZonePrograms;

		public ProgramState State
		{
			get
			{
				var previousState = ZonePrograms.First().State;
				if (ZonePrograms.Any(zoneProgram => zoneProgram.State != previousState))
				{
					throw new Exception("Program Set in inconsistent state. Some programs are started while others are not.");
				}
				return previousState;
			}
		}

		#endregion

		#region API

		/// <summary>
		/// Creates a set of programs that run on a given set of zones with the given input starting values
		/// </summary>
		public ProgramSet(string programName, IEnumerable<Zone> zones, bool sync, IEnumerable<InputBag> inputBags, string name, dynamic startingParameters = null)
		{
			if (!ZoneScaffolder.Instance.DoesProgramExist(programName))
				throw new Exception($"No program by the name '{programName}' exists.");

			var inputBagsListed = inputBags as IList<InputBag> ?? inputBags?.ToList();
			var zonesListed = zones as IList<Zone> ?? zones.ToList();
			if (inputBagsListed != null && inputBagsListed.Count() != 1 && inputBagsListed.Count() != zonesListed.Count())
			{
				throw new Exception("Number of items in inputBags should be either 1 or equal to number of zones.");
			}

			Name = name;
			Zones = zonesListed.ToList();
			ProgramName = programName;
			Sync = sync;
			StartingParameters = startingParameters;

			if (Sync)
			{
				Zones.ForEach(zone =>
				{
					zone.Stop(true);
					ZoneScaffolder.Instance.RunZone(zone, programName, null, true, SyncContext, true, startingParameters);
				});

				SyncContext = new SyncContext();
				SyncContext.Sync(Zones, inputBags: inputBagsListed);
			}
			else
			{
				for (int i = 0; i < Zones.Count; i++)
				{
					var zone = Zones[i];
					zone.Stop(true);

					ZoneScaffolder.Instance.RunZone(zone, programName,
						inputBagsListed?.Count() == zonesListed.Count() ? inputBagsListed.ElementAt(i) : inputBagsListed?.First(),
						startingParameters: startingParameters);
				}
			}
		}

		/// <summary>
		/// This constructor is only to be used by the JSON deserializer for loading from configuration file.
		/// </summary>
		[JsonConstructor]
		public ProgramSet(string programName, IEnumerable<Zone> zones, bool sync, string name)
		{
			if (!ZoneScaffolder.Instance.DoesProgramExist(programName))
				throw new Exception($"No program by the name '{programName}' exists.");

			Name = name;
			Zones = zones.ToList();
			ProgramName = programName;
			Sync = sync;
		}

		/// <summary>
		/// Creates a program set with a single program on a single zone.
		/// </summary>
		public ProgramSet(ZoneProgram program, Zone zone, InputBag inputBag, string setName, dynamic startingParameters = null)
		{
			Name = setName;
			ProgramName = program.Name;
			Sync = false;

			Zones = zone.Listify();
			zone.Stop(true);
			//ZoneScaffolder.Instance.RunZone(zone, "", inputBag); -- TODO: Why aren't we using this? This seems to be better than the next line
			zone.Run(program, inputBag, startingParameters: startingParameters);
		}

		public void Start(dynamic startingParameters = null)
		{
			if (SyncContext == null)
				ZonePrograms.ForEach(zp => zp.Start(startingParameters: startingParameters ?? StartingParameters));
			else
				SyncContext.Sync(ZonePrograms, startingParameters: startingParameters);
		}

		public void Stop(bool force = false)
		{
			ZonePrograms.Parallelize(zp => zp.Stop(force));
		}

		public void RemoveZone(Zone zone, bool force = true)
		{
			SyncContext?.Unsync(zone);
			Zones.Remove(zone);
		}

		public void Dispose()
		{
			Dispose(false);
		}

		public void Dispose(bool force)
		{
			Name = null;
			if (Zones != null)
				ZonePrograms.Parallelize(zp => zp?.Stop(force));
			ZonePrograms?.ForEach(zoneProgram => zoneProgram?.Dispose());
			Zones?.ForEach(zone => zone?.UnsetProgram());
			Zones = null;
			ProgramName = null;
			SyncContext?.Dispose();
			SyncContext = null;
		}

		public void SetInputs(InputBag inputBag)
		{
			ZonePrograms.Parallelize(zp => zp.SetInputs(inputBag));
		}

		#endregion
	}
}
