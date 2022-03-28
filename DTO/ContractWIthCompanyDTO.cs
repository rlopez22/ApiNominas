using ApiNominas.Entities;

namespace ApiNominas.DTO
{
    public class ContractWithCompanyDTO
    {
        public int Id { get; set; }

        public int SBase { get; set; }

        public string Position { get; set; }

        public string ProfCategory { get; set; }

        public int CompanyID { get; set; }

        public CompanyDTO Company { get; set; }
    }
}
