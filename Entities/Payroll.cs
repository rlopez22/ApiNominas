using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static ApiNominas.Utils.Constants;

namespace ApiNominas.Entities
{
    public class Payroll
    {
        private int nDays;

        public int Id { get; set; }

        [Required]
        public int WorkerId { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/yyyy}")]
        public DateTime Start { get; set; }
        
        [Display(Name = "End")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/yyyy}")]

        public DateTime End { get => Start.AddMonths(1).AddDays(-1); }
        
        [Display(Name = "MonthSalary")]
        [DataType(DataType.Currency)]
        public double MonthSalary { get => (double)(Worker.Contract?.SBase / Calendar.Months); }
        
        [Display(Name = "NDays")]
        public int NDays { get => (int)(End - Start).TotalDays; }

        [DefaultValue(typeof(double), Withholding.ProfContDefault)]
        public double ProfContingences { get; set; }

        [DefaultValue(typeof(double), Withholding.IRPFDefault)]
        public double IRPF { get; set; }
        
        [Display(Name = "MonthPayroll")]
        [DataType(DataType.Currency)]
        public double MonthPayroll { get => (double)(MonthSalary * (1 - (double)ProfContingences - (double)IRPF)); }

        public Worker Worker { get; set; }
    }
}
