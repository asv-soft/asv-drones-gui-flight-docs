using System.ComponentModel.Composition;
using Asv.Drones.Gui.Core;

namespace Asv.Drones.Gui.Plugin.FlightDocs;

[ExportView(typeof(FlightZoneMapViewModel))]
[PartCreationPolicy(CreationPolicy.NonShared)]
public class FlightZoneMapView : MapPageView
{
}