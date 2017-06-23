using System.Configuration;
using System.Drawing;
using System.Threading;
using NUnit.Framework;
using ZoneLighting;
using ZoneLighting.TestApparatus;
using ZoneLighting.ZoneNS;
using ZoneLighting.ZoneProgramNS;
using ZoneLighting.ZoneProgramNS.Factories;
using ZoneLightingTests.Resources.Programs;

namespace ZoneLightingTests
{
	public class ZoneProgramTests
	{
		[Test]
		public void ForceStop_Works()
		{
			//arrange
			var leftWing = new Zone(new TestLightingController("tlc1", null), "LeftWing");
			leftWing.AddLights(6);

			dynamic scrollDotDictionary = new ISV();
			scrollDotDictionary.DelayTime = 30;
			scrollDotDictionary.DotColor = (Color?)Color.Red;

			leftWing.Run(new ScrollDot(), scrollDotDictionary);

			//act
			leftWing.ZoneProgram.Stop(true);

			//assert
			var result = leftWing.ZoneProgram.StopTestingTrigger.WaitForFire(1000);

			//cleanup
			leftWing.Dispose();

			Assert.True(result);
		}

		[Test]
		[Timeout(10000)]
		public static void CooperativeStop_Works()
		{
			DebugTools.AddEvent("Test.CooperativeStop_Works", "START CooperativeStop_Works Test");

			//arrange
			var leftWing = new Zone(new TestLightingController("tlc1", null), "LeftWing");
			leftWing.AddLights(6);

			dynamic scrollDotDictionary = new ISV();
			scrollDotDictionary.DelayTime = 30;
			scrollDotDictionary.DotColor = (Color?)Color.Red;

			leftWing.Run(new ScrollDot(), scrollDotDictionary);

			//this is to fix the race condition that sometimes causes this test to fail - since this test 
			//is not designed to test race conditions, just whether cooperative stop works in normal conditions
			Thread.Sleep(100);

			//act -- cooperative stop
			leftWing.ZoneProgram.Stop(false);

			//assert
			var result = leftWing.ZoneProgram.StopTestingTrigger.WaitForFire(1000);

			//cleanup
			leftWing.Dispose();

			DebugTools.AddEvent("Test.CooperativeStop_Works", "END CooperativeStop_Works Test");

			Assert.True(result);
		}
	}
}
