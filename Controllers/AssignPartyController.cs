using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartyProductWebApi.Models;

namespace PartyProductWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignPartyController : ControllerBase
    {
        private readonly PartyProductWebApiContext _context;

        public AssignPartyController(PartyProductWebApiContext context)
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
                    ProductName = item.Product.ProductName
                });
            }
            return Ok(list);
        }

        [HttpPost]
        public async Task<ActionResult<AssignPartyAddDTO>> SaveAssignParty([FromBody] AssignPartyAddDTO assignPartyAddDto)
        {
            if (assignPartyAddDto == null)
                return BadRequest();

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
