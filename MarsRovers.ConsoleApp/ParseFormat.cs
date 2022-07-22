using MarsRovers.Domain;

namespace MarsRovers.ConsoleApp;

public static class ParseFormat
{
  public static List<RoverCommand> ParseMovementPlan(string input) =>
    input
      .Select(c => c switch
      {
        'L' => RoverCommand.TurnLeft,
        'R' => RoverCommand.TurnRight,
        'M' => RoverCommand.MoveForward,
        _ => throw new FormatException($"'{c}' is unsupported movement plan command. Please use the supported commands (L, R, M)."),
      })
      .ToList();

  public static string PositionToString(Location position)
  {
    var (point, direction) = position;

    var directionCharacter = direction switch
    {
      CardinalDirection.North => "N",
      CardinalDirection.East => "E",
      CardinalDirection.South => "S",
      CardinalDirection.West => "W",
      _ => throw new FormatException($"Unknown cardinal direction ('{direction}')"),
    };

    return $"{point.X} {point.Y} {directionCharacter}";
  }

  public static (Point point, CardinalDirection direction) ParseStartingPosition(string input)
  {
    var parts = input.Split(" ");
    var point = ParsePoint(parts);
    if (parts.Length < 3)
      throw new FormatException($"Direction is missing");

    var directionCharacter = parts[2];
    var direction = directionCharacter switch
    {
      "N" => CardinalDirection.North,
      "E" => CardinalDirection.East,
      "S" => CardinalDirection.South,
      "W" => CardinalDirection.West,
      _ => throw new FormatException($"Unknown cardinal direction ('{directionCharacter}')"),
    };
    return (point, direction);
  }

  public static Point ParsePoint(string input) =>
    ParsePoint(input.Split(" "));

  private static Point ParsePoint(IReadOnlyList<string> parts)
  {
    var x = int.Parse(parts.ElementAtOrDefault(0) ?? "");
    var y = int.Parse(parts.ElementAtOrDefault(1) ?? "");
    return new Point(x, y);
  }
}
