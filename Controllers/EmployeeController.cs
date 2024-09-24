using Dapper;
using MiniApi.Interface;
using Microsoft.AspNetCore.Mvc;
using MiniApi.Model;
using System.Threading.Tasks;

namespace OneApp.Controllers
{
    [Route("api/employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployee _employeeRepository;

        public EmployeeController(IEmployee employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        // Get all employees
        [HttpGet]
        public async Task<ActionResult> GetEmployee()
        {
            try
            {
                var employees = await _employeeRepository.GetEmployeeList();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Add a new employee
        [HttpPost]
        public async Task<ActionResult> AddEmployee(Employee employee)
        {
            try
            {
                var result = await _employeeRepository.CreateEmployee(employee); // Call CreateEmployee
                if (result)
                {
                    return Ok(new { Success = true, data = employee });
                }
                return BadRequest(new { Success = false, message = "Failed to add employee." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Update an existing employee
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateEmployee(int id, Employee employee)
        {
            try
            {
                var existingEmployee = await _employeeRepository.GetEmployee(id); // Fetch the employee by ID
                if (existingEmployee == null)
                {
                    return NotFound(new { Success = false, message = "No employee found." });
                }

                employee.Id = id; // Set the ID before updating
                var updatedEmployee = await _employeeRepository.UpdateEmployee(employee);
                return Ok(new { Success = true, message = "Employee updated successfully", data = updatedEmployee });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Delete an employee
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            try
            {
                var employee = await _employeeRepository.GetEmployee(id); // Fetch the employee by ID
                if (employee == null)
                {
                    return Ok(new { Success = false, message = "No employee found." });
                }

                var result = await _employeeRepository.DeleteEmployee(id); // Delete the employee
                return Ok(new { Success = result, message = result ? "Employee has been deleted successfully." : "Failed to delete employee." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
