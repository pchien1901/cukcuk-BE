using Microsoft.Extensions.DependencyInjection;
using MISA.CUKCUK.Core;
using MISA.CUKCUK.Core.Exceptions;
using MISA.CUKCUK.Core.Interfaces;
using MISA.CUKCUK.Core.Services;
using MISA.CUKCUK.Infrastructure.Interfaces;
using MISA.CUKCUK.Infrastructure.MISADatabaseContext;
using MISA.CUKCUK.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
Common.ConnectionString = builder.Configuration.GetConnectionString("Database1");

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Config DI
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICustomerGroupRepository, CustomerGroupRepository>();
builder.Services.AddScoped<ICustomerGroupService, CustomerGroupService>();
builder.Services.AddScoped<IMISADbContext, MariaDbContext>();


var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ch�n middleware
app.UseMiddleware<HandleExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
