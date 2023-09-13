using System.ComponentModel.Composition;
using Asv.Drones.Gui.Core;
using Avalonia.ReactiveUI;

namespace Asv.Drones.Gui.Plugin.FlightDocs;

[ExportView(typeof(FlightZoneActionViewModel))]
[PartCreationPolicy(CreationPolicy.NonShared)]
public partial class FlightZoneActionView : ReactiveUserControl<FlightZoneActionViewModel>
{
    public FlightZoneActionView()
    {
        InitializeComponent();
    }
}