using Asv.Avalonia.Map;
using Asv.Common;
using Asv.Drones.Gui.Core;
using Avalonia.Media;
using Material.Icons;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Asv.Drones.Gui.FlightDocs;

public class FlightZoneAnchor : MapAnchorBase
{
    public FlightZoneAnchor(string id, GeoPoint point, int order, ILocalizationService loc) : base(
        new Uri($"{FlightZoneMapViewModel.UriString}layer/ruler/{id}"))
    {
        Size = 48;
        OffsetX = OffsetXEnum.Center;
        OffsetY = OffsetYEnum.Bottom;
        StrokeThickness = 1;
        IconBrush = Brushes.Fuchsia;
        Stroke = Brushes.White;
        IsVisible = true;
        Icon = MaterialIconKind.MapMarker;
        IsEditable = true;
        Location = point;
        Name = $"{RS.FlightZoneAnchor_Name} {order}";
        Order = order;

        this.WhenAnyValue(_ => _.Location, __ => __.Name)
            .Subscribe(_ => Title = $"({Name}) {loc.Latitude.FromSiToStringWithUnits(Location.Latitude)}; " +
                                    $"{loc.Longitude.FromSiToStringWithUnits(Location.Longitude)}; " +
                                    $"{loc.Altitude.FromSiToStringWithUnits(Location.Altitude)}")
            .DisposeItWith(Disposable);
    }

    [Reactive] 
    public string Name { get; set; }
    [Reactive]
    public int Order { get; set; }
}