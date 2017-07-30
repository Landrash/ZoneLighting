using System.Drawing;
using NUnit.Framework;
using ZoneLighting.ZoneProgramNS;

namespace ZoneLightingTests
{
	public class InputStartingValuesTests
	{
		[Test]
		public void InputStartingValues_SetParameter_CreatesNewDictionaryEntry()
		{
			dynamic inputBag = new InputBag();
			inputBag.Speed = 1;
			inputBag.Color = Color.Red;
			inputBag.Name = "Test";

			Assert.True(inputBag["Speed"] == 1);
			Assert.True(inputBag["Color"] == Color.Red);
			Assert.True(inputBag["Name"] == "Test");
		}

		[Test]
		public void InputStartingValues_AddParameter_IsAccessibleUsingDotNotation()
		{
			dynamic inputBag = new InputBag();
			inputBag.Add("Speed", 1);
			inputBag.Add("Color", Color.Red);
			inputBag.Add("Name", "Test");

			Assert.True(inputBag.Speed == 1);
			Assert.True(inputBag.Color == Color.Red);
			Assert.True(inputBag.Name == "Test");
		}
	}
}
