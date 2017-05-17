using System.ComponentModel.Composition;
using System.Configuration;
using ZoneLighting.Communication;

namespace NodeMCULightingController
{
	[Export(typeof(LightingController))]
	[ExportMetadata("Name", "NodeMCUController")]
	public class NodeMCUController : OPCWebSocketController.OPCWebSocketController
    {

        #region Singleton

        private static NodeMCUController _instance;

        public static NodeMCUController Instance
            => _instance ?? (_instance = new NodeMCUController(ConfigurationManager.AppSettings["NodeMCUServerURL"]));

        #endregion

        public NodeMCUController(string serverURL) : base(serverURL)
        {
        }

        public new void Initialize()
        {
            if (!Initialized)
            {
                base.Initialize();
                Initialized = true;
            }
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
