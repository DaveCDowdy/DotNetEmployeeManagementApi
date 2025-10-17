using EmployeeManagement.Application.Interfaces; 
using EmployeeManagement.Domain;                
using EmployeeManagement.Infrastructure.Data;    
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Infrastructure.Repositories
{
    // Implements the IEmployeeRepository contract using EF Core/AppDbContext.
    public class EmployeeRepository(AppDbContext context) : IEmployeeRepository
    {
        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await context.Employees.ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await context.Employees.FindAsync(id);
        }

        public async Task<Employee> AddAsync(Employee employee)
        {
            await context.Employees.AddAsync(employee);
            await context.SaveChangesAsync();
            return employee; 
        }

        public async Task UpdateAsync(Employee employee)
        {
            context.Employees.Update(employee);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var employeeInDb = await context.Employees.FindAsync(id)
                               ?? throw new KeyNotFoundException($"Employee with id {id} not found.");

            context.Employees.Remove(employeeInDb);
            await context.SaveChangesAsync();
        }
    }
}