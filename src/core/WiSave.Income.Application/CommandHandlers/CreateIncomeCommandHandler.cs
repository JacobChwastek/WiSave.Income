using MassTransit;
using WiSave.Income.Contracts.v1.Commands;

namespace WiSave.Income.Application.CommandHandlers;

public class CreateIncomeCommandHandler : IConsumer<CreateIncome>
{
    public async Task Consume(ConsumeContext<CreateIncome> context)
    {
        var message = context.Message;
    }
}