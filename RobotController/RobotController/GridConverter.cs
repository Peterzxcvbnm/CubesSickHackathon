namespace RobotController;

public class GridConverter
{
    private Dictionary<Coordinate, int> _indexByCoordinate = new();

    public GridConverter(List<Point> points)
    {
        foreach (var point in points)
        {
            _indexByCoordinate.Add(new Coordinate(point.X, point.Y), point.Index);
        }
    }
    
    public int GetIndex(Coordinate coordinate)
    {
        
    }
}