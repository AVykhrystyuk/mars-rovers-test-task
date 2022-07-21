using MarsRovers.Domain;

namespace MarsRovers.ConsoleApp;

internal class InputOutput
{
  public static List<RoverCommand> ParseMovementPlan(string input) =>
    input
      .Select(c => c switch
      {
        'L' => RoverCommand.TurnLeft,
        'R' => RoverCommand.TurnRight,
        'M' => RoverCommand.Move,
        _ => throw new FormatException($"Unsupported movement plan command ('{c}')"),
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

  public static Location ParseStartingPosition(string input)
  {
    var parts = input.Split(" ");
    var point = ParsePoint(parts);
    var directionCharacter = parts.ElementAtOrDefault(2);
    var direction = directionCharacter switch
    {
      "N" => CardinalDirection.North,
      "E" => CardinalDirection.East,
      "S" => CardinalDirection.South,
      "W" => CardinalDirection.West,
      _ => throw new FormatException($"Unknown cardinal direction ('{directionCharacter}')"),
    };
    return new Location(point, direction);
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
