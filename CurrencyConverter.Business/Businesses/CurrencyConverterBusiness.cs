namespace CurrencyConverter.Business.Businesses;

public class CurrencyConverterBusiness : ICurrencyConverter
{
    #region Variables

    /// <summary>
    /// A dictionary that stores conversion rates between currencies.
    /// </summary>
    private Dictionary<string, Dictionary<string, double>> currencyDictionary;

    /// <summary>
    /// An instance of the CurrencyConverterBusiness class.
    /// </summary>
    private static CurrencyConverterBusiness? currencyConverterBusiness;

    /// <summary>
    /// An object used for locking to ensure thread safety.
    /// </summary>
    private static readonly object lockObject = new();

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor for the CurrencyConverterBusiness class. Initializes the currencyDictionary
    /// </summary>
    public CurrencyConverterBusiness()
    {
        currencyDictionary = [];
    }

    /// <summary>
    /// Property representing a singleton instance of the CurrencyConverterBusiness class.
    /// </summary>
    public static CurrencyConverterBusiness Instance
    {
        get
        {
            lock (lockObject)
            {
                if (currencyConverterBusiness == null)
                {
                    currencyConverterBusiness = new CurrencyConverterBusiness();
                }
                return currencyConverterBusiness;
            }
        }
    }

    #endregion

    #region Public Methods

    public Dictionary<string, Dictionary<string, double>> GetConfiguration()
    {
        lock (lockObject)
        {
            return currencyDictionary;
        }
    }

    public void ClearConfiguration()
    {
        lock (lockObject)
        {
            currencyDictionary.Clear();
        }
    }

    public void UpdateConfiguration(IEnumerable<Tuple<string, string, double>> conversionRates)
    {
        lock (lockObject)
        {
            foreach (Tuple<string, string, double> conversionRate in conversionRates)
            {
                AddOrUpdateRate(conversionRate.Item1, conversionRate.Item2, conversionRate.Item3);
                AddOrUpdateRate(conversionRate.Item2, conversionRate.Item1, 1.0 / conversionRate.Item3);
            }
        }
    }

    public double Convert(string fromCurrency, string toCurrency, double amount)
    {
        if (fromCurrency == toCurrency)
        {
            return amount;
        }

        lock (lockObject)
        {
            EnsureCurrencyExistsInConfiguration(fromCurrency);
            EnsureCurrencyExistsInConfiguration(toCurrency);

            HashSet<string>? visitedCurrencies = new();
            Tuple<string, double> initialConversion = Tuple.Create(fromCurrency, amount);
            double? convertedAmount = TraverseConversionPath(initialConversion, toCurrency, visitedCurrencies);

            if (convertedAmount.HasValue)
            {
                return convertedAmount.Value;
            }

            throw new ArgumentException("No conversion path found");
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Adds or updates a conversion rate between two currencies in the configuration.
    /// </summary>
    /// <param name="fromCurrency"></param>
    /// <param name="toCurrency"></param>
    /// <param name="rate"></param>
    private void AddOrUpdateRate(string fromCurrency, string toCurrency, double rate)
    {
        if (!currencyDictionary.TryGetValue(fromCurrency, out Dictionary<string, double>? rates))
        {
            rates = [];
            currencyDictionary[fromCurrency] = rates;
        }
        rates[toCurrency] = rate;
    }

    /// <summary>
    /// Traverses the conversion path to convert an amount from one currency to another.
    /// </summary>
    /// <param name="initialConversion"></param>
    /// <param name="targetCurrency"></param>
    /// <param name="visitedCurrencies"></param>
    /// <returns></returns>
    private double? TraverseConversionPath(Tuple<string, double> initialConversion, string targetCurrency, HashSet<string> visitedCurrencies)
    {
        Queue<Tuple<string, double>> queue = new();
        queue.Enqueue(initialConversion);

        while (queue.Count > 0)
        {
            var (currentCurrency, currentAmount) = queue.Dequeue();
            visitedCurrencies.Add(currentCurrency);

            foreach (var (nextCurrency, conversionRate) in currencyDictionary[currentCurrency])
            {
                double convertedAmount = currentAmount * conversionRate;

                if (nextCurrency == targetCurrency)
                {
                    return convertedAmount;
                }

                if (!visitedCurrencies.Contains(nextCurrency))
                {
                    queue.Enqueue(Tuple.Create(nextCurrency, convertedAmount));
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Ensures that a currency exists in the configuration.
    /// </summary>
    /// <param name="currency"></param>
    /// <exception cref="ArgumentException"></exception>
    private void EnsureCurrencyExistsInConfiguration(string currency)
    {
        if (!currencyDictionary.ContainsKey(currency))
        {
            throw new ArgumentException($"Currency '{currency}' not found in configuration");
        }
    }

    #endregion
}
