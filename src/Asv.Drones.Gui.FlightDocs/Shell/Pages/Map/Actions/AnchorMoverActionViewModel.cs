using System.ComponentModel.Composition;
using Asv.Drones.Gui.Core;

namespace Asv.Drones.Gui.FlightDocs;

[Export(FlightZoneMapViewModel.UriString, typeof(IMapAction))]
[PartCreationPolicy(CreationPolicy.NonShared)]
public class AnchorMoverActionViewModel : Gui.Core.AnchorMoverActionViewModel
{
    public AnchorMoverActionViewModel()
    {
    }
}