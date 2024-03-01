namespace CurrencyConverter.Business.ViewModels;

public class ConvertViewModel
{
    [Required] public string FromCurrency { get; set; } = null!;
    [Required] public string ToCurrency { get; set;} = null!;
    [Required] public double Amount { get; set; }
}
