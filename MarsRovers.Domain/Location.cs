namespace MarsRovers.Domain;

public class Location
{
  public Location(Point point, CardinalDirection direction, Plateau plateau)
  {
    Point = point;
    Direction = direction;
    Plateau = plateau;
  }

  public Point Point { get; private set; }
  public CardinalDirection Direction { get; private set; }
  public Plateau Plateau { get; }

  public Location Clone()
  {
    var clone = (Location)MemberwiseClone();
    return clone;
  }

  public void TurnLeft()
  {
    Direction = Direction.TurnLeft();
  }

  public void TurnRight()
  {
    Direction = Direction.TurnRight();
  }

  public void MoveForward()
  {
    var vector = GetMoveVector(Direction);
    var newPoint = new Point(Point.X + vector.X, Point.Y + vector.Y);

    if (!Plateau.Fits(newPoint))
      throw new ArgumentException($"Can not move beyond the plateau ({newPoint})");

    if (Plateau.IsPointTaken(newPoint, out var takenBy))
      throw new ArgumentException($"Cannot move to {newPoint} as this location is already taken by '{takenBy}'");

    Point = newPoint;
  }

  public void Deconstruct(out Point point, out CardinalDirection direction)
  {
    point = Point;
    direction = Direction;
  }

  private static Point GetMoveVector(CardinalDirection direction) => direction switch
  {
    CardinalDirection.North => new Point(0, 1),
    CardinalDirection.East => new Point(1, 0),
    CardinalDirection.South => new Point(0, -1),
    CardinalDirection.West => new Point(-1, 0),
    _ => throw new NotImplementedException($"Unknown cardinal direction ('{direction}')"),
  };
}
