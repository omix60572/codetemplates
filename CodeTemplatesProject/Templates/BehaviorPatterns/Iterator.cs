using System.Collections;

namespace CodeTemplatesProject.Templates.BehaviorPatterns;

public class CustomSimpleIterator<T> : IEnumerator<T> where T : class
{
    private int index = 0;
    private T[] values;

    private int count = 0;

    public CustomSimpleIterator(int count)
    {
        Console.WriteLine($"Creating array with elements count: {count}");

        this.values = new T[count];
        this.count = count;
    }

    public T Current =>
        this.values != null ? this.values[this.index] : null;

    object IEnumerator.Current =>
        this.Current;

    public void Dispose()
    {
        this.values = null;
        this.index = 0;
        this.count = 0;
    }

    public bool MoveNext()
    {
        this.index++;
        return this.index < this.count;
    }

    public void Reset() =>
        this.index = 0;

    public void FillRandomValues(Func<T> getValue)
    {
        Console.WriteLine($"Filling random values, array elements count: {this.count}");

        for (var i = 0; i < this.count; i++)
            this.values[i] = getValue();
    }
}

public class CustomIteratorTestItem
{
    public string Value { get; set; }
}

public class Iterator : ICodeTemplate
{
    public void Run()
    {
        CustomIteratorTestItem getRandomValue(int min, int max)
        {
            var rnd = new Random();
            return new CustomIteratorTestItem { Value = rnd.Next(min, max).ToString() };
        }

        Console.WriteLine("Iterator template...");

        var rnd = new Random();
        var count = rnd.Next(1, 10);
        var collection = new CustomSimpleIterator<CustomIteratorTestItem>(count);

        collection.FillRandomValues(() => getRandomValue(0, 1000));

        var printingIndex = 1;
        Console.WriteLine("Printing array values:");
        do
        {
            Console.WriteLine($"{printingIndex}: {collection.Current.Value}");
            printingIndex++;
        } while (collection.MoveNext());
    }
}
