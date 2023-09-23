using api.Data;
using api.MinimalApis;
using api.Repositories;
using api.Validators;
using api.ViewModels;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

// cors
services.AddCors(options =>
{
    options.AddDefaultPolicy(builder => builder
        .SetIsOriginAllowedToAllowWildcardSubdomains()
        .WithOrigins("http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .Build());
});

// ioc
services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase(databaseName: "Test"));
services.AddScoped<DataSeeder>();
services.AddScoped<IEmailRepository, EmailRepository>();
services.AddScoped<IDocumentRepository, DocumentRepository>();
services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
services.AddScoped<IValidator<ClientVm>, ClientValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Register APIs
app.RegisterClientApi();

app.UseCors();

// seed data
using (var scope = app.Services.CreateScope())
{
    var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();

    dataSeeder.Seed();
}

// error handling
app.UseExceptionHandler(c => c?.Run(async context =>
{
    var exception = context.Features.Get<IExceptionHandlerPathFeature>()?.Error;
    var response = new { error = exception?.Message ?? exception?.InnerException?.Message };
    await context.Response.WriteAsJsonAsync(response);
}));

// run app
app.Run();