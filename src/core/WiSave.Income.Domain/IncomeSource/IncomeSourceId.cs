using WiSave.Core.EventStore.Aggregate;

namespace WiSave.Income.Domain.IncomeSource;

public record IncomeSourceId : IAggregateIdentity
{
    public Guid Value { get; init; }
    
    public IncomeSourceId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("IncomeSourceId cannot be empty.", nameof(value));
        }

        Value = value;
    }

    public static IncomeSourceId New() => new(Guid.CreateVersion7());

    public static implicit operator Guid(IncomeSourceId id) => id.Value;

    public static implicit operator IncomeSourceId(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}