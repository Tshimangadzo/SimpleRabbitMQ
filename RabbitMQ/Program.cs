using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new()
{
    Uri = new Uri("amqp://rabbituser:password@localhost:5672"),
    ClientProvidedName = "Producer"
};

IConnection connection = factory.CreateConnection();
IModel channel = connection.CreateModel();

string exchangeName = "exchange-name";
string routingKey = "routing-key";
string queueName = "queue-name";
string messageToSend = "Message #";

channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, false, false,null);
channel.QueueDeclare(queueName, false, false, false, null);

channel.QueueBind(queueName,exchangeName,routingKey,null);

for (int i = 0; i < 250;i++) {
    byte[] bytes = Encoding.UTF8.GetBytes(messageToSend+$"{i}");
    channel.BasicPublish(exchangeName, routingKey, null, bytes);
    Console.WriteLine("Message sent");
}