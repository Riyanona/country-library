using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using CountryLibrary.Models;

namespace CountryLibrary.Services;

public class CountryLibraryService(HttpClient httpClient) : ICountryLibraryService
{
    private static readonly List<TeamMember> TeamMembers = new()
    {
        new TeamMember
        {
            Name = "Tep Bopha",
            Gender = "Female",
            Age = 30,
            Address = "123, St. Monivong, Phnom Penh, Cambodia",
            Email = "tep.bopha@techbodia.com"
        },
        new TeamMember
        {
            Name = "Jane Smith",
            Gender = "Female",
            Age = 28,
            Address = "456 Maple Ave, Toronto, ON M5V 2T6",
            Email = "jane.smith@example.com"
        },
        new TeamMember
        {
            Name = "Alex Johnson",
            Gender = "Non-Binary",
            Age = 25,
            Address = "789 Oak Rd, London, UK SW1A 1AA",
            Email = "alex.johnson@example.com"
        },
        new TeamMember
        {
            Name = "Maria Garcia",
            Gender = "Female",
            Age = 32,
            Address = "321 Elm St, São Paulo, SP 01310-100",
            Email = "maria.garcia@example.com"
        },
        new TeamMember
        {
            Name = "Chenda Mony",
            Gender = "Male",
            Age = 27,
            Address = "654 Pine Rd, Phnom Penh, KO 200040",
            Email = "chenda.mony@example.com"
        }
    };

    public List<TeamMember> GetTeamMembers()
    {
        return TeamMembers;
    }

    public async Task<CountryInfo?> GetCountryByName(string countryName)
    {
        if (string.IsNullOrEmpty(countryName))
        {
            return null;
        }

        try
        {
            var response = await httpClient.GetAsync($"name/{Uri.EscapeDataString(countryName)}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            var countries = JsonSerializer.Deserialize<List<RestCountry>>(json);

            var country = countries?.FirstOrDefault(c => c.Name.Equals(countryName, StringComparison.OrdinalIgnoreCase));
            if (country == null)
            {
                return null;
            }

            return new CountryInfo
            {
                Name = country.Name,
                Alpha2Code = country.Alpha2Code,
                Capital = country.Capital ?? string.Empty,
                CallingCodes = int.TryParse(country.CallingCodes?.FirstOrDefault() ?? "0", out var code) ? code : 0,
                FlagUrl = country.Flag ?? string.Empty,
                Timezones = string.Join(", ", country.Timezones ?? new List<string>())
            };
        }
        catch (HttpRequestException)
        {
            return null;
        }
        catch (JsonException)
        {
            return null;
        }
    }

    public async Task<List<CountryInfo>> GetCountryByArea(string region, string timezones)
    {
        try
        {
            var response = await httpClient.GetAsync("all");

            if (!response.IsSuccessStatusCode)
            {
                return new List<CountryInfo>();
            }

            var json = await response.Content.ReadAsStringAsync();
            var countries = JsonSerializer.Deserialize<List<RestCountry>>(json) ?? new List<RestCountry>();

            var filteredCountries = countries.AsEnumerable();

            if (!string.IsNullOrEmpty(region))
            {
                filteredCountries = filteredCountries.Where(c => c.Region != null && c.Region.Contains(region, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(timezones))
            {
                filteredCountries = filteredCountries.Where(c => c.Timezones != null && c.Timezones.Any(tz => tz.Contains(timezones, StringComparison.OrdinalIgnoreCase)));
            }

            return filteredCountries.Select(c => new CountryInfo
            {
                Name = c.Name,
                Alpha2Code = c.Alpha2Code,
                Capital = c.Capital ?? string.Empty,
                CallingCodes = int.TryParse(c.CallingCodes?.FirstOrDefault() ?? "0", out var code) ? code : 0,
                FlagUrl = c.Flag ?? string.Empty,
                Timezones = string.Join(", ", c.Timezones ?? new List<string>())
            }).ToList();
        }
        catch (HttpRequestException)
        {
            return new List<CountryInfo>();
        }
        catch (JsonException)
        {
            return new List<CountryInfo>();
        }
    }

    private class RestCountry
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("alpha2Code")]
        public string Alpha2Code { get; set; } = string.Empty;

        [JsonPropertyName("capital")]
        public string? Capital { get; set; }

        [JsonPropertyName("callingCodes")]
        public List<string>? CallingCodes { get; set; }

        [JsonPropertyName("flag")]
        public string? Flag { get; set; }

        [JsonPropertyName("timezones")]
        public List<string>? Timezones { get; set; }

        [JsonPropertyName("region")]
        public string? Region { get; set; }
    }
}