﻿using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Refigure;
using ZoneLighting.TriggerDependencyNS;
using ZoneLighting.ZoneNS;

namespace ZoneLighting.ZoneProgramNS
{
	public abstract class LoopingZoneProgram : ZoneProgram
	{
		protected LoopingZoneProgram(string name = null, SyncContext syncContext = null) : base(name, syncContext)
		{
			// ReSharper disable once DoNotCallOverridableMethodsInConstructor
			Setup();
			LoopCTS = new CancellationTokenSource();
			Running = false;
		}

		public override void Dispose(bool force)
		{
			base.Dispose(force);
			Unsetup();
			LoopCTS?.Dispose();
			LoopCTS = null;
			IsSynchronizable?.Dispose();
			IsSynchronizable = null;
			WaitForSync?.Dispose();
			WaitForSync = null;
		}

		#region Looping Stuff

		protected bool IsSyncStateRequested { get; set; }
		public Trigger IsSynchronizable { get; set; } = new Trigger("LoopingZoneProgram.IsSynchronizable");
		public Trigger WaitForSync { get; set; } = new Trigger("LoopingZoneProgram.WaitForSync");
		private object SyncStateRequestLock { get; } = new object();
		protected virtual int MaxSyncTimeout { get; }//TODO: wtf is this shit? to be used in syncing somehow? wtf man
		private bool Running { get; set; }
		protected CancellationTokenSource LoopCTS;
		private Task LoopingTask { get; set; }
		private Thread RunProgramThread { get; set; }
		protected virtual int LoopWaitTime { get; set; } = 1;

		protected void StartLoop()
		{
			if (!Running)
			{
				SetupRunProgramTask();

				//DebugTools.AddEvent("LoopingZoneProgram.StartLoop", "Running = FALSE");
				//DebugTools.AddEvent("LoopingZoneProgram.LoopingTask.Method", "Setting Running = TRUE");
				Running = true;

				//DebugTools.AddEvent("LoopingZoneProgram.StartLoop", "START StartLoop()");
				LoopingTask.Start();
				//DebugTools.AddEvent("LoopingZoneProgram.StartLoop", "END StartLoop()");
			}
			else
			{
				//DebugTools.AddEvent("LoopingZoneProgram.StartLoop", "Running = TRUE");
			}
		}

		private void SetupRunProgramTask()
		{
			LoopCTS.Dispose();
			LoopCTS = new CancellationTokenSource();
			try
			{
				LoopingTask?.Dispose();
			}
			catch (Exception ex)
			{ }
			LoopingTask = new Task(() =>
			{
				try
				{
					RunProgramThread = Thread.CurrentThread;
					while (true)
					{
						//if sync is requested, go into synchronizable state
						if (SyncContext != null)
						{
							lock (SyncStateRequestLock)
							{
								if (IsSyncStateRequested)
								{
									DebugTools.AddEvent("LoopingZoneProgram.LoopingTask", "Entering Sync-State: " + Name);
									IsSynchronizable.Fire(this, null);
									DebugTools.AddEvent("LoopingZoneProgram.LoopingTask",
										"In Sync-State - Waiting for Signal from SyncContext: " + Name);
									WaitForSync.WaitForFire();
									DebugTools.AddEvent("LoopingZoneProgram.LoopingTask", "Leaving Sync-State: " + Name);
									IsSyncStateRequested = false;
									DebugTools.AddEvent("LoopingZoneProgram.LoopingTask", "IsSyncStateRequested = false: " + Name);
								}
							}
						}

						//this is currently not doing anything
						LeftSyncTrigger.Fire(this, null);

						DebugTools.AddEvent("LoopingZoneProgram.LoopingTask", "Starting Loop: " + Name);
						//start loop
						Loop();
						DebugTools.AddEvent("LoopingZoneProgram.LoopingTask", "Finished Loop: " + Name);

						//if cancellation is requested, break out of loop after setting notification parameters for the consumer
						if (LoopCTS.IsCancellationRequested)
						{
							Running = false;
							StopTrigger.Fire(this, null);
							break;
						}

						//this makes the CPU consumption way lesser
						Thread.Sleep(LoopWaitTime);
					}
				}
				catch (ThreadAbortException ex)
				{
					//DebugTools.AddEvent("LoopingZoneProgram.LoopingTask.Method", "LoopingTask thread aborted");
					//DebugTools.AddEvent("LoopingZoneProgram.Stop", "START Setting Running = false");
					Running = false;
					StopTrigger.Fire(this, null);
					//DebugTools.AddEvent("LoopingZoneProgram.Stop", "END Setting Running = false");
				}
				catch (Exception ex)
				{
					Running = false;
					StopTrigger.Fire(this, null);
					DebugTools.AddEvent("LoopingZoneProgram.LoopingTask.Method",
						"Unexpected exception in LoopingTask: " + ex.Message + " | StackTrace: " + ex.StackTrace);
				}
			}, LoopCTS.Token);
		}

