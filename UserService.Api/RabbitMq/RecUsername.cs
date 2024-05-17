using System.Text;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using UserService.Application.Interfaces;
using UserService.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using UserService.Domain.Interfaces;

namespace UserService.Api.RabbitMq
{
    public class RecUsername : IHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private IConnection? _connection;
        private IModel? _channel;

        public RecUsername(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Create a connection factory
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declare a queue named "username"
            _channel.QueueDeclare(queue: "username",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            // Create a consumer for handling incoming messages
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            { 
                // Get the body of the message
                var body = ea.Body.ToArray();
    
                // Convert the body to a string
                var message = Encoding.UTF8.GetString(body); 
                
                // Create a new user object with the received message as the username
                var user = new User { Username = message };
    
                // Get services to save the user
                using var scope = _serviceScopeFactory.CreateScope();
                var save = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                
                // Add the user asynchronously
                await userService.AddAsync(user);
    
                // Save changes asynchronously
                await save.SaveChangesAsync();
            };

            // Start consuming messages from the "username" queue
            _channel.BasicConsume(queue: "username",
                autoAck: true,
                consumer: consumer);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Close the connection
            _channel?.Close();
            _connection?.Close();
            return Task.CompletedTask;
        }
    }
}
