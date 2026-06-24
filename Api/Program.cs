using Application.Common;
using Application.DTO;
using Application.XuLyMenu.UseCases;
using Domain.repositories;
using FluentValidation;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IMenuRepo, MenuRepository>();
builder.Services.AddScoped<INewsRepo, NewsRepository>();
builder.Services.AddScoped<IMenuNewsRepo, MenuNewsRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(GetAllMenuUseCase).Assembly));
builder.Services.AddControllers()
    .AddJsonOptions(o =>
        o.JsonSerializerOptions.ReferenceHandler 
            = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);
builder.Services.AddValidatorsFromAssembly(typeof(GetAllMenuUseCase).Assembly);
builder.Services.AddScoped<
    IWebsiteLocalizationWardRepo,
    WebsiteLocalizationWardRepository>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    
    app.UseSwaggerUI();
}
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (FluentValidation.ValidationException ex)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.Response.ContentType = "application/json";

        var errors = ex.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );

        await context.Response.WriteAsJsonAsync(new
        {
            message = "Dữ liệu không hợp lệ",
            errors
        });
    }
});
app.UseHttpsRedirection();


app.MapControllers(); 
app.Run();
