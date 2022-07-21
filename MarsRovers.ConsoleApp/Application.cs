namespace MarsRovers.ConsoleApp;

using MarsRovers.Domain;

using static DefaultInputs;
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
    var upperRightCoordinate = GetUpperRightCoordinate(); // TODO:
    var totalNumberOfRovers = GetTotalNumberOfRovers();

    for (int roverNumber = 1; roverNumber <= totalNumberOfRovers; roverNumber++)
    {
      var startingPosition = GetStartingPosition(roverNumber);
      var movementPlan = GetMovementPlan(roverNumber);

      var controller = new RoverController(startingPosition);

      try
      {
        var resultedPosition = controller.Execute(movementPlan);
        WriteLine($"Rover {roverNumber} Output: {PositionToString(resultedPosition)}");
      }
      catch (Exception ex)
      {
        WriteLine($"Rover {roverNumber} Error: {ex.Message}");
        return;
      }
    }
  }

  private List<RoverCommand> GetMovementPlan(int roverNumber) => TryParseInput(() =>
  {
    Write($"Rover {roverNumber} Movement Plan (Default: '{MovementPlan}'): ");
    return ParseMovementPlan(ReadLine().IfEmpty(MovementPlan));
  });

  private Location GetStartingPosition(int roverNumber) => TryParseInput(() =>
  {
    Write($"Rover {roverNumber} Starting Position (Default: '{StartingPosition}'): ");
    return ParseStartingPosition(ReadLine().IfEmpty(StartingPosition));
  });

  private int GetTotalNumberOfRovers() => TryParseInput(() =>
  {
    Write($"Enter Number Of Rovers Deployed (Default: '{TotalNumberOfRovers}'): ");
    return int.Parse(ReadLine().IfEmpty(TotalNumberOfRovers));
  });

  private Point GetUpperRightCoordinate() => TryParseInput(() =>
  {
    Write($"Enter Graph Upper Right Coordinate (Default: '{UpperRightCoordinate}'): ");
    return ParsePoint(ReadLine().IfEmpty(UpperRightCoordinate));
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
        WriteLine(ex.Message);
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
