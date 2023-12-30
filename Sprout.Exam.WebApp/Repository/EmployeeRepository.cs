using Microsoft.EntityFrameworkCore;
using Sprout.Exam.WebApp.Data;
using Sprout.Exam.WebApp.IRepository;
using Sprout.Exam.WebApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace Sprout.Exam.WebApp.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        ApplicationDbContext _dbContext;

        public EmployeeRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Employee> GetAllEmployees()
        {
            return _dbContext.Employees.ToList();
        }

        public Employee GetEmployeeByID(int id)
        {
            return _dbContext.Employees.AsNoTracking().FirstOrDefault(employee => employee.ID == id);
        }

        public Employee AddEmployee(Employee newEmployee)
        {
            _dbContext.Employees.Add(newEmployee);
            _dbContext.SaveChanges();

            return newEmployee;
        }

        public Employee UpdateEmployee(Employee newEmployee)
        {
            _dbContext.Employees.Update(newEmployee);
            _dbContext.SaveChanges();

            return newEmployee;
        }

        public Employee DeleteEmployee(Employee oldEmployee)
        {
            _dbContext.Employees.Remove(oldEmployee);
            _dbContext.SaveChanges();

            return oldEmployee;
        }
    }
}
