namespace CodeTemplatesProject.Templates.FactoryPatterns;

public class ImageStub
{
    public string Name { get; set; }
    public string Extension { get; set; }
    public string Content { get; private set; }

    public ImageStub(string path)
    {
        if (!path.Contains('.') && !path.Contains('/'))
            throw new ArgumentException("File name/path is invalid");

        var lastPartStart = path.Trim('/').LastIndexOf('/') + 1;
        var fileNameParts = path.Substring(lastPartStart).Split('.');

        this.Name = fileNameParts[0];
        this.Extension = fileNameParts[1];
    }

    public bool LoadFile()
    {
        var rnd = new Random();
        this.Content = $"[Some image file stub content is here: {rnd.Next()}]";
        return true;
    }
}

public abstract class Page
{
    public string Title { get; }

    protected Page(string title) =>
        this.Title = title;

    public abstract void Render();
}

public abstract class Document
{
    private readonly List<Page> pages = new();

    protected abstract Page CreatePage(string title, string content);

    protected abstract Page CreatePage(ImageStub image);

    public void AddTextPage(string title, string text) =>
        this.pages.Add(CreatePage(title, text));

    public void AddImagePage(string filePath)
    {
        var file = new ImageStub(filePath);
        this.pages.Add(CreatePage(file));
    }

    public void Render()
    {
        Console.WriteLine($"----- {GetType().Name} -----");

        foreach (var page in this.pages)
            page.Render();

        Console.WriteLine("-------------------------\n");
    }
}

public class TextPage : Page
{
    public string Content { get; }

    public TextPage(string title) : base(title) { }

    public TextPage(string title, string content) : base(title) =>
        this.Content = content;

    public override void Render()
    {
        Console.WriteLine($"=== {Title} ===\n\n");
        Console.WriteLine(Content);
        Console.WriteLine("\n\n=== --- ===");
    }
}

public class ImagePage : Page
{
    private readonly ImageStub image;

    public ImagePage(string title) : base(title) { }

    public ImagePage(ImageStub image) : base(image.Name) =>
        this.image = image;

    public override void Render()
    {
        this.image.LoadFile();

        Console.WriteLine($"##### {this.image.Name} #####");
        Console.WriteLine($"#\tContent of file {this.image.Name}.{this.image.Extension}");
        Console.WriteLine($"#\t{this.image.Content}");
        Console.WriteLine("#########################");
    }
}

public class WordDocument : Document
{
    protected override Page CreatePage(string title, string content) =>
        new TextPage(title, content);

    protected override Page CreatePage(ImageStub image) =>
        new ImagePage(image);
}

public enum DocumentTypes
{
    Word
}

public static class DocumentFactory
{
    public static Document CreateDocument(DocumentTypes type) =>
        type switch
        {
            DocumentTypes.Word => new WordDocument(),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unsupported document type")
        };
}

public class Factory : ICodeTemplate
{
    private readonly string loremIpsum;

    public Factory() =>
        this.loremIpsum =
        """
            Lorem ipsum dolor sit amet, consectetur adipiscing elit.
            Donec eu iaculis arcu. Proin at purus pulvinar, ultricies velit ac, facilisis erat.
            Sed ultrices viverra ultrices. Etiam vulputate porta enim, nec efficitur mi auctor eleifend.
            Pellentesque viverra est purus, ac hendrerit urna sollicitudin vitae.
            Morbi at convallis odio. Integer tellus mi, feugiat vel viverra vel, fermentum non sem.
            Sed mattis semper metus vitae viverra. Nullam id nisl turpis.
            Proin eu diam quis libero cursus tincidunt nec cursus erat. 
            Cras dui nunc, volutpat id quam in, blandit semper tortor. Nulla facilisi.
        """;

    public void Run()
    {
        Console.WriteLine("Factory pattern example, document creation\n\n");
        Console.WriteLine("Creation of word document\n\n");

        var wordDocument = DocumentFactory.CreateDocument(DocumentTypes.Word);
        wordDocument.AddTextPage("Title page", "Hello world! This is a word document");
        wordDocument.AddImagePage("disk1/folder1/documentImage.jpeg");
        wordDocument.AddTextPage("Lorem ipsum", this.loremIpsum);
        wordDocument.AddTextPage("Conclusion", "Document was created successfully");
        wordDocument.Render();

        Console.ReadLine();
    }
}
