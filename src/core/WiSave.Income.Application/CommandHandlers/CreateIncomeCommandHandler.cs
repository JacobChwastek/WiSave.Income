using MassTransit;
using WiSave.Core.EventStore.Aggregate;
using WiSave.Income.Contracts.v1.Commands;
using WiSave.Income.Domain.IncomeSource;
using WiSave.Shared.Telemetry;

namespace WiSave.Income.Application.CommandHandlers;

public class CreateIncomeCommandHandler(IAggregateRepository<IncomeSource, IncomeSourceId> repository,  IActivitySourceProvider activitySourceProvider) : IConsumer<CreateIncome>
{
    public async Task Consume(ConsumeContext<CreateIncome> context)
    {
        using var activity = activitySourceProvider.Source.StartActivity(nameof(CreateIncomeCommandHandler));
        var message = context.Message;
        
        activity?.SetTag(WiSaveTelemetryTags.Commands.CommandType, nameof(CreateIncome));
        activity?.SetTag(WiSaveTelemetryTags.Commands.CommandId, context.MessageId?.ToString());
        activity?.SetTag("income.amount", message.Amount);
        activity?.SetTag("income_source.name", message.IncomeSource.Name);
        

         var incomeSource = IncomeSource.Create(
             message.IncomeSource.Name,
             message.IncomeSource.Details,
             message.IncomeSource.IsRegular,
             message.IncomeSource.Tags,
             message.Date,
             message.Amount,
             message.Notes,
             message.Tags);
         
         await repository.Save(incomeSource, context.CancellationToken);

         // var xd = new IncomeSourceId(new Guid("0199d9d8-b090-7dae-b2e1-008f4b56a465"));
         //
         // var xdd =  await repository.GetById(xd);
    }
}