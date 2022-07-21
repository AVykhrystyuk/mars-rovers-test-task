namespace MarsRovers;

public static class StringExtensions
{
  public static string IfEmpty(this string? s, string @default) =>
    string.IsNullOrEmpty(s) ? @default : s;

  public static string IfWhiteSpace(this string? s, string @default) =>
    string.IsNullOrWhiteSpace(s) ? @default : s;
}
