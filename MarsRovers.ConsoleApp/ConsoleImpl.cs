namespace MarsRovers.ConsoleApp;

public class ConsoleImpl : IConsole
{
  public string Prompt(string text)
  {
    Console.Write(text);
    return Console.ReadLine() ?? string.Empty;
  }

  public void WriteLine(string line) => 
    Console.WriteLine(line);

  public void WriteError(Exception exception) => 
    Console.WriteLine($"Error: {exception.Message}");
}
