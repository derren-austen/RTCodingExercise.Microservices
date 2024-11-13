using Ardalis.Result;

using Catalog.Domain.Interfaces.Handlers;
using Catalog.Domain.Messages;
using Catalog.Domain.Models;

using MassTransit;

namespace Catalog.Minimal.Api.Consumers;

public class CreatePlateConsumer(
    ILogger<CreatePlateConsumer> logger,
    ICatalogApiHandler catalogApiHandler)
    : IConsumer<CreatePlateMessage>
{
    public async Task Consume(ConsumeContext<CreatePlateMessage> context)
    {
        logger.LogInformation("{id} - {registration} is being created", context.Message.Id, context.Message.Registration);

        var plate = MapMessageToPlate(context);

        var result = await catalogApiHandler.CreatePlateAsync(plate);

        if (result.Status is ResultStatus.Created)
            logger.LogInformation("{id} - {registration} has been created", context.Message.Id, context.Message.Registration);
        else
            logger.LogError("{id} - {registration} could not be created - {errorMessage}", context.Message.Id, context.Message.Registration, result.Errors);

    }

    private static Plate MapMessageToPlate(ConsumeContext<CreatePlateMessage> context)
    {
        return new Plate
        {
            Id = context.Message.Id,
            Registration = context.Message.Registration,
            PurchasePrice = context.Message.PurchasePrice,
            SalePrice = context.Message.SalePrice,
            Letters = context.Message.Letters,
            Numbers = context.Message.Numbers,
        };
    }
}
