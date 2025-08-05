using System.ComponentModel.DataAnnotations;

namespace WebApiJoinwithLinq.Model.Entities
{
    public class Login
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public required string Username { get; set; }
        [MaxLength(50)]
        public required string Password { get; set; }
    }
}
