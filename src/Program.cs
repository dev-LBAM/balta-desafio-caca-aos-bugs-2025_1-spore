using BugStore.Data;
using BugStore.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

if(builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

string cnnString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(cnnString));

Console.WriteLine($"Using connection string: {cnnString}");

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // Adiciona um customer de teste
    context.Customers.Add(new Customer { Id = new Guid(), Name = "Lucas", Email = "lucas@hotmail.com", Phone = "18997895096", BirthDate = new DateTime() });
    context.SaveChanges();

    // Lê os customers
    var all = context.Customers.ToList();
    Console.WriteLine($"Total de clientes: {all.Count}");
}

app.MapGet("/", () => "All Correct!");

app.Run();
