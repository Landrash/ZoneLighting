﻿namespace ZoneLightingTests
{
	public class InterruptingInputTests
	{
		//TODO: Fix
		//[Test]
		//public void InterruptingInput_InterruptsBackgroundProgramAndStartsReactiveProgram()
		//{
		//	//arrange
		//	var zoneScaffolder = new ZoneScaffolder();
		//	zoneScaffolder.Initialize(ConfigurationManager.AppSettings["TestProgramModuleDirectory"]);
			
		//	var leftWing = new FadeCandyZone("LeftWing");
		//	leftWing.AddOPCLights(PixelType.FadeCandyWS2812Pixel, 6, 1);

		//	var scrollDotDictionary = new InputStartingValues();
		//	scrollDotDictionary.Add("DelayTime", 30);
		//	scrollDotDictionary.Add("DotColor", (Color?)Color.Red);

		//	FadeCandyController.Instance.Initialize();	//needs to be faked somehow

		//	zoneScaffolder.InitializeZone(leftWing, "ScrollDot", scrollDotDictionary);

		//	leftWing.AddInterruptingProgram(new StaticColor());

		//	//act
		//	leftWing.InterruptingPrograms[0].Inputs[0].SetValue(Color.Blue);

		//	//assert
		//	Assert.True(leftWing.ZoneProgram.PauseTrigger.WaitForFire(1000));
		//	Assert.True(((InterruptingInput) leftWing.InterruptingPrograms[0].Inputs[0]).StartTrigger.WaitForFire(1000));
		//	Assert.True(((StaticColor)leftWing.InterruptingPrograms[0]).ChangeLightColorTrigger.WaitForFire(1000));
		//	Assert.True(((InterruptingInput) leftWing.InterruptingPrograms[0].Inputs[0]).StopTrigger.WaitForFire(1000));
		//	Assert.True(leftWing.ZoneProgram.ResumeTrigger.WaitForFire(1000));

		//	//cleaup
		//	leftWing.Dispose(true);
		//}
	}	
}
