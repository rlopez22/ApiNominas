using ApiNominas.DTO;
using ApiNominas.Entities;
using AutoMapper;

namespace ApiNominas.Utils
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CompanyCreateDTO, Company>();
            CreateMap<Company, CompanyDTO>();
            CreateMap<Company, CompanyWithContractsDTO>();

            CreateMap<ContractCreateDTO, Contract>();
            CreateMap<ContractPatchDTO, Contract>().ReverseMap();
            CreateMap<Contract, ContractDTO>();
            CreateMap<Contract, ContractWithCompanyDTO>();

            CreateMap<WorkerCreateDTO, Worker>();
            CreateMap<Worker, WorkerDTO>();
            //CreateMap<Worker, WorkerWithContractsDTO>();
        }
    }
}
