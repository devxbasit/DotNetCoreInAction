using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoApp.WebApi.Models;

namespace TodoApp.WebApi.Configuration.Entity;

public class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        builder.HasData(new List<TodoItem>
        {
            new TodoItem()
            {
                Id = 1,
                Title = "Todo 1",
                Details = "Details 1",
                Done = false,
            },
            new TodoItem()
            {
                Id = 2,
                Title = "Todo 2",
                Details = "Details 2",
                Done = false,
            }
        });
    }
}
