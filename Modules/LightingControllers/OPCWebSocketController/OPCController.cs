using System;
using ZoneLighting.Communication;

namespace OPCWebSocketController
{
    public abstract class OPCController : LightingController
    {
        public override Type PixelType => typeof(IOPCPixelContainer);
    }
}
