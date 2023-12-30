using System.Collections.Generic;
using Sprout.Exam.WebApp.Models;

namespace Sprout.Exam.WebApp.IRepository
{
    public interface IEmployeeRepository
    {
        public List<Employee> GetAllEmployees();
        public Employee GetEmployeeByID(int id);
        public Employee AddEmployee(Employee newEmployee);
        public Employee UpdateEmployee(Employee newEmployee);
        public Employee DeleteEmployee(Employee oldEmployee);
    }
}
