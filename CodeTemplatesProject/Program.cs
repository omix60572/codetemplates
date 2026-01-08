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
        // State

        // Define template to run
        var template = new State();
        template.Run();
    }
}
