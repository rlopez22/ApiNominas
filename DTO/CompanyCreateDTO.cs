using System.ComponentModel.DataAnnotations;

namespace ApiNominas.DTO
{
    public class CompanyCreateDTO
    {
        [Required]
        [StringLength(150)]
        public string CompanyName { get; set; }
    }
}
