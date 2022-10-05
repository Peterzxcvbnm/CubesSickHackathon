namespace RobotController.RobotInterface;

public class RobotController : IRobot
{
    private int currentPos = 0;
    
    public Task Goto(int index)
    {
        currentPos = index;
        return Task.CompletedTask;
    }
}