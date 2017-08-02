using System.Linq;
using Anshul.Utilities;
using NUnit.Framework;
using WebRemote;
using WebRemote.IoC;
using ZoneLighting;
using ZoneLighting.TestApparatus;
using ZoneLighting.ZoneProgramNS;

namespace WebRemoteTests
{
	[Category("Integration")]
	public class ZLMRPCTests
	{
		[TearDown]
		public void TearDown()
		{

		}

		[TestCase("FadeCandyZone", 8)]
		public void AddZone_Works(string name, int numberOfLights)
		{
			var zlm = new ZLM(false, false, false);
			var zlmrpc = new ZLMRPC(zlm);
			var testLC = new TestLightingController("tlc1");

			//act
			var zone = zlmrpc.AddZone(name, testLC, numberOfLights);

			//assert
			Assert.That(zone.Name, Is.EqualTo(name));
			Assert.That(zone.LightCount, Is.EqualTo(numberOfLights));
			Assert.That(zone.Running, Is.EqualTo(false));
			Assert.That(zone.ProgramName, Is.EqualTo(null));

			zlmrpc.Dispose();
		}

		[Test]
		[Category("Integration")]
		[TestCase("StepperSet", "Stepper", true)]
		public void CreateProgramSet_Works(string programSetName, string programName, bool sync = true)
		{
			var zlm = new ZLM(false, false, false);
			var zlmrpc = new ZLMRPC(zlm);
			var testLC = new TestLightingController("tlc1");
			zlm.AddZone("FadeCandyZone", testLC, 8);

			var zones = zlm.Zones.Select(z => z.Name).ToList();

			//act
			var programSet = zlmrpc.CreateProgramSet(programSetName, programName, zones, sync);

			//assert
			Assert.That(programSet.Name, Is.EqualTo(programSetName));
			Assert.That(programSet.ProgramName, Is.EqualTo(programName));
			Assert.That(programSet.Sync, Is.EqualTo(true));
			Assert.That(programSet.Zones.Select(zone => zone.Name).ToList(), Is.EqualTo(zones));
			Assert.That(programSet.State, Is.EqualTo(ProgramState.Started));

			zlmrpc.Dispose();
		}

		[Test]
		[Category("Integration")]
		[TestCase("StepperSet", "Stepper", true)]
		public void GetStatus_Works(string programSetName, string programName, bool sync = true)
		{
			var zlm = new ZLM(false, false, false);
			var zlmrpc = new ZLMRPC(zlm);
			var testLC = new TestLightingController("tlc1");
			zlm.AddZone("FadeCandyZone", testLC, 8);
			zlm.CreateProgramSet(programSetName, programName, zlm.Zones.Select(z => z.Name), sync);

			//act
			var status = zlmrpc.GetStatus();

			//assert
			Assert.That(status, Is.EqualTo("--ZoneLighting Summary--\r\n=============================\r\n1 ProgramSet(s) currently running:\r\nStepperSet running Stepper on zone(s) FadeCandyZone in sync\r\n--End of Summary--\r\n"));

			zlmrpc.Dispose();
		}

		[TestCase("StepperSet", "Stepper", true)]
		public void DisposeProgramSet_Works(string programSetName, string programName, bool sync = true)
		{
			var zlm = new ZLM(false, false, false);
			var zlmrpc = new ZLMRPC(zlm);
			var testLC = new TestLightingController("tlc1");
			zlm.AddZone("FadeCandyZone", testLC, 8);
			zlm.CreateProgramSet(programSetName, programName, zlm.Zones.Select(z => z.Name), sync);
			zlm.Zones.ForEach(zone =>
			{
				Assert.That(zone.ZoneProgram, Is.Not.Null);
				Assert.That(zone.ZoneProgram.State, Is.EqualTo(ProgramState.Started));
			});

			//act
			zlmrpc.DisposeProgramSet(programSetName);

			//assert
			zlm.Zones.ForEach(zone => Assert.That(zone.ZoneProgram, Is.Null));

			zlmrpc.Dispose();
		}

		[Test]
		public void CreateZLM_Works()
		{
			var zlm = new ZLM(false, false, false);
			var zlmrpc = new ZLMRPC(zlm);
			Assert.That(Container.ZLM, Is.Null);
			zlmrpc.DisposeZLM();

			//act
			zlmrpc.CreateZLM();

			//assert
			Assert.That(Container.ZLM, Is.Not.Null);

			zlmrpc.Dispose();
		}

