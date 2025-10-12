using WiSave.Income.Contracts.v1.Events;
using WiSave.Income.Domain.IncomeSource.ValueObjects;

namespace WiSave.Income.Domain.IncomeSource.Entities;

public class Incomes
{
    private readonly List<Income> _items = [];

    public IReadOnlyList<Income> Items => _items.AsReadOnly();
    
    public void Add(Income income)
    {
        _items.Add(income);
    }
    
    #region Play

    internal void Play(IncomeAdded @event)
    {
        var income = new Income(
            new IncomeId(@event.IncomeId),
            @event.Date,
            @event.Amount,
            @event.Notes,
            @event.Tags);

        _items.Add(income);
    }

    #endregion
}