using System.ComponentModel.Composition;
using Asv.Drones.Gui.Core;
using Avalonia.ReactiveUI;

namespace Asv.Drones.Gui.FlightDocs;

[ExportView(typeof(FlightPlanGeneratorMapWidgetViewModel))]
[PartCreationPolicy(CreationPolicy.NonShared)]
public partial class FlightPlanGeneratorMapWidgetView : ReactiveUserControl<FlightPlanGeneratorMapWidgetViewModel>
{
    public FlightPlanGeneratorMapWidgetView()
    {
        InitializeComponent();
    }
}