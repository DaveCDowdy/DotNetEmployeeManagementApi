using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Domain;
using EmployeeManagement.Infrastructure.Data;
using EmployeeManagement.Infrastructure.Repositories;

using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Application.Employees.Queries;
using EmployeeManagement.Application.Employees.Commands;
using EmployeeManagement.Application.Validation;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseInMemoryDatabase("EmployeeDb")
);

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllEmployeesQuery).Assembly));

builder.Services.AddValidatorsFromAssembly(typeof(GetAllEmployeesQuery).Assembly);

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCors", policyBuilder =>
    {
        policyBuilder.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.Use(async (context, next) =>
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (await dbContext.Employees.CountAsync() == 0)
    {
        dbContext.Employees.AddRange(
            new Employee { Id = 1, FirstName = "Alice", LastName = "Smith", Email = "alice@corp.com", Phone = "5551234567", Position = "Developer" },
            new Employee { Id = 2, FirstName = "Bob", LastName = "Jones", Email = "bob@corp.com", Phone = "5559876543", Position = "Manager" }
        );
        await dbContext.SaveChangesAsync();
    }
    await next();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee Management API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseCors("MyCors");

// FIXED: Helper function now takes HttpContext to manually fetch the validator service
async Task<IResult> ValidateAndRunAsync<T>(
    T model, 
    Func<Task<IResult>> handler,
    HttpContext httpContext) where T : notnull
{
    var validator = httpContext.RequestServices.GetService<IValidator<T>>();
    
    if (validator is null)
    {
        return Results.Problem($"Validator service for {typeof(T).Name} not found.", statusCode: 500); 
    }

    var validationResult = await validator.ValidateAsync(model);
    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }
    return await handler();
}

// GET ALL EMPLOYEES
app.MapGet("/api/employee", async (ISender sender) =>
{
    var employees = await sender.Send(new GetAllEmployeesQuery());
    return Results.Ok(employees);
})
.WithOpenApi();

// GET EMPLOYEE BY ID
app.MapGet("/api/employee/{id:int}", async (int id, ISender sender) =>
{
    var employee = await sender.Send(new GetEmployeeByIdQuery(id));
    return employee is null ? Results.NotFound() : Results.Ok(employee);
})
.WithName("GetEmployeeById")
.WithOpenApi();

// CREATE EMPLOYEE (POST)
// FIXED: Removed IValidator<CreateEmployeeCommand> parameter and added HttpContext
app.MapPost("/api/employee", async (CreateEmployeeCommand command, ISender sender, HttpContext httpContext) =>
{
    return await ValidateAndRunAsync(command, async () =>
    {
        var newEmployee = await sender.Send(command);
        return Results.CreatedAtRoute("GetEmployeeById", new { id = newEmployee.Id }, newEmployee);
    }, httpContext);
})
.WithOpenApi();

// UPDATE EMPLOYEE (PUT)
// FIXED: Removed IValidator<UpdateEmployeeCommand> parameter and added HttpContext
app.MapPut("/api/employee/{id:int}", async (int id, UpdateEmployeeCommand command, ISender sender, HttpContext httpContext) =>
{
    if (id != command.Id) return Results.BadRequest();

    return await ValidateAndRunAsync(command, async () =>
    {
        await sender.Send(command); 
        return Results.NoContent();
    }, httpContext);
})
.WithOpenApi();

// DELETE EMPLOYEE (DELETE)
app.MapDelete("/api/employee/{id:int}", async (int id, ISender sender) =>
{
    try
    {
        await sender.Send(new DeleteEmployeeCommand(id));
        return Results.NoContent();
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound();
    }
})
.WithOpenApi();


app.Run();