namespace WiSave.Income.Contracts.v1.Events;

public record IncomeSourceCreated(
    Guid IncomeSourceId,
    string Name,
    string Details,
    bool IsRegular,
    string[] Tags
);