// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Configuration;
using RobotController;

Console.WriteLine("Hello, World!");

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false);

IConfiguration config = builder.Build();
var settings = config.GetSection("Settings").Get<Settings>();

var gridConverter = new GridConverter(settings.GridCoordinates);

var robot = new global::RobotController.RobotInterface.RobotController();

var mqtt = new MqttController();

using var mqttClient = await mqtt.Connect();

var controller = new Controller(mqtt, gridConverter, robot);

mqtt.OnCardScanned += controller.CardScanned;
mqtt.OnCoordinateReceived += controller.CoordinateReceived;


while (true)
{
    if(Console.ReadLine() == "quit"){
        break;
    }
}

