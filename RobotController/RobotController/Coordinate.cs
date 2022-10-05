namespace RobotController;

public class Coordinate
{
    public int X { get; set; }
    public int Y { get; set; }

    public Coordinate()
    {
    }

    public Coordinate(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Coordinate(Coordinate coordinate)
    {
        X = coordinate.X;
        Y = coordinate.Y;
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }

    protected bool Equals(Coordinate other)
    {
        return X == other.X && Y == other.Y;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Coordinate) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public double DistanceTo(Coordinate pointCoordinate)
    {
        return Math.Sqrt(Math.Pow(X - pointCoordinate.X, 2) + Math.Pow(Y - pointCoordinate.Y, 2));
    }
}