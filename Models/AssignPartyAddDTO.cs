using System.ComponentModel.DataAnnotations;

namespace PartyProductWebApi.Models
{
    public class AssignPartyAddDTO
    {
        public int AssignPartyId { get; set; }
        [Required]
        public int PartyId { get; set; }
        [Required]
        public int ProductId { get; set; }
    }
}
