using System.Text;
using RabbitMQ.Client;

namespace AuthService.Api.Rabbit;

public static class SendUsername
{
    public static void Send(string username)
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost"
        };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        
        channel.QueueDeclare(
            queue: "username", 
            durable: false, 
            exclusive: false, 
            autoDelete: false, 
            arguments: null);
        
        var body = Encoding.UTF8.GetBytes(username);
        channel.BasicPublish(
            exchange: "", 
            routingKey: "username", 
            basicProperties: null, 
            body: body);
    }
}