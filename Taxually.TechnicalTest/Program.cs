using FluentValidation;
using System;
using Taxually.TechnicalTest.Exceptions.Handler;
using Taxually.TechnicalTest.Handlers;
using Taxually.TechnicalTest.Interface;
using Taxually.TechnicalTest.Models;
using Taxually.TechnicalTest.Services;
using Taxually.TechnicalTest.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IVatRegistrationStrategy, UkVatRegistrationHandler>();
builder.Services.AddScoped<IVatRegistrationStrategy, FrVatRegistrationHandler>();
builder.Services.AddScoped<IVatRegistrationStrategy, DeVatRegistrationHandler>();
builder.Services.AddTransient<IVatHandler, VatService>();

builder.Services.AddControllers();
builder.Services.AddScoped<IValidator<VatRegistrationRequest>, VatRegistrationValidator>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseMiddleware<ExceptionHandler>();

app.Run();
