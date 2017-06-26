using System;
using System.ComponentModel.Composition;
using LightingControllerBase;
using Newtonsoft.Json;

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
			var nodeMcuConfig = JsonConvert.DeserializeObject<NodeMCUControllerConfig>(lcConfig);
			var pixelMap = new PixelMap()
			{
				Pixels = nodeMcuConfig.Pixels
			};
			
			if (nodeMcuConfig.PixelMapperType == null || nodeMcuConfig.PixelMapperType == PixelMapperType.Static)
			{
				Initialize(nodeMcuConfig.Name, nodeMcuConfig.ServerURL, nodeMcuConfig.Username,
					nodeMcuConfig.Password, new StaticPixelMapper(pixelMap), nodeMcuConfig.OPCPixelType,
					nodeMcuConfig.Channel);
			}
			else
			{
				//TODO: still needs to be tested
				//instantiating a new instance of any of the resolvers might 
				//require passing in parameters, which is not happening here
				//todo: create an init method into which params are passed in
				//these params can be taken from the config also
				Initialize(nodeMcuConfig.Name, nodeMcuConfig.ServerURL,
					(IPixelToOPCPixelMapper) Activator.CreateInstance(
						new PixelMapperTypeResolver().Resolve((PixelMapperType) nodeMcuConfig.PixelMapperType)),
					nodeMcuConfig.OPCPixelType,
					nodeMcuConfig.Channel);
			}
			Initialized = true;

			//sample: base.Initialize("NodeMCUController1", "ws://192.168.29.113:81/", new JsonConfigPixelMapper(), OPCPixelType.OPCRGBPixel, 1);
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
