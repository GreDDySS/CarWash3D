using CarWash3D.Data.Repositories;

namespace CarWash3D.Services
{
    public class EmployeeService
    {
        private readonly EmployeeRepository _employeeRepository;

        public EmployeeService(EmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public bool AuthenticateEmployee(string username, string password)
        {
            var employee = _employeeRepository.GetEmployeeByUsernameAndPassword(username, password);
            return employee != null;
        }
    }
}