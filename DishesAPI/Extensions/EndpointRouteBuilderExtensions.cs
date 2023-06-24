namespace DishesAPI.Extensions
{
    using DishesAPI.EndpointHandlers;
    public static class EndpointRouteBuilderExtensions
    {
        public static void RegisterDishEndpoints(this IEndpointRouteBuilder builder)
        {
            var endpoint = builder.MapGroup("/dishes");

            endpoint.MapDelete("/{id:guid}", DishHandler.DeleteDishByIdAsync);

            endpoint.MapGet("", DishHandler.GetDishesAsync);
            endpoint.MapGet("/{id:guid}", DishHandler.GetDishByIdAsync).WithName("GetDish");
            endpoint.MapGet("/dishes/{name}", DishHandler.GetDishByNameAsync);

            endpoint.MapPost("", DishHandler.CreateDishAsync);
        }

        public static void RegisterIngredientEndpoints(this IEndpointRouteBuilder builder)
        {
            var endpoint = builder.MapGroup("/dishes");

            endpoint.MapGet("/{id:guid}/ingredients", IngredientHandler.GetIngredientsAsync);
        }
    }
}
