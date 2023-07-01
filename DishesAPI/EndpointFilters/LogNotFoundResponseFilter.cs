using System.Net;

namespace DishesAPI.EndpointFilters
{
    /// <summary>
    /// A filter can resolve constructed dependencies, but it, itself, cannot be resolved as such.
    /// </summary>
    public class LogNotFoundResponseFilter : IEndpointFilter
    {
        private readonly ILogger logger;
        public LogNotFoundResponseFilter(ILogger logger) => this.logger = logger;
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var result = await next.Invoke(context);

            var actual = (result is INestedHttpResult) ? 
                ((INestedHttpResult)result).Result : (IResult)result;

            if ((actual as IStatusCodeHttpResult)?.StatusCode == (int)HttpStatusCode.NotFound)
            {
                logger.LogInformation($"Resource {context.HttpContext.Request.Path} was not found.");
            }

            return result;
        }
    }
}
