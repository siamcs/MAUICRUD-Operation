using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticeMAUI.API2.Model;
using System;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(op => op.UseSqlServer(builder.Configuration.GetConnectionString("con")));
// Add services to the container.
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


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapGet("api/employees", ([FromServices] AppDbContext db) => db.Employees).WithName("GetEmployees").Produces<Employee[]>(StatusCodes.Status200OK);

app.MapGet("api/employees/{id}",
    ([FromRoute] string id, [FromServices] AppDbContext db) => db.Employees.FirstOrDefault(e => e.EmployeeId.ToString() == id)).WithName("GetEmployee").Produces<Employee>(StatusCodes.Status200OK);

app.MapPost("api/employees", async ([FromBody] Employee employee, [FromServices] AppDbContext db) =>
{
    try
    {
        db.Employees.Add(employee);
        await db.SaveChangesAsync();
        return Results.Created($"api/employees/{employee.EmployeeId}", employee);
    }
    catch (Exception ex)
    {
        return Results.BadRequest($"Failed to save Employee: {ex.Message}");
    }
}).WithOpenApi().Produces<Employee>(StatusCodes.Status201Created);

app.MapPut("api/employees/{id}", async ([FromRoute] string id, [FromBody] Employee employee, [FromServices] AppDbContext db) =>
{
    Employee? foundEmployee = await db.Employees.FindAsync(id);
    if (foundEmployee == null) return Results.NotFound();
    foundEmployee.EmployeeName = employee.EmployeeName;
    foundEmployee.Joindate = employee.Joindate;
    foundEmployee.IsActive = employee.IsActive;
    foundEmployee.Salary = employee.Salary;
    foundEmployee.ImageUrl = employee.ImageUrl;
    await db.SaveChangesAsync();
    return Results.NoContent();
}).WithOpenApi()
.Produces(StatusCodes.Status404NotFound)
.Produces(StatusCodes.Status204NoContent);
app.MapDelete("api/employees/{id}", async ([FromRoute] string id, [FromServices] AppDbContext db) =>
{
    try
    {
        var employee = await db.Employees.FindAsync(id);
        if (employee == null)
        {
            return Results.NotFound();
        }
        db.Employees.Remove(employee);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
    catch (Exception ex)
    {
        return Results.BadRequest($"Failed to delete employee: {ex.Message}");
    }
}).WithOpenApi().Produces(StatusCodes.Status404NotFound)
.Produces(StatusCodes.Status204NoContent);

app.Run();


