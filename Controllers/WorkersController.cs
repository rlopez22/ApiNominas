using ApiNominas.DTO;
using ApiNominas.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiNominas;

namespace ApiNominas.Controllers
{
    [ApiController]
    [Route("api/workers")]
    public class WorkersController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public WorkersController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet()]
        public async Task<ActionResult<List<WorkerDTO>>> Get()
        {
            List<Worker> workers = await context.Workers
                                                .Include(x => x.Contract)
                                                .ToListAsync();
            return mapper.Map<List<WorkerDTO>>(workers);
        }

        [HttpGet("{id:int}", Name = "GetWorker")]
        public async Task<ActionResult<List<WorkerDTO>>> Get(int id)
        {
            List<Worker> workers = await context.Workers
                                                .Where(x => x.Id == id)
                                                .Include(x => x.Contract)
                                                .ToListAsync();
            return mapper.Map<List<WorkerDTO>>(workers);
        }

        [HttpPost]
        public async Task<ActionResult> Post(WorkerCreateDTO workerCreate)
        {
            var existContract = await context.Contracts.AnyAsync(x => x.Id == workerCreate.ContractID);

            if (!existContract)
            {
                return BadRequest($"No exist Contract with ID: { workerCreate.ContractID }");
            }

            Worker newWorker = mapper.Map<Worker>(workerCreate);
            context.Add(newWorker);
            await context.SaveChangesAsync();

            var newWorkerDTO = mapper.Map<WorkerDTO>(newWorker);

            return CreatedAtRoute("GetWorker", new { id = newWorker.Id }, newWorkerDTO);
        }

        [HttpPut("{id:int}")]

        public async Task<ActionResult> Put(WorkerCreateDTO workerEdit, int id)
        {
            var worker = await context.Workers.FirstOrDefaultAsync(x => x.Id == id);

            if (worker == null)
            {
                return NotFound();
            }

            worker = mapper.Map(workerEdit, worker);

            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await context.Workers.AnyAsync(x => x.Id == id);

            if (!exist)
            {
                return NotFound();
            }

            context.Remove(new Worker() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
