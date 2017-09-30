using System.Drawing;
using System.Linq;
using Anshul.Utilities;
using NUnit.Framework;
using ZoneLighting;
using ZoneLighting.ConfigNS;
using ZoneLighting.StockPrograms;
using ZoneLighting.TestApparatus;
using ZoneLighting.Usables;
using ZoneLighting.ZoneNS;
using ZoneLighting.ZoneProgramNS;
using ZoneLighting.ZoneProgramNS.Factories;

namespace ZoneLightingTests
{
	public class ConfigTests
	{
		/// <summary>
		/// Sets up a zone configuration, saves it, and makes sure that it deserializes with the same
		/// properties (only some properties are checked).
		/// </summary>
		[Test]
		[Timeout(30000)]
		public void SerializeZones_SerializesZonesInCorrectFormat()
		{
			//arrange
			var zones = new BetterList<Zone>();

			zones.Add(new Zone("TestZone1")).AddLights(6);

			dynamic startingValuesOldTz1 = new InputBag();
			startingValuesOldTz1.DelayTime = 1;
			startingValuesOldTz1.Speed = 1;

			zones[0].Run(new Rainbow(), startingValuesOldTz1);

			zones.Add(new Zone("TestZone2")).AddLights(12);
			dynamic startingValuesOldTz2 = new InputBag();
			startingValuesOldTz2.DelayTime = 1;
			startingValuesOldTz2.DotColor = Color.BlueViolet;

			zones[1].Run(new ScrollDot(), startingValuesOldTz2);

			//act
			var serializedZones = Config.SerializeZones(zones);

			zones.ForEach(zone => zone.Dispose());

			//assert
			Assert.AreEqual(serializedZones,
				"{\r\n  \"$type\": \"Anshul.Utilities.BetterList`1[[ZoneLighting.ZoneNS.Zone, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], Anshul.Utilities, Version=1.0.20.0, Culture=neutral, PublicKeyToken=null\",\r\n  \"$values\": [\r\n    {\r\n      \"$type\": \"ZoneLighting.ZoneNS.Zone, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n      \"Name\": \"TestZone1\",\r\n      \"Lights\": {\r\n        \"$type\": \"System.Collections.Generic.List`1[[Graphics.IPixel, Graphics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\",\r\n        \"$values\": [\r\n          {\r\n            \"$type\": \"ZoneLighting.LED, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n            \"Index\": 0\r\n          },\r\n          {\r\n            \"$type\": \"ZoneLighting.LED, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n            \"Index\": 1\r\n          },\r\n          {\r\n            \"$type\": \"ZoneLighting.LED, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n            \"Index\": 2\r\n          },\r\n          {\r\n            \"$type\": \"ZoneLighting.LED, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n            \"Index\": 3\r\n          },\r\n          {\r\n            \"$type\": \"ZoneLighting.LED, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n            \"Index\": 4\r\n          },\r\n          {\r\n            \"$type\": \"ZoneLighting.LED, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n            \"Index\": 5\r\n          }\r\n        ]\r\n      },\r\n      \"LightingControllerName\": null,\r\n      \"Brightness\": 0.0,\r\n      \"ZoneProgramInputs\": {\r\n        \"$type\": \"System.Collections.Generic.Dictionary`2[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[ZoneLighting.ZoneProgramNS.ZoneProgramInputCollection, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\",\r\n        \"Rainbow\": {\r\n          \"$type\": \"ZoneLighting.ZoneProgramNS.ZoneProgramInputCollection, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n          \"$values\": [\r\n            {\r\n              \"$type\": \"ZoneLighting.ZoneProgramNS.Input.ZoneProgramInput, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n              \"Name\": \"Speed\",\r\n              \"Type\": \"System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\",\r\n              \"Value\": 1\r\n            },\r\n            {\r\n              \"$type\": \"ZoneLighting.ZoneProgramNS.Input.ZoneProgramInput, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n              \"Name\": \"DelayTime\",\r\n              \"Type\": \"System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\",\r\n              \"Value\": 1\r\n            },\r\n            {\r\n              \"$type\": \"ZoneLighting.ZoneProgramNS.Input.ZoneProgramInput, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n              \"Name\": \"SyncLevel\",\r\n              \"Type\": \"ZoneLighting.ZoneNS.SyncLevel, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n              \"Value\": {\r\n                \"$type\": \"ZoneLighting.ZoneNS.SyncLevel, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n                \"Name\": \"Fade\"\r\n              }\r\n            }\r\n          ]\r\n        }\r\n      }\r\n    },\r\n    {\r\n      \"$type\": \"ZoneLighting.ZoneNS.Zone, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n      \"Name\": \"TestZone2\",\r\n      \"Lights\": {\r\n        \"$type\": \"System.Collections.Generic.List`1[[Graphics.IPixel, Graphics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\",\r\n        \"$values\": [\r\n          {\r\n            \"$type\": \"ZoneLighting.LED, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n            \"Index\": 0\r\n          },\r\n          {\r\n            \"$type\": \"ZoneLighting.LED, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n            \"Index\": 1\r\n          },\r\n          {\r\n            \"$type\": \"ZoneLighting.LED, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n            \"Index\": 2\r\n          },\r\n          {\r\n            \"$type\": \"ZoneLighting.LED, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n            \"Index\": 3\r\n          },\r\n          {\r\n            \"$type\": \"ZoneLighting.LED, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n            \"Index\": 4\r\n          },\r\n          {\r\n            \"$type\": \"ZoneLighting.LED, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n            \"Index\": 5\r\n          },\r\n          {\r\n            \"$type\": \"ZoneLighting.LED, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n            \"Index\": 6\r\n          },\r\n          {\r\n            \"$type\": \"ZoneLighting.LED, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n            \"Index\": 7\r\n          },\r\n          {\r\n            \"$type\": \"ZoneLighting.LED, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n            \"Index\": 8\r\n          },\r\n          {\r\n            \"$type\": \"ZoneLighting.LED, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n            \"Index\": 9\r\n          },\r\n          {\r\n            \"$type\": \"ZoneLighting.LED, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n            \"Index\": 10\r\n          },\r\n          {\r\n            \"$type\": \"ZoneLighting.LED, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n            \"Index\": 11\r\n          }\r\n        ]\r\n      },\r\n      \"LightingControllerName\": null,\r\n      \"Brightness\": 0.0,\r\n      \"ZoneProgramInputs\": {\r\n        \"$type\": \"System.Collections.Generic.Dictionary`2[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[ZoneLighting.ZoneProgramNS.ZoneProgramInputCollection, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\",\r\n        \"ScrollDot\": {\r\n          \"$type\": \"ZoneLighting.ZoneProgramNS.ZoneProgramInputCollection, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n          \"$values\": [\r\n            {\r\n              \"$type\": \"ZoneLighting.ZoneProgramNS.Input.ZoneProgramInput, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n              \"Name\": \"DelayTime\",\r\n              \"Type\": \"System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\",\r\n              \"Value\": 1\r\n            },\r\n            {\r\n              \"$type\": \"ZoneLighting.ZoneProgramNS.Input.ZoneProgramInput, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n              \"Name\": \"DotColor\",\r\n              \"Type\": \"System.Nullable`1[[System.Drawing.Color, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\",\r\n              \"Value\": \"BlueViolet\"\r\n            }\r\n          ]\r\n        }\r\n      }\r\n    }\r\n  ]\r\n}");
		}

