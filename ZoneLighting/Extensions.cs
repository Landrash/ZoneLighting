using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Anshul.Utilities;
using ZoneLighting.Usables;

namespace ZoneLighting
{
	public static class Extensions
	{
		/// <summary>
		/// This runs the given actions for each item in the given list on separate threads simultaneously (TaskCreationOptions.LongRunning).
		/// </summary>
		public static void Parallelize<T>(this List<T> list, Action<T> action)
		{
			var tasks = new List<Task>();
			list.ForEach(t => tasks.Add(Task.Factory.StartNew(() => action(t), TaskCreationOptions.LongRunning)));
			Task.WaitAll(tasks.ToArray());
		}

		public static string SplitCamelCase(this string input)
		{
			return Regex.Replace(input, "(?<=[a-z])([A-Z])", " $1", RegexOptions.Compiled);
		}

		public static string ToPascalCase(this string input)
		{
			input = input.Replace(" ", "| ");
			input = input.SplitCamelCase();

			// If there are 0 or 1 characters, just return the string.
			if (input == null)
				return input;
			if (input.Length < 2)
				return input;

			var words = input.Split();
			
			// Combine the words.
			var returnValue = words.Aggregate(string.Empty, (current, t) => current + (t.Substring(0, 1).ToUpper() + t.Substring(1))).Replace("|", " ");

			return returnValue;
		}

		public static bool IsSubclassOfRawGeneric(this Type toCheck, Type generic)
		{
			while (toCheck != null && toCheck != typeof(object))
			{
				var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
				if (generic == cur)
				{
					return true;
				}
				toCheck = toCheck.BaseType;
			}
			return false;
		}
	}
}
