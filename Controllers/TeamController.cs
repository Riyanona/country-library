using Microsoft.AspNetCore.Mvc;
using CountryLibrary.Services;

namespace CountryLibrary.Controllers;

[ApiController]
[Route("api/Team")]
public class TeamController(ICountryLibraryService service) : ControllerBase
{
    [HttpGet("GetTeamMembers")]
    public IActionResult GetTeamMembers()
    {
        var teamMembers = service.GetTeamMembers();
        return Ok(teamMembers);
    }
}