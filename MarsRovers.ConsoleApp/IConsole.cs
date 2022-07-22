namespace MarsRovers.ConsoleApp;

public interface IConsole
{
  string Prompt(string text);
  void WriteLine(string line);
  void WriteError(Exception exception);
}
