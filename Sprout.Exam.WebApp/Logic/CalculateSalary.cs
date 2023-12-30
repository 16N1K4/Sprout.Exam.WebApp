using Sprout.Exam.WebApp.ILogic;
using System;
using System.Globalization;
using System.Linq;

namespace Sprout.Exam.WebApp.Logic
{
    public class CalculateSalary : ICalculateSalary
    {
        public decimal CalculateRegularSalary(decimal absentDays)
        {
            decimal absenceDeduction = (20000.00m / 22.00m) * absentDays;
            decimal taxDeduction = 2400.00m; //20000 * 0.12

            decimal netIncome = 20000.00m - absenceDeduction - taxDeduction;

            return netIncome;
        }

        public decimal CalculateContractualSalary(decimal workedDays)
        {
            decimal netIncome = 500.00m * workedDays;

            return netIncome;
        }
    }
}
