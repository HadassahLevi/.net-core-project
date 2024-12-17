
using System.Diagnostics;

namespace FirstMiddlewares;
public class FirstMiddleware
{
    private RequestDelegate next;

        public FirstMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext c)
    {
        await c.Response.WriteAsync($"Our Log Middleware start\n");
        var sw = new Stopwatch();
        sw.Start();
        await next(c);
        Console.WriteLine($"{c.Request.Path}.{c.Request.Method} took {sw.ElapsedMilliseconds}ms."
            + $" User: {c.User?.FindFirst("userId")?.Value ?? "unknown"}");
        await c.Response.WriteAsync("Our Log Middleware end\n");
    }

}

public static class FirstMiddlewareHelper
{
    public static void UseFirstMiddleware(
        this IApplicationBuilder a)
    {
        a.UseMiddleware<FirstMiddleware>();
    }
}