namespace MarsRovers.Domain;

public enum CardinalDirection
{
  North,
  East,
  South,
  West
}

public static class CardinalDirectionExtensions
{
  public static CardinalDirection TurnLeft(this CardinalDirection direction) => direction switch
  {
    CardinalDirection.North => CardinalDirection.West,
    CardinalDirection.East => CardinalDirection.North,
    CardinalDirection.South => CardinalDirection.East,
    CardinalDirection.West => CardinalDirection.South,
    _ => throw new NotImplementedException($"Unknown cardinal direction ('{direction}')"),
  };

  public static CardinalDirection TurnRight(this CardinalDirection direction) => direction switch
  {
    CardinalDirection.North => CardinalDirection.East,
    CardinalDirection.East => CardinalDirection.South,
    CardinalDirection.South => CardinalDirection.West,
    CardinalDirection.West => CardinalDirection.North,
    _ => throw new NotImplementedException($"Unknown cardinal direction ('{direction}')"),
  };
}
