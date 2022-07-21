namespace MarsRovers.Domain;

public class RoverController 
{
  private readonly Location _startingPosition;

  public RoverController(Location startingPosition)
  {
    _startingPosition = startingPosition;
  }

  public Location Execute(IReadOnlyList<RoverCommand> movementPlan)
  {
    return _startingPosition; //TODO:
  }
}
