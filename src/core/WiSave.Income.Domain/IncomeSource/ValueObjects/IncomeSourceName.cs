namespace WiSave.Income.Domain.IncomeSource.ValueObjects;

public record IncomeSourceName
{
    public string Value { get; init; }

    public IncomeSourceName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Income source name cannot be null or empty.", nameof(value));
        }

        if (value.Length > 200)
        {
            throw new ArgumentException("Income source name cannot exceed 200 characters.", nameof(value));
        }

        Value = value.Trim();
    }

    public static implicit operator string(IncomeSourceName incomeSourceName) => incomeSourceName.Value;

    public static implicit operator IncomeSourceName(string value) => new(value);

    public override string ToString() => Value;
}