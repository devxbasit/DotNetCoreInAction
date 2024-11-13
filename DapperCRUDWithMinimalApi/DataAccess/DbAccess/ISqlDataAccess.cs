namespace DataAccess.DbAccess;

public interface ISqlDataAccess
{
    Task<IEnumerable<T>> ExecuteQueryAsync<T, U>(string procedureName, U parameters, string connectionId = "Default");
    Task ExecuteCommandAsync<T>(string procedureName, T parameters, string connectionId = "Default");
}
