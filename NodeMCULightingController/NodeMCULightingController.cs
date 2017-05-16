using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoneLighting.Communication;

namespace NodeMCULightingController
{
    [Export(typeof(LightingController))]
    [ExportMetadata("Name", "NodeMCULightingController")]
    public class NodeMCULightingController
    {

    }
}
