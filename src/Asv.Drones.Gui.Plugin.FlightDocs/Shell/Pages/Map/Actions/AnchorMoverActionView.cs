using System.ComponentModel.Composition;
using Asv.Drones.Gui.Core;

namespace Asv.Drones.Gui.Plugin.FlightDocs;

[ExportView(typeof(AnchorMoverActionViewModel))]
[PartCreationPolicy(CreationPolicy.NonShared)]
public class AnchorMoverActionView : Gui.Core.AnchorMoverActionView
{
    public AnchorMoverActionView()
    {
    }
}