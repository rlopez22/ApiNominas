using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static ApiNominas.Utils.Constants;

namespace ApiNominas.DTO
{
    public class PayrollCreateDTO
    {
        [Required]
        public DateTime Start { get; set; }

        [DefaultValue(typeof(double), Withholding.ProfContDefault)]
        public double ProfContingences { get; set; }

        [DefaultValue(typeof(double), Withholding.IRPFDefault)]
        public double IRPF { get; set; }

    }
}
