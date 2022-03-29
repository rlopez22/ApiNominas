using ApiNominas.Entities;
using static ApiNominas.Utils.Constants;

namespace ApiNominas.DTO
{
    public class PayrollDTO
    {
        public int Id { get; set; }
        public int WorkerId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get => Start.AddMonths(1).AddDays(-1); }
        public double MonthSalary { get => (double)(Worker.Contract?.SBase / Calendar.Months); }
        public int NDays { get => (int)(End - Start).TotalDays; }
        public double ProfContingences { get; set; }
        public double IRPF { get; set; }
        public double MonthPayroll { get => (double)(MonthSalary * (1 - (double)ProfContingences - (double)IRPF)); }
        public Worker Worker { get; set; }
    }
}
