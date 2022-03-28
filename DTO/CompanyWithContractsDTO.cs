namespace ApiNominas.DTO
{
    public class CompanyWithContractsDTO
    {
        public int Id { get; set; }

        public string CompanyName { get; set; }

        public List<ContractDTO> Contracts { get; set; }
    }
}
