using System.ComponentModel.Composition;
using Asv.Drones.Gui.Core;

namespace Asv.Drones.Gui.FlightDocs;

[ExportView(typeof(Gui.Core.MapZoomActionViewModel))]
[PartCreationPolicy(CreationPolicy.NonShared)]
public class MapZoomActionView : Gui.Core.MapZoomActionView
{
}