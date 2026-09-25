using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi;
using BankAccountApi.Models;
using BankAccountApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<BankAccountContext>();
builder.Services.AddScoped<IBankAccountItemService, BankAccountItemService>();
builder.Services.AddAuthentication(o => 
{
   o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
   o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "/openapi/v1.json";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
