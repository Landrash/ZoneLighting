using System;
using System.Collections.Generic;
using ZoneLighting.Usables;

namespace ZoneLighting.Communication
{
	public interface ILightingController : IBetterListType
	{
		void SendPixelFrame(IPixelFrame opcPixelFrame);
		void SendLEDs(IList<ILightingControllerPixel> leds);

        /// <summary>
        /// Pixel type determines the way pixels will be sent out
        /// </summary>
        Type PixelType { get; }

		/// <summary>
		/// Sends a list of Lights to the implementing lighting controller
		/// </summary>
		void SendLights(IList<ILightingControllerPixel> lights);
	}
}