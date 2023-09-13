using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Asv.Common;
using Asv.Drones.Gui.Core;
using Avalonia.Collections;
using Avalonia.Media;
using DynamicData;
using ReactiveUI;

namespace Asv.Drones.Gui.Plugin.FlightDocs;

public class FlightZonePolygon : MapAnchorBase
{
    private readonly ReadOnlyObservableCollection<GeoPoint> _path;
    private readonly SourceList<IMapAnchor> _cache = new ();
    private IFlightZoneMap _flightZoneMap;
    
    public FlightZonePolygon() : base(new Uri(FlightZoneMapViewModel.UriString + "/layer/flight-zone-polygon"))
    {
        ZOrder = -1000;
        OffsetX = 0;
        OffsetY = 0;
        PathOpacity = 0.6;
        StrokeThickness = 5;
        Stroke = new SolidColorBrush(Color.FromRgb(255,255,255));
        Fill = new SolidColorBrush(Color.FromArgb(80,255, 255, 255));
        IsFilled = true;
        IsVisible = true;
        StrokeDashArray = new AvaloniaList<double>(2,2);

        _cache.Connect()
            .Transform(_ => _.Location)
            .Bind(out _path)
            .Subscribe()
            .DisposeItWith(Disposable);
    }

    protected override void InternalWhenMapLoaded(IMap map)
    {
        base.InternalWhenMapLoaded(map);
        _flightZoneMap = (FlightZoneMapViewModel)map;
        
        _flightZoneMap.FlightZoneAnchors.Connect()
            .OnItemAdded(_ => UpdatePath())
            .OnItemRemoved(_ => UpdatePath())
            .WhenPropertyChanged(_ => _.Location)
            .Sample(TimeSpan.FromMilliseconds(30), RxApp.MainThreadScheduler)
            .Subscribe(_ => UpdatePath())
            .DisposeItWith(Disposable);
    }

    private void UpdatePath()
    {
        var items = _flightZoneMap.FlightZoneAnchors.Items.ToArray();
        _cache.Clear();
        _cache.AddRange(items);
    }
    
    public override ReadOnlyObservableCollection<GeoPoint> Path => _path;
}