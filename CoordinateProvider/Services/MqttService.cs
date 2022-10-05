using System.Text.Json;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Implementations;

namespace CoordinateProvider.Services;

public class MqttService
{
    IMqttClient _mqttClient;
    private IConfiguration _configuration;
    private string _employeeIdTopicFormat;

    public MqttService(IConfiguration configuration)
    {
        _configuration = configuration;
        _mqttClient = new MqttFactory().CreateMqttClient();
        _employeeIdTopicFormat = _configuration["Mqtt:EmployeeIdTopicFormat"] ?? "/coordinate_provider/{0}/coordinate";
    }

    public async Task PublishAsync(string topic, string message)
    {
        await _mqttClient.PublishStringAsync(message, topic);
    }

    public async Task PublishEmployeeIdAsync(string Employee, double x, double y)
    {
        await EnsureConnectionAsync();
        await _mqttClient.PublishStringAsync(String.Format(_employeeIdTopicFormat, Employee), JsonSerializer.Serialize(new { X=x, Y=y }));
    }

    private async Task EnsureConnectionAsync()
    {
        if (!_mqttClient.IsConnected)
        {
            await _mqttClient.ConnectAsync(new MqttClientOptions()
            {
                ClientId = "CoordinateProvider",
                ChannelOptions = new MqttClientTcpOptions()
                {
                    Server = _configuration["Mqtt:Server"] ?? "10.10.10.10",
                    Port = int.Parse(_configuration["Mqtt:Port"] ?? "1883"),
                }
            });
        }
    }
}
