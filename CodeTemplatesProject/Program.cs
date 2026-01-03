using CodeTemplatesProject.Templates.BehaviorPatterns;

namespace CodeTemplatesProject;

public static class Program
{
    public static void Main(string[] args)
    {
        // Define template to run
        var template = new ChainOfResponsibility();
        template.Run();
    }
}
