namespace CodeTemplatesProject.Templates.FactoryPatterns;

public enum LogLevels
{
    Info,
    Warning,
    Error
}

public class LogMessage
{
    private readonly LogLevels logLevel;
    private readonly Dictionary<string, string> properties;
    private readonly string message;

    public LogMessage(LogLevels level, Dictionary<string, string> properties, string message)
    {
        this.logLevel = level;
        this.message = message;
        this.properties = properties;
    }

    public void Write()
    {
        var propertiesStr = string.Join('\n', this.properties.Select(x => $"[{x.Key}]: {x.Value}"));
        Console.WriteLine($"Log level: [{this.logLevel}]\n{propertiesStr}\nMessage text:{this.message}");
    }
}

public class LogBuilder
{
    private readonly Dictionary<string, string> properties = new();
    private LogLevels logLevel;
    private string message;

    public LogBuilder WithProperty(string key, string value)
    {
        if (this.properties.ContainsKey(key))
            throw new ArgumentException("Dublicated key provided");

        this.properties[key] = value;
        return this;
    }

    public LogBuilder WithLogLevel(LogLevels level)
    {
        this.logLevel = level;
        return this;
    }

    public LogBuilder WithMessage(string message)
    {
        this.message = message;
        return this;
    }

    public LogMessage Build()
    {
        return new LogMessage(this.logLevel, this.properties, this.message);
    }
}

public class Builder : ICodeTemplate
{
    public void Run()
    {
        Console.WriteLine("Builder code template example, log message builder\n");

        var logBuidler = new LogBuilder();
        logBuidler
            .WithProperty("date", DateTime.Now.ToShortDateString())
            .WithProperty("sender", nameof(Builder))
            .WithLogLevel(LogLevels.Info)
            .WithMessage("Hello world!")
            .Build()
            .Write();
    }
}
