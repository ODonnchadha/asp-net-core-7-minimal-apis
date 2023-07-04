using DishesAPI.DbContexts;
using DishesAPI.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<DishesDbContext>(o => o.UseSqlite(
    builder.Configuration["ConnectionStrings:X"]));
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();
builder.Services.AddAuthorizationBuilder().AddPolicy(
    "DULUTH_AUTHORIZATION_POLICY", policy =>
{
    policy.RequireRole("admin").RequireClaim("city", "Duluth");
});
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
app.UseAuthentication();
app.UseAuthorization();
app.RegisterDishEndpoints();
app.RegisterIngredientEndpoints();

#pragma warning disable CS8602 // Dereference of a possibly null reference.
using (var scope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DishesDbContext>();
    context.Database.EnsureDeleted();
    context.Database.Migrate();
}
#pragma warning restore CS8602 // Dereference of a possibly null reference.

app.Run();