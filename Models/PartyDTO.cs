using System.ComponentModel.DataAnnotations;

namespace PartyProductWebApi.Models
{
    public class PartyDTO
    {
        [Required]
        public int PartyId { get; set; }

        [Required]
        public string PartyName { get; set; }
    }
}
