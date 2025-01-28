using System.Security.Cryptography;
using EmployeeGrpc;
using Grpc.Core;

namespace Grpc.Server.Services.Grpc;

public class EmployeeService : EmployeeGrpc.EmployeeGrpcService.EmployeeGrpcServiceBase
{
    private static int LastInsertedEmployeeId { get; set; } = 100;

    public EmployeeService()
    {
        PrintExecutingMethodInfoToConsole("***Constructor Invoked***");
    }

    public override Task<AddEmployeeResponse> SayHello(AddEmployeeRequest request, ServerCallContext context)
    {
        PrintExecutingMethodInfoToConsole(nameof(SayHello));

        var response = new AddEmployeeResponse()
        {
            EmployeeId = ++EmployeeService.LastInsertedEmployeeId,
            EmployeeName = request.Employee.EmployeeName
        };

        return Task.FromResult<AddEmployeeResponse>(response);
    }

    public override async Task ServerStream(AddEmployeeRequest request, IServerStreamWriter<AddEmployeeResponse> responseStream, ServerCallContext context)
    {
        PrintExecutingMethodInfoToConsole(nameof(ServerStream));

        var insertCount = 10;

        Console.WriteLine($"--> Server: Adding {insertCount} entries in Employee table with same name.");
        for (int i = 0; i < insertCount; i++)
        {
            var response = new AddEmployeeResponse()
            {
                EmployeeId = ++EmployeeService.LastInsertedEmployeeId,
                EmployeeName = request.Employee.EmployeeName
            };

            Console.WriteLine($"--> Server: EmployeeId: {response.EmployeeId}, EmployeeName: {response.EmployeeName}");

            await responseStream.WriteAsync(response);
            await Task.Delay(TimeSpan.FromMilliseconds(300));
        }
    }

    public override async Task<AddEmployeeResponse> ClientStream(IAsyncStreamReader<AddEmployeeRequest> requestStream, ServerCallContext context)
    {
        PrintExecutingMethodInfoToConsole(nameof(ClientStream));

        while (await requestStream.MoveNext())
        {
            var currentRequest = requestStream.Current;
            Console.WriteLine($"--> Server: Adding employee with name: {currentRequest.Employee.EmployeeName}");
        }

        var response = new AddEmployeeResponse()
        {
            EmployeeId = ++EmployeeService.LastInsertedEmployeeId,
            EmployeeName = "Test Employee Name"
        };

        return response;
    }

    public override async Task BiDirectionalStream(IAsyncStreamReader<AddEmployeeRequest> requestStream, IServerStreamWriter<AddEmployeeResponse> responseStream, ServerCallContext context)
    {
        PrintExecutingMethodInfoToConsole(nameof(BiDirectionalStream));

        while (await requestStream.MoveNext())
        {
            var currentRequest = requestStream.Current;
            Console.WriteLine($"--> Server: Adding Employee with name: {currentRequest.Employee.EmployeeName}");

            var response = new AddEmployeeResponse()
            {
                EmployeeId = ++EmployeeService.LastInsertedEmployeeId,
                EmployeeName = currentRequest.Employee.EmployeeName
            };

            await responseStream.WriteAsync(response);
        }
    }

    private void PrintExecutingMethodInfoToConsole(string methodName)
    {
        Console.WriteLine($"--> Server: Executing method: {methodName} at {DateTime.Now}.");
    }
}
