using Catalog.Domain.Models;

namespace Catalog.TestHelpers;

public class PlateBuilder
{
    private Guid _id = Guid.NewGuid();
    private string _registration = "ABC123";
    private decimal _purchasePrice = 10m;
    private decimal _salePrice = 15m;
    private string _letters = "ABC";
    private int _numbers = 123;

    public PlateBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public PlateBuilder WithRegistration(string registration)
    {
        _registration = registration;
        return this;
    }

    public PlateBuilder WithPurchasePrice(decimal purchasePrice)
    {
        _purchasePrice = purchasePrice;
        return this;
    }

    public PlateBuilder WithSalePrice(decimal salePrice)
    {
        _salePrice = salePrice;
        return this;
    }

    public PlateBuilder WithLetters(string letters)
    {
        _letters = letters;
        return this;
    }

    public PlateBuilder WithNumbers(int numbers)
    {
        _numbers = numbers;
        return this;
    }

    public static implicit operator Plate(PlateBuilder builder) => builder.Build();
    
    private Plate Build()
    {
        return new Plate
        {
            Id = _id,
            Registration = _registration,
            PurchasePrice = _purchasePrice,
            SalePrice = _salePrice,
            Letters = _letters,
            Numbers = _numbers
        };
    }
}
