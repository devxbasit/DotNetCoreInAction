using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace TodoApp.WebApi.Models;

public class RefreshToken
{
    public int Id { get; set; } // default primary key column
    public string UserId { get; set; } // Linked to the AspNet Identity User ID
    public string Token { get; set; }
    public string JwtId { get; set; }

    public bool IsUsed { get; set; }
    public bool IsRevoked { get; set; }

    public DateTime AddedDateTime { get; set; }
    public DateTime ExpiryDateTime { get; set; }

    [ForeignKey(nameof(UserId))] public IdentityUser User { get; set; }
}
