using Catalog.Domain.Messages;

using MassTransit;

namespace Catalog.Minimal.Api.Consumers;

public class CreatePlateConsumer(
    ILogger<CreatePlateConsumer> logger)
    : IConsumer<CreatePlateMessage>
{
    public async Task Consume(ConsumeContext<CreatePlateMessage> context)
    {
        await Task.Delay(5000);

        logger.LogInformation("Registration of the incoming message: {message}", context.Message.Registration);
    }
}
