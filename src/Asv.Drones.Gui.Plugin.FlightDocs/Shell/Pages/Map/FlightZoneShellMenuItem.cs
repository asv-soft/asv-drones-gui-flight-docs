using System.ComponentModel.Composition;
using Asv.Drones.Gui.Core;
using Material.Icons;

namespace Asv.Drones.Gui.Plugin.FlightDocs;

[Export(typeof(IShellMenuItem))]
[PartCreationPolicy(CreationPolicy.NonShared)]
public class FlightZoneShellMenuItem : ShellMenuItem
{
    public FlightZoneShellMenuItem() : base("asv:shell.menu.flight-docs")
    {
        Name = RS.HeaderFlightDocsMenuItem_Title;
        NavigateTo = FlightZoneMapViewModel.Uri;
        Icon = MaterialIconDataProvider.GetData(MaterialIconKind.DocumentSign);
        Position = ShellMenuPosition.Top;
        Type = ShellMenuItemType.PageNavigation;
        Order = 10;
    }
}