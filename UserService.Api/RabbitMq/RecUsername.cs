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
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: "username",
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                    Console.WriteLine(message);
                    var user = new User { Username = message };
                    await userService.AddAsync(user);
                    var save = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    await save.SaveChangesAsync();
                }
            };

            _channel.BasicConsume(queue: "username",
                                  autoAck: true,
                                  consumer: consumer);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _channel?.Close();
            _connection?.Close();
            return Task.CompletedTask;
        }
    }
}
