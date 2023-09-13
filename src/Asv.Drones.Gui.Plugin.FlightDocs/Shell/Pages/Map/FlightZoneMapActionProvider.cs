using System.ComponentModel.Composition;
using Asv.Drones.Gui.Core;
using DynamicData;

namespace Asv.Drones.Gui.Plugin.FlightDocs;

[Export(FlightZoneMapViewModel.UriString, typeof(IViewModelProvider<IMapAction>))]
[PartCreationPolicy(CreationPolicy.NonShared)]
public class FlightZoneMapActionProvider : ViewModelProviderBase<IMapAction>
{
    [ImportingConstructor]
    public FlightZoneMapActionProvider([ImportMany(FlightZoneMapViewModel.UriString)]IEnumerable<IMapAction> items)
    {
        Source.AddOrUpdate(items);
    }
}