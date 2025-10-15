using System.Diagnostics;

namespace WiSave.Shared.Telemetry;

public interface IActivityScope
{
    Activity? Start(string name, StartActivityOptions? options = null);
    
    Task Run(
        string name,
        Func<Activity?, CancellationToken, Task> run,
        StartActivityOptions? options = null,
        CancellationToken ct = default
    );

    Task<TResult> Run<TResult>(
        string name,
        Func<Activity?, CancellationToken, Task<TResult>> run,
        StartActivityOptions? options = null,
        CancellationToken ct = default
    );
}