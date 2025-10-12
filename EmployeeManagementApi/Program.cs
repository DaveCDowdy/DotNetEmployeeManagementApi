using EmployeeManagementApi.Data;
using EmployeeManagementApi.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;

namespace EmployeeManagementApi
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("EmployeeDb")
            );

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyCors", policyBuilder =>
                {
                    policyBuilder.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee Management API v1");
                    c.RoutePrefix = string.Empty;
                });
            }

            app.UseCors("MyCors");

            app.MapControllers();

            app.Run();
        }
    }
}