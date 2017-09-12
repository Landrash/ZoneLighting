﻿using System;
using System.Reactive.Subjects;
using System.Threading.Tasks.Dataflow;
using ZoneLighting.TriggerDependencyNS;

namespace ZoneLighting.ZoneProgramNS.Input
{
	public class InterruptingInput : ZoneProgramInput
	{
		public InterruptingInput(string name, Type type, ZoneProgram zoneProgram) : base(name, type)
		{
			ZoneProgram = zoneProgram;
		}

		public Subject<object> StopSubject { get; set; } = new Subject<object>();

		public Trigger StopTrigger { get; } = new Trigger("InterruptingInputStopTrigger");

		public Trigger StartTrigger { get; } = new Trigger("InterruptingInputStartTrigger");

		private ActionBlock<InterruptInfo> InterruptQueue { get; set; }

		private ZoneProgram ZoneProgram { get; }

		/// <summary>
		/// Sets the interrupt queue to be post interrupts to when the input is set.
		/// </summary>
		public void SetInterruptQueue(ActionBlock<InterruptInfo> interruptQueue)
		{
			InterruptQueue = interruptQueue;
		}

		public void UnsetInterruptQueue()
		{
			InterruptQueue = null;
		}

		public override void Unsubscribe()
		{
			InterruptQueue = null;
			InputDisposable?.Dispose();
		}

		/// <summary>
		/// Sends data through the input to the program it's attached to.
		/// </summary>
		/// <param name="data">The data will be deconstructed/cast to the underlying type in the program.</param>
		public override void SetValue(object data)
		{
			if (InterruptQueue == null)
				throw new Exception("Interrupt Queue has not been set or has been set to null.");

			//DebugTools.AddEvent("InterruptingInput.SetValue", "START Posting to InterruptQueue");
			
			InterruptQueue.Post(new InterruptInfo()
			{
				Data = data,
				InputSubject = InputSubject,
				StopSubject = StopSubject,
				ZoneProgram = ZoneProgram,
				ZoneProgramToInterrupt = ZoneProgram.Zone.ZoneProgram //this is confusing, but it basically gets the background program by going into 
														//the zone and getting its ZoneProgram property, which is the background program that is to be interrupted.
			});

			//DebugTools.AddEvent("InterruptingInput.SetValue", "END Posting to InterruptQueue");

			Value = data;
		}
	}
}
