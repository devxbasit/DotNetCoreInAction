using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.WebApi.Data;
using TodoApp.WebApi.Models;

namespace TodoApp.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    private readonly AppDbContext _appDbContext;

    public TodoController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetItems()
    {
        var items = await _appDbContext.TodoItems.ToListAsync();
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetItem(int id)
    {
        var item = _appDbContext.TodoItems.FirstOrDefault(x => x.Id == id);

        if (item is null) return NotFound($"Item not found with Id {id}");

        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Add(TodoItem todoItem)
    {
        if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

        await _appDbContext.TodoItems.AddAsync(todoItem);
        await _appDbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetItem), new { Id = todoItem.Id }, todoItem);
    }

    [HttpPut]
    public async Task<IActionResult> Update(TodoItem todoItem)
    {
        if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

        if (await IsItemExists(todoItem.Id)) return NotFound($"Item not found with Id {todoItem.Id}");

        _appDbContext.TodoItems.Update(todoItem);
        await _appDbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (await IsItemExists(id)) return NotFound($"Item not found with Id {id}");

        var item = await _appDbContext.TodoItems.FirstOrDefaultAsync(x => x.Id == id);
        _appDbContext.TodoItems.Remove(item);
        await _appDbContext.SaveChangesAsync();
        return NoContent();
    }

    private async Task<bool> IsItemExists(int id)
    {
        var item = await _appDbContext.TodoItems.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return item is null ? true : false;
    }
}
