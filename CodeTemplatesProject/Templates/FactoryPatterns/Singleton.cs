namespace CodeTemplatesProject.Templates.FactoryPatterns;

public class DisposableSingleton : IDisposable
{
    private bool disposed = false;

    private DisposableSingleton() =>
        Console.WriteLine("DisposableSingleton created");

    public static readonly Lazy<DisposableSingleton> Instance =
        new Lazy<DisposableSingleton>(() => new DisposableSingleton(), isThreadSafe: true);

    public void DoJob(string taskName)
    {
        if (this.disposed)
            throw new ObjectDisposedException(nameof(DisposableSingleton));

        Console.WriteLine($"Doing some job! [{taskName}]");
    }

    public void Dispose()
    {
        if (!this.disposed)
        {
            Console.WriteLine("Disposed");
            this.disposed = true;
        }
    }
}

public class Singleton : ICodeTemplate
{
    public void Run()
    {
        Console.WriteLine("Singleton pattern example");

        var task1 = new Task(() =>
        {
            Console.WriteLine("Task1 started");
            var singleton = DisposableSingleton.Instance.Value;
            singleton.DoJob("Task1");
            Console.WriteLine("Task1 ended");
        });

        var task2 = new Task(() =>
        {
            Console.WriteLine("Task2 started");
            var singleton2 = DisposableSingleton.Instance.Value;
            singleton2.DoJob("Task2");
            Console.WriteLine("Task2 ended");
        });

        task1.Start();
        task2.Start();
        Task.WhenAll(task1, task2)
            .GetAwaiter()
            .GetResult();

        DisposableSingleton.Instance.Value.Dispose();
    }
}
