using AutoMapper;
using WebRemote.Models;
using ZoneLighting.ZoneNS;
using ZoneLighting.ZoneProgramNS;
using ZoneLighting.ZoneProgramNS.Input;

namespace WebRemote.Automapper
{
	//public class ZoneJsonModelConverter : ITypeConverter<Zone, ZoneJsonModel>
	//{
	//	public ZoneJsonModel Convert(Zone source, ZoneJsonModel destination, ResolutionContext context)
	//	{
	//		destination = context.Mapper.Map<Zone, ZoneJsonModel>(source);
	//		destination.Inputs = source.ZoneProgram.Inputs.ToISV().Dictionary;

	//		return destination;
	//	}
	//}

	public class ZoneJsonModelInputsResolver : IValueResolver<Zone, ZoneJsonModel, InputInfo>
	{
		public InputInfo Resolve(Zone source, ZoneJsonModel destination, InputInfo destMember, ResolutionContext context)
		{
			return source?.ZoneProgram?.Inputs?.ToInputInfo();
		}
	}
}
