using System.Diagnostics;

namespace WiSave.Shared.Telemetry;

public class ActivityScope(IActivitySourceProvider activitySourceProvider) : IActivityScope
{
    public Activity? Start(string name, StartActivityOptions? options = null)
    {
        options ??= new StartActivityOptions();

        var activityName = $"{activitySourceProvider.SourceName}.{name}";

        return options.Parent.HasValue
            ? activitySourceProvider.Source
                .CreateActivity(
                    activityName,
                    options.Kind,
                    parentContext: options.Parent.Value,
                    tags: options.Tags
                )?.Start()
            : activitySourceProvider.Source
                .CreateActivity(
                    activityName,
                    options.Kind,
                    parentId: options.ParentId ?? Activity.Current?.Id,
                    tags: options.Tags
                )?.Start();
    }

    public async Task Run(string name, Func<Activity?, CancellationToken, Task> run, StartActivityOptions? options = null, CancellationToken ct = default)
    {
        using var activity = Start(name, options);

        try
        {
            await run(activity, ct).ConfigureAwait(false);
            activity?.SetStatus(ActivityStatusCode.Ok);
        }
        catch (Exception ex)
        {
            activity?.SetStatus(ActivityStatusCode.Error);
            activity?.AddException(ex);
            throw;
        }
    }

    public async Task<TResult> Run<TResult>(string name, Func<Activity?, CancellationToken, Task<TResult>> run, StartActivityOptions? options = null, CancellationToken ct = default)
    {
        using var activity = Start(name, options);

        try
        {
            var result = await run(activity, ct).ConfigureAwait(false);
            activity?.SetStatus(ActivityStatusCode.Ok);
            return result;
        }
        catch (Exception ex)
        {
            activity?.SetStatus(ActivityStatusCode.Error);
            activity?.AddException(ex);
            throw;
        }
    }
}