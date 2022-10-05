using System.Text;
using System.Text.Json;
using MQTTnet;
using MQTTnet.Client;

namespace RobotController;

public class MqttController
{
    private MqttClient _client;
    private string _brokerAddress = Environment.GetEnvironmentVariable("MQTT_BROKER_ADDRESS") ?? "10.10.10.10";
    private int _brokerPort = int.TryParse(Environment.GetEnvironmentVariable("MQTT_BROKER_PORT"), out var port) ? port : 1883;
    private string _clientId = Environment.GetEnvironmentVariable("BOT_ID") ?? "N/A";
    private string _cardScannerTopic => $"/card_scanner/{_clientId}/current_employee";
    
    public event Func<string, Task>? OnCardScanned; 
    public event Func<Coordinate, Task>? OnCoordinateReceived; 

    public async Task Connect()
    {
        /*
         * This sample creates a simple MQTT client and connects to a public broker.
         *
         * Always dispose the client when it is no longer used.
         * The default version of MQTT is 3.1.1.
         */

        var mqttFactory = new MqttFactory();

        using var mqttClient = mqttFactory.CreateMqttClient();
        // Use builder classes where possible in this project.
        var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer(_brokerAddress).Build();
        
        mqttClient.ApplicationMessageReceivedAsync += OnMqttClientOnApplicationMessageReceivedAsync;

        // This will throw an exception if the server is not available.
        // The result from this message returns additional data which was sent 
        // from the server. Please refer to the MQTT protocol specification for details.
        var response = await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

        Console.WriteLine("The MQTT client is connected.");

        await Subscribe(_cardScannerTopic);
        
        Console.WriteLine("Subscribed to topic: " + _cardScannerTopic);
    }

    private Task OnMqttClientOnApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs e)
    {
        Console.WriteLine("Received application message on topic: " + e.ApplicationMessage.Topic);
        var message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
        if (e.ApplicationMessage.Topic == _cardScannerTopic)
        {
            OnCardScanned?.Invoke(message);
        }
        else
        {
            try
            {
                var coordinate = JsonSerializer.Deserialize<Coordinate>(message);
                OnCoordinateReceived?.Invoke(coordinate);
            }
            catch (JsonException ex)
            {
                Console.WriteLine("Invalid json received: " + message);
            }
        }

        return Task.CompletedTask;
    }

    public async Task Subscribe(string topic)
    {
        await _client.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(topic).Build());
        Console.WriteLine("Subscribed to topic: " + topic);
    }

    public async Task Unsubscribe(string topic)
    {
        await _client.UnsubscribeAsync(new MqttClientUnsubscribeOptionsBuilder().WithTopicFilter(topic).Build());
        Console.WriteLine("Unsubscribed from topic: " + topic);
    }
}