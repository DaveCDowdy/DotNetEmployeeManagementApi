using EmployeeManagementApi.Models;
using EmployeeManagementApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController(IEmployeeRepository repository) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployeeAsync(Employee employee)
        {
            await repository.AddEmployeeAsync(employee);

            return CreatedAtAction("GetEmployeeById", new { id = employee.Id }, employee);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAllEmployeesAsync()
        {
            return Ok(await repository.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        [ActionName("GetEmployeeById")]
        public async Task<ActionResult<Employee>> GetEmployeeByIdAsync(int id)
        {
            var employee = await repository.GetByIdAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteEmployeeAsync(int id)
        {
            await repository.DeleteEmployeeAsync(id);
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Employee>> UpdateEmployeeAsync(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }
            await repository.UpdateEmployeeAsync(employee);
            return CreatedAtAction("GetEmployeeById", new { id = employee.Id }, employee); 
        }
    }
}