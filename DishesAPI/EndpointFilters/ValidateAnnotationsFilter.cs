namespace DishesAPI.EndpointFilters
{
    using DishesAPI.Models;
    using MiniValidation;

    public class ValidateAnnotationsFilter : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(
            EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            //var model = context.GetArgument<DishForCreation>(2);
            //if (!MiniValidator.TryValidate(model, out var errors))
            //{
            //    return TypedResults.ValidationProblem(errors);
            //}

            return await next(context);
        }
    }
}
