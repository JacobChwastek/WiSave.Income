using WiSave.Income.Domain.IncomeSource.ValueObjects;

namespace WiSave.Income.Domain.IncomeSource.Entities;

public class Income
{
    public IncomeId Id { get; private set; }
    public DateTime Date { get; private set; }
    public decimal Amount { get; private set; }
    public string Notes { get; private set; }
    public string[] Tags { get; private set; }

    internal Income(IncomeId id, DateTime date, decimal amount, string notes, string[] tags)
    {
        Id = id;
        Date = date;
        Amount = amount;
        Notes = notes;
        Tags = tags;
    }
}