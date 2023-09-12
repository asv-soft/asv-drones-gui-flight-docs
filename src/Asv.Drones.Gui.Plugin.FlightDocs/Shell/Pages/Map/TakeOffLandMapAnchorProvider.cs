using System.ComponentModel.Composition;
using Asv.Common;
using Asv.Drones.Gui.Core;
using DynamicData;

namespace Asv.Drones.Gui.Plugin.FlightDocs;

[Export(FlightZoneMapViewModel.UriString, typeof(IViewModelProvider<IMapAnchor>))]
[PartCreationPolicy(CreationPolicy.NonShared)]
public class TakeOffLandMapAnchorProvider : ViewModelProviderBase<IMapAnchor>
{
    [ImportingConstructor]
    public TakeOffLandMapAnchorProvider()
    {
    }

    public void Update(SourceList<IMapAnchor> anchors)
    {
        anchors.Connect()
            .OnItemAdded(_ => Source.AddOrUpdate(_))
            .OnItemRemoved(_ => Source.Remove(_))
            .Subscribe()
            .DisposeItWith(Disposable);
    }
}