using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using my_api_project.Data;
using my_api_project.Models;
using my_api_project.DTOs;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace my_api_project.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public TransactionController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: /Transaction
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactions()
        {
            var transactions = await _context.Transactions.ToListAsync();
            var dtos = _mapper.Map<IEnumerable<TransactionDto>>(transactions);
            return Ok(dtos);
        }

        // GET: /Transaction/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDto>> GetTransaction(int id)
        {
            var t = await _context.Transactions.FirstOrDefaultAsync(t => t.TransactionId == id);
            if (t == null)
            {
                return NotFound();
            }
            var dto = _mapper.Map<TransactionDto>(t);
            return Ok(dto);
        }

        // POST: /Transaction
        [HttpPost]
        public async Task<ActionResult<TransactionDto>> CreateTransaction(CreateTransactionDto dto)
        {
            var transaction = _mapper.Map<Transaction>(dto);
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            var resultDto = _mapper.Map<TransactionDto>(transaction);
            return CreatedAtAction(nameof(GetTransaction), new { id = transaction.TransactionId }, resultDto);
        }

        // PUT: /Transaction/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransaction(int id, UpdateTransactionDto dto)
        {
            if (id != dto.TransactionId)
            {
                return BadRequest();
            }
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            _mapper.Map(dto, transaction);
            _context.Entry(transaction).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Transactions.Any(e => e.TransactionId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // DELETE: /Transaction/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}