		[Test]
		[Timeout(30000)]
		public void DeserializeZones_DeserializesZonesAndBringsThemToInitialConditions()
		{
			//arrange
			var zones = new BetterList<Zone>();

			var testController = new TestLightingController("tlc1", list => {});

			ZoneScaffolder.Instance.LightingControllers.Add(testController);

			zones.Add(new Zone(testController, "TestZone1")).AddLights(6);

			dynamic startingValuesOldTz1 = new InputBag();
			startingValuesOldTz1.DelayTime = 1;
			startingValuesOldTz1.Speed = 1;

			zones[0].Run(new Rainbow(), startingValuesOldTz1);

			zones.Add(new Zone(testController, "TestZone2")).AddLights(12);
			dynamic startingValuesOldTz2 = new InputBag();
			startingValuesOldTz2.DelayTime = 1;
			startingValuesOldTz2.DotColor = Color.BlueViolet;

			zones[1].Run(new ScrollDot(), startingValuesOldTz2);
			var serializedZones = Config.SerializeZones(zones);

			//act
			var deserializedZones = Config.DeserializeZones(serializedZones);

			//assert
			Assert.AreEqual(zones.Count, deserializedZones.Count());
			deserializedZones.ForEach(deserializedZone =>
			{
				Assert.AreEqual(deserializedZone.LightingController, zones[deserializedZone.Name].LightingController);
				Assert.AreEqual(deserializedZone.Brightness, zones[deserializedZone.Name].Brightness);
				Assert.AreEqual(deserializedZone.LightCount, zones[deserializedZone.Name].LightCount);
				Assert.AreEqual(deserializedZone.Running, false);
				Assert.AreEqual(deserializedZone.IsProgramLooping, false);
				Assert.AreEqual(deserializedZone.ZoneProgram, null);
			});

			zones.ForEach(zone => zone.Dispose(true));
		}

