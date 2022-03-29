using ApiNominas.DTO;
using ApiNominas.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiNominas;

namespace ApiNominas.Controllers
{
    [ApiController]
    [Route("api/workers/{workerId:int}/payrolls")]

    public class PayrollsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public PayrollsController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<List<PayrollDTO>>> Get(int workerId)
        {
            List<Payroll> payrolls = await context.Payrolls
                                                  .Where(x => x.WorkerId == workerId)
                                                  .Include(x => x.Worker)
                                                  .Include(x => x.Worker.Contract)
                                                  .ToListAsync();
            return mapper.Map<List<PayrollDTO>>(payrolls);
        }

        [HttpGet("{id:int}", Name = "GetPayroll")]
        public async Task<ActionResult<PayrollDTO>> Get(int workerId, int id)
        {
            Payroll payroll = await context.Payrolls
                                            .Where(x => x.WorkerId == workerId)
                                            .Include(x => x.Worker)
                                            .Include(x => x.Worker.Contract)
                                            .FirstOrDefaultAsync(c => c.Id == id);

            if (payroll == null)
                return NotFound();

            return mapper.Map<PayrollDTO>(payroll);
        }

        [HttpPost]
        public async Task<ActionResult> Post(int workerId, PayrollCreateDTO contractCreate)
        {
            var existWorker = await context.Workers.AnyAsync(x => x.Id == workerId);

            if (!existWorker)
            {
                return BadRequest($"No exist Worker with ID: { workerId }");
            }

            var existPayroll = await context.Payrolls.AnyAsync(x => x.WorkerId == workerId && 
                                                                    x.Start.Date.Year.Equals(contractCreate.Start.Date.Year) &&
                                                                    x.Start.Date.Month.Equals(contractCreate.Start.Date.Month));

            if (existPayroll)
            {
                return BadRequest($"Payroll already exist in this date.");
            }

            Payroll newPayroll = new Payroll();
            newPayroll = mapper.Map<Payroll>(contractCreate);
            newPayroll.Worker = context.Workers
                                       .Include(x => x.Contract)
                                       .FirstOrDefault(x => x.Id == workerId);
            newPayroll.WorkerId = workerId;

            context.Add(newPayroll);
            await context.SaveChangesAsync();

            var newPayrollDTO = mapper.Map<PayrollDTO>(newPayroll);

            return CreatedAtRoute("GetPayroll", new { workerId = newPayroll.WorkerId, id = newPayroll.Id }, newPayrollDTO);
        }

        [HttpPut("{id:int}")]

        public async Task<ActionResult> Put(int workerId, int id, PayrollCreateDTO payrollEdit)
        {
            var existWorker = await context.Workers.AnyAsync(x => x.Id == workerId);

            if (!existWorker)
            {
                return BadRequest($"No exist Worker with ID: { workerId }");
            }

            var payroll = await context.Payrolls
                                        .FirstOrDefaultAsync(x => x.Id == id &&
                                                                  x.WorkerId == workerId);

            if (payroll == null)
            {
                return NotFound();
            }

            payroll = mapper.Map(payrollEdit, payroll);

            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
