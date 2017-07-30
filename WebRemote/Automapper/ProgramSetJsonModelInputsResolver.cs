using System;
using System.Linq;
using AutoMapper;
using WebRemote.Models;
using ZoneLighting.ZoneNS;
using ZoneLighting.ZoneProgramNS;

namespace WebRemote.Automapper
{
	public class ProgramSetJsonModelInputsResolver : IValueResolver<ProgramSet, ProgramSetJsonModel, InputInfo>
	{
		public InputInfo Resolve(ProgramSet source, ProgramSetJsonModel destination, InputInfo destMember, ResolutionContext context)
		{
			if (source == null) return new InputInfo();
			
			var inputs = source.Zones.First().ZoneProgram.Inputs;
				
			foreach (var input in inputs)
			{
				if (source.ZonePrograms.Any(x =>
				{
					var value = x.Inputs[input.Name].Value;
					return value != null && ((input.Value == null) ||
					                         !value.Equals(input.Value));
				}))
				{
					input.SetValue("Indeterminate");
				}
			}

			return inputs.ToInputInfo();
		}
	}
}