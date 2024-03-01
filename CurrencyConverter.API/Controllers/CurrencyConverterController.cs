namespace CurrencyConverter.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class CurrencyConverterController : ControllerBase
{
    #region Variables

    private readonly CurrencyConverterBusiness _currencyConverterBusiness;

    #endregion

    #region Constructors

    public CurrencyConverterController(CurrencyConverterBusiness currencyConverterBusiness)
    {
        _currencyConverterBusiness = currencyConverterBusiness;
    }

    #endregion

    #region HttpGet

    [HttpGet]
    public IActionResult GetConfiguration()
        => Ok(_currencyConverterBusiness.GetConfiguration());

    #endregion

    #region HttpPost

    [HttpPost]
    public void UpdateConfiguration(IEnumerable<Tuple<string, string, double>> conversionRates)
        => _currencyConverterBusiness.UpdateConfiguration(conversionRates);

    [HttpPost]
    public IActionResult Convert(ConvertViewModel convertViewModel)
        => Ok(_currencyConverterBusiness.Convert(convertViewModel.FromCurrency, convertViewModel.ToCurrency, convertViewModel.Amount));

    [HttpPost]
    public void ClearConfiguration()
        => _currencyConverterBusiness.ClearConfiguration();

    #endregion
}
