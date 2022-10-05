using MQTTnet.Client;

namespace RobotController;

public class Controller
{
    private string _clientId = Environment.GetEnvironmentVariable("BOT_ID") ?? "N/A";
    private string _currentEmployee = string.Empty;
    private MqttController _mqtt;

    public Controller(MqttController mqtt)
    {
        _mqtt = mqtt;
    }

    public async Task CardScanned(string employeeId)
    {
        if (_currentEmployee == employeeId)
        {
            await _mqtt.Unsubscribe($"/coordinate_provider/{employeeId}/coordinate");
        }
        else
        {
            await _mqtt.Subscribe($"/coordinate_provider/{employeeId}/coordinate");
        }
    }

    public async Task CoordinateReceived(Coordinate coordinate)
    {
        
    }
}