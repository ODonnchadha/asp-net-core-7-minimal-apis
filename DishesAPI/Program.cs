using AutoMapper;
using DishesAPI.DbContexts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DishesDbContext>(o => o.UseSqlite(
    builder.Configuration["ConnectionStrings:ConnectionString"]));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
var app = builder.Build();
app.UseHttpsRedirection();

var endpoint = app.MapGroup("/dishes");
var endpointGuid = endpoint.MapGroup("/{id:guid}");
var endpointIngredients = endpointGuid.MapGroup("/ingredients");

#region MapDelete

endpointGuid.MapDelete("/dishes/{id:guid}", async Task<Results<NotFound, NoContent>> (
    Guid id, DishesDbContext context) =>
{
    var entity = await context.Dishes.FirstOrDefaultAsync(d => d.Id == id);

    if (entity == null)
    {
        return TypedResults.NotFound();
    }

    context.Dishes.Remove(entity);  
    await context.SaveChangesAsync();

    return TypedResults.NoContent();
});

#endregion

#region MapGet

endpoint.MapGet("", async Task<Ok<IEnumerable<DishesAPI.Models.Dish>>> 
    ([FromQuery] string ? name, DishesDbContext context, IMapper mapper) =>
{
    return TypedResults.Ok(mapper.Map<IEnumerable<DishesAPI.Models.Dish>>(
        await context.Dishes.Where(
            d => name == null || d.Name.Contains(name)).ToListAsync()));
});

endpointGuid.MapGet("", async Task<Results<NotFound, Ok< DishesAPI.Models.Dish>>> 
    (Guid id, DishesDbContext context, IMapper mapper) =>
{
    var entity = await context.Dishes.FirstOrDefaultAsync(d => d.Id == id);

    if (entity == null)
    {
        return TypedResults.NotFound();
    }

    return TypedResults.Ok(mapper.Map<DishesAPI.Models.Dish>(entity));
}).WithName("GetDish");

app.MapGet("/dishes/{name}", async Task<Results<NotFound, Ok<DishesAPI.Models.Dish>>> 
    (string name, DishesDbContext context, IMapper mapper) =>
{
    var entity = await context.Dishes.FirstOrDefaultAsync(d => d.Name == name);

    if (entity == null)
    {
        return TypedResults.NotFound();
    }

    return TypedResults.Ok(mapper.Map<DishesAPI.Models.Dish>(entity));
});

endpointIngredients.MapGet("", async Task<Results<NotFound, Ok<IEnumerable<DishesAPI.Models.Ingredient>>>>
    (Guid id, DishesDbContext context, IMapper mapper) =>
{
    var entity = await context.Dishes.FirstOrDefaultAsync(d => d.Id == id);

    if (entity == null)
    {
        return TypedResults.NotFound();
    }

    return TypedResults.Ok(mapper.Map<IEnumerable<DishesAPI.Models.Ingredient>>((
        await context.Dishes.Include(d => d.Ingredients).FirstOrDefaultAsync(
            d => d.Id == id))?.Ingredients));
});

#endregion

# region MapPost

endpoint.MapPost("", async Task<CreatedAtRoute<DishesAPI.Models.Dish>> (
        [FromBody]DishesAPI.Models.DishForCreation model, 
        DishesDbContext context, 
        // Microsoft.AspNetCore.Http.HttpContext http,
        // LinkGenerator generator,
        IMapper mapper) =>
{
    var entity = mapper.Map<DishesAPI.Entities.Dish>(model);
    
    context.Add(entity);
    await context.SaveChangesAsync();

    var dish = mapper.Map<DishesAPI.Models.Dish>(entity);

    // e.g.: CreatedAtRoute()
    return TypedResults.CreatedAtRoute(dish, "GetDish", new { id = dish.Id });

    // e.g.: LinkGenerator.
//    var link = generator.GetUriByName(http, "GetDish", new { id = dish.Id });
//#pragma warning disable CS8604 // Possible null reference argument.
//    return TypedResults.Created(link, dish);
//#pragma warning restore CS8604 // Possible null reference argument.
});

#endregion

# region MapPut

endpointGuid.MapPut("", async Task<Results<NotFound, NoContent>> (Guid id, 
    [FromBody] DishesAPI.Models.DishForUpdate model, DishesDbContext context, IMapper mapper) =>
    {
        var entity = await context.Dishes.FirstOrDefaultAsync(d => d.Id == id);

        if (entity == null)
        {
            return TypedResults.NotFound();
        }

        // Override values in the destinbation object by those in the source object.
        mapper.Map(model, entity);

        await context.SaveChangesAsync();

        return TypedResults.NoContent();
    });

# endregion

#pragma warning disable CS8602 // Dereference of a possibly null reference.
using (var scope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DishesDbContext>();
    context.Database.EnsureDeleted();
    context.Database.Migrate();
}
#pragma warning restore CS8602 // Dereference of a possibly null reference.

app.Run();