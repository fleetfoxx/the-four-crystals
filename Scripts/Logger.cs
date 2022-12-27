using System;
using System.Diagnostics;

public class Logger
{
  private Type _owner;

  public Logger(Type owner)
  {
    _owner = owner;
  }

  public void Log(string message)
  {
    Debug.WriteLine($"{DateTime.UtcNow.ToString()} [{_owner.Name}] {message}");
  }
}

public static class LoggerFactory
{
  public static Logger CreateLogger(Type owner)
  {
    return new Logger(owner);
  }
}