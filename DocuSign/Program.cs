using Domain.Interfaces;
using BL.Repositories;
using DAL;
using DocuSign.Api;
using DocuSign.Api.Middleware;
using DocuSign.Api.Controllers.Filters;

namespace DocuSign;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IURIRepository, URIRepository>();
        builder.Services.AddScoped<IStorage, Storage>();
        builder.Services.AddScoped<IUserStorageMapper, UserStorageMapper>();
        builder.Services.AddScoped<IURIStorageMapper, URIStorageMapper>();

        builder.Services.AddControllers(options => options.Filters.Add<ErrorHandlingFilterAttribute>());

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        //app.UseMiddleware<ErrorHandlingMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        //app.ConfigureExceptionHandler();

        app.MapControllers();

        app.Run();
    }
}
