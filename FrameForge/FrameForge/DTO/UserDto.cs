using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace FrameForge.DTO
{
    public class UserDto
    {
        [Key]
        public Guid StudentId { get; set; }

        [Required]
        [JsonProperty("name")]
        [MaxLength(50)]
        public string? Username { get; set; }

        [Required]
        [JsonProperty("email")]
        public string? Email { get; set; }
        public string? Password { get; set; }

        [JsonProperty("Id")]
        public string? GoogleId { get; set; }
        [Required]
        public double MoneyAmount { get; set; }

        [JsonProperty("picture")]
        public string? Picture { get; set; }
    }
}
