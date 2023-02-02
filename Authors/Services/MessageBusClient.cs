using System.Text;
using Authors.DTO;
using Authors.Services.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Authors.Services;

public class MessageBusClient : IMessageBusClient
{
    private readonly IConfiguration _configuration;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public MessageBusClient(IConfiguration configuration)
    {
        _configuration = configuration;
        var factory = new ConnectionFactory()
        {
            HostName = _configuration.GetConnectionString("RabbitMQHost"),
            Port = int.Parse(_configuration.GetConnectionString("RabbitMQPort"))
        };

        try
        {
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

            Console.WriteLine("--> Connected to RabbitMQ");
        }
        catch (Exception e)
        {
            Console.WriteLine($"--> Failed to connect to RabbitMQ: {e.Message}");
            throw;
        }
    }

    public void Publish(AuthorPublishedDto authorPublishedDto)
    {
        var message = JsonSerializer.Serialize(authorPublishedDto);
        if (_connection.IsOpen)
        {
            Console.WriteLine("--> RabbitMQ Connection open, sending message...");
            SendMessage(message);
        }
        else
        {
            Console.WriteLine("--> RabbitMQ Connection closed");
        }
    }

    private void SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(
            exchange: "trigger", 
            routingKey: "",
            basicProperties: null,
            body: body
            );

        Console.WriteLine($"--> Sent {message}");
    }

    public void Dispose()
    {
        Console.WriteLine("--> RabbitMQ Disposed");
        if (_channel.IsOpen)
        {
            _channel.Close();
            _connection.Close();
        }
    }
    
    private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        Console.WriteLine("--> RabbitMQ Connection Shutdown");
    }
}