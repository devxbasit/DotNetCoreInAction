using DriversAppApi.Models;
using DriversAppApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DriversAppApi.Controllers;

[ApiController]
[Route("api/v1/drivers")]
public class DriversControllers(DriverService driverServices) : ControllerBase
{
    private readonly DriverService _driverService = driverServices;

    [HttpGet]
    public async Task<IActionResult> GetAllDrivers()
    {
        var allDrivers = await _driverService.GetAllDrivers();

        return Ok(allDrivers);
    }

    [HttpGet("{id:length(24)}")]
    public async Task<IActionResult> GetDriverById(string id)
    {
        var existingDriver = await _driverService.GetDriverById(id);

        if (existingDriver is null)
        {
            return NotFound();
        }

        return Ok(existingDriver);
    }

    [HttpPost]
    public async Task<IActionResult> AddDriver(Driver driver)
    {
        await _driverService.AddDriver(driver);
        return CreatedAtAction(nameof(GetDriverById), new { id = driver.Id }, driver);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> UpdateDriver(string id, Driver driver)
    {
        var existingDriver = await _driverService.GetDriverById(id);

        if (existingDriver is null)
        {
            return BadRequest();
        }

        driver.Id = existingDriver.Id;
        await _driverService.UpdateDriver(driver);
        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> DeleteDriver(string id)
    {
        var existingDriver = await _driverService.GetDriverById(id);

        if (existingDriver is null)
        {
            return BadRequest();
        }

        await _driverService.DeleteDriver(existingDriver.Id!);
        return NoContent();
    }
}
