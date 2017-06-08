using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Anshul.Utilities;
using ZoneLighting.Usables;

namespace ZoneLighting
{
	public static class Extensions
	{
		public static BetterList<T> ToBetterList<T>(this IEnumerable<T> list) where T : IBetterListType
		{
			var betterList = new BetterList<T>();
			betterList.AddRange(list);
			return betterList;
		}


		/// <summary>
		/// This runs the given actions for each item in the given list on separate threads simultaneously (TaskCreationOptions.LongRunning).
		/// </summary>
		public static void Parallelize<T>(this List<T> list, Action<T> action)
		{
			var tasks = new List<Task>();
			list.ForEach(t => tasks.Add(Task.Factory.StartNew(() => action(t), TaskCreationOptions.LongRunning)));
			Task.WaitAll(tasks.ToArray());
		}
	}
}
