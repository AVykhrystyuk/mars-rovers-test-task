namespace MarsRovers.Domain.Tests;

public class RoverSimulatorTests
{
  [Fact]
  public void TurnsCorrectly()
  {
    var startAt = new Location(
      new Point(0, 0),
      CardinalDirection.North,
      Plateau.From(upperRightCoordinate: new Point(1, 1)));

    var rover = new RoverSimulator("name", startAt);

    var position1 = rover.Run(new[] { RoverCommand.TurnLeft });

    Assert.Equal(CardinalDirection.West, position1.Direction);
    Assert.Equal(startAt.Point, position1.Point);

    var position2 = rover.Run(new[] { RoverCommand.TurnLeft, RoverCommand.TurnLeft });

    Assert.Equal(CardinalDirection.East, position2.Direction);
    Assert.Equal(startAt.Point, position2.Point);

    var position3 = rover.Run(new[] { RoverCommand.TurnRight });

    Assert.Equal(CardinalDirection.South, position3.Direction);
    Assert.Equal(startAt.Point, position3.Point);

    var position4 = rover.Run(new[] { RoverCommand.TurnRight, RoverCommand.TurnRight });

    Assert.Equal(CardinalDirection.North, position4.Direction);
    Assert.Equal(startAt.Point, position4.Point);
  }

  [Fact]
  public void MovesCorrectlyY()
  {
    var startAt = new Location(
      new Point(0, 0),
      CardinalDirection.North,
      Plateau.From(upperRightCoordinate: new Point(5, 5)));

    var rover = new RoverSimulator("name", startAt);

    var position1 = rover.Run(new[] { RoverCommand.MoveForward, RoverCommand.MoveForward });

    Assert.Equal(CardinalDirection.North, position1.Direction);
    Assert.Equal(new Point(0, 2), position1.Point);

    var position2 = rover.Run(new[] { RoverCommand.MoveForward, RoverCommand.MoveForward, RoverCommand.MoveForward });

    Assert.Equal(CardinalDirection.North, position2.Direction);
    Assert.Equal(new Point(0, 5), position2.Point);

    var position3 = rover.Run(new[] { RoverCommand.TurnRight, RoverCommand.TurnRight, RoverCommand.MoveForward });

    Assert.Equal(CardinalDirection.South, position3.Direction);
    Assert.Equal(new Point(0, 4), position3.Point);
  }

  [Fact]
  public void MovesCorrectlyX()
  {
    var startAt = new Location(
      new Point(0, 0),
      CardinalDirection.East,
      Plateau.From(upperRightCoordinate: new Point(5, 5)));

    var rover = new RoverSimulator("name", startAt);

    var position1 = rover.Run(new[] { RoverCommand.MoveForward, RoverCommand.MoveForward });

    Assert.Equal(CardinalDirection.East, position1.Direction);
    Assert.Equal(new Point(2, 0), position1.Point);

    var position2 = rover.Run(new[] { RoverCommand.MoveForward, RoverCommand.MoveForward });

    Assert.Equal(CardinalDirection.East, position2.Direction);
    Assert.Equal(new Point(4, 0), position2.Point);

    var position3 = rover.Run(new[] { RoverCommand.TurnLeft, RoverCommand.TurnLeft, RoverCommand.MoveForward });

    Assert.Equal(CardinalDirection.West, position3.Direction);
    Assert.Equal(new Point(3, 0), position3.Point);
  }

  [Fact]
  public void MoveBeyondPlateauThrows()
  {
    var startAt = new Location(
      new Point(0, 0),
      CardinalDirection.South,
      Plateau.From(upperRightCoordinate: new Point(2, 2)));

    var rover = new RoverSimulator("name", startAt);

    var exception = Assert.Throws<ArgumentException>(() => rover.Run(new[] { RoverCommand.MoveForward }));

    Assert.StartsWith("Cannot move beyond the plateau", exception.Message);
  }
}