		[Test]
		[Timeout(30000)]
		public void SerializeProgramSets_SerializesProgramSetsInCorrectFormat()
		{
			//arrange
			TestHelpers.InitializeZoneScaffolder();
			var testController = new TestLightingController("tlc1", list => { });
			var zones = new BetterList<Zone>();
			
			zones.Add(new Zone(testController, "TestZone1")).AddLights(6);
			zones.Add(new Zone(testController, "TestZone2")).AddLights(12);

			var programSets = new BetterList<ProgramSet>
			{
				new ProgramSet("Stepper", zones["TestZone1"].Listify(), true, null, "StepperSet1"),
				new ProgramSet("Stepper", zones["TestZone2"].Listify(), true, null, "StepperSet2")
			};

			//act
			var serializedProgramSets = Config.SerializeProgramSets(programSets);

			//assert
			Assert.AreEqual(serializedProgramSets,
				"{\r\n  \"$type\": \"Anshul.Utilities.BetterList`1[[ZoneLighting.ZoneProgramNS.ProgramSet, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], Anshul.Utilities, Version=1.0.20.0, Culture=neutral, PublicKeyToken=null\",\r\n  \"$values\": [\r\n    {\r\n      \"$type\": \"ZoneLighting.ZoneProgramNS.ProgramSet, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n      \"Name\": \"StepperSet1\",\r\n      \"Zones\": {\r\n        \"$type\": \"System.Collections.Generic.List`1[[ZoneLighting.ZoneNS.Zone, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\",\r\n        \"$values\": [\r\n          {\r\n            \"$type\": \"ZoneLighting.ZoneNS.Zone, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n            \"Name\": \"TestZone1\",\r\n            \"Lights\": {\r\n              \"$type\": \"System.Collections.Generic.List`1[[Graphics.IPixel, Graphics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\",\r\n              \"$values\": [\r\n                {\r\n                  \"$type\": \"ZoneLighting.LED, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n                  \"Index\": 0\r\n                },\r\n                {\r\n                  \"$type\": \"ZoneLighting.LED, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n                  \"Index\": 1\r\n                },\r\n                {\r\n                  \"$type\": \"ZoneLighting.LED, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n                  \"Index\": 2\r\n                },\r\n                {\r\n                  \"$type\": \"ZoneLighting.LED, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n                  \"Index\": 3\r\n                },\r\n                {\r\n                  \"$type\": \"ZoneLighting.LED, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n                  \"Index\": 4\r\n                },\r\n                {\r\n                  \"$type\": \"ZoneLighting.LED, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n                  \"Index\": 5\r\n                }\r\n              ]\r\n            },\r\n            \"LightingControllerName\": \"tlc1\",\r\n            \"Brightness\": 1.0,\r\n            \"ZoneProgramInputs\": {\r\n              \"$type\": \"System.Collections.Generic.Dictionary`2[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[ZoneLighting.ZoneProgramNS.ZoneProgramInputCollection, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\r\n            }\r\n          }\r\n        ]\r\n      },\r\n      \"ProgramName\": \"Stepper\",\r\n      \"Sync\": true,\r\n      \"StartingParameters\": null\r\n    },\r\n    {\r\n      \"$type\": \"ZoneLighting.ZoneProgramNS.ProgramSet, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n      \"Name\": \"StepperSet2\",\r\n      \"Zones\": {\r\n        \"$type\": \"System.Collections.Generic.List`1[[ZoneLighting.ZoneNS.Zone, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\",\r\n        \"$values\": [\r\n          {\r\n            \"$type\": \"ZoneLighting.ZoneNS.Zone, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n            \"Name\": \"TestZone2\",\r\n            \"Lights\": {\r\n              \"$type\": \"System.Collections.Generic.List`1[[Graphics.IPixel, Graphics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\",\r\n              \"$values\": [\r\n                {\r\n                  \"$type\": \"ZoneLighting.LED, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n                  \"Index\": 0\r\n                },\r\n                {\r\n                  \"$type\": \"ZoneLighting.LED, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n                  \"Index\": 1\r\n                },\r\n                {\r\n                  \"$type\": \"ZoneLighting.LED, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n                  \"Index\": 2\r\n                },\r\n                {\r\n                  \"$type\": \"ZoneLighting.LED, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n                  \"Index\": 3\r\n                },\r\n                {\r\n                  \"$type\": \"ZoneLighting.LED, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n                  \"Index\": 4\r\n                },\r\n                {\r\n                  \"$type\": \"ZoneLighting.LED, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n                  \"Index\": 5\r\n                },\r\n                {\r\n                  \"$type\": \"ZoneLighting.LED, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n                  \"Index\": 6\r\n                },\r\n                {\r\n                  \"$type\": \"ZoneLighting.LED, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n                  \"Index\": 7\r\n                },\r\n                {\r\n                  \"$type\": \"ZoneLighting.LED, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n                  \"Index\": 8\r\n                },\r\n                {\r\n                  \"$type\": \"ZoneLighting.LED, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n                  \"Index\": 9\r\n                },\r\n                {\r\n                  \"$type\": \"ZoneLighting.LED, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n                  \"Index\": 10\r\n                },\r\n                {\r\n                  \"$type\": \"ZoneLighting.LED, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\r\n                  \"Index\": 11\r\n                }\r\n              ]\r\n            },\r\n            \"LightingControllerName\": \"tlc1\",\r\n            \"Brightness\": 1.0,\r\n            \"ZoneProgramInputs\": {\r\n              \"$type\": \"System.Collections.Generic.Dictionary`2[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[ZoneLighting.ZoneProgramNS.ZoneProgramInputCollection, ZoneLighting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\r\n            }\r\n          }\r\n        ]\r\n      },\r\n      \"ProgramName\": \"Stepper\",\r\n      \"Sync\": true,\r\n      \"StartingParameters\": null\r\n    }\r\n  ]\r\n}");

			zones.ForEach(zone => zone.Dispose(true));
		}

