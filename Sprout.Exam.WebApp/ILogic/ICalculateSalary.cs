namespace Sprout.Exam.WebApp.ILogic
{
    public interface ICalculateSalary
    {
        public decimal CalculateRegularSalary(decimal absentDays);
        public decimal CalculateContractualSalary(decimal workedDays);
    }
}
