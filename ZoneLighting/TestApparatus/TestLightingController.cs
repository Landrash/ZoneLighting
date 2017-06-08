using System;
using System.Collections.Generic;
using Graphics;
using LightingControllerBase;
using Newtonsoft.Json;

namespace ZoneLighting.TestApparatus
{
    public class TestLightingController : ILightingController
    {
        public TestLightingController(string name, Action<IList<IPixel>> sendLightsAction)
        {
            Name = name;
            SendLightsAction = sendLightsAction;
        }


        public string Name { get; }

        public Action<IList<IPixel>> SendLightsAction { get; }

        public void SendLights(IList<IPixel> lights)
        {
            SendLightsAction(lights);
        }

        public void Initialize(string lcConfig)
        {
            
        }

        public void Uninitialize()
        {
            
        }
    }
}