		[Test]
		[Timeout(30000)]
		public void DeserializeProgramSets_DeserializesProgramSetsAndBringsThemToInitialConditions()
		{
			//arrange
			TestHelpers.InitializeZoneScaffolder();
			var testController = new TestLightingController("tlc1", list => { });
			var zones = new BetterList<Zone>();

			zones.Add(new Zone(testController, "TestZone1")).AddLights(6);
			zones.Add(new Zone(testController, "TestZone2")).AddLights(12);

			var programSets = new BetterList<ProgramSet>
			{
				new ProgramSet("Stepper", zones["TestZone1"].Listify(), true, null, "StepperSet1"),
				new ProgramSet("Stepper", zones["TestZone2"].Listify(), true, null, "StepperSet2")
			};

			var serializedProgramSets = Config.SerializeProgramSets(programSets);

			//act
			var deserializedProgramSets = Config.DeserializeProgramSets(serializedProgramSets, zones);

			//assert
			Assert.AreEqual(programSets.Count, deserializedProgramSets.Count());
			deserializedProgramSets.ForEach(deserializedProgramSet =>
			{
				Assert.AreEqual(deserializedProgramSet.ProgramName, programSets[deserializedProgramSet.Name].ProgramName);
				Assert.AreEqual(deserializedProgramSet.Sync, programSets[deserializedProgramSet.Name].Sync);
				Assert.AreEqual(deserializedProgramSet.Zones, programSets[deserializedProgramSet.Name].Zones);
			});

			TestHelpers.ValidateSteppersRunning(zones.Select(z => z.ZoneProgram).Cast<IStepper>(), 100);

			zones.ForEach(zone => zone.Dispose(true));
		}
	}
}
