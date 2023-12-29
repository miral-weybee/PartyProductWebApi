using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PartyProductWebApi.Models;

namespace PartyProductWebApi.Controllers
{
    [Authorize]
    [Route("/AssignParty")]
    [ApiController]
    public class AssignPartyController : ControllerBase
    {
        private readonly EvaluationTaskDbContext _context;

        public AssignPartyController(EvaluationTaskDbContext context)
        {
            this._context = context;

        }
        [HttpGet]
        public async Task<ActionResult<List<AssignPartyDTO>>> GetAssignPartyList()
        {
            var list = new List<AssignPartyDTO>();
            var temp = await _context.AssignParties.Include(x => x.Party).Include(x => x.Product).ToListAsync();
            foreach (var item in temp)
            {
                list.Add(new AssignPartyDTO
                {
                    AssignPartyId = item.AssignPartyId,
                    PartyName = item.Party.PartyName,
                    ProductName = item.Product.ProductName,
                    PartyId = item.Party.PartyId,
                    ProductId = item.Product.ProductId
                });
            }
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<AssignPartyDTO>>> GetAssignPartyById(int id)
        {
            var temp = await _context.AssignParties.Include(x => x.Party).Include(x => x.Product).Where(x => x.AssignPartyId == id).ToListAsync();

            if (temp.IsNullOrEmpty())
             return NotFound();
            
            
            return Ok(new AssignPartyDTO
            {
                AssignPartyId = temp[0].AssignPartyId,
                PartyName = temp[0].Party.PartyName,
                ProductName = temp[0].Product.ProductName
            });
        }

        [HttpPost]
        public async Task<ActionResult<AssignPartyAddDTO>> SaveAssignParty([FromBody] AssignPartyAddDTO assignPartyAddDto)
        {
            if (assignPartyAddDto == null)
                return BadRequest();

            var assignparty = _context.AssignParties.Where(x => x.PartyId == assignPartyAddDto.PartyId && x.ProductId == assignPartyAddDto.ProductId);
            if(assignparty.Any())
                return Ok("Already Exists");

            _context.AssignParties.Add(new AssignParty()
            {
                PartyId = assignPartyAddDto.PartyId,
                ProductId = assignPartyAddDto.ProductId
            });
            await _context.SaveChangesAsync();
            return Ok("Assign Party Added Successfully..");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAssignParty([FromRoute] int id, AssignPartyAddDTO assignParty)
        {
            var assignparty = _context.AssignParties.Where(x => x.AssignPartyId == id).FirstOrDefault();
            if (assignparty == null)
            {
                return NotFound();
            }
            assignparty.PartyId = assignParty.PartyId;
            assignparty.ProductId = assignParty.ProductId;

            await _context.SaveChangesAsync();
            return Ok("Assign Party Updated Successfully..");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAssignParty([FromRoute] int id)
        {
            var assignparty = _context.AssignParties.SingleOrDefault(x => x.AssignPartyId == id);
            if (assignparty == null)
                return NotFound();

            _context.AssignParties.Remove(assignparty);
            await _context.SaveChangesAsync();
            return Ok("Assign Party Deleted Successfully..");

        }
    }
}
