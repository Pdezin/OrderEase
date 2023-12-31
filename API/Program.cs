using Domain.Workflows;
using Infrastructure.Contracts.UoW;
using Infrastructure.Data;
using Infrastructure.UoW;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Versioning;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

// Data Context Service
builder.Services.AddDbContext<DataContext>();

// Add services to the container.
// UnitOfWork Service
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Workflows
builder.Services.AddScoped(typeof(CategoriesWorkflow));
builder.Services.AddScoped(typeof(RolesWorkflow));
builder.Services.AddScoped(typeof(PriceListsWorkflow));
builder.Services.AddScoped(typeof(UsersWorkflow));
builder.Services.AddScoped(typeof(ProductsWorkflow));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = new UrlSegmentApiVersionReader();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        if (!app.Environment.IsDevelopment())
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = Text.Plain;

            await context.Response.WriteAsync("An error occurred.");
            return;
        }
        
        var exception = context.Features.Get<IExceptionHandlerFeature>();

        string erro = $"StatusCode: {context.Response.StatusCode}\n";
        string additionalInfo = "";

        if (exception != null)
        {
            erro += exception?.Error.Message ?? "";
            additionalInfo = exception?.Error.StackTrace ?? "";
        }

        await context.Response.WriteAsync($"{erro}\n{additionalInfo}");
    });
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
