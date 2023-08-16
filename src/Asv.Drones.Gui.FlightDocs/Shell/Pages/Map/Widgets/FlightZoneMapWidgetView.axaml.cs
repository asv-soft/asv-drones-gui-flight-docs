using System.ComponentModel.Composition;
using Asv.Drones.Gui.Core;
using Avalonia.ReactiveUI;

namespace Asv.Drones.Gui.FlightDocs;

[ExportView(typeof(FlightZoneMapWidgetViewModel))]
[PartCreationPolicy(CreationPolicy.NonShared)]
public partial class FlightZoneMapWidgetView : ReactiveUserControl<FlightZoneMapWidgetViewModel>
{
    public FlightZoneMapWidgetView()
    {
        InitializeComponent();
    }
}