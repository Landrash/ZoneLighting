using NUnit.Framework;
using ZoneLighting;

namespace ZoneLightingTests
{
	public class ExtensionsTests
	{
		[TestCase("colorScheme", "color Scheme")]
		[TestCase("Color Scheme", "Color Scheme")]
		public void SplitCamelCase_Works(string input, string expected)
		{
			Assert.That(input.SplitCamelCase(), Is.EqualTo(expected));
		}

		[TestCase("colorScheme", "ColorScheme")]
		[TestCase("Color Scheme", "Color Scheme")]
		[TestCase("color Scheme", "Color Scheme")]
		[TestCase("ColorScheme", "ColorScheme")]
		public void ToPascalCase_Works(string input, string expected)
		{
			Assert.That(input.ToPascalCase(), Is.EqualTo(expected));
		}
	}	
}
