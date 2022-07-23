using MarsRovers.Domain;

namespace MarsRovers.ConsoleApp;

using static ParseFormat;

public class Application
{
  private readonly IConsole _console;
  private readonly int _maxParseRetryCount;

  public Application(IConsole console, int maxParseRetryCount = 2)
  {
    _console = console;
    _maxParseRetryCount = maxParseRetryCount;
  }

  public void Run()
  {
    try
    {
      var plateau = Plateau.From(GetUpperRightCoordinate());
      var totalNumberOfRovers = GetTotalNumberOfRovers();

      for (var roverNumber = 1; roverNumber <= totalNumberOfRovers; roverNumber++)
      {
        var startingPosition = GetStartingPosition(roverNumber, plateau);
        var movementPlan = GetMovementPlan(roverNumber);

        var rover = new RoverSimulator($"Rover {roverNumber}", startingPosition);
        var roverPosition = rover.Run(movementPlan);
        plateau.MarkPointAsTaken(roverPosition.Point, rover.Name);
        _console.WriteLine($"{rover.Name} Output: {PositionToString(roverPosition)}");
      }
    }
    catch (Exception ex)
    {
      _console.WriteError(ex);
      return;
    }
  }

  private List<RoverCommand> GetMovementPlan(int roverNumber) => TryParseInput(() =>
  {
    var defaultValue = DefaultInputs.MovementPlan;
    var response = _console.Prompt($"Rover {roverNumber} Movement Plan (Default: '{defaultValue}'): ").Trim();
    return ParseMovementPlan(response.IfEmpty(defaultValue));
  });

  private Location GetStartingPosition(int roverNumber, Plateau plateau) => TryParseInput(() =>
  {
    var defaultValue = DefaultInputs.StartingPosition;
    var response = _console.Prompt($"Rover {roverNumber} Starting Position (Default: '{defaultValue}'): ").Trim();
    var (point, direction) = ParseStartingPosition(response.IfEmpty(defaultValue));

    ThrowIfLessThan("X", point.X, 0);
    ThrowIfLessThan("Y", point.Y, 0);

    if (plateau.IsPointTaken(point, out var takenBy))
      throw new ArgumentException($"Cannot start at {point} as this location is already taken by '{takenBy}'");

    return new Location(point, direction, plateau);
  });

  private int GetTotalNumberOfRovers() => TryParseInput(() =>
  {
    var defaultValue = DefaultInputs.TotalNumberOfRovers;
    var response = _console.Prompt($"Enter Number Of Rovers Deployed (Default: '{defaultValue}'): ").Trim();
    var number = int.Parse(response.IfEmpty(defaultValue));

    ThrowIfLessThan("Number Of Rovers", number, 1);

    return number;
  });

  private Point GetUpperRightCoordinate() => TryParseInput(() =>
  {
    var defaultValue = DefaultInputs.UpperRightCoordinate;
    var response = _console.Prompt($"Enter Plateau Upper Right Coordinate (Default: '{defaultValue}'): ").Trim();
    var point = ParsePoint(response.IfEmpty(defaultValue));

    ThrowIfLessThan("X", point.X, 1);
    ThrowIfLessThan("Y", point.Y, 1);

    return point;
  });

  private T TryParseInput<T>(Func<T> parse)
  {
    for (var i = 0; i < _maxParseRetryCount + 1; i++)
    {
      try
      {
        return parse();
      }
      catch (Exception ex)
      {
        _console.WriteError(ex);
      }
    }

    throw new Exception($"Maximum number of retries ({_maxParseRetryCount}) exceeded while parsing the input");
  }

  private static void ThrowIfLessThan(string name, int value, int min)
  {
    if (value < min)
      throw new ArgumentException($"{name} ({value}) cannot be less than {min}");
  }
}

public static class DefaultInputs
{
  public const string UpperRightCoordinate = "5 5";
  public const string TotalNumberOfRovers = "2";
  public const string StartingPosition = "1 2 N";
  public const string MovementPlan = "LMLMLMLMM";
}
