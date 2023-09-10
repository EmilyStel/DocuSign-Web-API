using Domain.Interfaces;
using BL.Repositories;
using DAL;
using DocuSign.Api.Controllers.Filters;
using DAL.Intefaces;
using Serilog;

namespace DocuSign;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddSingleton<IUserRepository, UserRepository>();
        builder.Services.AddSingleton<IURIRepository, URIRepository>();
        builder.Services.AddSingleton<IStorage, Storage>();
        builder.Services.AddSingleton<IUserStorageMapper, UserStorageMapper>();
        builder.Services.AddSingleton<IURIStorageMapper, URIStorageMapper>();

        builder.Services.AddControllers(options => options.Filters.Add<ErrorHandlingFilterAttribute>());

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Host.UseSerilog((context, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);
        });

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
