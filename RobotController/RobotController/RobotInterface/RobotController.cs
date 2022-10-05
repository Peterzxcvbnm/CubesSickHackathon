using System.Net.Http.Json;
using SafelogSimulator;

namespace RobotController.RobotInterface;

public class RobotController : IRobot
{
    private HttpClient _client = new HttpClient();
    private string _robotUrl = Environment.GetEnvironmentVariable("ROBOT_URL") ?? "10.10.10.20";
    private int currentPos = 0;
    
    public async Task Goto(int index)
    {
        if (currentPos == index) return;
        currentPos = index;
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
                            Value = index.ToString()
                        }
                    }
                }
            }
        });
        var response = await _client.PostAsync(_robotUrl + "api/instantActions", content);

        response.EnsureSuccessStatusCode();
    }
}