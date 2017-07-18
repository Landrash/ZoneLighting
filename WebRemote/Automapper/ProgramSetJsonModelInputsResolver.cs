using System;
using System.Linq;
using AutoMapper;
using WebRemote.Models;
using ZoneLighting.ZoneNS;
using ZoneLighting.ZoneProgramNS;

namespace WebRemote.Automapper
{
	public class ProgramSetJsonModelInputsResolver : IValueResolver<ProgramSet, ProgramSetJsonModel, ISV>
	{
		public ISV Resolve(ProgramSet source, ProgramSetJsonModel destination, ISV destMember, ResolutionContext context)
		{
			if (source == null) return new ISV();
			
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

			return inputs.ToISV();
		}
	}
}