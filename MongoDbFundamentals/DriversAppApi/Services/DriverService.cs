using DriversAppApi.Configurations;
using DriversAppApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DriversAppApi.Services;

public class DriverService
{
    private readonly IMongoCollection<Driver> _driverCollection;

    public DriverService(IOptions<MongoDbConnectionOptions> mongoDbConnectionOptions)
    {
        var mongoClient = new MongoClient(mongoDbConnectionOptions.Value.ConnectionString);
        var mongoDb = mongoClient.GetDatabase(mongoDbConnectionOptions.Value.DatabaseName);
        _driverCollection = mongoDb.GetCollection<Driver>(mongoDbConnectionOptions.Value.CollectionName);
    }

    public async Task<List<Driver>> GetAllDrivers()
    {
        return await _driverCollection.Find(_ => true).ToListAsync();
    }

    public async Task<Driver?> GetDriverById(string id)
    {
        return await _driverCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task AddDriver(Driver driver)
    {
        await _driverCollection.InsertOneAsync(driver);
    }

    public async Task UpdateDriver(Driver driver)
    {
        await _driverCollection.ReplaceOneAsync(x => x.Id == driver.Id, driver);
    }

    public async Task DeleteDriver(string id)
    {
        await _driverCollection.DeleteOneAsync(x => x.Id == id);
    }
}