		[Test]
		public void DisposeZLM_Works()
		{
			var zlm = new ZLM(false, false, false);
			var zlmrpc = new ZLMRPC(zlm);

			//act
			zlmrpc.DisposeZLM();

			//assert
			Assert.That(zlm.Zones, Is.Null);
			Assert.That(zlm.ProgramSets, Is.Null);

			zlmrpc.Dispose();
		}

		[TestCase("StepperSet", "Stepper", true)]
		public void DisposeProgramSets_Works(string programSetName, string programName, bool sync = true)
		{
			var zlm = new ZLM(false, false, false);
			var zlmrpc = new ZLMRPC(zlm);
			var testLC = new TestLightingController("tlc1");
			zlm.AddZone("FadeCandyZone", testLC, 8);
			zlm.CreateProgramSet(programSetName, programName, zlm.Zones.Select(z => z.Name), sync);
			zlm.Zones.ForEach(zone =>
			{
				Assert.That(zone.ZoneProgram, Is.Not.Null);
				Assert.That(zone.ZoneProgram.State, Is.EqualTo(ProgramState.Started));
			});

			//act
			zlmrpc.DisposeProgramSets();

			//assert
			zlm.Zones.ForEach(zone => Assert.That(zone.ZoneProgram, Is.Null));

			zlmrpc.Dispose();
		}

		[Test]
		public void RecreateProgramSet_Works()
		{
			string programSetName = "RainbowSet";
			string programName = "Rainbow";
			var zoneName = "FadeCandyZone";
			dynamic inputBag = new InputBag();
			inputBag.Speed = 50;

			var zlm = new ZLM(false, false, false);
			var zlmrpc = new ZLMRPC(zlm);
			var testLC = new TestLightingController("tlc1");
			zlm.AddZone("FadeCandyZone", testLC, 8);
			zlm.CreateProgramSet(programSetName, programName, zlm.Zones.Select(z => z.Name), true, inputBag);
			zlm.Zones.ForEach(zone =>
			{
				Assert.That(zone.ZoneProgram, Is.Not.Null);
				Assert.That(zone.ZoneProgram.State, Is.EqualTo(ProgramState.Started));
				Assert.That(zone.ZoneProgram.GetInput("Speed").Value, Is.EqualTo(50));
			});

			inputBag.Speed = 90;

			//act
			zlmrpc.RecreateProgramSet(programSetName, programName, zoneName.Listify(), inputBag);

			//assert
			zlm.Zones.ForEach(zone =>
			{
				Assert.That(zone.ZoneProgram.GetInput("Speed").Value, Is.EqualTo(90));
			});

			zlmrpc.Dispose();
		}

		[Test]
		public void StopProgramSet_Works()
		{
			var programSetName = "StepperSet";

			var zlm = new ZLM(false, false, false);
			var zlmrpc = new ZLMRPC(zlm);
			var testLC = new TestLightingController("tlc1");
			zlm.AddZone("FadeCandyZone", testLC, 8);
			zlm.CreateProgramSet(programSetName, "Stepper", zlm.Zones.Select(z => z.Name));
			zlm.Zones.ForEach(zone =>
			{
				Assert.That(zone.ZoneProgram.State, Is.EqualTo(ProgramState.Started));
			});

			//act
			zlmrpc.StopProgramSet(programSetName);

			//assert
			zlm.Zones.ForEach(zone =>
			{
				Assert.That(zone.ZoneProgram.State, Is.EqualTo(ProgramState.Stopped));
			});

			zlmrpc.Dispose();
		}


		[Test]
		public void StartProgramSet_Works()
		{
			var programSetName = "StepperSet";

			var zlm = new ZLM(false, false, false);
			var zlmrpc = new ZLMRPC(zlm);
			var testLC = new TestLightingController("tlc1");
			zlm.AddZone("FadeCandyZone", testLC, 8);
			zlm.CreateProgramSet(programSetName, "Stepper", zlm.Zones.Select(z => z.Name));
			zlm.Zones.ForEach(zone =>
			{
				Assert.That(zone.ZoneProgram.State, Is.EqualTo(ProgramState.Started));
			});

			zlm.ProgramSets[programSetName].Stop();

			zlm.Zones.ForEach(zone =>
			{
				Assert.That(zone.ZoneProgram.State, Is.EqualTo(ProgramState.Stopped));
			});

			//act
			zlmrpc.StartProgramSet(programSetName);

			//assert
			zlm.Zones.ForEach(zone =>
			{
				Assert.That(zone.ZoneProgram.State, Is.EqualTo(ProgramState.Started));
			});

			zlmrpc.Dispose();
		}

