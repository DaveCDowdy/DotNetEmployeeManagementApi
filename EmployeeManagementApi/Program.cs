using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Application.Services;
using EmployeeManagement.Domain;
using EmployeeManagement.Infrastructure.Data;
using EmployeeManagement.Infrastructure.Repositories;

using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- SERVICES (Dependency Injection) ---

// Infrastructure: DbContext Registration (In-Memory)
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseInMemoryDatabase("EmployeeDb")
);

// Infrastructure & Application: Register Concrete Repository against Interface
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

// Application: Register Application Service (Use Cases)
builder.Services.AddScoped<EmployeeService>();

// Application: Register Fluent Validation from the Application assembly
builder.Services.AddValidatorsFromAssembly(typeof(EmployeeService).Assembly);

// Presentation: CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCors", policyBuilder =>
    {
        policyBuilder.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Presentation: API Explorer / Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Seed data function to prepopulate the In-Memory database
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

// --- PIPELINE CONFIGURATION ---

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

// --- ENDPOINT VALIDATION HELPER ---
async Task<IResult> ValidateAndRunAsync<T>(
    IValidator<T> validator, 
    T model, 
    Func<Task<IResult>> handler)
{
    var validationResult = await validator.ValidateAsync(model);
    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }
    return await handler();
}

// --- MINIMAL API ENDPOINTS (Replacing your Controller Actions) ---

// GET ALL EMPLOYEES
app.MapGet("/api/employee", async (EmployeeService service) =>
    Results.Ok(await service.GetAllEmployeesAsync()))
    .WithOpenApi();

// GET EMPLOYEE BY ID
app.MapGet("/api/employee/{id:int}", async (int id, EmployeeService service) =>
{
    var employee = await service.GetEmployeeByIdAsync(id);
    return employee is null ? Results.NotFound() : Results.Ok(employee);
})
.WithName("GetEmployeeById")
.WithOpenApi();

// CREATE EMPLOYEE (POST)
app.MapPost("/api/employee", async (Employee employee, EmployeeService service, IValidator<Employee> validator) =>
{
    return await ValidateAndRunAsync(validator, employee, async () =>
    {
        var newEmployee = await service.CreateEmployeeAsync(employee);
        return Results.CreatedAtRoute("GetEmployeeById", new { id = newEmployee.Id }, newEmployee);
    });
})
.WithOpenApi();

// UPDATE EMPLOYEE (PUT)
app.MapPut("/api/employee/{id:int}", async (int id, Employee employee, EmployeeService service, IValidator<Employee> validator) =>
{
    if (id != employee.Id) return Results.BadRequest();

    return await ValidateAndRunAsync(validator, employee, async () =>
    {
        await service.UpdateEmployeeAsync(employee);
        return Results.CreatedAtRoute("GetEmployeeById", new { id = employee.Id }, employee); 
    });
})
.WithOpenApi();

// DELETE EMPLOYEE (DELETE)
app.MapDelete("/api/employee/{id:int}", async (int id, EmployeeService service) =>
{
    try
    {
        await service.DeleteEmployeeAsync(id);
        return Results.NoContent();
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound();
    }
})
.WithOpenApi();


app.Run();