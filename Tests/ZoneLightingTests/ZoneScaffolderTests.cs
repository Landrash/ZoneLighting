﻿using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using ZoneLighting.ConfigNS;
using ZoneLighting.ZoneNS;
using ZoneLighting.ZoneProgramNS;
using ZoneLighting.ZoneProgramNS.Factories;

namespace ZoneLightingTests
{
	public class ZoneScaffolderTests
	{
		//[Ignore("Needs fixing")]
		//[Test]
		////TODO: Fix
		//public void InitializeFromZoneConfiguration_Works()
		//{
		//	//arrange
		//	var zones = new List<Zone>();
		//	var zone = new Zone("TestZone");

		//	zones.Add(zone);
		//	zone.AddLights(6);

		//	var zoneScaffolder = new ZoneScaffolder();
		//	ZoneScaffolder.Instance.Initialize(ConfigurationManager.AppSettings["ProgramDLLFolder"], ConfigurationManager.AppSettings["LightingControllerDLLFolder"]);

		//	//act
		//	zoneScaffolder.InitializeFromZoneConfiguration(zones, ConfigurationManager.AppSettings["InitializeFromZoneConfiguration_Works_TestFile"]);

		//	//assert
		//	Assert.AreEqual(zone.Name, "TestZone");
		//	Assert.AreEqual(zone.ZoneProgram.Name, "ScrollDot");

		//	var inputNames = zone.ZoneProgram.GetInputNames();

		//	foreach (var inputName in inputNames)
		//	{
		//		if (inputName == "DelayTime")
		//		{
		//			Assert.AreEqual(zone.ZoneProgram.GetInputValue(inputName), 30);
		//		}
		//		else if (inputName == "DotColor")
		//		{
		//			Assert.AreEqual(zone.ZoneProgram.GetInputValue(inputName), Color.Red);
		//		}
		//	}
		//}

		[Test]
		public void Initialize_LightingController_Works()
		{
			var zoneScaffolder = new ZoneScaffolder();
			zoneScaffolder.Initialize("Resources\\Programs", "Resources\\LightingControllers");
			Assert.That(zoneScaffolder.LightingControllerFactories.ToList().Select(x => x.Metadata.Name).Contains("TestPluginController"));
		}

		[Test]
		public void InitLightingController_Works()
		{
			var zoneScaffolder = new ZoneScaffolder();
			zoneScaffolder.Initialize("Resources\\Programs", "Resources\\LightingControllers");
			zoneScaffolder.InitLightingControllers();
			Assert.That(zoneScaffolder.LightingControllerFactories.ToList().Select(x => x.Metadata.Name).Contains("TestPluginController"));
		}

		///// <summary>
		///// This is just there to generate the test file for this test in case the code changes something
		///// fundamental that is reflected in the saved configuration.
		///// </summary>
		///// <param name="filename"></param>
		//[Ignore("Not a test, just a piece of code that needs to be reused often.")]
		//[TestCase(@"C:\Temp\test.config")] //NOTE: Insert file path in test case to generate to the path.
		//public void GenerateTestConfiguration(string filename)
		//{
		//	var zoneScaffolder = new ZoneScaffolder();
		//	zoneScaffolder.Initialize(ConfigurationManager.AppSettings["TestProgramModuleDirectory"]);

		//	var leftWing = new OPCZone(FadeCandyController.Instance, "TestZone");
		//	leftWing.AddOPCLights(6);

		//	dynamic scrollDotDictionary = new InputBag();
		//	scrollDotDictionary.DelayTime = 30;
		//	scrollDotDictionary.DotColor = (Color?)Color.Red;

		//	zoneScaffolder.RunZone(leftWing, "ScrollDot", scrollDotDictionary);

		//	Config.SaveZones(new List<Zone>() { leftWing }, filename);
		//}
	}
}

