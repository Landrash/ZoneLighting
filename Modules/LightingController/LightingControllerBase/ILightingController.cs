using System.Collections.Generic;
using Anshul.Utilities;
using Graphics;

namespace LightingControllerBase
{
	public interface ILightingController : IBetterListType
	{
        void SendLights(IList<IPixel> lights);
	    void Initialize(string lcConfig);
	    void Uninitialize();
	}
}