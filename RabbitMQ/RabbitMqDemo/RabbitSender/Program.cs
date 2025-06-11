using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new ();
factory.Uri = new Uri(uriString: "amqp://guest:guest@localhost:5672");
factory.ClientProvidedName = "Rabbit Sender App";

IConnection cnn = factory.CreateConnection ();

IModel channel = cnn.CreateModel ();

string exchangeName = "DemoExchange";
string routingKey = "demo-routing-key";
string queueName = "DemoQueue";

channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete:false, arguments:null);
channel.QueueBind(queueName, exchangeName , routingKey, arguments:null);

for (int i=0;i<60;i++)
{
    Console.WriteLine(value: $"Sending Message {i}");
    byte[] messageBodyBytes = Encoding.UTF8.GetBytes($"Message #{i}");
    channel.BasicPublish(exchangeName, routingKey, null, messageBodyBytes);
    Thread.Sleep(1000);
}




channel.Close();
cnn.Close();