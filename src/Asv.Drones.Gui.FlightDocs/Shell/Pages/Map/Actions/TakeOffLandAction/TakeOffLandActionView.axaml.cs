using System.ComponentModel.Composition;
using Asv.Drones.Gui.Core;
using Avalonia.ReactiveUI;

namespace Asv.Drones.Gui.FlightDocs;

[ExportView(typeof(TakeOffLandActionViewModel))]
[PartCreationPolicy(CreationPolicy.NonShared)]
public partial class TakeOffLandActionView : ReactiveUserControl<TakeOffLandActionViewModel>
{
    public TakeOffLandActionView()
    {
        InitializeComponent();
    }
}