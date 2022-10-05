namespace RobotController;

public class AgvPoint
{
    
    public int X { get; set; }
    public int Y { get; set; }
    public Direction Direction { get; set; }

    public static AgvPoint ParseString(string input)
    {
        var point = new AgvPoint
        {
            X = int.Parse(input[1].ToString()),
            Y = int.Parse(input[0].ToString()),
            Direction = (Direction)Enum.Parse(typeof(Direction), input[3].ToString())
        };
        return point;
    }
    
    public override string ToString()
    {
        return $"{Y}{X}0{(int)Direction}";
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