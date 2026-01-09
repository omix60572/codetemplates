using CodeTemplatesProject.Templates.FactoryPatterns;

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

        // FactoryPatterns:
        // Factory

        // Define template to run
        var template = new Factory();
        template.Run();
    }
}
