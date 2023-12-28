using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.IdentityModel.Tokens;
using PartyProductWebApi.Models;

namespace PartyProductWebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Party")]
    public class PartyController : ControllerBase
    {
        private readonly EvaluationTaskDbContext _context;

        public PartyController(EvaluationTaskDbContext context)
        {
            this._context = context;
            
        }
        [HttpGet]
        public async Task<ActionResult<List<PartyDTO>>> GetPartyList()
        {
            var list = new List<PartyDTO>();
            var temp= await _context.Parties
                .ToListAsync();
            foreach(var item in temp)
            {
                list.Add(new PartyDTO
                {
                    PartyId = item.PartyId,
                    PartyName = item.PartyName
                });
            }
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<PartyDTO>>> GetPartyById(int id)
        {
            var temp = await _context.Parties.Where(i => i.PartyId == id).ToListAsync();

            if (temp.IsNullOrEmpty())
                return NotFound();
              
            
            return Ok(new PartyDTO
            {
                PartyId = temp[0].PartyId,
                PartyName = temp[0].PartyName
            });
        }

        [HttpPost]
        public async Task<ActionResult<PartyDTO>> SaveParty([FromBody] Party party)
        {
            if (party == null)
                return BadRequest();

            _context.Parties.Add(party);
            await _context.SaveChangesAsync();
            return Ok(party);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateParty([FromRoute] int id, PartyDTO partydto)
        {
            var party = await _context.Parties.Where(x => x.PartyId == id).FirstOrDefaultAsync();
            if (party == null)
            {
                return NotFound();
            }
            party.PartyId= partydto.PartyId;
            party.PartyName = partydto.PartyName;

            _context.SaveChanges();
            return Ok("Party Updated Successfully");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteParty([FromRoute] int id)
        {
            var party = _context.Parties.SingleOrDefault(x => x.PartyId == id);
            if(party == null)
                return NotFound();

            _context.Parties.Remove(party);
            await _context.SaveChangesAsync();
            return Ok("Party Deleted Successfully..");
        }
    }
}
