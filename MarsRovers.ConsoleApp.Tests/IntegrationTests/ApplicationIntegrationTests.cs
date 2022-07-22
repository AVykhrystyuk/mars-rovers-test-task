namespace MarsRovers.ConsoleApp.Tests.IntegrationTests;

public class ApplicationIntegrationTests
{
  [Fact]
  public void SuppliedSample()
  {
    // Arrange
    var mockConsole = new Mock<IConsole>();
    SetupUpperRightPlateauCoordinate(mockConsole, "5 5");
    SetupNumberOfRoversDeployed(mockConsole, "2");
    SetupRover(mockConsole, number: 1, startAt: "1 2 N", movements: "LMLMLMLMM");
    SetupRover(mockConsole, number: 2, startAt: "3 3 E", movements: "MMRMMRMRRM");

    var app = new Application(mockConsole.Object);

    // Act
    app.Run();

    // Assert
    mockConsole.Verify(c => c.WriteLine("Rover 1 Output: 1 3 N"), Times.Once());
    mockConsole.Verify(c => c.WriteLine("Rover 2 Output: 5 1 E"), Times.Once());
  }

  [Fact]
  public void DefaultValuesAreUsed()
  {
    // Arrange
    var mockConsole = new Mock<IConsole>();
    SetupUpperRightPlateauCoordinate(mockConsole, "");
    SetupNumberOfRoversDeployed(mockConsole, "");
    SetupRover(mockConsole, number: 1, startAt: "", movements: "");
    SetupRover(mockConsole, number: 2, startAt: "3 3 E", movements: "MMRMMRMRRM");

    var app = new Application(mockConsole.Object);

    // Act
    app.Run();

    // Assert
    mockConsole.Verify(c => c.WriteLine("Rover 1 Output: 1 3 N"), Times.Once());
    mockConsole.Verify(c => c.WriteLine("Rover 2 Output: 5 1 E"), Times.Once());
  }

  [Fact]
  public void MoveCollisionIsHandled()
  {
    // Arrange
    var mockConsole = new Mock<IConsole>();
    SetupUpperRightPlateauCoordinate(mockConsole, "5 5");
    SetupNumberOfRoversDeployed(mockConsole, "2");
    SetupRover(mockConsole, number: 1, startAt: "1 2 N", movements: "LMLMLMLMM");
    SetupRover(mockConsole, number: 2, startAt: "1 2 N", movements: "LMLMLMLMM");

    var app = new Application(mockConsole.Object);

    // Act
    app.Run();

    // Assert
    mockConsole.Verify(c => c.WriteLine("Rover 1 Output: 1 3 N"), Times.Once());
    mockConsole.Verify(c => c.WriteError(It.Is<Exception>(ex => ex.Message.StartsWith("Cannot move to"))), Times.Once());
  }

  [Fact]
  public void StartAtCollisionIsHandled()
  {
    // Arrange
    var mockConsole = new Mock<IConsole>();
    SetupUpperRightPlateauCoordinate(mockConsole, "5 5");
    SetupNumberOfRoversDeployed(mockConsole, "2");
    SetupRover(mockConsole, number: 1, startAt: "1 2 N", movements: "L");
    SetupRover(mockConsole, number: 2, startAt: "1 2 N", movements: "L");

    var app = new Application(mockConsole.Object, maxParseRetryCount: 0);

    // Act
    var exception = Assert.ThrowsAny<Exception>(() => app.Run());

    // Assert
    Assert.StartsWith("Maximum number of retries", exception.Message);
    mockConsole.Verify(c => c.WriteError(It.Is<Exception>(ex => ex.Message.StartsWith("Cannot start at"))), Times.Once());
  }

  [Fact]
  public void RetriesTypos()
  {
    // Arrange
    var mockConsole = new Mock<IConsole>();
    SetupUpperRightPlateauCoordinate(mockConsole, "5 5");
    SetupNumberOfRoversDeployed(mockConsole, "2");
    SetupSequenceRover(mockConsole, number: 1, startAtArray: new[] { "TYPO1", "1 2 N" }, movementsArray: new[] { "TYPO2", "M" });
    SetupSequenceRover(mockConsole, number: 2, startAtArray: new[] { "TYPO1", "2 0 N" }, movementsArray: new[] { "TYPO2", "M" });

    var app = new Application(mockConsole.Object, maxParseRetryCount: 1);

    // Act
    app.Run();

    // Assert
    mockConsole.Verify(c => c.WriteLine("Rover 1 Output: 1 3 N"), Times.Once());
    mockConsole.Verify(c => c.WriteLine("Rover 2 Output: 2 1 N"), Times.Once());
    mockConsole.Verify(c => c.WriteError(It.Is<Exception>(ex => ex.Message.StartsWith("Input string was not in a correct format"))), Times.Exactly(2));
    mockConsole.Verify(c => c.WriteError(It.Is<Exception>(ex => ex.Message.EndsWith("is unsupported movement plan command. Please use the supported commands (L, R, M)."))), Times.Exactly(2));
  }

  [Theory]
  [InlineData("5 5", "M")]
  [InlineData("1 1", "LLMM")]
  public void MoveBeyondPlateauAreHandled(string position, string movements)
  {
    // Arrange
    var mockConsole = new Mock<IConsole>();
    SetupUpperRightPlateauCoordinate(mockConsole, position);
    SetupNumberOfRoversDeployed(mockConsole, "1");
    SetupRover(mockConsole, number: 1, startAt: $"{position} N", movements);

    var app = new Application(mockConsole.Object);

    // Act
    app.Run();

    // Assert
    mockConsole.Verify(c => c.WriteError(It.Is<Exception>(ex => ex.Message.StartsWith("Cannot move beyond the plateau"))), Times.Once());
  }

  private static void SetupUpperRightPlateauCoordinate(Mock<IConsole> mockConsole, string coordinate)
  {
    mockConsole
      .Setup(c => c.Prompt(It.Is<string>(s => s.StartsWith("Enter Plateau Upper Right Coordinate"))))
      .Returns(coordinate);
  }

  private static void SetupNumberOfRoversDeployed(Mock<IConsole> mockConsole, string number)
  {
    mockConsole
      .Setup(c => c.Prompt(It.Is<string>(s => s.StartsWith("Enter Number Of Rovers Deployed"))))
      .Returns(number);
  }

  private static void SetupRover(Mock<IConsole> mockConsole, int number, string startAt, string movements)
  {
    mockConsole
      .Setup(c => c.Prompt(It.Is<string>(s => s.StartsWith($"Rover {number} Starting Position"))))
      .Returns(startAt);

    mockConsole
      .Setup(c => c.Prompt(It.Is<string>(s => s.StartsWith($"Rover {number} Movement Plan"))))
      .Returns(movements);
  }

  private static void SetupSequenceRover(Mock<IConsole> mockConsole, int number, string[] startAtArray, string[] movementsArray)
  {
    var startAtSetupSequence = mockConsole.SetupSequence(c => c.Prompt(It.Is<string>(s => s.StartsWith($"Rover {number} Starting Position"))));
    foreach (var startAt in startAtArray)
      startAtSetupSequence = startAtSetupSequence.Returns(startAt);

    var movementsSetupSequence = mockConsole.SetupSequence(c => c.Prompt(It.Is<string>(s => s.StartsWith($"Rover {number} Movement Plan"))));
    foreach (var movements in movementsArray)
      movementsSetupSequence = movementsSetupSequence.Returns(movements);
  }
}
