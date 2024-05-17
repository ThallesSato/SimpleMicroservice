using System.Text;
using RabbitMQ.Client;

namespace AuthService.Api.Rabbit;

public static class SendUsername
{
    public static void Send(string username)
    {
        // Define connection factory (domain)
        var factory = new ConnectionFactory
        {
            HostName = "localhost"
        };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        
        // Declare queue
        channel.QueueDeclare(
            queue: "username", 
            durable: false, 
            exclusive: false, 
            autoDelete: false, 
            arguments: null);
        
        // Get username and include it in body
        var body = Encoding.UTF8.GetBytes(username);
        
        // Send message
        channel.BasicPublish(
            exchange: "", 
            routingKey: "username", 
            basicProperties: null, 
            body: body);
    }
}