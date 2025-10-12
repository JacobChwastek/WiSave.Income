namespace WiSave.Income.Domain.IncomeSource.ValueObjects;

public record IncomeSourceTags
{
    public IReadOnlyList<string> Value { get; init; }

    public IncomeSourceTags(params string[] tags)
    {
        ArgumentNullException.ThrowIfNull(tags);

        var validTags = tags
            .Where(tag => !string.IsNullOrWhiteSpace(tag))
            .Select(tag => tag.Trim())
            .Where(tag => tag.Length <= 50)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        Value = validTags;
    }

    public IncomeSourceTags(IEnumerable<string> tags) : this(tags?.ToArray() ?? [])
    {
    }

    public static IncomeSourceTags Empty => new();

    public bool IsEmpty => Value.Count == 0;

    public bool Contains(string tag) => 
        Value.Any(t => t.Equals(tag, StringComparison.OrdinalIgnoreCase));

    public static implicit operator string[](IncomeSourceTags tags) => 
        tags.Value.ToArray();

    public static implicit operator IncomeSourceTags(string[] tags) => 
        new(tags);

    public override string ToString() => 
        Value.Count == 0 ? string.Empty : string.Join(", ", Value);
}