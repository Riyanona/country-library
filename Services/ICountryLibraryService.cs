using CountryLibrary.Models;

namespace CountryLibrary.Services;

public interface ICountryLibraryService
{
    List<TeamMember> GetTeamMembers();
    Task<CountryInfo?> GetCountryByName(string countryName);
    Task<List<CountryInfo>> GetCountryByArea(string region, string timezones);
}