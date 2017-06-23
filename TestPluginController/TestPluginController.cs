using System.Collections.Generic;
using System.ComponentModel.Composition;
using Graphics;
using LightingControllerBase;

namespace TestPluginController
{
    [Export(typeof(ILightingController))]
    [ExportMetadata("Name", "TestPluginController")]
    public class TestPluginController : ILightingController
    {
        [ImportingConstructor]
        public TestPluginController()
        {
        }
        

        public void SendLights(IList<IPixel> lights)
        {
            
        }

        public void Initialize(string lcConfig)
        {
        }

        public void Uninitialize()
        { 
        }

        public string Name { get; }
    }
}
