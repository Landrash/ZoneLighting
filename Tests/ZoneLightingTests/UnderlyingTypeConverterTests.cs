﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Newtonsoft.Json;
using NUnit.Framework;
using ZoneLighting.ZoneProgramNS;
using ZoneLighting.ZoneProgramNS.Input;

//using Xunit;

namespace ZoneLightingTests
{
	public class UnderlyingTypeConverterTests
	{
		private static void AddConsoleListener()
		{
			Debug.Listeners.Add(new DefaultTraceListener());
		}

		//[Fact]
		[Test]
		public void ValueDeserializesWithUnderlyingType()
		{
			AddConsoleListener();
			var jsonSerializerSettings = new JsonSerializerSettings()
			{
				TypeNameHandling = TypeNameHandling.All,
				Formatting = Formatting.Indented,
			};

			var jsonDeserializerSettings = new JsonSerializerSettings()
			{
				TypeNameHandling = TypeNameHandling.All,
				Formatting = Formatting.Indented,
				Converters = new List<JsonConverter>() { new UnderlyingTypeConverter() }
			};

			var dotColor = new ZoneProgramInput("DotColor", typeof(Color));
			dotColor.SetValue(Color.Red);

			var serializedDotColor = JsonConvert.SerializeObject(dotColor, jsonSerializerSettings);
			var deserializedDotColor = JsonConvert.DeserializeObject<ZoneProgramInput>(serializedDotColor, jsonDeserializerSettings);
			Console.WriteLine(deserializedDotColor.Value.GetType());
			Assert.True(deserializedDotColor.Value is Color);
		}


		//[Fact]
		[Test]
		public void ValueDeserializesWithUnderlyingType2()
		{
			AddConsoleListener();
			var jsonSerializerSettings = new JsonSerializerSettings()
			{
				TypeNameHandling = TypeNameHandling.All,
				Formatting = Formatting.Indented,
			};

			var jsonDeserializerSettings = new JsonSerializerSettings()
			{
				TypeNameHandling = TypeNameHandling.All,
				Formatting = Formatting.Indented,
				Converters = new List<JsonConverter>() { new UnderlyingTypeConverter() }
			};

			var dotColor = new ZoneProgramInput("DelayTime", typeof(int));
			dotColor.SetValue(1);

			var serializedDotColor = JsonConvert.SerializeObject(dotColor, jsonSerializerSettings);
			var deserializedDotColor = JsonConvert.DeserializeObject<ZoneProgramInput>(serializedDotColor, jsonDeserializerSettings);
			Console.WriteLine(deserializedDotColor.Value.GetType());
			Assert.True(deserializedDotColor.Value is int);
		}

		//[Fact]
		[Test]
		public void ValueDeserializesWithUnderlyingType3()
		{
			AddConsoleListener();
			var jsonSerializerSettings = new JsonSerializerSettings()
			{
				TypeNameHandling = TypeNameHandling.All,
				Formatting = Formatting.Indented,
			};

			var jsonDeserializerSettings = new JsonSerializerSettings()
			{
				TypeNameHandling = TypeNameHandling.All,
				Formatting = Formatting.Indented,
				Converters = new List<JsonConverter>() { new UnderlyingTypeConverter() }
			};

			var dotColor = new ZoneProgramInput("DelayTime", typeof(ComplexClass));
			dotColor.SetValue(new ComplexClass()
			{
				Name = "ComplexClass",
				SimpleClass = new SimpleClass()
				{
					Name = "SimpleClass",
					AppDomainSetups = new List<AppDomainSetup>()
					{
						new AppDomainSetup()
						{
							ApplicationName = "TestAppDomainSetup"
						}
					}
				}
			});

			var serializedDotColor = JsonConvert.SerializeObject(dotColor, jsonSerializerSettings);
			var deserializedDotColor = JsonConvert.DeserializeObject<ZoneProgramInput>(serializedDotColor, jsonDeserializerSettings);
			Console.WriteLine(deserializedDotColor.Value.GetType());
			Assert.True(deserializedDotColor.Value is ComplexClass);
		}

		/// <summary>
		/// These two classes are just for testing.
		/// </summary>
		public class ComplexClass
		{
			public string Name { get; set; }
			public SimpleClass SimpleClass { get; set; }
		}

		public class SimpleClass
		{
			public string Name { get; set; }
			public List<AppDomainSetup> AppDomainSetups { get; set; }
		}
	}
}
