namespace Catalog.Minimal.Api.ViewModels;

public record CreatePlate
(
    string Registration,
    decimal PurchasePrice,
    decimal SalePrice,
    string Letters,
    int Numbers
);
