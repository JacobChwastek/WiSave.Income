using WiSave.Core.EventStore.Aggregate;
using WiSave.Income.Contracts.v1.Events;
using WiSave.Income.Domain.IncomeSource.ValueObjects;

namespace WiSave.Income.Domain.IncomeSource;

public class IncomeSource : Aggregate<IncomeSourceId>
{
    public IncomeSourceName Name { get; private set; }
    public IncomeSourceTags Tags { get; private set; }
    public bool IsRegular { get; private set; }

    public IncomeSource() {}

    public static IncomeSource Create(
        string name,
        string details,
        bool isRegular,
        string[] tags,
        DateTime initialIncomeDate,
        decimal initialIncomeAmount,
        string initialIncomeNotes,
        string[] initialIncomeTags)
    {
        var incomeSource = new IncomeSource();
        var incomeSourceId = IncomeSourceId.New();
        
        var sourceCreated = new IncomeSourceCreated(
            incomeSourceId.Value,
            name,
            details,
            isRegular,
            tags);
        
        incomeSource.RaiseEvent(sourceCreated);
        
        var incomeAdded = new IncomeAdded(
            incomeSourceId.Value,
            IncomeId.New().Value,
            initialIncomeDate,
            initialIncomeAmount,
            initialIncomeNotes,
            initialIncomeTags);
        
        incomeSource.RaiseEvent(incomeAdded);
        
        return incomeSource;
    }
    
    public void AddIncome(DateTime date, decimal amount, string notes, string[] tags)
    {
        if (amount <= 0)
            throw new ArgumentException("Income amount must be positive", nameof(amount));
        
        if (date > DateTime.UtcNow)
            throw new ArgumentException("Income date cannot be in the future", nameof(date));
        var @event = new IncomeAdded(
            Identifier.Value,
            IncomeId.New().Value,
            date,
            amount,
            notes,
            tags ?? []);
        
        RaiseEvent(@event);
    }
    
    #region Play

    private void Play(IncomeSourceCreated @event)
    {
        Identifier = new IncomeSourceId(@event.IncomeSourceId);
        Name = new IncomeSourceName(@event.Name);
        Tags = new IncomeSourceTags(@event.Tags);
        IsRegular = @event.IsRegular;
    }

    private void Play(IncomeAdded @event)
    {
        //TODO:
    }

    #endregion
}
