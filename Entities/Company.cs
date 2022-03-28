using System.ComponentModel.DataAnnotations;

namespace ApiNominas.Entities
{
    public class Company
    {
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string CompanyName { get; set; }

        public List<Contract> Contracts { get; set; }
    }
}
