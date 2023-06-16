using DishesAPI.DbContexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DishesDbContext>(o => o.UseSqlite(
    builder.Configuration["ConnectionStrings:ConnectionString"]));

var app = builder.Build();
app.UseHttpsRedirection();

app.MapGet("/dishes", (DishesDbContext context) =>
{
    return context.Dishes;
});

using (var scope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DishesDbContext>();
    context.Database.EnsureDeleted();
    context.Database.Migrate();
}

app.Run();