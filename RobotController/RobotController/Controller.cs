using MQTTnet.Client;
using RobotController.RobotInterface;

namespace RobotController;

public class Controller
{
    private string _clientId = Environment.GetEnvironmentVariable("BOT_ID") ?? "N/A";
    private string _currentEmployee = string.Empty;
    private MqttController _mqtt;
    private readonly GridConverter _gridConcerter;
    private readonly IRobot _robot;

    public Controller(MqttController mqtt, GridConverter gridConcerter, IRobot robot)
    {
        _mqtt = mqtt;
        _gridConcerter = gridConcerter;
        _robot = robot;
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
        var targetIndex = _gridConcerter.GetIndex(coordinate);
        await _robot.Goto(targetIndex);
    }
}