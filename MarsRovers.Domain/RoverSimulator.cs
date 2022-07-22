namespace MarsRovers.Domain;

public class RoverSimulator
{
  private Location _position;

  public string Name { get; }

  public RoverSimulator(string roverName, Location startAt)
  {
    Name = roverName;
    _position = startAt.Clone();
  }

  public Location Run(IReadOnlyList<RoverCommand> movementPlan)
  {
    var newPosition = _position.Clone();
    SimulateMovements(newPosition, movementPlan);
    _position = newPosition; // newPosition is validated at this point
    return _position.Clone();
  }

  private static void SimulateMovements(Location position, IReadOnlyList<RoverCommand> movementPlan)
  {
    foreach (var command in movementPlan)
    {
      ExecuteCommand(position, command);
    }
  }

  private static void ExecuteCommand(Location position, RoverCommand command)
  {
    switch (command)
    {
      case RoverCommand.TurnLeft:
        position.TurnLeft();
        break;
      case RoverCommand.TurnRight:
        position.TurnRight();
        break;
      case RoverCommand.MoveForward:
        position.MoveForward();
        break;
      default:
        throw new NotImplementedException($"Unknown rover command ('{command}')");
    }
  }
}