		[Test]
		public void SetProgramSetInputs_Works()
		{
			string programSetName = "RainbowSet";
			string programName = "Rainbow";
			var zoneName = "FadeCandyZone";
			dynamic inputBag = new InputBag();
			inputBag.Speed = 50;

			var zlm = new ZLM(false, false, false);
			var zlmrpc = new ZLMRPC(zlm);
			var testLC = new TestLightingController("tlc1");
			zlm.AddZone(zoneName, testLC, 8);
			zlm.CreateProgramSet(programSetName, programName, zlm.Zones.Select(z => z.Name), true, inputBag);
			zlm.Zones.ForEach(zone =>
			{
				Assert.That(zone.ZoneProgram, Is.Not.Null);
				Assert.That(zone.ZoneProgram.State, Is.EqualTo(ProgramState.Started));
				Assert.That(zone.ZoneProgram.GetInput("Speed").Value, Is.EqualTo(50));
			});

			inputBag.Speed = 90;

			//act
			zlmrpc.SetProgramSetInputs(programSetName, inputBag);

			//assert
			zlm.Zones.ForEach(zone =>
			{
				Assert.That(zone.ZoneProgram.GetInput("Speed").Value, Is.EqualTo(90));
			});

			zlmrpc.Dispose();
		}

		[Test]
		public void RecreateProgramSetWithoutZone_Works()
		{
			string programSetName = "RainbowSet";
			string programName = "Rainbow";
			var zoneName = "FadeCandyZone";
			dynamic inputBag = new InputBag();
			inputBag.Speed = 50;

			var zlm = new ZLM(false, false, false);
			var zlmrpc = new ZLMRPC(zlm);

			var testLC = new TestLightingController("tlc1");
			var fadeCandyZone1 = zlm.AddZone("FadeCandyZone1", testLC, 8);
			var fadeCandyZone2 = zlm.AddZone("FadeCandyZone2", testLC, 8);
			zlm.CreateProgramSet(programSetName, programName, zlm.Zones.Select(z => z.Name), true, inputBag);

			Assert.That(zlm.ProgramSets[programSetName].Zones, Contains.Item(fadeCandyZone1));
			Assert.That(zlm.ProgramSets[programSetName].Zones, Contains.Item(fadeCandyZone2));

			//act
			zlmrpc.RecreateProgramSetWithoutZone(programSetName, fadeCandyZone2.Name);

			//assert
			Assert.That(zlm.ProgramSets[programSetName].Zones, Contains.Item(fadeCandyZone1));
			Assert.That(zlm.ProgramSets[programSetName].Zones, Is.Not.Contains(fadeCandyZone2));

			Assert.That(zlm.AvailableZones, Contains.Item(fadeCandyZone2));

			zlmrpc.Dispose();
		}

		[Test]
		public void StopZone_Works()
		{
			var programSetName = "StepperSet";

			var zlm = new ZLM(false, false, false);
			var zlmrpc = new ZLMRPC(zlm);
			var zoneName = "FadeCandyZone";
			var testLC = new TestLightingController("tlc1");
			zlm.AddZone(zoneName, testLC, 8);
			zlm.CreateProgramSet(programSetName, "Stepper", zlm.Zones.Select(z => z.Name));
			Assert.That(zlm.Zones[zoneName].Running, Is.True);

			//act
			zlmrpc.StopZone(zoneName, false);

			//assert
			Assert.That(zlm.Zones[zoneName].Running, Is.False);

			zlmrpc.Dispose();
		}

