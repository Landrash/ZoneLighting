﻿using System.Threading;
using NUnit.Framework;
using ZoneLighting;
using ZoneLighting.Communication;
using ZoneLighting.ConfigNS;
using ZoneLighting.StockPrograms;
using ZoneLighting.ZoneProgramNS;
using ZoneLighting.ZoneProgramNS.Factories;

namespace ZoneLightingTests
{
	public class ShimmerTests
	{
		[TestCase(60)]
		[Ignore("Manual")]
		public void Shimmer_Works(int sleepSeconds)
		{
			//act
			var zlm = new ZLM(false, false, false, zlmInner =>
			{
				var isv = new ISV();
				isv.Add("MaxFadeSpeed", 1);
				isv.Add("MaxFadeDelay", 20);
				isv.Add("Density", 16);
				isv.Add("Brightness", 0.4);
				var neomatrix = ZoneScaffolder.Instance.AddFadeCandyZone(zlmInner.Zones, "NeoMatrix", PixelType.FadeCandyWS2812Pixel, 64, 1);
				zlmInner.CreateSingleProgramSet("", new Shimmer(), isv, neomatrix);
			}, Config.Get("Shimmer_Works"));

			Thread.Sleep(sleepSeconds * 1000);

			//cleanup
			zlm.Dispose();
		}
	}
}
