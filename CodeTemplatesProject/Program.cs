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
        // Builder
        // Singleton

        // Define template to run
        var template = new Singleton();
        template.Run();
    }
}
