using ApiNominas.DTO;
using ApiNominas.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiNominas;

namespace ApiNominas.Controllers
{
    [ApiController]
    [Route("api/companies")]
    public class CompaniesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public CompaniesController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<CompanyDTO>>> Get()
        {
            List<Company> companies = await context.Companies
                                                   .Include(x => x.Contracts)
                                                   .ToListAsync();
            return mapper.Map<List<CompanyDTO>>(companies);
        }

        [HttpGet("{id:int}", Name = "GetCompany")]
        public async Task<ActionResult<CompanyWithContractsDTO>> Get(int id)
        {
            Company company = await context.Companies
                                           .Include(x => x.Contracts)
                                           .FirstOrDefaultAsync(c => c.Id == id);

            if (company == null)
            {
                return NotFound();
            }

            return mapper.Map<CompanyWithContractsDTO>(company);
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<List<CompanyDTO>>> Get(string name)
        {
            var companies = await context.Companies.Include(x => x.Contracts)
                                                   .Where(c => c.CompanyName.Contains(name))
                                                   .ToListAsync();

            return mapper.Map<List<CompanyDTO>>(companies);
        }

        [HttpPost]
        public async Task<ActionResult> Post(CompanyCreateDTO companyCreate)
        {
            var existCompany = await context.Companies.AnyAsync(x => x.CompanyName.ToLower().Equals(companyCreate.CompanyName.ToLower()));

            if (existCompany)
            {
                return BadRequest($"{companyCreate.CompanyName} already exist.");
            }


            Company comp = mapper.Map<Company>(companyCreate);

            context.Add(comp);
            await context.SaveChangesAsync();

            var compDTO = mapper.Map<CompanyDTO>(comp);

            return CreatedAtRoute("GetCompany", new { id = comp.Id }, compDTO);
        }

        [HttpPut("{id:int}")]

        public async Task<ActionResult> Put(CompanyCreateDTO companyEdit, int id)
        {
            var company = await context.Companies.FirstOrDefaultAsync(x => x.Id == id);

            if (company == null)
            {
                return NotFound();
            }
            
            company = mapper.Map(companyEdit, company);

            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await context.Companies.AnyAsync(x => x.Id == id);

            if (!exist)
            {
                return NotFound();
            }

            context.Remove(new Company() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
