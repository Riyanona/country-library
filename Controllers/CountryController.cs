using Microsoft.AspNetCore.Mvc;
using CountryLibrary.Models;
using CountryLibrary.Services;

namespace CountryLibrary.Controllers;

[ApiController]
[Route("api/Country")]
public class CountryController(ICountryLibraryService service) : ControllerBase
{
    [HttpPost("GetCountryByName")]
    public async Task<IActionResult> GetCountryByName([FromBody] CountryByNameRequest request)
    {
        if (string.IsNullOrEmpty(request.CountryByName))
        {
            return BadRequest("Country name is required.");
        }

        var country = await service.GetCountryByName(request.CountryByName);
        if (country == null)
        {
            return NotFound($"Country '{request.CountryByName}' not found.");
        }

        return Ok(country);
    }

    [HttpPost("GetCountryByArea")]
    public async Task<IActionResult> GetCountryByArea([FromBody] AreaInfoRequest request)
    {
        if (string.IsNullOrEmpty(request.Region) && string.IsNullOrEmpty(request.Timezones))
        {
            return BadRequest("At least one of Region or Timezones must be provided.");
        }

        var countries = await service.GetCountryByArea(request.Region, request.Timezones);
        if (!countries.Any())
        {
            return NotFound("No countries found matching the criteria.");
        }

        return Ok(countries);
    }
}