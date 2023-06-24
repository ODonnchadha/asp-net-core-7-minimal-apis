using AutoMapper;
using DishesAPI.DbContexts;
using DishesAPI.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DishesDbContext>(o => o.UseSqlite(
    builder.Configuration["ConnectionStrings:ConnectionString"]));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

app.UseHttpsRedirection();
app.RegisterDishEndpoints();
app.RegisterIngredientEndpoints();

app.MapPut("dishes/{id:guid}", async Task<Results<NotFound, NoContent>> (Guid id, 
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

#pragma warning disable CS8602 // Dereference of a possibly null reference.
using (var scope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DishesDbContext>();
    context.Database.EnsureDeleted();
    context.Database.Migrate();
}
#pragma warning restore CS8602 // Dereference of a possibly null reference.

app.Run();