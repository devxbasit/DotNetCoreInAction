using EfCore.ConsoleApp.Data;
using EfCore.ConsoleApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EfCore.ConsoleApp;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            await DeleteAllData();
            await AddManagerAndEmployee();

            await EagerLoading();
            await ExplicitLoading();
            await LazyLoading();

            await TransactionExample();

            Console.ReadKey();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    static async Task TransactionExample()
    {
        await using (var _dbContext = new AppDbContext())
        {
            await using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var manager1 = _dbContext.Managers.Add(new Manager()
                    {
                        Name = "Manager x",
                    }).Entity;
                    await _dbContext.SaveChangesAsync(); // First operation

                    _dbContext.Managers.Add(new Manager()
                    {
                        Name = $"Manager y - {manager1.ManagerId}",
                    });
                    await _dbContext.SaveChangesAsync(); // Second operation

                    // Commit the transaction if both operations succeed
                    await _dbContext.Database.CommitTransactionAsync();

                }
                catch
                {
                    // Rollback the transaction if any operation fails
                    transaction.Rollback();
                }
            }
        }
    }

    static async Task LazyLoading()
    {
        await Task.CompletedTask;
        // mark navigation properties with virtual to lazy load them automatically
        // we just have to use them directly, no null exception will be thrown, they will be
        // loaded automatically on demand
    }

    static async Task ExplicitLoading()
    {
        Console.WriteLine("********** ExplicitLoading Example **********");

        await using (var _dbContext = new AppDbContext())
        {
            var manager = await _dbContext.Managers.FirstAsync();
            await _dbContext.Entry(manager).Collection(x => x.Employees).LoadAsync();

            var employee = await _dbContext.Employees.FirstAsync();
            await _dbContext.Entry(employee).Reference(x => x.EmployeeDetails).LoadAsync();
        }
    }

    static async Task EagerLoading()
    {
        Console.WriteLine("********** EagerLoading Example **********");

        // EfCore by default does not load related entity
        // To load eagerly related entites we can use Include() & ThenInclude()
        await using (var _dbContext = new AppDbContext())
        {
            var employees = await _dbContext.Employees
                .Include(x => x.Manager)
                .Include(x => x.EmployeeDetails)
                .ToListAsync();

            foreach (var employee in employees)
            {
                Console.WriteLine($"Employee Name: {employee.Name}, Email: {employee.EmployeeDetails.Email}, Manager Name: {employee.Manager.Name}");
            }
        }
    }

    static async Task AddManagerAndEmployee()
    {
        await using (var _dbContext = new AppDbContext())
        {
            var manager = new Manager()
            {
                Name = "John Doe - Manager",
            };

            var employeeDetails = new EmployeeDetails()
            {
                Address = "Address of employee",
                Email = "email@email.com",
            };

            var employee = new Employee()
            {
                Name = "Sam Smith - Employee",
                Salary = 1000,
                Manager = manager,
                EmployeeDetails = employeeDetails
            };

            await _dbContext.AddAsync(employee);
            await _dbContext.SaveChangesAsync();
        }
    }

    static async Task DeleteAllData()
    {
        await using (var _dbContext = new AppDbContext())
        {
            _dbContext.Employees.RemoveRange(await _dbContext.Employees.ToListAsync());
            _dbContext.Managers.RemoveRange(await _dbContext.Managers.ToListAsync());
            await _dbContext.SaveChangesAsync();
        }
    }
}