		[Test]
		public void GetZoneSummary_OneZone_Running_Works()
		{
			string expected = "Currently 1 zones are loaded.\r\n===================\r\nZone: FadeCandyZone\r\nProgram: Stepper\r\n-------------------\r\n";
			var programSetName = "StepperSet";

			var zlm = new ZLM(false, false, false);
			var zlmrpc = new ZLMRPC(zlm);
			var zoneName = "FadeCandyZone";
			var testLC = new TestLightingController("tlc1");
			zlm.AddZone(zoneName, testLC, 8);
			zlm.CreateProgramSet(programSetName, "Stepper", zlm.Zones.Select(z => z.Name));

			var zoneSummary = zlmrpc.GetZoneSummary();

			Assert.That(zoneSummary, Is.EqualTo(expected));

			zlmrpc.Dispose();
		}

		[Test]
		public void GetZoneSummary_OneZone_Stopped_Works()
		{
			string expected = "Currently 1 zones are loaded.\r\n===================\r\nZone: FadeCandyZone\r\nProgram: None\r\n-------------------\r\n";
			var programSetName = "StepperSet";

			var zlm = new ZLM(false, false, false);
			var zlmrpc = new ZLMRPC(zlm);
			var zoneName = "FadeCandyZone";
			var testLC = new TestLightingController("tlc1");
			zlm.AddZone(zoneName, testLC, 8);

			var zoneSummary = zlmrpc.GetZoneSummary();

			Assert.That(zoneSummary, Is.EqualTo(expected));

			zlmrpc.Dispose();
		}

		[Test]
		public void GetZoneSummary_TwoZones_Running_Works()
		{
			string expected = "Currently 2 zones are loaded.\r\n===================\r\nZone: FadeCandyZone1\r\nProgram: Rainbow\r\n-------------------\r\nZone: FadeCandyZone2\r\nProgram: Rainbow\r\n-------------------\r\n";
			string programSetName = "RainbowSet";
			string programName = "Rainbow";
			var zoneName = "FadeCandyZone";
			dynamic inputBag = new InputBag();
			inputBag.Speed = 50;

			var zlm = new ZLM(false, false, false);
			var zlmrpc = new ZLMRPC(zlm);
			var testLC = new TestLightingController("tlc1");
			var fadeCandyZone1 = zlm.AddZone("FadeCandyZone1", testLC, 8);
			var fadeCandyZone2 = zlm.AddZone("FadeCandyZone2", testLC, 8);
			zlm.CreateProgramSet(programSetName, programName, zlm.Zones.Select(z => z.Name), true, inputBag);

			var zoneSummary = zlmrpc.GetZoneSummary();

			Assert.That(zoneSummary, Is.EqualTo(expected));

			zlmrpc.Dispose();
		}

		[Test]
		public void GetZoneSummary_TwoZones_Stopped_Works()
		{
			string expected = "Currently 2 zones are loaded.\r\n===================\r\nZone: FadeCandyZone1\r\nProgram: None\r\n-------------------\r\nZone: FadeCandyZone2\r\nProgram: None\r\n-------------------\r\n";
			string programSetName = "RainbowSet";
			string programName = "Rainbow";
			var zoneName = "FadeCandyZone";
			dynamic inputBag = new InputBag();
			inputBag.Speed = 50;

			var zlm = new ZLM(false, false, false);
			var zlmrpc = new ZLMRPC(zlm);
			var testLC = new TestLightingController("tlc1");
			var fadeCandyZone1 = zlm.AddZone("FadeCandyZone1", testLC, 8);
			var fadeCandyZone2 = zlm.AddZone("FadeCandyZone2", testLC, 8);

			var zoneSummary = zlmrpc.GetZoneSummary();

			Assert.That(zoneSummary, Is.EqualTo(expected));

			zlmrpc.Dispose();
		}

		[Test]
		public void SetColor_Works()
		{
			var zlm = new ZLM(false, false, false);
			var zlmrpc = new ZLMRPC(zlm);

			var zoneName = "Zone1";
			var testLC = new TestLightingController("tlc1");
			zlm.AddZone(zoneName, testLC, 8);

			zlmrpc.SetZoneColor("Zone1", "Blue", (float)0.5);

			zlm.Zones["Zone1"].SortedLights.ToList().ForEach(light =>
			{
				Assert.That(light.Value.Color.A, Is.EqualTo(255));
				Assert.That(light.Value.Color.B, Is.EqualTo(127));
				Assert.That(light.Value.Color.R, Is.EqualTo(0));
				Assert.That(light.Value.Color.G, Is.EqualTo(0));
			});

			zlmrpc.Dispose();
		}

