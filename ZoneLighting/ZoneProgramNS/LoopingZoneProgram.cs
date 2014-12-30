﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ZoneLighting.ZoneProgramNS
{
	public abstract class LoopingZoneProgram : ZoneProgram
	{
		protected LoopingZoneProgram()
		{
			LoopCTS = new CancellationTokenSource();
			Running = false;
		}

		#region Looping Stuff

		public bool Running { get; private set; }

		public CancellationTokenSource LoopCTS;
		protected Task LoopingTask { get; set; }
		protected Thread RunProgramThread { get; set; }

		protected void StartLoop()
		{
			SetupRunProgramTask();
			if (!Running)
			{
				DebugTools.AddEvent("LoopingZoneProgram.StartLoop", "Running = FALSE");
				DebugTools.AddEvent("LoopingZoneProgram.LoopingTask.Method", "Setting Running = TRUE");
				Running = true;

				DebugTools.AddEvent("LoopingZoneProgram.StartLoop", "START StartLoop()");
				LoopingTask.Start();
				DebugTools.AddEvent("LoopingZoneProgram.StartLoop", "END StartLoop()");
			}
			else
			{
				DebugTools.AddEvent("LoopingZoneProgram.StartLoop", "Running = TRUE");
			}
		}

		private void SetupRunProgramTask()
		{
			LoopingTask = new Task(() =>
			{
				try
				{
					RunProgramThread = Thread.CurrentThread;
					while (true)
					{
						Loop();
						if (LoopCTS.IsCancellationRequested)
						{
							Running = false;
							break;
						}
					}
					StopTrigger.Fire(this, null);
				}
				catch (ThreadAbortException ex)
				{
					DebugTools.AddEvent("LoopingZoneProgram.LoopingTask.Method", "LoopingTask thread aborted");
					DebugTools.AddEvent("LoopingZoneProgram.Stop", "START Setting Running = false");
					Running = false;
					DebugTools.AddEvent("LoopingZoneProgram.Stop", "END Setting Running = false");
				}
				catch
				{
					DebugTools.AddEvent("LoopingZoneProgram.LoopingTask.Method", "Unexpected exception in LoopingTask");
				}
			}, LoopCTS.Token);
		}

		#region Overrideables

		public abstract void Setup();
		public abstract void Loop();


		#endregion

		#endregion

		#region Overridden
		
		//public override void StartBase(InputStartingValues inputStartingValues = null)
		//{
		//	Start();
		//}
		
		protected override void StartCore()
		{
			Setup();
			StartLoop();
		}

		public override void Stop(bool force)
		{
			DebugTools.AddEvent("LoopingZoneProgram.Stop", "START Stopping BG Program");

			if (Running)
			{
				DebugTools.AddEvent("LoopingZoneProgram.Stop", "Running = TRUE");

				if (force)
				{
					DebugTools.AddEvent("LoopingZoneProgram.Stop", "START Force aborting BG Program thread");
					RunProgramThread.Abort();
					DebugTools.AddEvent("LoopingZoneProgram.Stop", "END Force aborting BG Program thread");
				}
				else
				{
					LoopCTS.Cancel();
					StopTrigger.WaitForFire();
				}

				DebugTools.AddEvent("LoopingZoneProgram.Stop", "START Clearing Inputs");

				//clear inputs because they will be re-added by the setup
				foreach (var zoneProgramInput in Inputs)
				{
					zoneProgramInput.Dispose();
				}
				Inputs.Clear();

				DebugTools.AddEvent("LoopingZoneProgram.Stop", "END Clearing Inputs");
			}
			else
			{
				DebugTools.AddEvent("LoopingZoneProgram.Stop", "Running = FALSE");
			}

			DebugTools.AddEvent("LoopingZoneProgram.Stop", "END Stopping BG Program");

			StopTestingTrigger.Fire(this, null);
		}

		public override void Resume()
		{
			//TODO: Implement resume logic - for now, it's just gonna call start
			Start();

		}

		protected override void Pause()
		{
			//TODO: Implement pause logic - for now, it's just gonna call stop forcibly
			Stop(true);
		}

		#endregion
	}
}
