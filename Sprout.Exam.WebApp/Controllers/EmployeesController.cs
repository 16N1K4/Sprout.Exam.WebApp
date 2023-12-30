using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Common.Enums;
using Sprout.Exam.WebApp.IRepository;
using Sprout.Exam.WebApp.Models;
using Sprout.Exam.WebApp.Logic;
using Sprout.Exam.WebApp.ILogic;

namespace Sprout.Exam.WebApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        /*
         * Original logic used for static employee list preserved in comment blocks like these
         */

        IEmployeeRepository _repository;
        ICalculateSalary _calculateSalary;

        public EmployeesController(IEmployeeRepository repository, ICalculateSalary calculateSalary)
        {
            _repository = repository;
            _calculateSalary = calculateSalary;
        }

        /// <summary>
        /// Returns a full list of employees from the database
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            
            var employeeList = _repository.GetAllEmployees();
            var employeeListDTO = new List<EmployeeDto>();

            foreach(var employee in employeeList)
            {
                employeeListDTO.Add(new EmployeeDto
                {
                    Id = employee.ID,
                    FullName = employee.Name,
                    Birthdate = employee.BirthDate,
                    Tin = employee.TIN,
                    TypeId = employee.EmployeeTypeID
                });
            }

            return Ok(employeeListDTO);
            

            /*
            var result = await Task.FromResult(StaticEmployees.ResultList);
            return Ok(result);
            */
        }

        /// <summary>
        /// Returns a single employee from the database using the ID primary key as a parameter
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var employee = _repository.GetEmployeeByID(id);
            if(employee == null)
            {
                return NotFound();
            }

            var employeeDTO = new EmployeeDto
            {
                Id = employee.ID,
                FullName = employee.Name,
                Birthdate = employee.BirthDate,
                Tin = employee.TIN,
                TypeId = employee.EmployeeTypeID
            };

            return Ok(employeeDTO);


            /*
            var result = await Task.FromResult(StaticEmployees.ResultList.FirstOrDefault(m => m.Id == id));
            return Ok(result);
            */
        }

        /// <summary>
        /// Updates the information of an existing employee entry in the database
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(EditEmployeeDto input)
        {
            if(_repository.GetEmployeeByID(input.Id) == null)
            {
                return NotFound();
            }

            if(input.FullName == null || input.Tin == null)
            {
                return BadRequest();
            }

            var updatedEmployee = new Employee
            {
                ID = input.Id,
                Name = input.FullName,
                BirthDate = input.Birthdate.ToString("yyyy-MM-dd"),
                TIN = input.Tin,
                EmployeeTypeID = input.TypeId
            };

            _repository.UpdateEmployee(updatedEmployee);

            var updatedEmployeeDTO = new EmployeeDto
            {
                FullName = input.FullName,
                Birthdate = input.Birthdate.ToString("yyyy-MM-dd"),
                Tin = input.Tin,
                TypeId = input.TypeId
            };
            
            return Ok(updatedEmployeeDTO);

            /*
            if (ModelState.IsValid) { 
                var item = await Task.FromResult(StaticEmployees.ResultList.FirstOrDefault(m => m.Id == input.Id));
                if (item == null) return NotFound();
                item.FullName = input.FullName;
                item.Tin = input.Tin;
                item.Birthdate = input.Birthdate.ToString("yyyy-MM-dd");
                item.TypeId = input.TypeId;
                return Ok(item);
            }

            return BadRequest();
            */
        }

        /// <summary>
        /// Creates a new employee entry in the database
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(CreateEmployeeDto input)
        {
            if(input.FullName == null || input.Tin == null)
            {
                return BadRequest();
            }

            var newEmployee = new Employee
            {
                Name = input.FullName,
                BirthDate = input.Birthdate.ToString("yyyy-MM-dd"),
                TIN = input.Tin,
                EmployeeTypeID = input.TypeId
            };

            var newEmployeeID = _repository.AddEmployee(newEmployee).ID;

            return Created($"/api/employees/{newEmployeeID}", newEmployeeID);

            /*
            var id = await Task.FromResult(StaticEmployees.ResultList.Max(m => m.Id) + 1);

            StaticEmployees.ResultList.Add(new EmployeeDto
            {
                Birthdate = input.Birthdate.ToString("yyyy-MM-dd"),
                FullName = input.FullName,
                Id = id,
                Tin = input.Tin,
                TypeId = input.TypeId
            });

            return Created($"/api/employees/{id}", id);
            */
        }


        /// <summary>
        /// Deletes an employee entry from the database (specified by the ID primary key)
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (_repository.GetEmployeeByID(id) == null)
            {
                return NotFound();
            }

            return Ok(_repository.DeleteEmployee(_repository.GetEmployeeByID(id)).ID);


            /*
            var result = await Task.FromResult(StaticEmployees.ResultList.FirstOrDefault(m => m.Id == id));
            if (result == null) return NotFound();
            StaticEmployees.ResultList.RemoveAll(m => m.Id == id);
            return Ok(id);
            */
        }



        /// <summary>
        /// Calculates the net income of a selected employee based on their employment type
        /// </summary>
        /// <param name="id"></param>
        /// <param name="absentDays"></param>
        /// <param name="workedDays"></param>
        /// <returns></returns>
        [HttpPost("{id}/calculate/{absentDays}/{workedDays}")]
        public async Task<IActionResult> Calculate(int id, decimal absentDays, decimal workedDays)
        {
            var employee = _repository.GetEmployeeByID(id);

            if(employee == null)
            {
                return NotFound();
            }

            /*
            var result = await Task.FromResult(StaticEmployees.ResultList.FirstOrDefault(m => m.Id == id));
            if (result == null) return NotFound();
            */

            var type = (EmployeeType) employee.EmployeeTypeID;
            return type switch
            {
                EmployeeType.Regular =>
                    Ok(_calculateSalary.CalculateRegularSalary(absentDays)),
                EmployeeType.Contractual =>
                    Ok(_calculateSalary.CalculateContractualSalary(workedDays)),
                _ => NotFound("Employee Type not found")
            };

        }

    }
}