		[Test]
		public void SetLightColor_Works()
		{
			var zlm = new ZLM(false, false, false);
			var zlmrpc = new ZLMRPC(zlm);

			var zoneName = "Zone1";
			var testLC = new TestLightingController("tlc1");
			zlm.AddZone(zoneName, testLC, 8);

			zlmrpc.SetLightColor("Zone1", "Blue", 0, (float)0.5);

			var color = zlm.Zones["Zone1"].SortedLights[0].Color;

			Assert.That(color.A, Is.EqualTo(255));
			Assert.That(color.B, Is.EqualTo(127));
			Assert.That(color.R, Is.EqualTo(0));
			Assert.That(color.G, Is.EqualTo(0));

			zlmrpc.Dispose();
		}

		[Test]
		public void SetAllZonesColor_Works()
		{
			var zlm = new ZLM(false, false, false);
			var zlmrpc = new ZLMRPC(zlm);

			var testLC = new TestLightingController("tlc1");
			var zone1 = zlm.AddZone("Zone1", testLC, 8);
			var zone2 = zlm.AddZone("Zone2", testLC, 8);

			zlmrpc.SetAllZonesColor("Blue", (float)0.5);

			zlm.Zones.ToList().ForEach(zone =>
				zone.SortedLights.ToList().ForEach(light =>
				{
					Assert.That(light.Value.Color.A, Is.EqualTo(255));
					Assert.That(light.Value.Color.B, Is.EqualTo(127));
					Assert.That(light.Value.Color.R, Is.EqualTo(0));
					Assert.That(light.Value.Color.G, Is.EqualTo(0));
				}));

			zlmrpc.Dispose();
		}

		[Test]
		public void SetZoneInputs_Works()
		{
			string programSetName = "RainbowSet";
			string programName = "Rainbow";
			dynamic inputBag = new InputBag();
			inputBag.Speed = 50;

			var zlm = new ZLM(false, false, false);
			var zlmrpc = new ZLMRPC(zlm);
			var testLC = new TestLightingController("tlc1");
			var zone1 = zlm.AddZone("Zone1", testLC, 8);
			var zone2 = zlm.AddZone("Zone2", testLC, 8);

			zlm.CreateProgramSet(programSetName, programName, zlm.Zones.Select(z => z.Name), true, inputBag);

			inputBag.Speed = 90;

			//act
			zlmrpc.SetZoneInputs("Zone1", inputBag);

			//assert
			Assert.That(zone1.ZoneProgram.GetInput("Speed").Value, Is.EqualTo(90));
			Assert.That(zone2.ZoneProgram.GetInput("Speed").Value, Is.EqualTo(50));

			zlmrpc.Dispose();
		}

		[Test]
		public void GetZones_ZonesRunning_Works()
		{
			string programSetName = "RainbowSet";
			string programName = "Rainbow";
			dynamic inputBag = new InputBag();
			inputBag.Speed = 50;

			var zlm = new ZLM(false, false, false);
			var zlmrpc = new ZLMRPC(zlm);
			var testLC = new TestLightingController("tlc1");
			var zone1 = zlm.AddZone("Zone1", testLC, 8);
			var zone2 = zlm.AddZone("Zone2", testLC, 8);

			zlm.CreateProgramSet(programSetName, programName, zlm.Zones.Select(z => z.Name), true, inputBag);

			//act
			var zones = zlmrpc.GetZones();

			//assert
			Assert.That(zones.First(x => x.Name == "Zone1").Brightness, Is.EqualTo(zlm.Zones["Zone1"].Brightness));
			Assert.That(zones.First(x => x.Name == "Zone1").LightCount, Is.EqualTo(zlm.Zones["Zone1"].LightCount));
			Assert.That(zones.First(x => x.Name == "Zone1").Running, Is.EqualTo(zlm.Zones["Zone1"].Running));
			Assert.That(zones.First(x => x.Name == "Zone1").ProgramName, Is.EqualTo(zlm.Zones["Zone1"].ZoneProgram.Name));

			Assert.That(zones.First(x => x.Name == "Zone2").Brightness, Is.EqualTo(zlm.Zones["Zone2"].Brightness));
			Assert.That(zones.First(x => x.Name == "Zone2").LightCount, Is.EqualTo(zlm.Zones["Zone2"].LightCount));
			Assert.That(zones.First(x => x.Name == "Zone2").Running, Is.EqualTo(zlm.Zones["Zone2"].Running));
			Assert.That(zones.First(x => x.Name == "Zone2").ProgramName, Is.EqualTo(zlm.Zones["Zone2"].ZoneProgram.Name));

			zlmrpc.Dispose();
		}

