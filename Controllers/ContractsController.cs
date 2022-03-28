using ApiNominas.DTO;
using ApiNominas.Entities;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiNominas;

namespace ApiNominas.Controllers
{
    [ApiController]
    [Route("api/company/{companyId:int}/contracts")]
    public class ContractsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ContractsController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("/allContracts")]
        public async Task<ActionResult<List<ContractWithCompanyDTO>>> Get()
        {
            List<Contract> contracts = await context.Contracts
                                                     .Include(x => x.Company)
                                                     .ToListAsync();

            return mapper.Map<List<ContractWithCompanyDTO>>(contracts);
        }

        [HttpGet]
        public async Task<ActionResult<List<ContractDTO>>> Get(int companyId)
        {
            List<Contract> contracts = await context.Contracts
                                                    .Where(x => x.CompanyID == companyId)
                                                    .ToListAsync();
            return mapper.Map<List<ContractDTO>>(contracts);
        }

        [HttpGet("{id:int}", Name = "GetContract")]
        public async Task<ActionResult<ContractDTO>> Get(int companyId, int id)
        {
            Contract contract = await context.Contracts
                                             .Where(x => x.CompanyID == companyId)
                                             .Include(x => x.Company)
                                             .FirstOrDefaultAsync(c => c.Id == id);

            if (contract == null)
                return NotFound();

            return mapper.Map<ContractDTO>(contract);
        }

        [HttpPost]
        public async Task<ActionResult> Post(int companyId, ContractCreateDTO contractCreate)
        {
            var existCompany = await context.Companies.AnyAsync(x => x.Id == companyId);

            if (!existCompany)
            {
                return BadRequest($"No exist Company with ID: { companyId }");
            }

            Contract newContract = mapper.Map<Contract>(contractCreate);
            newContract.CompanyID = companyId;

            context.Add(newContract);
            await context.SaveChangesAsync();

            var newContractDTO = mapper.Map<ContractDTO>(newContract);

            return CreatedAtRoute("GetContract", new { companyId = newContract.CompanyID, id = newContract.Id }, newContractDTO);
        }

        [HttpPut("{id:int}")]

        public async Task<ActionResult> Put(int companyId, int id, ContractCreateDTO contractEdit)
        {
            var existCompany = await context.Companies.AnyAsync(x => x.Id == companyId);

            if (!existCompany)
            {
                return BadRequest($"No exist Company with ID: { companyId }");
            }

            var contract = await context.Contracts
                                        .FirstOrDefaultAsync(x => x.Id == id && 
                                                                  x.CompanyID == companyId);

            if (contract == null)
            {
                return NotFound();
            }

            contract = mapper.Map(contractEdit, contract);

            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int companyId, int id)
        {
            var existCont = await context.Contracts.AnyAsync(x => x.CompanyID == companyId &&
                                                                  x.Id == id);

            if (!existCont)
            {
                return NotFound();
            }

            context.Remove(new Contract() { Id = id });
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int companyId)
        {
            var existCont = await context.Contracts.AnyAsync(x => x.CompanyID == companyId);

            if (!existCont)
            {
                return NotFound();
            }
            
            var contracts = await context.Contracts
                                         .Where(x => x.CompanyID == companyId)
                                         .ToListAsync(); ;
                                            
            context.RemoveRange(contracts);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int companyId, int id, JsonPatchDocument patchDocument)
        {
            if(patchDocument == null)
            {
                return BadRequest();
            }

            var contract = await context.Contracts
                                        .FirstOrDefaultAsync(x => x.CompanyID == companyId && 
                                                                  x.Id == id);

            if(contract == null)
            {
                return NotFound();
            }

            var contractDTO = mapper.Map<ContractPatchDTO>(contract);

            patchDocument.ApplyTo(contractDTO);

            var isValid = TryValidateModel(contractDTO);

            if(!isValid)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(contractDTO, contract);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
