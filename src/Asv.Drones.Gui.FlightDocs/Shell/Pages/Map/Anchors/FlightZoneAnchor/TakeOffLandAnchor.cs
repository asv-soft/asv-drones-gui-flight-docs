using Asv.Avalonia.Map;
using Asv.Common;
using Asv.Drones.Gui.Core;
using Avalonia.Media;
using Material.Icons;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Asv.Drones.Gui.FlightDocs;

public enum TakeOffLand
{
    TakeOff = 0,
    Land = 1
}

public class TakeOffLandAnchor : MapAnchorBase
{
    public TakeOffLandAnchor(string id, GeoPoint point, TakeOffLand takeOffLand, ILocalizationService loc) : base(new Uri($"{FlightZoneMapViewModel.UriString}layer/ruler/{id}"))
    {
        Size = 48;
        OffsetX = OffsetXEnum.Center;
        OffsetY = OffsetYEnum.Bottom;
        StrokeThickness = 1;
        IconBrush = new SolidColorBrush(Color.FromRgb(89,46,131));
        Stroke = Brushes.White;
        IsVisible = true;
        Icon = MaterialIconKind.MapMarker;
        IsEditable = true;
        Location = point;
        Name = $"{RS.TakeOffLandAnchor_Name} {takeOffLand}";
        TakeOffLand = takeOffLand;
        
        this.WhenAnyValue(_ => _.Location, __ => __.Name)
            .Subscribe(_ => Title = $"({Name}) {loc.Latitude.FromSiToStringWithUnits(Location.Latitude)}; " +
                                    $"{loc.Longitude.FromSiToStringWithUnits(Location.Longitude)}; " +
                                    $"{loc.Altitude.FromSiToStringWithUnits(Location.Altitude)}")
            .DisposeItWith(Disposable);
    }
    
    [Reactive] 
    public string Name { get; set; }
    [Reactive]
    public TakeOffLand TakeOffLand { get; set; }
}