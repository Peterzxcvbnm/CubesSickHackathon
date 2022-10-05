using System.Net.Http.Json;
using SafelogSimulator;

namespace RobotController.RobotInterface;

public class RobotController : IRobot
{
    private HttpClient _client = new HttpClient();
    private string _robotUrl = Environment.GetEnvironmentVariable("ROBOT_URL") ?? "https://10.10.10.20:8900";
    private AgvPoint? currentPos = null;
    
    public async Task Goto(string index)
    {
        try
        {
        var nextPoint = AgvPoint.ParseString(index);
        if (currentPos is not null && currentPos == nextPoint) return;

        GetNextDirection(nextPoint);   

        var content = JsonContent.Create(new RootDto()
        {
            InstantActions = new List<InstantAction>
            {
                new()
                {
                    ActionName = "goto",
                    ActionId = 0,
                    BlockingType = "NONE",
                    ActionParameters = new List<ActionParameter>()
                    {
                        new()
                        {
                            Key = "end",
                            Value = nextPoint.ToString()
                        }
                    }
                }
            }
        });
        Console.WriteLine("Going to: " + nextPoint.ToString());
        
            //var response = await _client.PostAsync(_robotUrl + "/api/instantActions", content);

            //response.EnsureSuccessStatusCode();

            currentPos = nextPoint;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message + e.StackTrace);
        }
    }

    private void GetNextDirection(AgvPoint nextPoint)
    {
        if (currentPos is null)
        {
            return;
        }
        if (currentPos.X < nextPoint.X && currentPos.Y < nextPoint.Y)
        {
            nextPoint.Direction = Direction.East;
        }
        else if (currentPos.X < nextPoint.X && currentPos.Y > nextPoint.Y)
        {
            nextPoint.Direction = Direction.West;
        }
        else if (currentPos.X > nextPoint.X && currentPos.Y < nextPoint.Y)
        {
            nextPoint.Direction = Direction.East;
        }
        else if (currentPos.X > nextPoint.X && currentPos.Y > nextPoint.Y)
        {
            nextPoint.Direction = Direction.West;
        }
        else if (currentPos.X == nextPoint.X && currentPos.Y < nextPoint.Y)
        {
            nextPoint.Direction = Direction.East;
        }
        else if (currentPos.X == nextPoint.X && currentPos.Y > nextPoint.Y)
        {
            nextPoint.Direction = Direction.West;
        }
        else if (currentPos.X < nextPoint.X && currentPos.Y == nextPoint.Y)
        {
            nextPoint.Direction = Direction.North;
        }
        else if (currentPos.X > nextPoint.X && currentPos.Y == nextPoint.Y)
        {
            nextPoint.Direction = Direction.South;
        }
    }
}