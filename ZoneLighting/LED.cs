﻿using System.Drawing;
using System.Runtime.Serialization;
using Graphics;
using LightingControllerBase;
using ZoneLighting.MEF;

namespace ZoneLighting
{
    /// <summary>
    /// Represents an LED. This class must implement the ILightingControllerPixel for each type of lighting controller
    /// that it needs to be output on (for example is currently implements IOPCPixelContainer which inherits from ILightingControllerPixel).
    /// </summary>
    [DataContract]
    public class LED : IPixel //ILogicalRGBLight, IOPCPixelContainer
	{
		#region CORE

        public Color Color { get; set; }

  //      [DataMember]
		//public OPCPixel OPCPixel { get; set; }

	    [DataMember]
	    public int Index { get; set; }

	    #endregion
        
		#region C+I

	    public LED(Color? color = null, int? index = null)
		{
			//OPCPixel = GetOPCPixelInstance(pixelType);

		    if (color != null)
		        Color = (Color)color;
            if (index != null)
				Index = (int)index;
			//if (fadeCandyChannel != null)
			//	OPCPixel.Channel = (byte)fadeCandyChannel;
			//if (fadeCandyIndex != null)
			//	OPCPixel.PhysicalIndex = (int)fadeCandyIndex;
		}

        #endregion
        
	    #region Color Parts

	    #region HSB

	    /// <summary>
	    /// The hue, in degrees, of the underlying System.Drawing.Color. 
	    /// The hue is measured in degrees, ranging from 0.0 through 360.0, in HSB color space.
	    /// </summary>
	    public float Hue => Color.GetHue();

	    /// <summary>
	    /// The saturation of the underlying System.Drawing.Color.
	    /// The saturation ranges from 0.0 through 1.0, where 0.0 is grayscale and 1.0 is the most saturated.
	    /// </summary>
	    public float Saturation => Color.GetSaturation();

	    /// <summary>
	    /// The brightness of the underlying System.Drawing.Color. 
	    /// The brightness ranges from 0.0 through 1.0, where 0.0 represents black and 1.0 represents white.
	    /// </summary>
	    public float Brightness => Color.GetBrightness();

	    #endregion

	    #endregion

        //#region API
        
  //      public bool SetColor(Color color)
		//{
		//	Color = color;
		//	return true;
		//}

		//public Color GetColor()
		//{
		//	return Color;
		//}

		//#endregion
	}
}
