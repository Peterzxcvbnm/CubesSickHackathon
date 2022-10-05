namespace RobotController;

public class AgvPoint
{
    
    public int X { get; set; }
    public int Y { get; set; }
    public Direction Direction { get; set; }

    public static AgvPoint ParseString(string input)
    {
        var inputAsInt = int.Parse(input);
        var point = new AgvPoint
        {
            X = (inputAsInt/100)%10,
            Y = inputAsInt/1000,
            Direction = (Direction) (inputAsInt%10)
        };
        return point;
    }
    
    public override string ToString()
    {
        var toReturn = $"{Y}{X}0{(int) Direction}";
        for(int i = 0; i < 4; i++)
        {
            if (toReturn.StartsWith('0'))
            {
                toReturn = toReturn[1..];
            }
            else
            {
                break;
            }
        }
        return toReturn;
    }
    
    public static bool operator ==(AgvPoint a, AgvPoint b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(AgvPoint a, AgvPoint b)
    {
        return !(a == b);
    }

    protected bool Equals(AgvPoint other)
    {
        return X == other.X && Y == other.Y;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((AgvPoint) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}