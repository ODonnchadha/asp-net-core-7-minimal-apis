namespace DishesAPI.EndpointHandlers
{
    using AutoMapper;
    using DishesAPI.DbContexts;
    using Microsoft.AspNetCore.Http.HttpResults;
    using Microsoft.EntityFrameworkCore;

    public static class IngredientHandler
    {
        public static async Task<Results<NotFound, Ok<IEnumerable<Models.Ingredient>>>> GetIngredientsAsync(
            Guid id, DishesDbContext context, IMapper mapper)
        {
            var entity = await context.Dishes.FirstOrDefaultAsync(d => d.Id == id);

            if (entity == null)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(mapper.Map<IEnumerable<DishesAPI.Models.Ingredient>>((
                await context.Dishes.Include(d => d.Ingredients).FirstOrDefaultAsync(
                    d => d.Id == id))?.Ingredients));
        }
    }
}
