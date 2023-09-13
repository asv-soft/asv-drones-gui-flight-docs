using System.ComponentModel.Composition;
using Asv.Drones.Gui.Core;
using Avalonia.ReactiveUI;

namespace Asv.Drones.Gui.Plugin.FlightDocs;

[ExportView(typeof(TakeOffLandActionViewModel))]
[PartCreationPolicy(CreationPolicy.NonShared)]
public partial class TakeOffLandActionView : ReactiveUserControl<TakeOffLandActionViewModel>
{
    public TakeOffLandActionView()
    {
        InitializeComponent();
    }
}