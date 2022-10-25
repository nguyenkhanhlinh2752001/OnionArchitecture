using Application;
using Application.Features.CategoryFeatures.Commands.CreateCategoryCommand;
using FluentValidation.AspNetCore;
using Persistence;
using WebApi.Extensions;
using WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerExtension();

builder.Services.AddAuthenticationServices(builder.Configuration);

builder.Services.AddApplication();

builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddIdentityServices();

builder.Services.AddCurrentUserService();

builder.Services.AddMailService(builder.Configuration);

builder.Services.AddUserService();

builder.Services.AddDatabaseSeed();

builder.Services.AddHttpContextAccessor();

builder.Services.AddApiVersioningService();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.InitializeDb();

app.Run();