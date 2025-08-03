using System.ComponentModel.DataAnnotations;

namespace WebApiJoinwithLinq.Model.Entities
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [MaxLength(50)]
        public required string Name { get; set; }

    }
}
