﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using ZoneLighting.Communication;

namespace ZoneLighting.ZoneProgram.Programs
{
	/// <summary>
	/// Scrolls a dot across the entire length of Lights
	/// </summary>
	public class ScrollDot : LoopingZoneProgram
	{
		public override void Loop(IZoneProgramParameter parameter)
		{
			var scrollDotParameter = (ScrollDotParameter)parameter;
			
			var colors = new List<Color>();
			colors.Add(Color.Red);
			colors.Add(Color.Blue);
			colors.Add(Color.Yellow);
			colors.Add(Color.Green);
			colors.Add(Color.Purple);
			colors.Add(Color.RoyalBlue);
			colors.Add(Color.MediumSeaGreen);

			for (int i = 0; i < Zone.Lights.Count; i++)
			{
				Lights.SetColor(Color.FromArgb(0, 0, 0));								//set all lights to black
				Lights[i].SetColor(scrollDotParameter.Color != null
					? (Color)scrollDotParameter.Color
					: colors[new Random().Next(0, colors.Count - 1)]);					//set one to white
				LightingController.SendLEDs(Lights.Cast<LED>().ToList());				//send frame
				ProgramCommon.Delay(scrollDotParameter.DelayTime);						//pause before next iteration
			}
		}

		public override IEnumerable<Type> AllowedParameterTypes
		{
			get
			{
				return new List<Type>()
				{
					typeof (ScrollDotParameter)
				};
			}
		}
	}

	public class ScrollDotParameter : IZoneProgramParameter
	{
		public ScrollDotParameter(int delayTime, Color? color = null)
		{
			DelayTime = delayTime;
			Color = color;
		}
		public int DelayTime { get; set; }
		public Color? Color { get; set; }
	}
}
