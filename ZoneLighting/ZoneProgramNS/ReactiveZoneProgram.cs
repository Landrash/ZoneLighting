﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using ZoneLighting.TriggerDependencyNS;
using ZoneLighting.ZoneNS;

namespace ZoneLighting.ZoneProgramNS
{
	public abstract class ReactiveZoneProgram : ZoneProgram
	{
		protected abstract override void StartCore();

		protected override void StopCore(bool force)
		{
			
		}

		public override void Resume()
		{
			//TODO: Implement resume logic
		}

		protected override void Pause()
		{
			//TODO: Implement pause logic
		}

		private bool IsSyncStateRequested { get; set; }
		public Trigger IsSynchronizable { get; set; } = new Trigger("LoopingZoneProgram.IsSynchronizable");
		public Trigger WaitForSync { get; set; } = new Trigger("LoopingZoneProgram.WaitForSync");

		/// <summary>
		/// Adds a live input to the zone program. A live input is an input that can be controlled while
		/// the program is running and the program will respond to it in the way it's designed to.
		/// </summary>
		/// <param name="name">Name of the input.</param>
		/// <param name="action">The action that should occur when the input is set to a certain value. This will be defined by the 
		/// subclasses of this class to perform certain actions when the this input is set to a value.</param>
		/// <returns>The input that was just added.</returns>
		protected ZoneProgramInput AddInterruptingInput<T>(string name, Action<object> action, SyncContext syncContext = null)
		{
			var input = new InterruptingInput(name, typeof(T));
			Inputs.Add(input);

			//if sync is requested, go into synchronizable state
			if (syncContext != null)
			{
				IsSynchronizable.Fire(this, null);
				WaitForSync.WaitForFire();
				IsSyncStateRequested = false;
			}

			//input.AttachBarrier(syncContext?.Barrier);
			input.Subscribe(data =>				//when the input's OnNext is called, do whatever it was programmed to do and then fire the StopSubject
			{
				input.StartTrigger.Fire(this, null);
				action(data);
				//input.DetachBarrier();
				input.StopSubject.OnNext(null);
				input.StopTrigger.Fire(this, null);
			});
			return input;
		}

		public void SetInterruptQueue(ActionBlock<InterruptInfo> interruptQueue)
		{
			Inputs.Where(input => input is InterruptingInput)
				.ToList()
				.ForEach(input => ((InterruptingInput)input).SetInterruptQueue(interruptQueue));
		}

		public void RemoveInterruptQueue()
		{
			Inputs.Where(input => input is InterruptingInput)
				.ToList()
				.ForEach(input => ((InterruptingInput)input).RemoveInterruptQueue());
		}

		//protected Task RunProgram { get; set; }
		//protected Thread RunProgramThread { get; set; }
	}
}
