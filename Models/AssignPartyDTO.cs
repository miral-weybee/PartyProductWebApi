using System.ComponentModel.DataAnnotations;

namespace PartyProductWebApi.Models
{
    public class AssignPartyDTO
    {
        [Required]
        public int AssignPartyId { get; set; }
        [Required]
        public string PartyName { get; set; }
        [Required]
        public string ProductName { get; set; }

        public int PartyId { get; set; }

        public int ProductId { get; set; }
    }
}
