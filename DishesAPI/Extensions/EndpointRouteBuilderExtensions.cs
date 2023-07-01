namespace DishesAPI.Extensions
{
    using DishesAPI.EndpointFilters;
    using DishesAPI.EndpointHandlers;
    public static class EndpointRouteBuilderExtensions
    {
        public static void RegisterDishEndpoints(this IEndpointRouteBuilder builder)
        {
            var endpoint = builder.MapGroup("/dishes");
            var FISH_MASALA = new Guid("98929bd4-f099-41eb-a994-f1918b724b5a");

            endpoint.MapDelete("/{id:guid}", DishHandler.DeleteDishByIdAsync)
                .AddEndpointFilter(new DishIsLockedFilter(new(FISH_MASALA.ToString())))
                .AddEndpointFilter<LogNotFoundResponseFilter>();

            endpoint.MapGet("", DishHandler.GetDishesAsync);
            endpoint.MapGet("/{id:guid}", DishHandler.GetDishByIdAsync).WithName("GetDish");
            endpoint.MapGet("/dishes/{name}", DishHandler.GetDishByNameAsync);

            endpoint.MapPost("", DishHandler.CreateDishAsync)
                .AddEndpointFilter<ValidateAnnotationsFilter>(); ;

            endpoint.MapPut("/{id:guid}", DishHandler.UpdateDishAsync)
                .AddEndpointFilter(new DishIsLockedFilter(new(FISH_MASALA.ToString())));
            //.AddEndpointFilter(async (context, next) =>
            //{
            //    var id = context.GetArgument<Guid>(2);
            //    var FISH_MASALA = new Guid("98929bd4-f099-41eb-a994-f1918b724b5a");

            //    if (id == FISH_MASALA)
            //    {
            //        return TypedResults.Problem(new()
            //        {
            //            Status = 418,
            //            Title = "I'm a teapot",
            //            Detail = "Cannot update Fish Masala. Fish Masala is perfect"
            //        });
            //    }

            //    // Invoke the next filter.
            //    var result = await next.Invoke(context);

            //    return result;
            //});
        }

        public static void RegisterIngredientEndpoints(this IEndpointRouteBuilder builder)
        {
            var endpoint = builder.MapGroup("/dishes");

            endpoint.MapGet("/{id:guid}/ingredients", IngredientHandler.GetIngredientsAsync);
        }
    }
}
