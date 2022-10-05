// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Configuration;
using RobotController;

Console.WriteLine("Hello, World!");

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false);

IConfiguration config = builder.Build();

var mqtt = new MqttController();

await mqtt.Connect();

var controller = new Controller(mqtt);

mqtt.OnCardScanned += controller.CardScanned;
mqtt.OnCoordinateReceived += controller.CoordinateReceived;