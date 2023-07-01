namespace DishesAPI.EndpointFilters
{
    /// <summary>
    /// Request filter.
    /// </summary>
    public class DishIsLockedFilter : IEndpointFilter
    {
        private readonly Guid locked;
        public DishIsLockedFilter(Guid locked) => this.locked = locked;

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            Guid id;
            if (context.HttpContext.Request.Method == "PUT")
            {
                id = context.GetArgument<Guid>(2);
            } 
            else if (context.HttpContext.Request.Method == "DELETE")
            {
                id = context.GetArgument<Guid>(1);
            }
            else
            {
                throw new NotSupportedException("HTTP verb is not supported for this request.");
            }

            if (id == locked)
            {
                return TypedResults.Problem(new()
                {
                    Status = 418,
                    Title = "I'm a teapot",
                    Detail = "You cannot update or delete this dish. This dish is perfect."
                });
            }

            // Invoke the next filter:
            return await next.Invoke(context);
        }
    }
}
