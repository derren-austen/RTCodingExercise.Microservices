namespace Catalog.Domain.Messages;

public record CreatePlateMessage
(
    Guid Id,
    string Registration,
    decimal PurchasePrice,
    decimal SalePrice,
    string Letters,
    int Numbers
);