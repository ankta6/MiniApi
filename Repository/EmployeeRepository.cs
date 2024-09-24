using Dapper;
using MiniApi.Interface;
using MiniApi.Model;
using System.Data;

namespace CommonLibrary.Repository
{
    public class EmployeeRepository : IEmployee
    {
        private readonly MiniDBContext _context;

        public EmployeeRepository(MiniDBContext context)
        {
            _context = context;
        }

        // Method to get all employees
        public async Task<List<Employee>> GetEmployeeList()
        {
            var sql = "SELECT * FROM Employee";
            using (var connection = _context.CreateConnection())
            {
                var employees = await connection.QueryAsync<Employee>(sql);
                return employees.ToList();
            }
        }

        // Method to create a new employee
        public async Task<bool> CreateEmployee(Employee employee)
        {
            var sql = "INSERT INTO Employee(Name, Age) VALUES(@Name, @Age);";
            var param = new DynamicParameters();
            param.Add("Name", employee.Name, DbType.String);
            param.Add("Age", employee.Age, DbType.Int32);

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(sql, param);
                return result > 0; // Return true if the insert was successful
            }
        }

        // Method to update an existing employee
        public async Task<Employee> UpdateEmployee(Employee employee)
        {
            var sql = "UPDATE Employee SET Name = @Name, Age = @Age WHERE Id = @Id";
            var param = new DynamicParameters();
            param.Add("Id", employee.Id, DbType.Int32);
            param.Add("Name", employee.Name, DbType.String);
            param.Add("Age", employee.Age, DbType.Int32);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(sql, param);
                return employee; // Return the updated employee
            }
        }

        // Method to delete an employee by Id
        public async Task<bool> DeleteEmployee(int id)
        {
            var sql = "DELETE FROM Employee WHERE Id = @Id";
            var param = new DynamicParameters();
            param.Add("Id", id, DbType.Int32);

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(sql, param);
                return result > 0; // Return true if delete was successful
            }
        }

        // Method to get a single employee by Id
        public async Task<Employee> GetEmployee(int id)
        {
            var sql = "SELECT * FROM Employee WHERE Id = @Id";
            using (var connection = _context.CreateConnection())
            {
                var employee = await connection.QuerySingleOrDefaultAsync<Employee>(sql, new { Id = id });
                return employee;
            }
        }
    }
}
