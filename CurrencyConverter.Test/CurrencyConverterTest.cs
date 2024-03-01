namespace CurrencyConverter.Test;

public class CurrencyConverterTest
{
    /// <summary>
    /// Tests the Convert method of the CurrencyConverterBusiness class with valid configuration.
    /// It verifies that the correct result is returned when converting from one currency to another.
    /// </summary>
    [Fact]
    public void Convert_WithValidConfiguration_ShouldReturnCorrectResult()
    {
        // Arrange
        CurrencyConverterBusiness currencyConverterBusiness = CurrencyConverterBusiness.Instance;
        List<Tuple<string, string, double>> conversionRates = new()
        {
            Tuple.Create("USD", "CAD", 1.34),
            Tuple.Create("CAD", "GBP", 0.58),
            Tuple.Create("USD", "EUR", 0.86)
        };
        currencyConverterBusiness.UpdateConfiguration(conversionRates);

        // Act & Assert
        Assert.Equal(86, currencyConverterBusiness.Convert("USD", "EUR", 100));
    }

    /// <summary>
    /// Tests the Convert method of the CurrencyConverterBusiness class with invalid configuration.
    /// It verifies that an ArgumentException is thrown when trying to convert with invalid configuration.
    /// </summary>
    [Fact]
    public void Convert_WithInvalidConfiguration_ShouldThrowException()
    {
        // Arrange
        CurrencyConverterBusiness currencyConverterBusiness = CurrencyConverterBusiness.Instance;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => currencyConverterBusiness.Convert("USD", "EUR", 100));
    }
}
