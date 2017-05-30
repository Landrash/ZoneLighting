using System.Collections.Generic;
using Anshul.Utilities;

namespace ZoneLighting.MEF
{
	public interface ILightingController : IBetterListType
	{
        void SendLights(IList<IPixel> lights);
	    void Initialize(string lcConfig);
	    void Uninitialize();
	}
}