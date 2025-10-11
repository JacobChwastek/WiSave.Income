using MassTransit;
using MassTransit.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using WiSave.Income.Contracts.v1.Commands;
using WiSave.Shared.Income.Infrastructure.MassTransit;

namespace WiSave.Income.WebApi.Endpoints;

internal static class IncomeEndpoints
{
    public static void MapIncomeEndpoints(this IEndpointRouteBuilder app)
    {
        var builder = app.MapGroup("/api/incomes").WithTags("Incomes");

        builder
            .MapPost("", CreateIncomeAsync)
            .WithName("CreateIncome")
            .WithDescription("Create income")
            .Produces(StatusCodes.Status200OK);
    }

    private static async Task<IResult> CreateIncomeAsync([FromBody] CreateIncome command, [FromServices] Bind<IIncomeBus, IPublishEndpoint> publishEndpoint, CancellationToken token)
    {
        await publishEndpoint.Value.Publish(command, token);
        
        return Results.Ok();
    }
}