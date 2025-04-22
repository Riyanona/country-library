namespace CountryLibrary.Models;

public class CountryInfo
{
    public string Name { get; set; } = string.Empty;
    public string Alpha2Code { get; set; } = string.Empty;
    public string Capital { get; set; } = string.Empty;
    public int CallingCodes { get; set; }
    public string FlagUrl { get; set; } = string.Empty;
    public string Timezones { get; set; } = string.Empty;
}