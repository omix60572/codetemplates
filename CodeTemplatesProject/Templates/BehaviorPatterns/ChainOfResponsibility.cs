namespace CodeTemplatesProject.Templates.BehaviorPatterns;

public abstract class Middleware
{
    protected Middleware next;

    public abstract bool Invoke(UserRequest request);

    protected bool InvokeNext(UserRequest request)
    {
        if (next != null)
            return this.next.Invoke(request);

        // Calls chain finished
        return true;
    }
}

public class AuthMiddleware : Middleware
{
    public AuthMiddleware(Middleware next) => this.next = next;

    public override bool Invoke(UserRequest request)
    {
        Console.WriteLine("Auth middleware processing request");

        if (string.IsNullOrEmpty(request.AuthCookie))
        {
            Console.WriteLine("Error, secret auth cookie is missing");
            return false;
        }

        Console.WriteLine("No auth errors found");
        return this.InvokeNext(request);
    }
}

public class RequestValidationMiddleware : Middleware
{
    public RequestValidationMiddleware(Middleware next) => this.next = next;

    public override bool Invoke(UserRequest request)
    {
        Console.WriteLine("Request validation middleware processing request");

        if (string.IsNullOrEmpty(request.Content))
        {
            Console.WriteLine("Error, request content is missing");
            return false;
        }

        Console.WriteLine("No validation errors found");
        return this.InvokeNext(request);
    }
}

public class UserRequest
{
    public string Content { get; set; }
    public string AuthCookie { get; set; }
}

public class TestApiController
{
    public void ApiCall(Middleware middleware, UserRequest request)
    {
        Console.WriteLine($"Start handle of user request: {request.Content}");

        if (middleware.Invoke(request))
            Console.WriteLine("Request completed succesfully");
        else
            Console.WriteLine("Some errors found");

        Console.Write("\n\n");
    }
}


public class ChainOfResponsibility : ICodeTemplate
{
    private readonly TestApiController controller;
    private readonly Middleware middleware;

    public ChainOfResponsibility()
    {
        this.controller = new TestApiController();
        this.middleware = new AuthMiddleware(new RequestValidationMiddleware(null));
    }

    private void MakeApiCall(UserRequest request) =>
        this.controller.ApiCall(this.middleware, request);

    public void Run()
    {
        var request1 = new UserRequest
        {
            AuthCookie = "secret_auth_cookie",
            Content = "Correct user api request"
        };
        var request2 = new UserRequest
        {
            AuthCookie = null,
            Content = "INCORRECT user api request"
        };

        var request3 = new UserRequest
        {
            AuthCookie = "secret_auth_cookie",
            Content = null
        };

        this.MakeApiCall(request1);
        this.MakeApiCall(request2);
        this.MakeApiCall(request3);
    }
}
