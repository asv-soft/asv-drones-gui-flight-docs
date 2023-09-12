using System.ComponentModel.Composition;
using Asv.Drones.Gui.Core;
using Avalonia.ReactiveUI;

namespace Asv.Drones.Gui.Plugin.FlightDocs;

[ExportView(typeof(FlightPlanViewModel))]
[PartCreationPolicy(CreationPolicy.NonShared)]
public partial class FlightPlanView : ReactiveUserControl<FlightPlanViewModel>
{
    public FlightPlanView()
    {
        InitializeComponent();
    }
}