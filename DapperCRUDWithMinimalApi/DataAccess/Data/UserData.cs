using DataAccess.DbAccess;
using DataAccess.Models;

namespace DataAccess.Data;

public class UserData : IUserData
{
    private readonly ISqlDataAccess _db;

    public UserData(ISqlDataAccess db)
    {
        _db = db;
    }

    public async Task<IEnumerable<User>> GetUsers()
    {
        var result = await _db.ExecuteQueryAsync<User, dynamic>("dbo.spUser_GetAll", new { });
        return result;
    }

    public async Task<User?> GetUser(int id)
    {
        var result = await _db.ExecuteQueryAsync<User, dynamic>("dbo.spUser_Get", new { id });
        return result.FirstOrDefault();
    }

    public async Task InsertUser(User user)
    {
        await _db.ExecuteCommandAsync("dbo.spUser_Insert", new { user.FirstName, user.LastName });
    }

    public async Task UpdateUser(User user)
    {
        await _db.ExecuteCommandAsync("dbo.spUser_Update", user);
    }
    public async Task DeleteUser(int id)
    {
        await _db.ExecuteCommandAsync("dbo.spUser_Delete", new {Id = id});
    }
}
