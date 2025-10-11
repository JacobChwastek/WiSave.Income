using WiSave.Income.Contracts.v1.Models;

namespace WiSave.Income.Contracts.v1.Commands;

public record CreateIncome(
    Guid? IncomeSourceId,
    CreateIncomeSourceDto IncomeSource,
    DateTime Date,
    decimal Amount,
    string Notes,
    string[] Tags
);