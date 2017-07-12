using System;
using System.Linq;
using System.Text.RegularExpressions;
using WebRemote.IoC;
using WebRemote.Models;
using ZoneLighting;
using ZoneLighting.ZoneNS;
using ZoneLighting.ZoneProgramNS;

namespace WebRemote.Extensions
{
	public static class Extensions
	{
		
		public static TDestination ToJsonModel<TSource, TDestination>(this TSource input)
		{
			var output = Container.AutoMapper.Map<TSource, TDestination>(input);
			return output;
		}

		public static ZLMJsonModel ToZLMJsonModel(this ZLM zlm)
		{
			var output = Container.AutoMapper.Map<ZLM, ZLMJsonModel>(zlm);
			//output.ProgramSets = zlm.ProgramSets.Select(x => x.ToJsonModel<ProgramSet, ProgramSetJsonModel>()).ToBetterList();
			//output.Zones = zlm.Zones.Select(x => x.ToZoneJsonModel()).ToBetterList();

			return output;
		}

		public static ZoneJsonModel ToZoneJsonModel(this Zone zone)
		{
			var output = zone.ToJsonModel<Zone, ZoneJsonModel>();
			output.Inputs = zone.ZoneProgram.Inputs.ToISV();

			return output;
		}

		public static string SplitCamelCase(this string input)
		{
			return Regex.Replace(input, "(?<=[a-z])([A-Z])", " $1", RegexOptions.Compiled);
		}
		public static string ToPascalCase(this string input)
		{
			input = input.SplitCamelCase();

			// If there are 0 or 1 characters, just return the string.
			if (input == null)
				return input;
			if (input.Length < 2)
				return input;

			// Split the string into words.
			string[] words = input.Split(
				new char[] { },
				StringSplitOptions.RemoveEmptyEntries);

			// Combine the words.
			return words.Aggregate(string.Empty, (current, t) => current + (t.Substring(0, 1).ToUpper() + t.Substring(1)));
		}
	}
}