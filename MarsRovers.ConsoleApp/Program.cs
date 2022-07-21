using Microsoft.Extensions.DependencyInjection;

namespace MarsRovers.ConsoleApp;

internal class Program
{
  static void Main(string[] args)
  {
    var services = new ServiceCollection()
      .AddSingleton<Application>();
    using var serviceProvider = ConfigureServices(services)
      .BuildServiceProvider();

    var app = serviceProvider.GetRequiredService<Application>();
    app.Run();
  }

  private static IServiceCollection ConfigureServices(IServiceCollection services) => 
    services
      .AddTransient<IService, ServiceImpl>();
}

[Obsolete]
public interface IService { }

[Obsolete]
public class ServiceImpl : IService { }
