using System;
using System.ComponentModel.Composition;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ZoneLighting.MEF;

namespace OPCWebSocketController
{
	[Export(typeof(ILightingController))]
	[ExportMetadata("Name", "NodeMCUController")]
	public class NodeMCUController : OPCWebSocketController
    {
        [ImportingConstructor]
        public NodeMCUController()
        {
        }

		public override void Initialize(string lcConfig)
		{
			//dynamic nodeMcuConfig = JsonConvert.DeserializeObject(lcConfig.Replace("\n", string.Empty)
			//	.Replace("\r", string.Empty)
			//	.Replace("\t", string.Empty));

			dynamic nodeMcuConfig =
				JsonConvert.DeserializeObject(
					"{ 'Name': 'Jon Smith' }");

			try
			{
				Console.WriteLine(nodeMcuConfig.Name);
				Console.WriteLine(nodeMcuConfig.ServerURL);

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}


			//if (nodeMcuConfig.PixelMap != null)
	  //      {
			//	Initialize((string) nodeMcuConfig.Name, (string) nodeMcuConfig.ServerURL,
			//        new StaticPixelMapper((PixelMap) nodeMcuConfig.PixelMap), (OPCPixelType) nodeMcuConfig.OPCPixelType,
			//        (byte) nodeMcuConfig.Channel);
	  //      }
	  //      else
	  //      {
			//	//this will not work because pixelmapper cannot be read from a config file
			//	//need to convert it into a factory lookup type thing
			//	Initialize((string) nodeMcuConfig.Name, (string) nodeMcuConfig.ServerURL,
			//        (IPixelToOPCPixelMapper) nodeMcuConfig.PixelMapper, (OPCPixelType) nodeMcuConfig.OPCPixelType,
			//        (byte) nodeMcuConfig.Channel);
	  //      }
	        //base.Initialize("NodeMCUController1", "ws://192.168.29.113:81/", new JsonConfigPixelMapper(), OPCPixelType.OPCRGBPixel, 1);
            Initialized = true;
        }

        public new void Uninitialize()
        {
            if (Initialized)
            {
                base.Uninitialize();
                Initialized = false;
            }
        }
    }
}
