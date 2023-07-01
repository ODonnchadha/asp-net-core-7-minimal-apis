namespace DishesAPI.EndpointHandlers
{
    using AutoMapper;
    using DishesAPI.DbContexts;
    using Microsoft.AspNetCore.Http.HttpResults;
    using Microsoft.EntityFrameworkCore;

    public static class DishHandler
    {
        public static async Task<CreatedAtRoute<Models.Dish>> CreateDishAsync(
            Models.DishForCreation model, DishesDbContext context, IMapper mapper)
        {
            var entity = mapper.Map<Entities.Dish>(model);

            context.Add(entity);
            await context.SaveChangesAsync();

            var dish = mapper.Map<Models.Dish>(entity);

            return TypedResults.CreatedAtRoute(dish, "GetDish", new { id = dish.Id });
        }

        public static async Task<Results<NotFound, NoContent>> DeleteDishByIdAsync(
            Guid id, DishesDbContext context)
        {
            var entity = await context.Dishes.FirstOrDefaultAsync(d => d.Id == id);

            if (entity == null)
            {
                return TypedResults.NotFound();
            }

            context.Dishes.Remove(entity);  
            await context.SaveChangesAsync();

            return TypedResults.NoContent();
        }

        public static async Task<Ok<IEnumerable<Models.Dish>>> GetDishesAsync(
            string? name, DishesDbContext context, IMapper mapper)
        {
            return TypedResults.Ok(mapper.Map<IEnumerable<Models.Dish>>(
                await context.Dishes.Where(d => name == null || d.Name.Contains(
                    name)).ToListAsync()));
        }

        public static async Task<Results<NotFound, Ok<Models.Dish>>> GetDishByIdAsync(
            Guid id, DishesDbContext context, IMapper mapper)
        {
            var entity = await context.Dishes.FirstOrDefaultAsync(d => d.Id == id);

            if (entity == null)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(mapper.Map<Models.Dish>(entity));
        }

        public static async Task<Results<NotFound, Ok<Models.Dish>>> GetDishByNameAsync(
            string name, DishesDbContext context, IMapper mapper)
        {
            var entity = await context.Dishes.FirstOrDefaultAsync(d => d.Name == name);

            if (entity == null)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(mapper.Map<Models.Dish>(entity));
        }

        public static async Task<Results<NotFound, NoContent>> UpdateDishAsync(
            Guid id, Models.DishForUpdate model, DishesDbContext context, 
            ILogger<Models.Dish> logger, IMapper mapper)
        {
            var entity = await context.Dishes.FirstOrDefaultAsync(d => d.Id == id);

            if (entity == null)
            {
                logger.LogInformation($"Dish Id {id} was not found.");
                return TypedResults.NotFound();
            }

            // NOTE: Override values in the destinbation object by those in the source object.
            mapper.Map(model, entity);
            await context.SaveChangesAsync();

            return TypedResults.NoContent();
        }
    }
}
