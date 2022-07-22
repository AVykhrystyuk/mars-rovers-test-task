namespace MarsRovers.ConsoleApp;

using MarsRovers.Domain;

using static Console;
using static InputOutput;

public class Application
{
  private readonly IService _service;
  private readonly int maxParseRetryCount = 3;

  public Application(IService service) =>
    _service = service;

  public void Run()
  {
    var plateau = Plateau.From(GetUpperRightCoordinate());
    var totalNumberOfRovers = GetTotalNumberOfRovers();

    for (int roverNumber = 1; roverNumber <= totalNumberOfRovers; roverNumber++)
    {
      var startingPosition = GetStartingPosition(roverNumber, plateau);
      var movementPlan = GetMovementPlan(roverNumber);

      var rover = new RoverSimulator($"Rover {roverNumber}", startingPosition);

      try
      {
        var roverPosition = rover.Run(movementPlan);
        WriteLine($"{rover.Name} Output: {PositionToString(roverPosition)}");
        plateau.MarkPointAsTaken(roverPosition.Point, rover.Name);
      }
      catch (Exception ex)
      {
        PrintError(ex);
        return;
      }
    }
  }

  private List<RoverCommand> GetMovementPlan(int roverNumber) => TryParseInput(() =>
  {
    var defaultValue = DefaultInputs.MovementPlan;
    Write($"Rover {roverNumber} Movement Plan (Default: '{defaultValue}'): ");
    return ParseMovementPlan(ReadLine().IfEmpty(defaultValue).Trim());
  });

  private Location GetStartingPosition(int roverNumber, Plateau plateau) => TryParseInput(() =>
  {
    var defaultValue = DefaultInputs.StartingPosition;
    Write($"Rover {roverNumber} Starting Position (Default: '{defaultValue}'): ");
    var (point, direction) = ParseStartingPosition(ReadLine().IfEmpty(defaultValue).Trim());

    ThrowIfLessThan("X", point.X, 0);
    ThrowIfLessThan("Y", point.Y, 0);

    if (plateau.IsPointTaken(point, out var takenBy))
      throw new ArgumentException($"Cannot start at {point} as this location is already taken by '{takenBy}'");

    return new Location(point, direction, plateau);
  });

  private int GetTotalNumberOfRovers() => TryParseInput(() =>
  {
    var defaultValue = DefaultInputs.TotalNumberOfRovers;
    Write($"Enter Number Of Rovers Deployed (Default: '{defaultValue}'): ");
    var number = int.Parse(ReadLine().IfEmpty(defaultValue).Trim());

    ThrowIfLessThan("Number Of Rovers", number, 1);

    return number;
  });

  private Point GetUpperRightCoordinate() => TryParseInput(() =>
  {
    var defaultValue = DefaultInputs.UpperRightCoordinate;
    Write($"Enter Graph Upper Right Coordinate (Default: '{defaultValue}'): ");
    var point = ParsePoint(ReadLine().IfEmpty(defaultValue).Trim());

    ThrowIfLessThan("X", point.X, 1);
    ThrowIfLessThan("Y", point.Y, 1);

    return point;
  });

  private T TryParseInput<T>(Func<T> parse)
  {
    for (int i = 0; i < maxParseRetryCount; i++)
    {
      try
      {
        return parse();
      }
      catch (Exception ex)
      {
        PrintError(ex);
      }
    }

    throw new Exception($"Maximum number of retries ({maxParseRetryCount}) exceeded while parsing the input");
  }
}

public static class DefaultInputs
{
  public const string UpperRightCoordinate = "5 5";
  public const string TotalNumberOfRovers = "2";
  public const string StartingPosition = "1 2 N";
  public const string MovementPlan = "LMLMLMLMM";
}
