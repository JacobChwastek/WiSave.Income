namespace WiSave.Income.Contracts.v1.Events;

public record IncomeAdded(
    Guid IncomeSourceId,
    Guid IncomeId,
    DateTime Date,
    decimal Amount,
    string Notes,
    string[] Tags
);