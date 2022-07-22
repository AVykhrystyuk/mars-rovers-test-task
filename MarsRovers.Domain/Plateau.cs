using System.Diagnostics.CodeAnalysis;

namespace MarsRovers.Domain;

public class Plateau
{
  public Point Start { get; init; }
  public Point End { get; init; }
  private Dictionary<Point, string> _takenPoints { get; } = new Dictionary<Point, string>();

  public static Plateau From(Point upperRightCoordinate) => new()
  {
    Start = new Point(0, 0),
    End = upperRightCoordinate,
  };

  public bool IsPointTaken(Point point, [MaybeNullWhen(false)] out string? takenBy) => _takenPoints.TryGetValue(point, out takenBy);

  public void MarkPointAsTaken(Point point, string takenBy) => _takenPoints[point] = takenBy;

  public bool Fits(Point point) =>
    Start.X <= point.X && point.X <= End.X
    &&
    Start.Y <= point.Y && point.Y <= End.Y;

  public override string ToString()
    => $"({Start.X}, {Start.Y}) - ({End.X}, {End.Y})";
}
