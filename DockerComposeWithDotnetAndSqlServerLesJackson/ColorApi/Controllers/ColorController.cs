using ColorApi.Data;
using ColorApi.Dtos;
using ColorApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ColorApi.Controllers;

[ApiController]
[Route("api/v1/color")]
public class ColorController : ControllerBase
{
    private readonly AppDbContext _appDbContext;

    public ColorController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetColors()
    {
        var colors = await _appDbContext.Colors.AsNoTracking().ToListAsync();
        return Ok(colors);
    }

    [HttpPost]
    public async Task<IActionResult> AddColor([FromBody] ColorCreateDto colorCreateDto)
    {
        var colorModel = new Color() {
            ColorName = colorCreateDto.ColorName,
            HexValue = colorCreateDto.HexValue
        };

        await _appDbContext.Colors.AddAsync(colorModel);
        await _appDbContext.SaveChangesAsync();

        return Ok(colorModel);
    }
}
