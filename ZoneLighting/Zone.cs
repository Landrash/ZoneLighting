﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZoneLighting.Communication;

namespace ZoneLighting
{
	/// <summary>
	/// Represents a zone (room or whatever) that contains the lights to be controlled.
	/// </summary>
    public class Zone : IInitializable, IDisposable
	{
		#region CORE

		/// <summary>
		/// Name of the zone.
		/// </summary>
		public string Name;

		/// <summary>
		/// Zones can contain other zones in a recursive fashion.
		/// </summary>
		public IList<Zone> Zones { get; private set; }

		/// <summary>
		/// All lights in the zone.
		/// </summary>
		private SortedList<int, ILight> Lights { get; set; }

		/// <summary>
		/// The Lighting Controller used to control this Zone.
		/// </summary>
		public ILightingController LightingController { get; private set; }

		/// <summary>
		/// Scrolls a dot across the entire length of Lights
		/// </summary>
		private void ScrollDot()
		{
			while (!TaskCTS.IsCancellationRequested)
			{
				var Colors = new List<Color>();
				Colors.Add(Color.Red);
				Colors.Add(Color.Blue);
				Colors.Add(Color.Yellow);
				Colors.Add(Color.Green);
				Colors.Add(Color.Purple);
				Colors.Add(Color.RoyalBlue);
				Colors.Add(Color.MediumSeaGreen);

				for (int i = 0; i < 6; i++)
				{
					Lights.Values.ToList().ForEach(x => x.SetColor(Color.FromArgb(0, 0, 0))); //set all lights to black
					Lights[i].SetColor(Colors[new Random().Next(0, 7)]); //set one to white

					//TODO: This is where the mapping provider would map the Lights collection to the byte order of the data in the OPCPixelFrame
					//send frame 
					LightingController.SendPixelFrame(OPCPixelFrame.CreateFromLightsCollection(0, Lights.Values.Cast<LED>().ToList()));
					Thread.Sleep(50);
				}
			}
		}

		/// <summary>
		/// Static Red
		/// </summary>
		private void StaticRed()
		{
			while (!TaskCTS.IsCancellationRequested)
			{
				var color = Color.Red;

				Lights.Values.ToList().ForEach(x => x.SetColor(color)); //set all lights to black
				LightingController.SendPixelFrame(OPCPixelFrame.CreateFromLightsCollection(0, Lights.Values.Cast<LED>().ToList()));
			}
		}

		/// <summary>
		/// Rainbow
		/// </summary>
		private void Rainbow()
		{
			SetAllLightsColor(Color.FromArgb(0, 0, 0)); //turn all lights off

			while (!TaskCTS.IsCancellationRequested)
			{
				var colors = new List<Color>();
				colors.Add(Color.Violet);
				colors.Add(Color.Indigo);
				colors.Add(Color.Blue);
				colors.Add(Color.Green);
				colors.Add(Color.Yellow);
				colors.Add(Color.Orange);
				colors.Add(Color.Red);

				for (int i = 0; i < 7; i++)
				{
					SetAllLightsColor(colors[i]);

					//send frame 
					LightingController.SendPixelFrame(OPCPixelFrame.CreateFromLightsCollection(0, Lights.Values.Cast<LED>().ToList()));
					Thread.Sleep(5000);
				}
			}
		}

		private void SetAllLightsColor(Color color)
		{
			Lights.Values.ToList().ForEach(x => x.SetColor(color)); //set all lights to black
		}


		public void CancelTask()
		{
			TaskCTS.Cancel();
		}

		private CancellationTokenSource TaskCTS { get; set; }

		#endregion

		#region C+I

		public Zone(ILightingController lightingController, string name = "")
		{
			Zones = new List<Zone>();
			Lights = new SortedList<int, ILight>();
			LightingController = lightingController;
			TaskCTS = new CancellationTokenSource();
			Name = name;
		}

		public void Initialize()
		{
			if (!Initialized)
			{
				foreach (var zone in Zones)
				{
					zone.Initialize();
				}
				//Task.Factory.StartNew(ScrollDot);
				Task.Factory.StartNew(Rainbow);
				Initialized = true;
			}
		}

		public bool Initialized { get; private set; }

		public void Uninitialize()
		{
			if (Initialized)
			{
				CancelTask();
				Initialized = false;
			}
		}

		public void Dispose()
		{
			Zones.Clear();
			Zones = null;
			Lights.Clear();
			Lights = null;
			LightingController.Dispose();
			LightingController = null;
		}

		#endregion

		#region API

		//public void SetColor(Color color)
		//{
		//	foreach (var light in Lights)
		//	{
		//		light.SetColor(color);
		//	}
		//}

		public void AddLight(ILight light)
		{
			Lights.Add(Lights.Count, light);
		}

		public void AddZone(Zone zone)
		{
			Zones.Add(zone);
		}

		#endregion
	}
}