		[Test]
		public void GetZones_ZonesStopped_Works()
		{
			string programSetName = "RainbowSet";
			string programName = "Rainbow";
			dynamic inputBag = new InputBag();
			inputBag.Speed = 50;

			var zlm = new ZLM(false, false, false);
			var zlmrpc = new ZLMRPC(zlm);
			var testLC = new TestLightingController("tlc1");
			var zone1 = zlm.AddZone("Zone1", testLC, 8);
			var zone2 = zlm.AddZone("Zone2", testLC, 8);


			//act
			var zones = zlmrpc.GetZones();

			//assert
			Assert.That(zones.First(x => x.Name == "Zone1").Brightness, Is.EqualTo(zlm.Zones["Zone1"].Brightness));
			Assert.That(zones.First(x => x.Name == "Zone1").LightCount, Is.EqualTo(zlm.Zones["Zone1"].LightCount));
			Assert.That(zones.First(x => x.Name == "Zone1").Running, Is.EqualTo(zlm.Zones["Zone1"].Running));
			Assert.That(zones.First(x => x.Name == "Zone1").ProgramName, Is.EqualTo(string.Empty).Or.EqualTo(null));

			Assert.That(zones.First(x => x.Name == "Zone2").Brightness, Is.EqualTo(zlm.Zones["Zone2"].Brightness));
			Assert.That(zones.First(x => x.Name == "Zone2").LightCount, Is.EqualTo(zlm.Zones["Zone2"].LightCount));
			Assert.That(zones.First(x => x.Name == "Zone2").Running, Is.EqualTo(zlm.Zones["Zone2"].Running));
			Assert.That(zones.First(x => x.Name == "Zone2").ProgramName, Is.EqualTo(string.Empty).Or.EqualTo(null));

			zlmrpc.Dispose();
		}

		[Test]
		public void GetZones_OneRunningOneStopped_Works()
		{
			string programSetName = "RainbowSet";
			string programName = "Rainbow";
			dynamic inputBag = new InputBag();
			inputBag.Speed = 50;

			var zlm = new ZLM(false, false, false);
			var zlmrpc = new ZLMRPC(zlm);
			var testLC = new TestLightingController("tlc1");
			var zone1 = zlm.AddZone("Zone1", testLC, 8);
			var zone2 = zlm.AddZone("Zone2", testLC, 8);

			zlm.CreateProgramSet(programSetName, programName, "Zone2".Listify(), true, inputBag);

			//act
			var zones = zlmrpc.GetZones();

			//assert
			Assert.That(zones.First(x => x.Name == "Zone1").Brightness, Is.EqualTo(zlm.Zones["Zone1"].Brightness));
			Assert.That(zones.First(x => x.Name == "Zone1").LightCount, Is.EqualTo(zlm.Zones["Zone1"].LightCount));
			Assert.That(zones.First(x => x.Name == "Zone1").Running, Is.EqualTo(zlm.Zones["Zone1"].Running));
			Assert.That(zones.First(x => x.Name == "Zone1").ProgramName, Is.EqualTo(string.Empty).Or.EqualTo(null));

			Assert.That(zones.First(x => x.Name == "Zone2").Brightness, Is.EqualTo(zlm.Zones["Zone2"].Brightness));
			Assert.That(zones.First(x => x.Name == "Zone2").LightCount, Is.EqualTo(zlm.Zones["Zone2"].LightCount));
			Assert.That(zones.First(x => x.Name == "Zone2").Running, Is.EqualTo(zlm.Zones["Zone2"].Running));
			Assert.That(zones.First(x => x.Name == "Zone2").ProgramName, Is.EqualTo(zlm.Zones["Zone2"].ZoneProgram.Name));

			zlmrpc.Dispose();
		}
	}
}
