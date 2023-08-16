using System.ComponentModel.Composition;
using Asv.Common;
using Asv.Drones.Gui.Core;
using Avalonia.Controls;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI.Fody.Helpers;

namespace Asv.Drones.Gui.FlightDocs;

[Export(FlightZoneMapViewModel.UriString, typeof(IMapAction))]
[PartCreationPolicy(CreationPolicy.NonShared)]
public class FlightZoneActionViewModel : MapActionBase
{
    private CancellationTokenSource _cancellationTokenSource = new ();
    private readonly ILocalizationService _loc;
    private readonly ILogService _log;
    private bool _awaitingNextPoint;
    private IFlightZoneMap _flightZoneMap;
    private int _order;
    
    [ImportingConstructor]
    public FlightZoneActionViewModel(ILocalizationService loc, ILogService log) : base("asv:shell.page.map.action.flight-zone")
    {
        _loc = loc;
        _log = log;
        Dock = Dock.Left;
        Order = 2;

        this.WhenValueChanged(_ => _.IsEditModeEnabled)
            .Subscribe(SetUpFlightZone)
            .DisposeItWith(Disposable);
    }
    
    protected override void InternalWhenMapLoaded(IMap context)
    {
        base.InternalWhenMapLoaded(context);
        _flightZoneMap = (FlightZoneMapViewModel)context;
        _order = _flightZoneMap.FlightZoneAnchors.Count;
    }

    private async void SetUpFlightZone(bool isVisible)
    {
        if (Map == null) return;

        if (isVisible)
        {
            while (IsEditModeEnabled)
            {
                try
                {
                    _awaitingNextPoint = true;
                    var point = await Map.ShowTargetDialog(RS.FlightZoneActionViewModel_SelectPoint_Title, _cancellationTokenSource.Token);
                    _order = _flightZoneMap.FlightZoneAnchors.Count - 1;
                    _order++;
                    _flightZoneMap.FlightZoneAnchors.Add(new FlightZoneAnchor(Guid.NewGuid().ToString(), point, _order, _loc));
                }
                catch (OperationCanceledException e)
                {
                    _log.Warning(e.Source, e.Message);
                }
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
    public bool IsEditModeEnabled { get; set; }
}