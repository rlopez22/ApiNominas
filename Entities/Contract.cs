using System.ComponentModel.DataAnnotations;

namespace ApiNominas.Entities
{
    public class Contract
    {
        public int Id { get; set; }
        
        [Required]
        [Range(0, int.MaxValue)]
        public int SBase { get; set; }

        [Required]
        [StringLength(100)]
        public string Position { get; set; }

        [Required]
        [StringLength(100)]
        public string ProfCategory { get; set; }

        [Required]
        public int CompanyID { get; set; }

        public Company Company { get; set; }
    }
}
