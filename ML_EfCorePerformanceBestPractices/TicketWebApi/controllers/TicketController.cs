using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketWebApi.Data;

namespace TicketWebApi.controllers;

[Route("Ticket")]
public class TicketController : ControllerBase
{
    private readonly AppDbContext _db;

    public TicketController(AppDbContext appDbContext)
    {
        _db = appDbContext;
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var list = await _db.Tickets.ToListAsync();
        return Ok(list);
    }

    [HttpPut("UpdateAll")]
    public IActionResult UpdateAll()
    {
        return NoContent();
    }
}
