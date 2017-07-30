using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using MIDIator.UIGenerator.Consumables;

namespace ZoneLighting.ZoneProgramNS
{
	[DataContract]
	public class ZoneProgramInputCollection : KeyedCollection<string, ZoneProgramInput>
	{

		public ZoneProgramInputCollection() : base(StringComparer.OrdinalIgnoreCase) { }

		protected override string GetKeyForItem(ZoneProgramInput item)
		{
			return item.Name;
		}

		protected override void InsertItem(int index, ZoneProgramInput item)
		{
			if (item == null)
				throw new Exception("Cannot insert null values into this collection.");
			//try
			//{
			////override if it already exists
			//if (base.Contains(item.Name))
			//{
			//	//index = base.First(x => x.Name == i
			//	base.Remove(item.Name);
			//}

			base.InsertItem(index, item);
			//}
			//catch (Exception ex)
			//{
			//	// ignored
			//}
		}

		public InputBag ToInputBag()
		{
			var inputStartingValues = new InputBag();
			this.ToList().ForEach(input =>
			{
				if (input.Value != null)
					inputStartingValues.Add(input.Name, input.Value);

			});
			return inputStartingValues;
		}

		public InputInfo ToInputInfo()
		{
			var inputStartingValues = new InputInfo();
			this.ToList().ForEach(input =>
			{
				if (input is RangedZoneProgramInput<int>)
				{
					inputStartingValues.Add(input.Name.ToPascalCase(),
						new
						{
							Value = input.Value,
							Min = ((RangedZoneProgramInput<int>)input).Min,
							Max = ((RangedZoneProgramInput<int>)input).Max,
							Type = input.Type.FullName
						});
				}
				else if (input is RangedZoneProgramInput<double>)
				{
					inputStartingValues.Add(input.Name.ToPascalCase(),
						new
						{
							Value = input.Value,
							Min = ((RangedZoneProgramInput<double>)input).Min,
							Max = ((RangedZoneProgramInput<double>)input).Max,
							Type = input.Type.FullName
						});
				}
				else if (input.Type.IsSubclassOfRawGeneric(typeof(List<>)))
				{
					inputStartingValues.Add(input.Name.ToPascalCase(),
						new
						{
							Value = input.Value,
							Options = input.Type.GetProperties(BindingFlags.Static | BindingFlags.Public).Select(x => x.Name),
							Type = typeof(Enum).FullName
						});
				}
				else
				{
					inputStartingValues.Add(input.Name.ToPascalCase(),
						new
						{
							Value = input.Value,
							Type = input.Type.FullName
						});
				}

			});
			return inputStartingValues;
		}
	}
}
