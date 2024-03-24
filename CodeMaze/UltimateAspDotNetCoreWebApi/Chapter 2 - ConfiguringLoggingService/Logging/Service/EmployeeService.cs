using Contracts;
using Services.Contracts;

namespace Service;

internal sealed class EmployeeService : IEmployeeService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogManager _logger;

    public EmployeeService(IRepositoryManager repository, ILogManager logger)
    {
        _repository = repository;
        _logger = logger;
    }
}