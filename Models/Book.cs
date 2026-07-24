using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreMVC.Models
{
    [Table("books")]
    public class Book
    {
        [Key]
        [Column("book_id")]
        public int BookId { get; set; }

        [Required]
        [Column("title")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Column("author")]
        public string Author { get; set; } = string.Empty;

        [Column("publisher")]
        public string? Publisher { get; set; }

        [Column("language")]
        public string? Language { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("stock")]
        public int Stock { get; set; }

        [Column("image_url")]
        public string? ImageUrl { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("category_id")]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }
    }
}