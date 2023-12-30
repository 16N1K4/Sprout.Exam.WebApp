using System;
using System.ComponentModel.DataAnnotations;

namespace Sprout.Exam.WebApp.Models
{
    public class Employee
    {
        [Key]
        public int ID { get; set; }
        [Required]  
        public string Name { get; set; }
        [Required]
        public string BirthDate { get; set; }
        public int EmployeeTypeID { get; set; }
        [Required]
        public string TIN { get; set; }
    }
}
