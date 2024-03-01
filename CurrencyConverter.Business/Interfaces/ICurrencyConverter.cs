namespace CurrencyConverter.Business.Interfaces;

public interface ICurrencyConverter
{
    /// <summary>
    /// Clears any prior configuration.
    /// </summary>
    void ClearConfiguration();

    /// <summary>
    /// Returns the current configuration of currency conversion rates.
    /// </summary>
    /// <returns></returns>
    Dictionary<string, Dictionary<string, double>> GetConfiguration();

    /// <summary>
    /// Updates the configuration. Rates are inserted or replaced internally.
    /// </summary>
    void UpdateConfiguration(IEnumerable<Tuple<string, string, double>> conversionRates);

    /// <summary>
    /// Converts the specified amount to the desired currency.
    /// </summary>
    double Convert(string fromCurrency, string toCurrency, double amount);
}
