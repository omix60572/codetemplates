using CodeTemplatesProject.Templates.BehaviorPatterns;

namespace CodeTemplatesProject;

public static class Program
{
    public static void Main()
    {
        // BehaviorPatterns:
        // ChainOfResponsibility
        // Command
        // Iterator

        // Define template to run
        var template = new Command();
        template.Run();
    }
}
