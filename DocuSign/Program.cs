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
        builder.Services.AddSingleton<IURIStorageMapper, URIStorageMapper>();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Host.UseSerilog((context, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);
        });

        var app = builder.Build();

        app.UseMiddleware<ExceptionMiddleware>();

        //app.UseExceptionHandler(exceptionHandlerApp =>
        //{
        //    exceptionHandlerApp.Run(async context =>
        //    {
        //        var exception = context.Features.Get<IExceptionHandlerPathFeature>();

        //        context.Response.ContentType = "application/json";
        //        var result = JsonSerializer.Serialize(new { error = exception.Error.Message });

        //        context.Response.StatusCode = exception.Error switch
        //        {
        //            NotFoundException => (int)HttpStatusCode.NotFound,
        //            AlreadyExistException => (int)HttpStatusCode.Conflict,
        //            InvalidException => (int)HttpStatusCode.BadRequest,
        //            _ => (int)HttpStatusCode.InternalServerError,
        //        };
        //        await context.Response.WriteAsync(result);
        //    });
        //});

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        //app.UseExceptionHandler("/api/error");

        app.Run();
    }
}
