using System.ComponentModel.DataAnnotations;

namespace ApiNominas.DTO
{
    public class ContractCreateDTO
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int SBase { get; set; }

        [Required]
        [StringLength(100)]
        public string Position { get; set; }

        [Required]
        [StringLength(100)]
        public string ProfCategory { get; set; }
    }
}
