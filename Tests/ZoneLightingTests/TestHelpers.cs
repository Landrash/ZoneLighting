using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using NUnit.Framework;
using Refigure;
using ZoneLighting;
using ZoneLighting.StockPrograms;
using ZoneLighting.TestApparatus;
using ZoneLighting.ZoneNS;
using ZoneLighting.ZoneProgramNS.Factories;

namespace ZoneLightingTests
{
	public static class TestHelpers
	{
		public static void InitializeZoneScaffolder()
		{
			ZoneScaffolder.Instance.Initialize(Config.Get("ProgramModuleDirectory"), Config.Get("LightingControllerModuleDirectory"));
		}

		public static void ValidateSteppersInSync(IEnumerable<IStepper> steppers, int numberOfChecks, int msToWaitBeforeStart = 10, int msToWaitBetweenChecks = 1, bool print = false)
		{
			int[,] stepperSteps;
			var stepperArray = steppers as IStepper[] ?? steppers.ToArray();
			var invalidStepIndex = SyncContextTests.ValidateStepperSyncPhase(stepperArray.ToArray(), out stepperSteps, numberOfChecks);
			if (print) SyncContextTests.PrintStepperSteps(stepperArray.ToArray(), stepperSteps);
			Assert.True(invalidStepIndex.Length == 0 && stepperSteps.Length != 0);
		}

		public static void ValidateSteppersRunning(IEnumerable<IStepper> steppers, int numberOfChecks, int msToWaitBeforeStart = 10, int msToWaitBetweenChecks = 1)
		{
			int[,] stepperSteps;
			SyncContextTests.ValidateStepperSyncPhase(steppers.ToArray(), out stepperSteps, numberOfChecks);
			Assert.True(stepperSteps.Length != 0);
		}

		public static void ValidateSteppersNotRunning(IEnumerable<IStepper> steppers, int numberOfChecks, int msToWaitBeforeStart = 10, int msToWaitBetweenChecks = 1)
		{
			int[,] stepperSteps;
			SyncContextTests.ValidateStepperSyncPhase(steppers.ToArray(), out stepperSteps, numberOfChecks);
			Assert.True(stepperSteps.Length == 0);
		}

		public static void ValidateSteppersOutOfSync(IEnumerable<IStepper> steppers, int numberOfChecks,
			int msToWaitBeforeStart = 10, int msToWaitBetweenChecks = 1, int minOutOfSyncStepsThreshold = 0)
		{
			int[,] stepperSteps;
			var invalidStepIndex = SyncContextTests.ValidateStepperSyncPhase(steppers.ToArray(), out stepperSteps, numberOfChecks);
			Assert.True(invalidStepIndex.Length > minOutOfSyncStepsThreshold);
		}

		public static void AddFourZonesAndStepperProgramSetWithSyncToZLM(ZLM zlm)
		{
			var notificationSyncContext = new SyncContext("NotificationContext");
			var testLC = new TestLightingController("tlc1", null);

			//add zones
			var zoneA = ZoneScaffolder.Instance.AddZone(zlm.Zones, "ZoneA", testLC, 16);
			var zoneB = ZoneScaffolder.Instance.AddZone(zlm.Zones, "ZoneB", testLC, 16);
			var zoneC = ZoneScaffolder.Instance.AddZone(zlm.Zones, "ZoneC", testLC, 16);
			var zoneD = ZoneScaffolder.Instance.AddZone(zlm.Zones, "ZoneD", testLC, 16);

			zlm.CreateProgramSet("StepperSet", "Stepper", true, null, zlm.Zones);

			//setup interrupting inputs - in the real code this method should not be used. The ZoneScaffolder.AddInterruptingProgram should be used.
			zoneA.AddInterruptingProgram(new BlinkColorReactive(), null, notificationSyncContext);
			zoneB.AddInterruptingProgram(new BlinkColorReactive(), null, notificationSyncContext);
			zoneC.AddInterruptingProgram(new BlinkColorReactive(), null, notificationSyncContext);
			zoneD.AddInterruptingProgram(new BlinkColorReactive(), null, notificationSyncContext);

			//synchronize and start interrupting programs
			notificationSyncContext.Sync(zoneA.InterruptingPrograms[0],
				zoneB.InterruptingPrograms[0],
				zoneC.InterruptingPrograms[0],
				zoneD.InterruptingPrograms[0]);

			zoneA.InterruptingPrograms[0].Start();
			zoneB.InterruptingPrograms[0].Start();
			zoneC.InterruptingPrograms[0].Start();
			zoneD.InterruptingPrograms[0].Start();
		}

		public static Action AddFourZonesAndStepperProgramSetWithSyncToZLMAction(ZLM zlm)
		{
			return () =>
			{
				AddFourZonesAndStepperProgramSetWithSyncToZLM(zlm);
			};
		}
	}
}
