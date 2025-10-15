using System.Diagnostics;
using System.Reflection;
using System.Text.Json;

namespace WiSave.Shared.Telemetry;

public static class ActivityExtensions
{
    /// <summary>
    /// Add tags from object properties automatically
    /// </summary>
    public static Activity? AddTagsFromObject(this Activity? activity, object? obj, string prefix = "")
    {
        if (activity == null || obj == null) return activity;

        var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var prop in properties)
        {
            var value = prop.GetValue(obj);
            if (value == null) continue;

            var tagName = string.IsNullOrEmpty(prefix)
                ? ToCamelCase(prop.Name)
                : $"{prefix}.{ToCamelCase(prop.Name)}";

            if (IsSimpleType(prop.PropertyType))
            {
                activity.SetTag(tagName, value);
            }
            else if (value is IEnumerable<object> collection)
            {
                activity.SetTag(tagName, JsonSerializer.Serialize(collection));
            }
            else
            {
                activity.SetTag(tagName, JsonSerializer.Serialize(value));
            }
        }

        return activity;
    }

    /// <summary>
    /// Add exception details to activity
    /// </summary>
    public static Activity? AddException(this Activity? activity, Exception exception)
    {
        if (activity == null || exception == null) return activity;

        activity.SetTag("exception.type", exception.GetType().FullName);
        activity.SetTag("exception.message", exception.Message);
        activity.SetTag("exception.stacktrace", exception.StackTrace);
        
        if (exception.InnerException != null)
        {
            activity.SetTag("exception.inner.type", exception.InnerException.GetType().FullName);
            activity.SetTag("exception.inner.message", exception.InnerException.Message);
        }

        activity.AddException(exception);
        return activity;
    }

    private static bool IsSimpleType(Type type)
    {
        return type.IsPrimitive
               || type.IsEnum
               || type == typeof(string)
               || type == typeof(decimal)
               || type == typeof(DateTime)
               || type == typeof(DateTimeOffset)
               || type == typeof(TimeSpan)
               || type == typeof(Guid);
    }

    private static string ToCamelCase(string str)
    {
        if (string.IsNullOrEmpty(str) || char.IsLower(str[0]))
            return str;

        return char.ToLowerInvariant(str[0]) + str.Substring(1);
    }
}