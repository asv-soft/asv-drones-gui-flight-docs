using System.ComponentModel.Composition;
using Asv.Drones.Gui.Core;
using Avalonia.Input;
using Material.Icons;
using ReactiveUI;

namespace Asv.Drones.Gui.FlightDocs;

[Export(HeaderMenuItem.UriString + "/tools", typeof(IHeaderMenuItem))]
[PartCreationPolicy(CreationPolicy.NonShared)]
public class HeaderFlightDocsMenuItem : HeaderMenuItem
{
    public const string UriString = HeaderMenuItem.UriString + "tools/flight-docs";
    public static readonly Uri Uri = new(UriString);

    private readonly INavigationService _navigation;

    [ImportingConstructor]
    public HeaderFlightDocsMenuItem(INavigationService navigation) : base(Uri)
    {
        _navigation = navigation;
        
        Header = RS.HeaderFlightDocsMenuItem_Title;
        Icon = MaterialIconKind.DocumentSign;
        Order = short.MinValue + 2;
        HotKey = KeyGesture.Parse("Alt+D");
        Command = ReactiveCommand.Create(() => _navigation.GoTo(FlightZoneMapViewModel.Uri));
    }
}