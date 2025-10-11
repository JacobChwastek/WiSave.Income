namespace WiSave.Income.Contracts.v1.Models;

/// <summary>
/// 
/// </summary>
/// <param name="Name"></param>
/// <param name="Details"></param>
/// <param name="IsRegular"></param>
/// <param name="Tags"></param>
public record CreateIncomeSourceDto(string Name, string Details, bool IsRegular, string[] Tags);