using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreMVC.Models
{
    [Table("categories")]
    public class Category
    {
        [Key]
        [Column("category_id")]
        public int CategoryId { get; set; }

        [Required]
        [Column("category_name")]
        public string CategoryName { get; set; } = string.Empty;

        public ICollection<Book>? Books { get; set; }
    }
}