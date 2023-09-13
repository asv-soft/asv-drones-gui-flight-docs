using System.ComponentModel.Composition;
using Asv.Drones.Gui.Core;
using Avalonia.ReactiveUI;

namespace Asv.Drones.Gui.Plugin.FlightDocs;

[ExportView(typeof(TakeOffLandMapWidgetViewModel))]
[PartCreationPolicy(CreationPolicy.NonShared)]
public partial class TakeOffLandMapWidgetView : ReactiveUserControl<TakeOffLandMapWidgetViewModel>
{
    public TakeOffLandMapWidgetView()
    {
        InitializeComponent();
    }
}