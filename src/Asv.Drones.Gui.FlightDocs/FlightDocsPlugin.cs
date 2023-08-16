using System.ComponentModel.Composition;
using Asv.Drones.Gui.Core;

namespace Asv.Drones.Gui.FlightDocs;

[PluginEntryPoint("FlightDocs", CorePlugin.Name)]
[PartCreationPolicy(CreationPolicy.NonShared)]
public class FlightDocsPlugin : IPluginEntryPoint
{
    [ImportingConstructor]
    public FlightDocsPlugin()
    {
    }
    public void Initialize()
    {
    }

    public void OnFrameworkInitializationCompleted()
    {
    }

    public void OnShutdownRequested()
    {
    }
}