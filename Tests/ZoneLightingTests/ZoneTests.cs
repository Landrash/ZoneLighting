using System.Drawing;
using NUnit.Framework;
using ZoneLighting.StockPrograms;
using ZoneLighting.TestApparatus;
using ZoneLighting.ZoneNS;
using ZoneLighting.ZoneProgramNS;

namespace ZoneLightingTests
{
	public class ZoneTests
	{
		[Test]
		public void SetAllLightsColor_Works()
		{
			var zone = new Zone(new TestLightingController("tlc1", null), "TestZone");
			zone.AddLights(6);
			var color = Color.Red;
			zone.SetAllLightsColor(color);

			for (int i = 0; i < zone.LightCount; i++)
			{
				Assert.AreEqual(zone.GetColor(i), color);
            }
		}

		[Test]
		public void Run_Works()
		{
			var zone = new Zone(new TestLightingController("tlc1", null), "TestZone");
			var program = new Rainbow();
			zone.AddLights(6);
			Assert.DoesNotThrow(() => zone.Run(program));
			Assert.True(zone.Running);
			Assert.True(program.State == ProgramState.Started);
		}

		[Test]
		public void Run_WithSync_Works()
		{
			var zone = new Zone(new TestLightingController("tlc1", null), "TestZone");
			var program = new Rainbow();
			var syncContext = new SyncContext();
			zone.AddLights(6);
			Assert.DoesNotThrow(() => zone.Run(program, null, true, syncContext));
			Assert.True(zone.Running);
			Assert.True(program.State == ProgramState.Started);
			zone.Dispose();
		}

		[Test]
		public void Stop_Works()
		{
			var lightingController = new TestLightingController("tlc1", null);
			var zone = new Zone(lightingController, "TestZone");
			var program = new Rainbow();
			zone.AddLights(6);
			zone.Run(program);
			Assert.DoesNotThrow(() => zone.Stop(true));
			Assert.False(zone.Running);
			Assert.True(program.State == ProgramState.Stopped);
		}
	}
}
