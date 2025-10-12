namespace WiSave.Income.Domain.IncomeSource.ValueObjects;

public record IncomeId(Guid Value)
{
    public static IncomeId New() => new(Guid.CreateVersion7());
}