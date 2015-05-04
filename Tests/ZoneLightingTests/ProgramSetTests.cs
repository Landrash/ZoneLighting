﻿using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ZoneLighting.Communication;
using ZoneLighting.StockPrograms;
using ZoneLighting.Usables;
using ZoneLighting.Usables.TestInterfaces;
using ZoneLighting.ZoneNS;
using ZoneLighting.ZoneProgramNS;
using ZoneLighting.ZoneProgramNS.Factories;

namespace ZoneLightingTests
{
	[Category("Integration")]
	public class ProgramSetTests
	{
		[Test]
		public void ProgramSetConstructor_WithSync_SteppersSynced_ZonesInit_ProgramsInit()
		{
			//arrange
			TestHelpers.InitializeZoneScaffolder();

			//create zones
			var zones = new BetterList<Zone>();
			FadeCandyController.Instance.Initialize();
			var leftWing = ZoneScaffolder.Instance.AddFadeCandyZone(zones, "LeftWing", PixelType.FadeCandyWS2812Pixel, 6, 1);
			var rightWing = ZoneScaffolder.Instance.AddFadeCandyZone(zones, "RightWing", PixelType.FadeCandyWS2811Pixel, 21, 2);

			//act
			var programSet = new ProgramSet("Stepper", zones.ToList(), true, null, "StepperSet");

			//assert
			TestHelpers.ValidateSteppersSync(((ITestProgramSet)programSet).ZoneProgramsTest.Cast<IStepper>(), 100);
			Assert.That(leftWing.Initialized, Is.True);
			Assert.That(rightWing.Initialized, Is.True);
			Assert.That(leftWing.ZoneProgram.State == ProgramState.Started, Is.True);
			Assert.That(rightWing.ZoneProgram.State == ProgramState.Started, Is.True);

			//cleanup
			programSet.Dispose();
			leftWing.Dispose();
			rightWing.Dispose();
		}

		[Test]
		public void ProgramSetConstructor_NoSync_ZonesInit_ProgramsInit()
		{
			//arrange
			TestHelpers.InitializeZoneScaffolder();

			var zones = new BetterList<Zone>();
			FadeCandyController.Instance.Initialize();
			var leftWing = ZoneScaffolder.Instance.AddFadeCandyZone(zones, "LeftWing", PixelType.FadeCandyWS2812Pixel, 6, 1);
			var rightWing = ZoneScaffolder.Instance.AddFadeCandyZone(zones, "RightWing", PixelType.FadeCandyWS2811Pixel, 21, 2);

			//act
			var programSet = new ProgramSet("Stepper", zones.ToList(), false, null, "StepperSet");

			//assert
			TestHelpers.ValidateSteppersRunning(((ITestProgramSet)programSet).ZoneProgramsTest.Cast<IStepper>(), 100);
			Assert.That(leftWing.Initialized, Is.True);
			Assert.That(rightWing.Initialized, Is.True);
			Assert.That(leftWing.ZoneProgram.State == ProgramState.Started, Is.True);
			Assert.That(rightWing.ZoneProgram.State == ProgramState.Started, Is.True);

			//cleanup
			programSet.Dispose();
			leftWing.Dispose();
			rightWing.Dispose();
		}

		[Test]
		public void RemoveZone_ZoneIsUnintializedAndNotInZonesCollection()
		{
			//arrange
			TestHelpers.InitializeZoneScaffolder();

			var zones = new BetterList<Zone>();
			FadeCandyController.Instance.Initialize();
			var leftWing = ZoneScaffolder.Instance.AddFadeCandyZone(zones, "LeftWing", PixelType.FadeCandyWS2812Pixel, 6, 1);
			var rightWing = ZoneScaffolder.Instance.AddFadeCandyZone(zones, "RightWing", PixelType.FadeCandyWS2811Pixel, 21, 2);

			var programSet = new ProgramSet("Stepper", zones.ToList(), false, null, "StepperSet");

			//act
			programSet.RemoveZone(rightWing);

			//assert
			TestHelpers.ValidateSteppersRunning(((ITestProgramSet)programSet).ZoneProgramsTest.Cast<IStepper>(), 100);
			Assert.That(leftWing.Initialized, Is.True);
			Assert.That(rightWing.Initialized, Is.False);
			Assert.That(leftWing.ZoneProgram.State == ProgramState.Started, Is.True);
			Assert.That(programSet.Zones.Contains(rightWing), Is.False);

			//cleanup
			programSet.Dispose();
			leftWing.Dispose();
			rightWing.Dispose();

		}

		/// <summary>
		/// Tests to see if two program sets can be run simultaneously.
		/// </summary>
		[Test]
		public void TwoProgramSets_CanBeRunSimultaneously()
		{
			//arrange
			TestHelpers.InitializeZoneScaffolder();

			//create zones
			var zones = new BetterList<Zone>();
			FadeCandyController.Instance.Initialize();
			var leftWing = ZoneScaffolder.Instance.AddFadeCandyZone(zones, "LeftWing", PixelType.FadeCandyWS2812Pixel, 6, 1);
			var rightWing = ZoneScaffolder.Instance.AddFadeCandyZone(zones, "RightWing", PixelType.FadeCandyWS2811Pixel, 21, 2);
			var center = ZoneScaffolder.Instance.AddFadeCandyZone(zones, "Center", PixelType.FadeCandyWS2811Pixel, 21, 2);
			var baiClock = ZoneScaffolder.Instance.AddFadeCandyZone(zones, "BaiClock", PixelType.FadeCandyWS2812Pixel, 24, 4);

			//act
			var stepperSet1 = new ProgramSet("Stepper", new List<Zone>() { leftWing, rightWing }, true, null, "StepperSet1");
			var stepperSet2 = new ProgramSet("Stepper", new List<Zone>() { center, baiClock }, true, null, "StepperSet2");

			//assert
			TestHelpers.ValidateSteppersSync(((ITestProgramSet)stepperSet1).ZoneProgramsTest.Cast<IStepper>(), 100);
			TestHelpers.ValidateSteppersSync(((ITestProgramSet)stepperSet2).ZoneProgramsTest.Cast<IStepper>(), 100);
			Assert.That(leftWing.Initialized, Is.True);
			Assert.That(rightWing.Initialized, Is.True);
			Assert.That(center.Initialized, Is.True);
			Assert.That(baiClock.Initialized, Is.True);
			Assert.That(leftWing.ZoneProgram.State == ProgramState.Started, Is.True);
			Assert.That(rightWing.ZoneProgram.State == ProgramState.Started, Is.True);
			Assert.That(center.ZoneProgram.State == ProgramState.Started, Is.True);
			Assert.That(baiClock.ZoneProgram.State == ProgramState.Started, Is.True);

			//cleanup
			stepperSet1.Dispose();
			stepperSet2.Dispose();
			leftWing.Dispose();
			rightWing.Dispose();
			center.Dispose();
			baiClock.Dispose();
		}
	}
}
