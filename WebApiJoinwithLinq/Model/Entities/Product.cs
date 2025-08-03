using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiJoinwithLinq.Model.Entities
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [MaxLength(100)]
        public required string Name { get; set; }
        public required int CategoryId { get; set; }

        public required Category? Category { get; set; }
    }
}
