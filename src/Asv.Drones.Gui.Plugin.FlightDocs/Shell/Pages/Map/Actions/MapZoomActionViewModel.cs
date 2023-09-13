using System.ComponentModel.Composition;
using Asv.Drones.Gui.Core;

namespace Asv.Drones.Gui.Plugin.FlightDocs;

[Export(FlightZoneMapViewModel.UriString, typeof(IMapAction))]
[PartCreationPolicy(CreationPolicy.NonShared)]
public class MapZoomActionViewModel : Gui.Core.MapZoomActionViewModel
{
}