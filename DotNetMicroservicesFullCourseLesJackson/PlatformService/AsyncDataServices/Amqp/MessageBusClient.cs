using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using PlatformService.Configurations;
using PlatformService.Dtos;
using PlatformService.Utils.StaticClasses;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PlatformService.AsyncDataServices.Amqp;

public class MessageBusClient : IMessageBusClient, IAsyncDisposable
{
    private IConnection _connection;
    private IChannel _channel;

    public MessageBusClient(IOptions<RabbitMqConnectionOptions> rabbitMqConnectionOptions)
    {
        InitializeRabbitMq(rabbitMqConnectionOptions.Value);
    }

    public async Task PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
    {
        var message = JsonSerializer.Serialize(platformPublishedDto);

        if (_connection.IsOpen)
        {
            Console.WriteLine($"--> RabbitMq Connection Open, sending message");
            await SendMessage(message);
        }
        else
        {
            Console.WriteLine($"--> RabbitMQ connection is closed, not sending message");
        }
    }

    private async Task SendMessage(string message)
    {
        try
        {
            var body = Encoding.UTF8.GetBytes(message);
            await _channel.BasicPublishAsync(exchange: RabbitMqExchangesNames.TriggerExchange, routingKey: "", body: body);
            Console.WriteLine($"--> Sent to Message Bus: {message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Rabbit MQ could not publish message: {ex.Message}");
        }
    }

    public async ValueTask DisposeAsync()
    {
        Console.Write("MessageBus Disposed");

        if (_channel.IsOpen)
        {
            await _channel.CloseAsync();
            await _connection.CloseAsync();
        }
    }

    private async Task RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
    {
        Console.WriteLine("--> RabbitMQ Connection Shutdown");
        await Task.CompletedTask;
    }

    private void InitializeRabbitMq(RabbitMqConnectionOptions rabbitMqConfig)
    {
        var connectionFactory = new ConnectionFactory()
        {
            HostName = rabbitMqConfig.Host,
            Port = int.Parse(rabbitMqConfig.Port),
            UserName = rabbitMqConfig.Username,
            Password = rabbitMqConfig.Password
        };

        try
        {
            _connection = connectionFactory.CreateConnectionAsync().Result;
            _channel = _connection.CreateChannelAsync().Result;
            _channel.ExchangeDeclareAsync(exchange: RabbitMqExchangesNames.TriggerExchange, type: ExchangeType.Fanout).Wait();
            _connection.ConnectionShutdownAsync += RabbitMQ_ConnectionShutdown;

            Console.WriteLine("--> Connected to Message Bus");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could not connect to Message Bus: {ex.Message}");
            throw;
        }
    }
}
