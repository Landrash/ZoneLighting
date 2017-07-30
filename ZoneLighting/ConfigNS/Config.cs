using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Anshul.Utilities;
using Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ZoneLighting.Usables;
using ZoneLighting.ZoneNS;
using ZoneLighting.ZoneProgramNS;
using ZoneLighting.ZoneProgramNS.Factories;

namespace ZoneLighting.ConfigNS
{
	public class Config
	{
		public class InterfaceToClassMappings
		{
			public IDictionary<string, Type> TypeMap { get; set; }

			public InterfaceToClassMappings()
			{
				TypeMap = new Dictionary<string, Type>
				{
					[typeof(List<IPixel>).FullName] = typeof(List<LED>)
				};
			}

			public bool CanMap(string fullTypeName)
			{
				return TypeMap.Keys.Contains(fullTypeName);
			}

			public Type Map(string fullTypeName)
			{
				if (CanMap(fullTypeName))
					return TypeMap[fullTypeName];

				throw new Exception($"Type {fullTypeName} is not in candidate types.");
			}
		}

		public class TypeNameSerializationBinder : SerializationBinder
		{
			private InterfaceToClassMappings InterfaceToClassMappings { get; } = new InterfaceToClassMappings();
			private SerializationBinder DefaultSerializationBinder { get; } = new DefaultSerializationBinder();
				
			public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
			{
				DefaultSerializationBinder.BindToName(serializedType, out assemblyName, out typeName);
			}

			public override Type BindToType(string assemblyName, string typeName)
			{
				if (InterfaceToClassMappings.CanMap(typeName))
				{
					return InterfaceToClassMappings.Map(typeName);
				}

				return DefaultSerializationBinder.BindToType(assemblyName, typeName);
			}
		}


		#region Serialization Settings

		public static TypeNameHandling TypeNameHandling { get; } = TypeNameHandling.All;

		public static SerializationBinder SerializationBinder { get; } = new TypeNameSerializationBinder();

		public static JsonSerializerSettings SaveZonesSerializerSettings => new JsonSerializerSettings()
		{
			//PreserveReferencesHandling = PreserveReferencesHandling.All,
			TypeNameHandling = TypeNameHandling,
			TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Full,
			Formatting = Formatting.Indented,
			Binder = SerializationBinder
		};

		public static JsonSerializerSettings LoadZonesSerializerSettings => new JsonSerializerSettings()
		{
			//PreserveReferencesHandling = PreserveReferencesHandling.All,
			TypeNameHandling = TypeNameHandling,
			TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Full,
			Formatting = Formatting.Indented,
			Converters = new JsonConverter[] { new UnderlyingTypeConverter() },
			Binder = SerializationBinder
		};

		public static JsonSerializerSettings LoadProgramSetsSerializerSettings => new JsonSerializerSettings()
		{
			//PreserveReferencesHandling = PreserveReferencesHandling.All,
			TypeNameHandling = TypeNameHandling,
			TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Full,
			Formatting = Formatting.Indented,
			Converters = new JsonConverter[] { new UnderlyingTypeConverter() },
			Binder = SerializationBinder
		};

		public static JsonSerializerSettings SaveProgramSetsSerializerSettings => new JsonSerializerSettings()
		{
			//PreserveReferencesHandling = PreserveReferencesHandling.All,
			TypeNameHandling = TypeNameHandling,
			TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Full,
			Formatting = Formatting.Indented,
			Binder = SerializationBinder
		};

		#endregion

		public static string SerializeProgramSets(IEnumerable<ProgramSet> programSets)
		{
			return Serialize(programSets, SaveProgramSetsSerializerSettings);
		}

		public static string SerializeZones(IEnumerable<Zone> zones)
		{
			zones.ToList().ForEach(zone => zone.SetZoneProgramInputs());
			return Serialize(zones, SaveZonesSerializerSettings);
		}

		public static string Serialize(object toSerialize, JsonSerializerSettings settings)
		{
			return JsonConvert.SerializeObject(toSerialize, settings);
		}

		public static BetterList<Zone> DeserializeZones(string config)
		{
			var des = JsonConvert.DeserializeObject<IEnumerable<Zone>>(config, LoadZonesSerializerSettings);
			var zones = des.ToBetterList();
			zones.ForEach(AssignLightingController);
			return zones.ToBetterList();
		}

		public static BetterList<ProgramSet> DeserializeProgramSets(string config, IEnumerable<Zone> zones)
		{
			//deserialize the program sets
			var deserializedProgramSets = ((IEnumerable<ProgramSet>)JsonConvert.DeserializeObject(config, LoadProgramSetsSerializerSettings))
				.ToBetterList();

			//recreate the program sets from scratch using values from the deserialized ones
			var reinstantiatedProgramSets = new BetterList<ProgramSet>();
			deserializedProgramSets.ForEach(deserializedProgramSet =>
			{
				var zonesEnumerated = zones as IList<Zone> ?? zones.ToList();
				var zonesToPassIn = zonesEnumerated.Where(z => deserializedProgramSet.Zones.Select(dz => dz.Name).Contains(z.Name));
				List<InputBag> inputBags = null;

				//prepare inputBags from deserialized zones
				foreach (var zone in deserializedProgramSet.Zones.Where(zone => zone.ZoneProgramInputs.ContainsKey(deserializedProgramSet.ProgramName)))
				{
					if (inputBags == null)
						inputBags = new List<InputBag>();

					inputBags.Add(zone.ZoneProgramInputs[deserializedProgramSet.ProgramName].ToInputInfo());
				}

				//create new program set with all values from the deserialized version
				reinstantiatedProgramSets.Add(new ProgramSet(deserializedProgramSet.ProgramName, zonesToPassIn,
					deserializedProgramSet.Sync, inputBags, deserializedProgramSet.Name));
			});

			//dump the deserialized program sets
			deserializedProgramSets.ForEach(programSet =>
			{
				programSet.Zones.Clear();
				programSet.Dispose(true);
			});

			return reinstantiatedProgramSets;
		}
		public static void SaveZones(IEnumerable<Zone> zones, string filename)
		{
			var json = SerializeZones(zones);
			File.WriteAllText(filename, json);
		}

		public static void SaveProgramSets(IEnumerable<ProgramSet> programSets, string filename)
		{
			var json = SerializeProgramSets(programSets);
			File.WriteAllText(filename, json);
		}

		private static void AssignLightingController(Zone zone)
		{


			zone.SetLightingController(ZoneScaffolder.Instance.LightingControllers[zone.LightingControllerName]);
		}

		public static IEnumerable<Zone> LoadZones(string filename = "", string zoneConfiguration = "")
		{
			var deserializedZones =
				JsonConvert.DeserializeObject(
					string.IsNullOrEmpty(zoneConfiguration) ? File.ReadAllText(filename) : zoneConfiguration,
					LoadZonesSerializerSettings);
			return (IEnumerable<Zone>)deserializedZones;
		}
	}
}
