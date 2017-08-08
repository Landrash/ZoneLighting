using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
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
					dynamic value = new ExpandoObject();

					value.Value = input.Value;
					value.Min = ((RangedZoneProgramInput<int>)input).Min;
					value.Max = ((RangedZoneProgramInput<int>)input).Max;
					value.Type = input.Type.FullName;

					inputStartingValues.Add(input.Name.ToPascalCase(),
						value);
				}
				else if (input is RangedZoneProgramInput<double>)
				{
					dynamic value = new ExpandoObject();
					
					value.Value = input.Value;
					value.Min = ((RangedZoneProgramInput<double>)input).Min;
					value.Max = ((RangedZoneProgramInput<double>)input).Max;
					value.Type = input.Type.FullName;

					inputStartingValues.Add(input.Name.ToPascalCase(),
						value);
				}
				else if (input is RangedZoneProgramInput<decimal>)
				{
					dynamic value = new ExpandoObject();

					value.Value = input.Value;
					value.Min = ((RangedZoneProgramInput<decimal>)input).Min;
					value.Max = ((RangedZoneProgramInput<decimal>)input).Max;
					value.Type = input.Type.FullName;

					inputStartingValues.Add(input.Name.ToPascalCase(),
						value);
				}
				else if (input.Type.IsSubclassOfRawGeneric(typeof(List<>)))
				{
					dynamic value = new ExpandoObject();

					value.Value = input.Value;
					value.Options = input.Type.GetProperties(BindingFlags.Static | BindingFlags.Public).Select(x => x.Name);
					value.Type = typeof(Enum).FullName;
					
					inputStartingValues.Add(input.Name.ToPascalCase(),
						value);
				}
				else
				{
					dynamic value = new ExpandoObject();

					value.Value = input.Value;
					value.Type = input.Type.FullName;
					
					inputStartingValues.Add(input.Name.ToPascalCase(),
						value);
				}

			});
			return inputStartingValues;
		}
	}
}
