using DocuSign.Interfaces;
using BL.Repositories;
using DAL;
using DAL.Intefaces;
using Serilog;
using DocuSign.Middleware;


namespace DocuSign;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddSingleton<IUserRepository, UserRepository>();
        builder.Services.AddSingleton<IURIRepository, URIRepository>();
        builder.Services.AddSingleton<IStorage, Storage>();
        builder.Services.AddSingleton<IUserStorageMapper, UserStorageMapper>();
        builder.Services.AddSingleton<IURIStorage, URIStorage>();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Host.UseSerilog((context, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);
        });

        var app = builder.Build();

        app.UseMiddleware<ExceptionMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
