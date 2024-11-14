namespace Catalog.Minimal.Api.Config;

public class EventBusOptions
{
    public static string EventBusConfig = nameof(EventBusConfig);

    public required string Connection { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string CreatePlateQueueName { get; set; }
}
