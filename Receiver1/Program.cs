using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new()
{
    Uri = new Uri("amqp://rabbituser:password@localhost:5672"),
    ClientProvidedName = "Consumer 1"
};
IConnection connection = factory.CreateConnection();
IModel channel = connection.CreateModel();

string exchangeName = "exchange-name";
string routingKey = "routing-key";
string queueName = "queue-name";

channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, false, false, null);
channel.QueueDeclare(queueName, false, false, false, null);

channel.QueueBind(queueName, exchangeName, routingKey, null);

channel.BasicQos(0, 1, false);

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (sender, e) =>
{
    Thread.Sleep(1000);
    var body = e.Body.ToArray();
    string bodyMessage = Encoding.UTF8.GetString(body);
    Console.WriteLine("Consumer 1: " + bodyMessage);
    channel.BasicAck(e.DeliveryTag, false);

};

string tag = channel.BasicConsume(queueName, false, consumer);

Console.ReadLine();
channel.BasicCancel(tag);

channel.Close();
connection.Close();