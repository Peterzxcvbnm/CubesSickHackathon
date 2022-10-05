using System.Diagnostics;

namespace RobotController;

public class GridConverter
{
    private Dictionary<Coordinate, string> _indexByCoordinate = new();

    public GridConverter(List<Point> points)
    {
        foreach (var point in points)
        {
            _indexByCoordinate.Add(new Coordinate(point.X, point.Y), point.Index);
        }
    }
    
    public string GetIndex(Coordinate coordinate)
    {
        if(_indexByCoordinate.TryGetValue(coordinate, out var index))
        {
            return index;
        }

        Coordinate closestCoordinate = null;
        var closestDistance = double.MaxValue;
        foreach (var pointCoordinate in _indexByCoordinate.Keys)
        {
            var distance = coordinate.DistanceTo(pointCoordinate);
            if (!(distance < closestDistance)) continue;
            closestCoordinate = pointCoordinate;
            closestDistance = distance;
        }

        if (closestCoordinate is null)
        {
            throw new InvalidOperationException("No closest coordinate found");
        }

        return _indexByCoordinate[closestCoordinate];
    }
}