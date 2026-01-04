using CodeTemplatesProject.Templates.BehaviorPatterns;

namespace CodeTemplatesProject;

public static class Program
{
    public static void Main()
    {
        // BehaviorPatterns:
        // ChainOfResponsibility
        // Iterator

        // Define template to run
        var template = new Iterator();
        template.Run();
    }
}
