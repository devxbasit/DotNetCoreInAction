using AutoMapper;
using Contracts;
using Services.Contracts;

namespace Service;

internal sealed class EmployeeService : IEmployeeService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogManager _logger;
    private readonly IMapper _mapper;

    public EmployeeService(IRepositoryManager repository, ILogManager logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }
}