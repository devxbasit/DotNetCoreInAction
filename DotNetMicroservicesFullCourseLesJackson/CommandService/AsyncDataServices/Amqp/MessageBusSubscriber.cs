using System.Text;
using CommandService.Configurations;
using CommandService.EventProcessing;
using CommandService.Utils.StaticClasses;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommandService.AsyncDataService;

public class MessageBusSubscriber : BackgroundService, IAsyncDisposable
{
    private readonly IEventProcessor _eventProcessor;
    private IConnection _connection;
    private IChannel _channel;
    private string _queueName;

    public MessageBusSubscriber(IEventProcessor eventProcessor, IOptions<RabbitMqConnectionOptions> rabbitMqConnectionOptions)
    {
        _eventProcessor = eventProcessor;
        InitializeRabbitMq(rabbitMqConnectionOptions.Value);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (moduleHandler, ea) =>
            {
                Console.WriteLine("--> Event Received");
                var body = ea.Body;
                var notificationMessage = Encoding.UTF8.GetString(body.ToArray());
                _eventProcessor.ProcessEvent(notificationMessage);
                await Task.CompletedTask;
            };

            await _channel.BasicConsumeAsync(_queueName, autoAck: true, consumer: consumer);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in {nameof(MessageBusSubscriber)} : {ex.Message}");
            throw;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel.IsOpen)
        {
            await _channel.CloseAsync();
            await _connection.CloseAsync();
        }
    }

    private void InitializeRabbitMq(RabbitMqConnectionOptions rabbitMqConfig)
    {
        var connectionFactory = new ConnectionFactory()
        {
            HostName = rabbitMqConfig.Host,
            Port = int.Parse(rabbitMqConfig.Port),
            UserName = rabbitMqConfig.Username,
            Password = rabbitMqConfig.Password,
        };

        _connection = connectionFactory.CreateConnectionAsync().Result;
        _channel = _connection.CreateChannelAsync().Result;

        _channel.ExchangeDeclareAsync(exchange: RabbitMqExchangesNames.TriggerExchange, type: ExchangeType.Fanout);
        _queueName = _channel.QueueDeclareAsync().Result.QueueName;
        _channel.QueueBindAsync(_queueName, exchange: RabbitMqExchangesNames.TriggerExchange, routingKey: "");

        Console.WriteLine("--> Listening on message bus...");

        _connection.ConnectionShutdownAsync += RabbitMqConnectionShutdown;
    }

    private async Task RabbitMqConnectionShutdown(object sender, ShutdownEventArgs e)
    {
        await Task.CompletedTask;
        Console.WriteLine("--> Rabbit MQ Connection Shutdown");
    }
}