		public Trigger LeftSyncTrigger { get; set; } = new Trigger("LeftSyncTrigger");

		public virtual SyncLevel SyncLevel { get; set; }

		public override void SetSyncContext(SyncContext syncContext)
		{
			//if same sync context is being passed, ignore request
			if (syncContext == SyncContext)
				return;

			//remove from old sync context, if any
			SyncContext?.Unsync(this);

			if (State == ProgramState.Stopped || IsSyncStateRequested == true)
				SyncContext = syncContext;
			else
				throw new Exception("Can only set sync context while program is stopped or if it's in synchronizable state.");
		}

		#region Overrideables

		public abstract void Setup();
		public abstract void Loop();

		/// <summary>
		/// Subclass can have Unsetup, but doesn't need to.
		/// </summary>
		public virtual void Unsetup()
		{

		}

		#endregion

		#endregion


		#region Transport Controls

		/// <summary>
		/// Requests the program to pause when it's at its synchronizable state.
		/// </summary>
		/// <returns></returns>
		public void RequestSyncState()
		{
			lock (SyncStateRequestLock)
			{
				IsSyncStateRequested = true;
				DebugTools.AddEvent("LoopingZoneProgram.RequestSyncState", "IsSyncStateRequested = true: " + Name);
			}
		}

		/// <summary>
		/// Cancels Sync State request and releases from the sync state, if in that state.
		/// </summary>
		public void CancelSyncState()
		{
			//if this program is in its sync state, release it
			lock (SyncStateRequestLock)
			{
				if (IsSyncStateRequested)
				{
					WaitForSync.Fire(null, null);
				}
				IsSyncStateRequested = false;
			}
		}

		protected override void StartCore(dynamic parameters = null)
		{ 
			//handle sync state request
			//StartTrigger.Fire(this, null);
			//if (isSyncRequested)
			//	RequestSyncState();
			PreLoopStart();

			StartLoop();
		}

		protected virtual void PreLoopStart() { }

		protected virtual void PreStop(bool force) { }

		protected virtual void PostStop(bool force) { }

		protected override void StopCore(bool force)
		{
			//subclass processing
			PreStop(force);

			DebugTools.AddEvent("LoopingZoneProgram.StopCore", "STOP " + Name);
			//DebugTools.AddEvent("LoopingZoneProgram.StopCore", "Canceling Sync-State on Program " + Name);

			//cancel sync state req and release from sync state
			CancelSyncState();

			//DebugTools.AddEvent("LoopingZoneProgram.Stop", "START Stopping BG Program");

			if (Running)
			{
				//DebugTools.AddEvent("LoopingZoneProgram.Stop", "Running = TRUE");

				if (force)
				{
					if (RunProgramThread != null)
					{
						//DebugTools.AddEvent("LoopingZoneProgram.Stop", "START Force aborting BG Program thread");
						RunProgramThread.Abort();
						StopTrigger.WaitForFire();
						//DebugTools.AddEvent("LoopingZoneProgram.Stop", "END Force aborting BG Program thread");
					}
					else
					{
						//DebugTools.AddEvent("LoopingZoneProgram.Stop", "RunProgramThread was null");
						//DebugTools.Print();
					}
				}
				else
				{
					LoopCTS.Cancel();
					if (!StopTrigger.WaitForFire())
					{
						//DebugTools.AddEvent("LoopingZoneProgram.Stop", "Loop did not cancel cooperatively.");
						//DebugTools.Print();
					}
				}
			}

			PostStop(force);

			//DebugTools.AddEvent("LoopingZoneProgram.Stop", "END Stopping BG Program");

			StopTestingTrigger.Fire(this, null);
		}

		#endregion
	}
}
