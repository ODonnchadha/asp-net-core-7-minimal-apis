using AutoMapper;
using DishesAPI.DbContexts;
using DishesAPI.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<DishesDbContext>(o => o.UseSqlite(
    builder.Configuration["ConnectionStrings:X"]));
builder.Services.AddProblemDetails();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();

    //app.UseExceptionHandler(builder =>
    //{
    //    builder.Run(async context =>
    //    {
    //        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
    //        context.Response.ContentType = "text/html";
    //        await context.Response.WriteAsync("An error has occured.");
    //    });
    //});
}

app.UseHttpsRedirection();
app.RegisterDishEndpoints();
app.RegisterIngredientEndpoints();

app.MapPut("dishes/{id:guid}", async Task<Results<NotFound, NoContent>> (Guid id, 
    [FromBody]DishesAPI.Models.DishForUpdate model, 
    DishesDbContext context, 
    ILogger<DishesAPI.Models.Dish> logger,
    IMapper mapper) =>
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