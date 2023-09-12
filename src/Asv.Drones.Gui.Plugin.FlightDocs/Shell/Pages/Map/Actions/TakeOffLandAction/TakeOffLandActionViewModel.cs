using System.ComponentModel.Composition;
using Asv.Common;
using Asv.Drones.Gui.Core;
using Avalonia.Controls;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI.Fody.Helpers;

namespace Asv.Drones.Gui.Plugin.FlightDocs;

[Export(FlightZoneMapViewModel.UriString, typeof(IMapAction))]
[PartCreationPolicy(CreationPolicy.NonShared)]
public class TakeOffLandActionViewModel : MapActionBase
{
    private CancellationTokenSource _cancellationTokenSource = new ();
    private readonly ILocalizationService _loc;
    private readonly ILogService _log;
    private bool _awaitingNextPoint;
    private IFlightZoneMap _flightZoneMap;
    private RxValue<GeoPoint?> _takeOff = new ();
    private RxValue<GeoPoint?> _land = new ();
    
    [ImportingConstructor]
    public TakeOffLandActionViewModel(ILocalizationService loc, ILogService log) : base("asv:shell.page.map.action.take-off-land")
    {
        _loc = loc;
        _log = log;
        Dock = Dock.Left;
        Order = 3;
        
        this.WhenValueChanged(_ => _.IsTakeOffLandModeEnabled)
            .Subscribe(SetUpTakeOffLand)
            .DisposeItWith(Disposable);
    }
    
    protected override void InternalWhenMapLoaded(IMap context)
    {
        base.InternalWhenMapLoaded(context);
        _flightZoneMap = (FlightZoneMapViewModel)context;
    }

    private async void SetUpTakeOffLand(bool isVisible)
    {
        if (Map == null) return;

        if (isVisible)
        {
            try
            {
                _awaitingNextPoint = true;
                
                if (!_takeOff.Value.HasValue)
                {
                    _takeOff.OnNext(await Map.ShowTargetDialog(RS.TakeOffLandActionViewModel_SelectTakeOffPoint_Title, _cancellationTokenSource.Token));
                    _flightZoneMap.TakeOffLandAnchors.Add(new TakeOffLandAnchor(Guid.NewGuid().ToString(), _takeOff.Value.Value, TakeOffLand.TakeOff, _loc));
                }
                
                if (!_land.Value.HasValue)
                {
                    _land.OnNext(await Map.ShowTargetDialog(RS.TakeOffLandActionViewModel_SelectLandPoint_Title, _cancellationTokenSource.Token));
                    _flightZoneMap.TakeOffLandAnchors.Add(new TakeOffLandAnchor(Guid.NewGuid().ToString(), _land.Value.Value, TakeOffLand.Land, _loc));
                }

                IsTakeOffLandModeEnabled = false;
            }
            catch (OperationCanceledException e)
            {
                _log.Warning(e.Source, e.Message);
            }
        }
        else if (!isVisible && _awaitingNextPoint)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            Map.IsInDialogMode = false;
            _awaitingNextPoint = false;
            _cancellationTokenSource = new CancellationTokenSource();
        }
    }
    
    [Reactive]
    public bool IsTakeOffLandModeEnabled { get; set; }
}